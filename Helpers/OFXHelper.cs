using DevelopersChallengeNIBO.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DevelopersChallengeNIBO.Helpers
{
    public class OFXHelper
    {
        public static List<OFXRecord> GetOFXObjects(string OFXFilePath)
        {
            XElement doc = ToXElement(OFXFilePath);

            return (from c in doc.Descendants("STMTTRN")
                    where c.Element("TRNTYPE").Value == "DEBIT" || c.Element("TRNTYPE").Value == "CREDIT"
                    select new OFXRecord
                    {
                        Amount = double.Parse(c.Element("TRNAMT").Value.Replace("-", ""),
                                               NumberFormatInfo.InvariantInfo),
                        Date = DateTime.ParseExact(c.Element("DTPOSTED").Value,
                                                   "yyyyMMdd", null),
                        Memo = c.Element("MEMO").Value,
                        TransactionType = c.Element("TRNTYPE").Value
                    }).ToList();

        }

        public static List<OFXRecord> Merge(List<OFXRecord> recordsDB, List<OFXRecord> recordsUpload)
        {
            List<OFXRecord> merged = new List<OFXRecord>();

            // If db is empty adds imported records if not iterates uploaded list and checks for duplicates
            if (recordsDB.Count == 0)
                merged.AddRange(recordsUpload);
            else 
                foreach (OFXRecord record in recordsUpload)
                {
                    if (!recordsDB.Where(x => x.Amount == record.Amount && x.Date.ToShortDateString() == record.Date.ToShortDateString() && x.Memo == record.Memo).Any())
                        merged.Add(record);
                }

            return merged;
        }

        public static XElement ToXElement(string pathToOfxFile)
        {
            var tags = from line in File.ReadAllLines(pathToOfxFile)
                       where line.Contains("<STMTTRN>") ||
                       line.Contains("<TRNTYPE>") ||
                       line.Contains("<DTPOSTED>") ||
                       line.Contains("<TRNAMT>") ||
                       line.Contains("<FITID>") ||
                       line.Contains("<CHECKNUM>") ||
                       line.Contains("<MEMO>")
                       select line;


            XElement el = new XElement("root");
            XElement son = null;

            foreach (var l in tags)
            {
                if (l.IndexOf("<STMTTRN>") != -1)
                {
                    son = new XElement("STMTTRN");
                    el.Add(son);
                    continue;
                }

                var tagName = getTagName(l);
                var elSon = new XElement(tagName);
                elSon.Value = getTagValue(l);
                son.Add(elSon);
            }

            return el;
        }
        private static string getTagName(string line)
        {
            int pos_init = line.IndexOf("<") + 1;
            int pos_end = line.IndexOf(">");
            pos_end = pos_end - pos_init;
            return line.Substring(pos_init, pos_end);
        }

        private static string getTagValue(string line)
        {
            int pos_init = line.IndexOf(">") + 1;
            string retValue = line.Substring(pos_init).Trim();
            if (retValue.IndexOf("[") != -1)
            {
                //date--lets get only the 8 date digits
                retValue = retValue.Substring(0, 8);
            }
            return retValue;
        }
    }
}
