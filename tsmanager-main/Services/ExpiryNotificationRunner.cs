using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TSManager.Data;
using TSManager.Models;

namespace TSManager.Services
{
    public class ExpiryNotificationRunner
    {
        private readonly AppDbContext _db;
        private readonly EmailService _email;
        private readonly UserManager<IdentityUser> _userManager;

        public ExpiryNotificationRunner(AppDbContext db, EmailService email, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _email = email;
            _userManager = userManager;
        }

        public async Task RunAsync(CancellationToken ct)
        {
            var today = DateTime.Today;
            var daysList = new[] { 30, 14, 7, 1 };

            // Get admin recipients
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var recipients = admins
                .Select(a => a.Email)
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Distinct()
                .ToList();

            if (recipients.Count == 0)
                return; // no admin emails to send to

            foreach (var daysBefore in daysList)
            {
                var target = today.AddDays(daysBefore);

                var domains = await _db.Domains
                    .Where(d => d.RenewalDate.Date == target)
                    .OrderBy(d => d.RenewalDate)
                    .ToListAsync(ct);

                var software = await _db.SoftwareSubscriptions
                    .Where(s => s.RenewalDate.Date == target)
                    .OrderBy(s => s.RenewalDate)
                    .ToListAsync(ct);

                // Filter out already-sent (for today + same item + same daysBefore)
                var domainIds = domains.Select(d => d.Id).ToList();
                var softwareIds = software.Select(s => s.Id).ToList();

                var alreadySent = await _db.NotificationLogs
                    .Where(n => n.SentOnDate.Date == today.Date && n.DaysBefore == daysBefore)
                    .ToListAsync(ct);

                domains = domains
                    .Where(d => !alreadySent.Any(n => n.ItemType == "Domain" && n.ItemId == d.Id))
                    .ToList();

                software = software
                    .Where(s => !alreadySent.Any(n => n.ItemType == "Software" && n.ItemId == s.Id))
                    .ToList();

                if (!domains.Any() && !software.Any())
                    continue;

                var subject = $"TSManager: Expiry alerts ({daysBefore} days)";
                var body = BuildHtml(daysBefore, domains, software);

                foreach (var to in recipients)
                    await _email.SendAsync(to!, subject, body);

                // Log sent items
                foreach (var d in domains)
                {
                    _db.NotificationLogs.Add(new NotificationLog
                    {
                        ItemType = "Domain",
                        ItemId = d.Id,
                        DaysBefore = daysBefore,
                        SentOnDate = today
                    });
                }

                foreach (var s in software)
                {
                    _db.NotificationLogs.Add(new NotificationLog
                    {
                        ItemType = "Software",
                        ItemId = s.Id,
                        DaysBefore = daysBefore,
                        SentOnDate = today
                    });
                }

                await _db.SaveChangesAsync(ct);
            }
        }

        private static string BuildHtml(int daysBefore, List<DomainDetail> domains, List<SoftwareSubscription> software)
        {
            string Esc(string? x) => System.Net.WebUtility.HtmlEncode(x ?? "");

            var html = $@"
<h2>TSManager Expiry Alerts</h2>
<p>Items expiring in <b>{daysBefore}</b> days.</p>";

            if (domains.Any())
            {
                html += @"
<h3>Domains</h3>
<table border='1' cellpadding='8' cellspacing='0'>
<tr><th>Domain</th><th>Registrar</th><th>Renewal</th><th>Amount</th></tr>";
                foreach (var d in domains)
                {
                    html += $"<tr><td>{Esc(d.DomainName)}</td><td>{Esc(d.Registrar)}</td><td>{d.RenewalDate:yyyy-MM-dd}</td><td>{d.Amount:0.00}</td></tr>";
                }
                html += "</table>";
            }

            if (software.Any())
            {
                html += @"
<h3>Software</h3>
<table border='1' cellpadding='8' cellspacing='0'>
<tr><th>Software</th><th>Plan</th><th>Renewal</th><th>Amount</th></tr>";
                foreach (var s in software)
                {
                    html += $"<tr><td>{Esc(s.Software)}</td><td>{Esc(s.PlanType)}</td><td>{s.RenewalDate:yyyy-MM-dd}</td><td>{s.Amount:0.00}</td></tr>";
                }
                html += "</table>";
            }

            html += "<p style='margin-top:16px;color:#666'>Sent by TSManager</p>";
            return html;
        }
    }
}
