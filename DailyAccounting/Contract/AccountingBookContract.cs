using DailyAccounting.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DailyAccounting.Contract
{
    internal class AccountingBookContract
    {
        public interface IAccountingBookView
        {
            void GetRecordsResponse(List<RecordViewModel> recordViewModel);
        }
        public interface IAccountingBookPresenter
        {
            void SearchRecordsByDateRange(DateTime startDate, DateTime endDate);
            void RemoveRecord(RecordViewModel recordViewModel);
            void EditRecords(RecordViewModel recordViewModel);

        }
    }
}
