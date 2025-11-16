using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyAccounting.Models.DTOs
{
    internal class DataModelDTO
    {
        public List<string> Category;
        public List<string> Purpose;
        public List<string> PayWay;
        public List<string> Member;
        public DataModelDTO(List<string> category, List<string> purpose, List<string> payWay, List<string> member)
        {
            this.Category = category;
            this.Purpose = purpose;
            this.PayWay = payWay;
            this.Member = member;
        }
        public DataModelDTO()
        {

        }
    }
}
