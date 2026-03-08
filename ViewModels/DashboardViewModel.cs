using TSManager.Models;

namespace TSManager.ViewModels
{
    public class DashboardViewModel
    {
        public int Days30 { get; set; }
        public int Days60 { get; set; }

        public List<SoftwareSubscription> SoftwareExpiring30 { get; set; } = new();
        public List<SoftwareSubscription> SoftwareExpiring60 { get; set; } = new();

        public List<DomainDetail> DomainsExpiring30 { get; set; } = new();
        public List<DomainDetail> DomainsExpiring60 { get; set; } = new();
    }
}
 