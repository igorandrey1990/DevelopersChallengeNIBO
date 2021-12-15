using DevelopersChallengeNIBO.Models;
using System.Collections.Generic;

namespace DevelopersChallengeNIBO.Interfaces.Services
{
    public interface IOFXRecordsService
    {
        public List<OFXRecord> Get();

        public OFXRecord Get(string id);

        public OFXRecord Create(OFXRecord OFXREcord);

        public void Update(string id, OFXRecord OFXREcord);

        public void Remove(OFXRecord OFXRecord);

        public void Remove(string id);

        public void DeleteAll();

        public void ImportOFX(string OFXFilepath);
    }
}
