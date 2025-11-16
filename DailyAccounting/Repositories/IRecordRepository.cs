using DailyAccounting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyAccounting.Repositories
{
    internal interface IRecordRepository
    {
        void CreateRecord(RecordModel recordModel);
        void CreateListRecord(List<RecordModel> recordModels);
        List<RecordModel> GetRecords(DateTime date);
        List<RecordModel> GetRecords(DateTime start, DateTime end);

        void DeletRecord(RecordModel recordModel);
        void UpdateRecord(RecordModel recordModel);

    }
}
