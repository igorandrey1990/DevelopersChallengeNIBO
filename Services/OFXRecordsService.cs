using DevelopersChallengeNIBO.Helpers;
using DevelopersChallengeNIBO.Interfaces.Database;
using DevelopersChallengeNIBO.Interfaces.Services;
using DevelopersChallengeNIBO.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace DevelopersChallengeNIBO.Services
{
    public class OFXRecordsService : IOFXRecordsService
    {
        private readonly IMongoCollection<OFXRecord> _OFXRecords;

        public OFXRecordsService(IOFXRecordsDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _OFXRecords = database.GetCollection<OFXRecord>(settings.OFXRecordsCollectionName);
        }

        public List<OFXRecord> Get()
        {
            return _OFXRecords.Find(OFXREcord => true).ToList();
        }

        public OFXRecord Get(string id)
        {
            return _OFXRecords.Find<OFXRecord>(OFXREcord => OFXREcord.Id == id).FirstOrDefault();
        }

        public OFXRecord Create(OFXRecord OFXREcord)
        {
            _OFXRecords.InsertOne(OFXREcord);
            return OFXREcord;
        }

        public void Import(List<OFXRecord> records)
        {
            _OFXRecords.InsertMany(records);
        }

        public void Update(string id, OFXRecord OFXREcord)
        {
            _OFXRecords.ReplaceOne(book => book.Id == id, OFXREcord);
        }

        public void Remove(OFXRecord OFXRecord)
        {
            _OFXRecords.DeleteOne(book => book.Id == OFXRecord.Id);
        }

        public void DeleteAll()
        {
            _OFXRecords.DeleteMany(OFXRecord => true);
        }

        public void Remove(string id)
        {
            _OFXRecords.DeleteOne(OFXRecord => OFXRecord.Id == id);
        }

        public void ImportOFX(string OFXFilepath)
        {
            // Loads boths lists for comparison.
            List<OFXRecord> records = Get();
            List<OFXRecord> parsedRecords = OFXHelper.GetOFXObjects(OFXFilepath);

            // Calls method to merge lists and checks for duplicates in DB
            List<OFXRecord> mergedRecords = OFXHelper.Merge(records, parsedRecords);

            // Inserts merged records to the db
            if (mergedRecords.Count > 0)
                Import(mergedRecords);
        }
    }
}
