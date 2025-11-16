using CSV;
using DailyAccounting.Models;
using DailyAccounting.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DailyAccounting.Repositories
{
    internal class RecordOneRepository : IRecordRepository
    {
        String recordPath = ConfigurationManager.AppSettings["CSVPath"].ToString();
        public void CreateRecord(RecordModel recordModel)
        {
            string day = recordModel.Day;
            string filePath = Path.Combine(recordPath, day, "record.csv");
            CSVHelper.Write<RecordModel>(filePath, recordModel);
        }
        public void CreateListRecord(List<RecordModel> recordModels)
        {
            string day = recordModels[0].Day;
            string filePath = Path.Combine(recordPath, day, "record.csv");
            CSVHelper.Write<RecordModel>(filePath, recordModels);
        }

        public void DeletRecord(RecordModel recordModel)
        {
            string image1URL = recordModel.ImageURL1;
            string image2URL = recordModel.ImageURL2;
            string newImage1URL = image1URL.Replace("40x40_", "50L_");
            string newImage2URL = image2URL.Replace("40x40_", "50L_");
            List<RecordModel> recordModelSameDay = this.GetRecords(DateTime.Parse(recordModel.Day));
            if (recordModelSameDay.Count == 1)
            {
                Directory.Delete(Path.Combine(recordPath, recordModel.Day), true);
            }
            else
            {
                RecordModel record = recordModelSameDay.FirstOrDefault(x => x.ImageURL1.Equals(image1URL));
                recordModelSameDay.Remove(record);
                File.Delete(image1URL);
                File.Delete(newImage1URL);
                File.Delete(image2URL);
                File.Delete(newImage2URL);
                File.Delete(Path.Combine(recordPath, recordModel.Day, "record.csv"));
                this.CreateListRecord(recordModelSameDay);
            }
        }

        public void UpdateRecord(RecordModel recordModel)
        {
            List<RecordModel> recordModelSameDay = this.GetRecords(DateTime.Parse(recordModel.Day));
            int index1 = recordModelSameDay.Select((x, index) => new { x, index }).FirstOrDefault(y => y.x.ImageURL1.Equals(recordModel.ImageURL1))?.index ?? -1;
            if (index1 == -1)
                return;
            RecordModel recordUpdate = recordModelSameDay.FirstOrDefault(x => x.ImageURL1.Equals(recordModel.ImageURL1));
            recordModelSameDay.Insert(index1, recordModel);
            recordModelSameDay.Remove(recordUpdate);
            File.Delete(Path.Combine(recordPath, $"{recordModel.Day}", "record.csv"));
            this.CreateListRecord(recordModelSameDay);
        }

        public List<RecordModel> GetRecords(DateTime date)
        {
            string dayFile = Path.Combine(recordPath, $"{date.ToString("yyyy-MM-dd")}\\record.csv");
            return CSVHelper.Read<RecordModel>(dayFile);
        }

        public List<RecordModel> GetRecords(DateTime start, DateTime end)
        {
            List<RecordModel> recordModels = new List<RecordModel>();
            TimeSpan diff = end - start;
            for (int i = 0; i <= diff.Days; i++)
            {
                string day = start.AddDays(i).ToString("yyyy-MM-dd");
                string dayFile = Path.Combine(recordPath, $"{day}\\record.csv");
                if (!File.Exists(dayFile))
                {
                    continue;
                }
                recordModels.AddRange(this.GetRecords(DateTime.Parse(day)));
            }
            return recordModels;
        }
    }
}
