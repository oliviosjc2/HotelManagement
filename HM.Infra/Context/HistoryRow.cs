namespace HM.Infra.Context
{
    public class HistoryRow
    {
        public virtual string MigrationId { get; set; } = string.Empty;
        public virtual string ProductVersion { get; set; } = string.Empty;

        public HistoryRow() { }

        public HistoryRow(string migrationId, string productVersion)
        {
            MigrationId = migrationId;
            ProductVersion = productVersion;
        }
    }
}
