using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyAccounting.Repositories
{
    internal interface IDataModelRepository
    {
        List<string> GetCategoryDataSource();
        List<string> GetPurposeDataSource(string categoryString);
        List<string> GetPayWayDataSource();
        List<string> GetMemberDataSource();
    }
}
