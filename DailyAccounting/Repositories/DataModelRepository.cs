using DailyAccounting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyAccounting.Repositories
{
    internal class DataModelRepository : IDataModelRepository
    {
        public List<string> GetCategoryDataSource()
        {
            return DataModel.Category;
        }

        public List<string> GetMemberDataSource()
        {
            return DataModel.Member;
        }

        public List<string> GetPayWayDataSource()
        {
            return DataModel.PayWay;
        }

        public List<string> GetPurposeDataSource(string categoryString)
        {
            return DataModel.Purpose[categoryString];
        }
    }
}
