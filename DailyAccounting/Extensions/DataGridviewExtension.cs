using DailyAccounting.Attributes;
using DailyAccounting.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DailyAccounting.Extensions
{
    internal static class DataGridviewExtension
    {
        public static void CreateComboBoxColumns(this DataGridView dataGridView, PropertyInfo propertyInfo)
        {
            string headerText = propertyInfo.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
            var attrs = propertyInfo.GetCustomAttributes();
            string propName = propertyInfo.Name;
            if (attrs.Any(x => x is ComboBoxColumnAttribute))
            {
                DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
                combo.Name = propName + "_Combo";
                combo.Tag = propName;
                combo.DataPropertyName = propName;
                combo.HeaderText = headerText;
                combo.DataSource = propName != "Purpose" ? typeof(DataModel).GetField(propName, BindingFlags.Public | BindingFlags.Static).GetValue(null) : null;
                int index = dataGridView.Columns[propName].Index;
                dataGridView.Columns.Insert(index, combo);
            }

        }
        public static void CreateImageColumns(this DataGridView dataGridView, PropertyInfo propertyInfo)
        {
            string headerText = propertyInfo.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
            var attrs = propertyInfo.GetCustomAttributes();
            string propName = propertyInfo.Name;
            if (attrs.Any(x => x is ImageColumnAttribute))
            {
                dataGridView.AddImageColumn(headerText, propName);
            }
        }
        private static void AddImageColumn(this DataGridView dataGridView, string headerText, string propName, string defaultImage = "")
        {
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn.Name = propName + "_Image";
            imageColumn.Tag = propName;
            imageColumn.HeaderText = headerText;
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;// 依據cell大小去填滿
            if (defaultImage != "")
            {
                imageColumn.DefaultCellStyle = new DataGridViewCellStyle()
                {
                    NullValue = new Bitmap(defaultImage)
                };
            }

            if (dataGridView.Columns.Contains(propName))
            {
                int index = dataGridView.Columns[propName].Index;
                dataGridView.Columns.Insert(index, imageColumn);
            }
            else
            {
                dataGridView.Columns.Add(imageColumn);// index 13
            }
        }
        public static void CreateImageColumns(this DataGridView dataGridView, string headerText, string propName, string defaultImage = "")
        {

            dataGridView.AddImageColumn(headerText, propName, defaultImage);

        }
        public static void SetComboBoxCells(this DataGridViewCellCollection dataGridViewCellCollection)
        {
            dataGridViewCellCollection.OfType<DataGridViewComboBoxCell>().ToList().ForEach(x =>
            {
                string referenceColumn = x.OwningColumn.Tag.ToString();
                if (referenceColumn == "Purpose")
                {
                    string categoryString = dataGridViewCellCollection["Category"].Value.ToString();
                    IDictionary dicts = (IDictionary)typeof(DataModel).GetField("Purpose", BindingFlags.Public | BindingFlags.Static).GetValue(null);
                    x.DataSource = dicts[categoryString];
                }
                x.Value = dataGridViewCellCollection[referenceColumn].Value;
            });
        }
        public static void SetImageCells(this DataGridViewCellCollection dataGridViewCellCollection)
        {
            dataGridViewCellCollection.OfType<DataGridViewImageCell>().ToList().ForEach(x =>
            {
                string referenceColumn = x.OwningColumn.Tag.ToString();
                string filePaht = dataGridViewCellCollection[referenceColumn].Value.ToString();
                byte[] imageBuffer = File.ReadAllBytes(filePaht);
                MemoryStream imageStream = new MemoryStream(imageBuffer);
                x.Value = new Bitmap(imageStream);
            });
        }
    }
}
