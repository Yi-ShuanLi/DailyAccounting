using CSV;
using DailyAccounting.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DailyAccounting.Forms
{
    public partial class 記一筆 : Form
    {
        public 記一筆()
        {
            InitializeComponent();
            comboBoxCategory.DataSource = DataModel.Category;
            comboBoxCategory.SelectedIndex = 0;
            comboBoxPurpose.DataSource = DataModel.CategoryAndPurpose[DataModel.Category[0].ToString()];
            comboBoxPayWay.DataSource = DataModel.PayWay;
            comboBoxMember.DataSource = DataModel.Member;
            pictureBox1.Image = Image.FromFile("D:\\c#_Leo老師\\DailyAccounting\\上傳示意圖2.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Image = Image.FromFile("D:\\c#_Leo老師\\DailyAccounting\\上傳示意圖2.png");
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void 記一筆_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "圖片檔|*.png;*jpg";
            //兩個enum互相比對，左邊是打開視窗後按下確定或取消後回傳enum，右邊是enum的表達方法
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBoxCategory.SelectedIndex;
            comboBoxPurpose.DataSource = DataModel.CategoryAndPurpose[DataModel.Category[index].ToString()];
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string day = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string amount = textBoxAmount.Text;
            string category = comboBoxCategory.Text;
            string purpose = comboBoxPurpose.Text;
            string payWay = comboBoxPayWay.Text;
            string member = comboBoxMember.Text;
            string imageURL1 = $"D:\\c#_Leo老師\\記帳資料\\Pictures\\{Guid.NewGuid()}.png";
            string imageURL2 = $"D:\\c#_Leo老師\\記帳資料\\Pictures\\{Guid.NewGuid()}.png";
            pictureBox1.Image.Save(imageURL1);
            pictureBox2.Image.Save(imageURL2);
            RecordModel recordModel = new RecordModel(day, amount, category, purpose, payWay, member, imageURL1, imageURL2);
            CSVHelper.Write<RecordModel>("D:\\c#_Leo老師\\記帳資料\\record.csv", recordModel);


            //StreamWriter writer = new StreamWriter("D:\\c#_Leo老師\\記帳資料\\record.csv", true, Encoding.UTF8);
            //string line = $"{day},{amount},{category},{purpose},{payWay},{member},{imageURL1},{imageURL2}";
            //writer.WriteLine(line); // 寫入該行資料
            //writer.Flush();
            //writer.Close();

            //Streamming => 串流，多媒體影音串流平台: Youtube/Netflix/KKBox ..etc
            //檔案下載演進史:
            //固定將軟體下載到電腦裡面，必須完整下載才能使用
            //續傳技術:將檔案分割成小檔案，每一次都只傳輸部分檔案(分割檔)，就可以支援今天沒下載完的隔天再開啟電腦重新聯網下載
            //串流:可以允許支援編播邊下載，代表軟體:迅雷影音 => 邊播放邊下載同時支援續傳軟體
            //P2P => 點對點傳輸 => 任意兩台電腦可以不透過伺服器交涉，直接進行對傳連線
            //m3u8
            //buffer => 一個容器，一端會不斷放入二進位資料，另一端會取出二進位資料
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filePath = "D:\\c#_Leo老師\\記帳資料\\record.csv";
            StreamReader reader = new StreamReader(filePath, Encoding.UTF8);
            List<RecordModel> recordModels = new List<RecordModel>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] fields = line.Split(',');
                RecordModel recordModel = new RecordModel(fields[0], fields[1], fields[2], fields[3], fields[4], fields[5], fields[6], fields[7]);
                recordModels.Add(recordModel);
                Console.WriteLine(fields[0]);
            }
            foreach (RecordModel recordModel in recordModels)
            {
                //Console.Write(recordModel.DateTime + " ");
                Console.Write(recordModel.Amount + " ");
                Console.Write(recordModel.Category + " ");
                Console.Write(recordModel.Purpose + " ");
                Console.Write(recordModel.PayWay + " ");
                Console.Write(recordModel.Member + " ");
                Console.Write(recordModel.ImageURL1 + " ");
                Console.Write(recordModel.ImageURL2 + " ");
                Console.WriteLine();
            }



        }
    }
}
