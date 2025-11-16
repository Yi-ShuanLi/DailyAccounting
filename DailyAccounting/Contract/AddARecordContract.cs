using DailyAccounting.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyAccounting.Contract
{
    internal class AddARecordContract
    {
        public interface IAddRecordView
        {
            void GetComboBoxDataResponse(DataModelDTO dataModelDTO);
            void ChangeCategoryResponse(List<string> purpose);
        }
        public interface IAddRecordPresenter
        {
            void GetComboBoxDataRequest();
            void ChangeCategoryRequest(string categoryName);
            void SaveRecord(RecordModelDTO recordModelDTO);
        }
    }
}
