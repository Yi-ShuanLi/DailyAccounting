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
    public partial class 記帳本 : Form
    {
        List<RecordModel> datas = null;
        public 記帳本()
        {
            InitializeComponent();
            dataGridView1.CurrentCellDirtyStateChanged += new EventHandler(dataGridView1_CurrentCellDirtyStateChanged);

        }
        void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            datas = CSVHelper.Read<RecordModel>("D:\\c#_Leo老師\\記帳資料\\record.csv");
            ShowTable();
        }
        private void ShowTable()
        {
            dataGridView1.DataSource = datas;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //DataGridView組成:
            //1.先將list 的Model 進行反射，取得所有Props
            //2.根據所有的Prop 去創建 DataGridViewColumn(父類別) => 創建DataGridViewTextboxColumn
            //3.跑迴圈對list當中的每一筆資料，創建 DataGridViewRow = datagridview.Rows[4]
            //4.在創建Row的時候，每一個Row都會有N個Cell,Cell的數量相等於Columns的數量 => datagridviewTextboxCell
            //5.在透過反射的GetValue去取得Rows[i] 資料的每一格，去填充 => datagridview.Rows[1].Cell[3].Value = XXXX

            //dataGridView1.Columns[0].Visible = false;
            //dataGridView1.Columns.Insert(0, new DataGridViewTextBoxColumn()
            //{
            //    HeaderText = "日期"
            //});
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;

            DataGridViewImageColumn imageColumn3 = new DataGridViewImageColumn();
            imageColumn3.Name = "Trash_Image"; // Set a name for the column
            imageColumn3.HeaderText = "刪除"; // Set the header text
            imageColumn3.ImageLayout = DataGridViewImageCellLayout.Zoom; // Optional: Adjust image layout (Zoom, Stretch, Normal, etc.)
            dataGridView1.Columns.Add(imageColumn3);// index 13

            DataGridViewImageColumn imageColumn2 = new DataGridViewImageColumn();
            imageColumn2.Name = "ImageURL2_Image"; // Set a name for the column
            imageColumn2.HeaderText = "圖片2"; // Set the header text
            imageColumn2.ImageLayout = DataGridViewImageCellLayout.Zoom; // Optional: Adjust image layout (Zoom, Stretch, Normal, etc.)
            dataGridView1.Columns.Insert(7, imageColumn2);// index 13

            DataGridViewImageColumn imageColumn1 = new DataGridViewImageColumn();
            imageColumn1.Name = "ImageURL1_Image"; // Set a name for the column
            imageColumn1.HeaderText = "圖片1"; // Set the header text
            imageColumn1.ImageLayout = DataGridViewImageCellLayout.Zoom; // Optional: Adjust image layout (Zoom, Stretch, Normal, etc.)
            dataGridView1.Columns.Insert(6, imageColumn1);// index 13

            DataGridViewComboBoxColumn combo1 = new DataGridViewComboBoxColumn();
            combo1.Name = "Member_Combo";
            combo1.HeaderText = "對象";
            combo1.DataSource = DataModel.Member;
            dataGridView1.Columns.Insert(5, combo1);
            DataGridViewComboBoxColumn combo2 = new DataGridViewComboBoxColumn();
            combo2.Name = "PayWay_Combo";
            combo2.HeaderText = "付款方式";
            combo2.DataSource = DataModel.PayWay;
            dataGridView1.Columns.Insert(4, combo2);
            DataGridViewComboBoxColumn combo3 = new DataGridViewComboBoxColumn();
            combo3.Name = "Purpose_Combo";
            combo3.HeaderText = "目的";
            dataGridView1.Columns.Insert(3, combo3);
            DataGridViewComboBoxColumn combo4 = new DataGridViewComboBoxColumn();
            combo4.Name = "Category_Combo";
            combo4.HeaderText = "類別";
            combo4.DataSource = DataModel.Category;
            dataGridView1.Columns.Insert(2, combo4);



            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[2].Value = dataGridView1.Rows[i].Cells[3].Value;
                string categoryString = dataGridView1.Rows[i].Cells[3].Value.ToString();
                DataGridViewComboBoxCell comboPurpose = (DataGridViewComboBoxCell)dataGridView1.Rows[i].Cells[4];
                comboPurpose.DataSource = DataModel.CategoryAndPurpose[categoryString];
                comboPurpose.Value = dataGridView1.Rows[i].Cells[5].Value;
                dataGridView1.Rows[i].Cells[4].Value = dataGridView1.Rows[i].Cells[5].Value;
                dataGridView1.Rows[i].Cells[6].Value = dataGridView1.Rows[i].Cells[7].Value;
                dataGridView1.Rows[i].Cells[8].Value = dataGridView1.Rows[i].Cells[9].Value;
                string img10 = dataGridView1.Rows[i].Cells[11].Value.ToString();
                dataGridView1.Rows[i].Cells[10].Value = new Bitmap(img10);
                string img12 = dataGridView1.Rows[i].Cells[13].Value.ToString();
                dataGridView1.Rows[i].Cells[12].Value = new Bitmap(img12);
                dataGridView1.Rows[i].Cells[14].Value = new Bitmap("D:\\c#_Leo老師\\DailyAccounting\\垃圾桶icon.png");
            }
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);

        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //Type type = e.GetType();
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            //Console.WriteLine($"第:{rowIndex}列，第{columnIndex}欄");
            if (dataGridView1.Rows[rowIndex].Cells[columnIndex].Value == null)
            {
                dataGridView1.Rows[rowIndex].Cells[columnIndex].Value = "0";
                return;
            }

            //Console.WriteLine(dataGridView1.Rows[rowIndex].Cells[columnIndex].Value.ToString());

            if (columnIndex == 2)
            {
                string categoryString = dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();
                DataGridViewComboBoxCell comboPurpose = (DataGridViewComboBoxCell)dataGridView1.Rows[rowIndex].Cells[4];
                comboPurpose.DataSource = DataModel.CategoryAndPurpose[categoryString];
                comboPurpose.Value = DataModel.CategoryAndPurpose[categoryString][0];
            }
            if (columnIndex == 2)
                datas[rowIndex].Category = dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();
            if (columnIndex == 4)
                datas[rowIndex].Purpose = dataGridView1.Rows[rowIndex].Cells[4].Value.ToString();
            if (columnIndex == 6)
                datas[rowIndex].PayWay = dataGridView1.Rows[rowIndex].Cells[6].Value.ToString();
            if (columnIndex == 8)
                datas[rowIndex].Member = dataGridView1.Rows[rowIndex].Cells[8].Value.ToString();


            File.Delete("D:\\c#_Leo老師\\記帳資料\\record.csv");
            CSVHelper.Write<RecordModel>("D:\\c#_Leo老師\\記帳資料\\record.csv", datas);



        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            if (columnIndex == 10 || columnIndex == 12)
            {
                columnIndex++;
                string filePath = dataGridView1.Rows[rowIndex].Cells[columnIndex].Value.ToString();
                ImageForm imageForm = new ImageForm(filePath);
                imageForm.ShowDialog();
            }
            else if (columnIndex == 14)
            {
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                datas.RemoveAt(rowIndex);
                ShowTable();
                File.Delete("D:\\c#_Leo老師\\記帳資料\\record.csv");
                CSVHelper.Write<RecordModel>("D:\\c#_Leo老師\\記帳資料\\record.csv", datas);
            }



        }
    }
}
