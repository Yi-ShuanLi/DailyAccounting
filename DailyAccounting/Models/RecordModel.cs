using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DailyAccounting.Models
{
    internal class RecordModel
    {
        [DisplayName("日期")]
        public string Day { get; set; }
        [DisplayName("金額")]
        public string Amount { get; set; }
        [DisplayName("類別")]
        public string Category { get; set; }
        [DisplayName("目的")]
        public string Purpose { get; set; }
        [DisplayName("付款方式")]
        public string PayWay { get; set; }
        [DisplayName("對象")]
        public string Member { get; set; }
        [DisplayName("圖片1")]
        public string ImageURL1 { get; set; }
        [DisplayName("圖片2")]
        public string ImageURL2 { get; set; }

        public RecordModel(string day, string amount, string category, string purpose, string payWay, string member, string imageURL1, string imageURL2)
        {
            Day = day;
            Amount = amount;
            Category = category;
            Purpose = purpose;
            PayWay = payWay;
            Member = member;
            ImageURL1 = imageURL1;
            ImageURL2 = imageURL2;
        }
        public RecordModel()
        {

        }
    }
}
