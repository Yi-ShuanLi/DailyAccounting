using CSV;
using DailyAccounting.Attributes;
using DailyAccounting.Extensions;
using DailyAccounting.Models;
using DailyAccounting.Models.DTOs;
using DailyAccounting.Presenters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DailyAccounting.Contract.AccountingBookContract;
using static System.Net.Mime.MediaTypeNames;

namespace DailyAccounting.Forms
{
    public partial class 記帳本 : Form, IAccountingBookView
    {
        BindingList<RecordViewModel> datas = null;
        IAccountingBookPresenter accountingBookPresenter;
        public 記帳本()
        {
            InitializeComponent();
            dataGridView1.CurrentCellDirtyStateChanged += new EventHandler(dataGridView1_CurrentCellDirtyStateChanged);
            accountingBookPresenter = new AccountingBookPresenter(this);
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

                if (dateTimePickerEnd.Value.Date < dateTimePickerStart.Value.Date)
                {
                    MessageBox.Show("結束日期必須大於等於開始日期!!!");
                    return;
                }
                accountingBookPresenter.SearchRecordsByDateRange(dateTimePickerStart.Value.Date, dateTimePickerEnd.Value.Date);
            });

        }

        private void ShowTable()
        {
            dataGridView1.Rows.OfType<DataGridViewRow>().ToList().ForEach(row => { row.Cells.OfType<DataGridViewImageCell>().Select(cell => (Bitmap)cell.Value).ToList().ForEach(image => { image?.Dispose(); }); });
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            GC.Collect();
            datas.AllowNew = false;
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

            this.dataGridView1.CreateImageColumns("刪除", "Trash_Image", "Images\\垃圾桶icon.png");
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);

        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            if (dataGridView1.Rows[rowIndex].Cells[columnIndex] is DataGridViewImageCell)
            {
                return;
            }

            if (dataGridView1.Rows[rowIndex].Cells[columnIndex].Value == null)
            {
                dataGridView1.Rows[rowIndex].Cells[columnIndex].Value = "0";
                return;
            }
            accountingBookPresenter.EditRecords(datas[rowIndex]);
            //string day = dataGridView1.Rows[rowIndex].Cells["Day"].Value.ToString();
            //List<RecordViewModel> recordModelSameDay = datas.Where(x => x.Day.Equals(day)).ToList();
            //File.Delete(Path.Combine(recordPath, $"{day}\\record.csv"));
            //CSVHelper.Write<RecordViewModel>(recordPath + $"{day}\\record.csv", recordModelSameDay);
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
                    accountingBookPresenter.RemoveRecord(datas[rowIndex]);
                    datas.RemoveAt(rowIndex);
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

        void IAccountingBookView.GetRecordsResponse(List<RecordViewModel> recordViewModel)
        {
            datas = new BindingList<RecordViewModel>(recordViewModel);
            ShowTable();
        }
    }
}
