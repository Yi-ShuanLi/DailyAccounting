using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DailyAccounting.Forms
{
    public partial class ImageForm : Form
    {
        public ImageForm(string imageFile)
        {
            InitializeComponent();
            pictureBox1.Image = Image.FromFile(imageFile);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }


        private void ImageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            pictureBox1.Image.Dispose();
            GC.Collect();
        }
    }
}
