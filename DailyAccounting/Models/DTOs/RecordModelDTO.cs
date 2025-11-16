using DailyAccounting.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyAccounting.Models.DTOs
{
    internal class RecordModelDTO
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
        public Bitmap BitmapImage1 { get; set; }

        [DisplayName("圖片2")]
        [ImageColumn]
        public Bitmap BitmapImage2 { get; set; }

        public RecordModelDTO(string day, string amount, string category, string purpose, string payWay, string member, Bitmap bitmapImage1, Bitmap bitmapImage2)
        {
            Day = day;
            Amount = amount;
            Category = category;
            Purpose = purpose;
            PayWay = payWay;
            Member = member;
            BitmapImage1 = bitmapImage1;
            BitmapImage2 = bitmapImage2;
        }
        public RecordModelDTO()
        {

        }
    }
}
