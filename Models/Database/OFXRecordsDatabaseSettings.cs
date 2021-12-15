using DevelopersChallengeNIBO.Interfaces.Database;

namespace DevelopersChallengeNIBO.Models.Database
{
    public class OFXRecordsDatabaseSettings : IOFXRecordsDBSettings
    {
        public string OFXRecordsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
