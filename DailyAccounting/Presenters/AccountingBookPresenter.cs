using CSV;
using DailyAccounting.Models;
using DailyAccounting.Models.DTOs;
using DailyAccounting.Repositories;
using DailyAccounting.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DailyAccounting.Contract.AccountingBookContract;

namespace DailyAccounting.Presenters
{
    internal class AccountingBookPresenter : IAccountingBookPresenter
    {
        IAccountingBookView accountingBookView;
        IRecordRepository recordOneRepository;
        public AccountingBookPresenter(IAccountingBookView view)
        {
            this.accountingBookView = view;
            recordOneRepository = new RecordOneRepository();
        }


        public void SearchRecordsByDateRange(DateTime startDate, DateTime endDate)
        {
            List<RecordModel> recordModels = recordOneRepository.GetRecords(startDate, endDate);
            accountingBookView.GetRecordsResponse(Mapper.Map<RecordModel, RecordViewModel>(recordModels));
        }
        public void RemoveRecord(RecordViewModel recordViewModel)
        {
            recordOneRepository.DeletRecord(Mapper.Map<RecordViewModel, RecordModel>(recordViewModel));
        }

        public void EditRecords(RecordViewModel recordViewModel)
        {
            recordOneRepository.UpdateRecord(Mapper.Map<RecordViewModel, RecordModel>(recordViewModel));
        }
    }
}
