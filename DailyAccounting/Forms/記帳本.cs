using CSV;
using DailyAccounting.Attributes;
using DailyAccounting.Extensions;
using DailyAccounting.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

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
            this.DebounceTime(() =>
            {
                datas = new List<RecordModel>();
                if (dateTimePickerEnd.Value.Date < dateTimePickerStart.Value.Date)
                {
                    MessageBox.Show("結束日期必須大於等於開始日期!!!");
                    return;
                }
                TimeSpan diff = dateTimePickerEnd.Value - dateTimePickerStart.Value;
                for (int i = 0; i < diff.Days; i++)
                {
                    string day = dateTimePickerStart.Value.AddDays(i).ToString("yyyy-MM-dd");
                    string dayFile = $"D:\\c#_Leo老師\\記帳資料\\{day}\\record.csv";
                    if (!File.Exists(dayFile))
                    {
                        continue;
                    }
                    datas.AddRange(CSVHelper.Read<RecordModel>(dayFile));
                }
                ShowTable();
            });

        }

        private void ShowTable()
        {
            dataGridView1.Rows.OfType<DataGridViewRow>().ToList().ForEach(row => { row.Cells.OfType<DataGridViewImageCell>().Select(cell => (Bitmap)cell.Value).ToList().ForEach(image => { image?.Dispose(); }); });
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            GC.Collect();
            dataGridView1.DataSource = datas;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns["Day"].ReadOnly = true;
            //DataGridView組成:
            //1.先將list 的Model 進行反射，取得所有Props
            //2.根據所有的Prop 去創建 DataGridViewColumn(父類別) => 創建DataGridViewTextboxColumn
            //3.跑迴圈對list當中的每一筆資料，創建 DataGridViewRow = datagridview.Rows[4]
            //4.在創建Row的時候，每一個Row都會有N個Cell,Cell的數量相等於Columns的數量 => datagridviewTextboxCell
            //5.在透過反射的GetValue去取得Rows[i] 資料的每一格，去填充 => datagridview.Rows[1].Cell[3].Value = XXXX


            var props = typeof(RecordModel).GetProperties();

            foreach (var prop in props)
            {
                string headerText = prop.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                var attrs = prop.GetCustomAttributes();
                if (attrs.Count() < 2)
                    continue;
                dataGridView1.Columns[prop.Name].Visible = false;
                this.dataGridView1.CreateComboBoxColumns(prop);
                this.dataGridView1.CreateImageColumns(prop);

            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCellCollection dataGridViewCellCollection = dataGridView1.Rows[i].Cells;
                dataGridViewCellCollection.SetComboBoxCells();
                dataGridViewCellCollection.SetImageCells();

                //為了避免直接讀取檔案路徑而造成圖檔鎖定(所以其他檔案無法存取甚至是刪除)
                //所以先透過讀取檔案並轉成二進位(副本)
                //並將副本(Buffer) 存放到記憶體串流中
                //這樣Bitmap就可以從串流中將檔案讀取出來轉乘Bitmap
            }

            this.dataGridView1.CreateImageColumns("刪除", "Trash_Image", "D:\\c#_Leo老師\\DailyAccounting\\垃圾桶icon.png");
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);

        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            if (dataGridView1.Rows[rowIndex].Cells[columnIndex].Value == null)
            {
                dataGridView1.Rows[rowIndex].Cells[columnIndex].Value = "0";
                return;
            }
            string day = dataGridView1.Rows[rowIndex].Cells["Day"].Value.ToString();
            List<RecordModel> recordModelSameDay = datas.Where(x => x.Day.Equals(day)).ToList();
            File.Delete($"D:\\c#_Leo老師\\記帳資料\\{day}\\record.csv");
            CSVHelper.Write<RecordModel>($"D:\\c#_Leo老師\\記帳資料\\{day}\\record.csv", recordModelSameDay);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            if (dataGridView1.Rows[rowIndex].Cells[columnIndex] is DataGridViewImageCell imageCell)
            {
                string columnName = imageCell.OwningColumn.Name.ToString();
                if (columnName.Contains("Trash"))
                {
                    string day = dataGridView1.Rows[rowIndex].Cells["Day"].Value.ToString();
                    string image1URL = dataGridView1.Rows[rowIndex].Cells["ImageURL1"].Value.ToString();
                    string image2URL = dataGridView1.Rows[rowIndex].Cells["ImageURL2"].Value.ToString();
                    string newImage1URL = image1URL.Replace("40x40_", "50L_");
                    string newImage2URL = image2URL.Replace("40x40_", "50L_");
                    datas.RemoveAt(rowIndex);
                    List<RecordModel> recordModelSameDay = datas.Where(x => x.Day.Equals(day)).ToList();
                    if (recordModelSameDay.Count == 0)
                    {
                        Directory.Delete($"D:\\c#_Leo老師\\記帳資料\\{day}", true);
                    }
                    else
                    {
                        File.Delete(image1URL);
                        File.Delete(newImage1URL);
                        File.Delete(image2URL);
                        File.Delete(newImage2URL);
                        File.Delete($"D:\\c#_Leo老師\\記帳資料\\{day}\\record.csv");
                        CSVHelper.Write<RecordModel>($"D:\\c#_Leo老師\\記帳資料\\{day}\\record.csv", recordModelSameDay);
                    }
                    ShowTable();
                }
                else
                {
                    string referenceColumn = imageCell.OwningColumn.Tag.ToString();
                    string filePath = dataGridView1.Rows[rowIndex].Cells[referenceColumn].Value.ToString();
                    string newImageURL = filePath.Replace("40x40_", "50L_");
                    ImageForm imageForm = new ImageForm(newImageURL);
                    imageForm.ShowDialog();
                }


            }

        }

        private void dateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {
            dateTimePickerEnd.MinDate = dateTimePickerStart.Value.AddDays(1);
        }

        private void dateTimePickerEnd_ValueChanged(object sender, EventArgs e)
        {
            dateTimePickerStart.MaxDate = dateTimePickerEnd.Value.AddDays(-1);
        }
    }
}
