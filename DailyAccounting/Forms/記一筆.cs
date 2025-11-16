using CSV;
using DailyAccounting.Extensions;
using DailyAccounting.Models;
using DailyAccounting.Models.DTOs;
using DailyAccounting.Presenters;
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
using static DailyAccounting.Contract.AddARecordContract;

namespace DailyAccounting.Forms
{
    public partial class 記一筆 : Form, IAddRecordView
    {
        RecordPresenter recordPresenter;
        public 記一筆()
        {
            InitializeComponent();

            recordPresenter = new RecordPresenter(this);
            recordPresenter.GetComboBoxDataRequest();

            pictureBox1.Image = Image.FromFile("Images\\上傳示意圖2.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Image = Image.FromFile("Images\\上傳示意圖2.png");
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
                pictureBox.Image.Dispose();
                pictureBox.Image = null;
                GC.Collect();
                var t = (Bitmap)Image.FromFile(openFileDialog.FileName);
                pictureBox.Image = ImageCompress.Compress(t);
            }
        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            recordPresenter.ChangeCategoryRequest(comboBoxCategory.Text);
        }
        public void ChangeCategoryResponse(List<string> purpose)
        {
            comboBoxPurpose.DataSource = purpose;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DebounceTime(() =>
            {
                string day = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                string amount = textBoxAmount.Text;
                string category = comboBoxCategory.Text;
                string purpose = comboBoxPurpose.Text;
                string payWay = comboBoxPayWay.Text;
                string member = comboBoxMember.Text;

                RecordModelDTO recordModelDTO = new RecordModelDTO(day, amount, category, purpose, payWay, member, (Bitmap)pictureBox1.Image, (Bitmap)pictureBox2.Image);
                recordPresenter.SaveRecord(recordModelDTO);

                pictureBox1.Image = Image.FromFile("Images\\上傳示意圖2.png");
                pictureBox2.Image = Image.FromFile("Images\\上傳示意圖2.png");

                //Streamming => 串流，多媒體影音串流平台: Youtube/Netflix/KKBox ..etc
                //檔案下載演進史:
                //固定將軟體下載到電腦裡面，必須完整下載才能使用
                //續傳技術:將檔案分割成小檔案，每一次都只傳輸部分檔案(分割檔)，就可以支援今天沒下載完的隔天再開啟電腦重新聯網下載
                //串流:可以允許支援編播邊下載，代表軟體:迅雷影音 => 邊播放邊下載同時支援續傳軟體
                //P2P => 點對點傳輸 => 任意兩台電腦可以不透過伺服器交涉，直接進行對傳連線
                //m3u8
                //buffer => 一個容器，一端會不斷放入二進位資料，另一端會取出二進位資料
            });


        }

        void IAddRecordView.GetComboBoxDataResponse(DataModelDTO dataModelDTO)
        {
            comboBoxCategory.DataSource = dataModelDTO.Category;
            comboBoxCategory.SelectedIndex = 0;
            comboBoxPurpose.DataSource = dataModelDTO.Purpose;
            comboBoxPayWay.DataSource = dataModelDTO.PayWay;
            comboBoxMember.DataSource = dataModelDTO.Member;
        }


    }
}
