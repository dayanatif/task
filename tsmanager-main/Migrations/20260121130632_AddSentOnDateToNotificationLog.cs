using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSManager.Migrations
{
    /// <inheritdoc />
    public partial class AddSentOnDateToNotificationLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SentOn",
                table: "NotificationLogs",
                newName: "SentOnDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SentOnDate",
                table: "NotificationLogs",
                newName: "SentOn");
        }
    }
}
