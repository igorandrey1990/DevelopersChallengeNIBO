using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DevelopersChallengeNIBO.Models
{
    public class OFXRecord : IEquatable<OFXRecord>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Memo { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string TransactionType { get; set; }

        public override int GetHashCode()
        {
            return (Memo == null ? 0 : Memo.GetHashCode()) ^ Amount.GetHashCode();
        }
        public bool Equals(OFXRecord record)
        {
            return Memo.Equals(record.Memo) && Amount.Equals(record.Amount) && Date.Equals(record.Date);
        }
    }
}
