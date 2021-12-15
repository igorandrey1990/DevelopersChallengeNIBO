namespace DevelopersChallengeNIBO.Interfaces.Database
{
    public interface IOFXRecordsDBSettings
    {
        string OFXRecordsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
