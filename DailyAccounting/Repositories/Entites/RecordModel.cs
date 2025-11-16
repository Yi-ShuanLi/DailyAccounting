using DailyAccounting.Attributes;
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
        [ComboBoxColumn]
        public string Category { get; set; }

        [DisplayName("目的")]
        [ComboBoxColumn]
        public string Purpose { get; set; }

        [DisplayName("付款方式")]
        [ComboBoxColumn]
        public string PayWay { get; set; }

        [DisplayName("對象")]
        [ComboBoxColumn]
        public string Member { get; set; }

        [DisplayName("圖片1")]
        [ImageColumn]
        public string ImageURL1 { get; set; }

        [DisplayName("圖片2")]
        [ImageColumn]
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
