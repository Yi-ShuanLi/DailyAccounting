using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyAccounting.Models
{
    internal class DataModel
    {

        public static List<string> Category = new List<string>() { "飲食", "日常用品", "交通", "水電瓦斯", "電話網路", "服飾", "機車", "娛樂", "美容美髮", "交際", "學習", "保險", "稅金", "醫療保健", "教育", "轉帳手續費" };
        public static List<string> PurposeOnFood = new List<string>() { "早餐", "中餐", "下午茶", "晚餐", "消夜" };
        public static List<string> PurposeOnGroceries = new List<string>() { "衛生紙", "衛生棉", "牙膏", "牙刷" };
        public static List<string> PuroseOnTransportation = new List<string>() { "火車", "客運", "計程車", "捷運" };
        public static List<string> PurposeOnUtility = new List<string>() { "水費", "電費", "天然氣費" };
        public static List<string> PurposeOnTelephoneAndNetWork = new List<string>() { "電信費_家用", "網路費", "手機月租費" };
        public static List<string> PurposeOnClothing = new List<string>() { "衣服", "褲子", "襪子", "內衣", "內褲", "帽子", "外套", "裙子", "襯衫", "西裝褲", "西裝外套" };
        public static List<string> PurposeOnMotorcycle = new List<string>() { "加油", "停車費", "保養", "機油", "零件更換", "車險" };
        public static List<string> PurposeOnEntertainment = new List<string>() { "電影票", "演唱會", "KTV", "遊戲", "旅遊", "桌遊", "運動" };
        public static List<string> PurposeOnBeauty = new List<string>() { "剪髮", "染髮", "燙髮", "美甲", "美容", "按摩", "護膚" };
        public static List<string> PurposeOnSocial = new List<string>() { "聚餐", "送禮", "婚禮紅包", "喪禮奠儀", "節慶禮品" };
        public static List<string> PurposeOnLearning = new List<string>() { "書籍", "線上課程", "補習班", "教材", "研討會" };
        public static List<string> PurposeOnInsurance = new List<string>() { "醫療險", "壽險", "車險", "旅平險", "意外險" };
        public static List<string> PurposeOnTax = new List<string>() { "所得稅", "房屋稅", "地價稅", "牌照稅", "營業稅" };
        public static List<string> PurposeOnMedical = new List<string>() { "掛號費", "藥品", "健檢", "醫療器材", "住院費" };
        public static List<string> PurposeOnEducation = new List<string>() { "學費", "學雜費", "教材費", "社團費", "校外活動" };
        public static List<string> PurposeOnTransferFee = new List<string>() { "跨行轉帳手續費", "匯款手續費", "國際轉帳費" };
        public static Dictionary<string, List<string>> CategoryAndPurpose = new Dictionary<string, List<string>>() {
            {"飲食",PurposeOnFood },
            {"日常用品",PurposeOnGroceries },
            {"交通",PuroseOnTransportation },
            {"水電瓦斯",PurposeOnUtility},
            {"電話網路", PurposeOnTelephoneAndNetWork},
            {"服飾", PurposeOnClothing},
            {"機車", PurposeOnMotorcycle},
            {"娛樂", PurposeOnEntertainment},
            {"美容美髮", PurposeOnBeauty},
            {"交際",PurposeOnSocial },
            { "學習",PurposeOnLearning },
            {"保險", PurposeOnInsurance},
            {"稅金",PurposeOnTax },
            {"醫療保健",PurposeOnMedical },
            {"教育",PurposeOnEducation },
            {"轉帳手續費",PurposeOnTransferFee }
        };
        public static List<string> PayWay = new List<string>() { "現金", "銀行帳戶", "信用卡" };
        public static List<string> Member = new List<string>() { "自己", "家人", "朋友" };

    }

}
