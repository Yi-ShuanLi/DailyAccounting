using DailyAccounting.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DailyAccounting.Components
{
    public partial class Navbar : UserControl
    {


        public Navbar()
        {

            InitializeComponent();
            GenerateButtons();
        }

        private void ChangePage_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Form form = SignletonForm.GetForm(button.Text);
            form.Show();
            //SignletonForm.DisableButton(form, button.Text);
        }
        public void DisableButton(string formName)
        {
            Button button = flowLayoutPanel1.Controls.OfType<Button>().First(x => x.Text == formName);
            button.Enabled = false;

        }

        private void GenerateButtons()
        {
            flowLayoutPanel1.Controls.Clear();
            var formNames = Assembly.GetExecutingAssembly().DefinedTypes.Where(x => x.BaseType == typeof(Form) && x != typeof(ImageForm)).Select(x => x.Name).OrderBy(x => x).ToList();

            foreach (string name in formNames)
            {

                Button button = new Button();
                button.Text = name;
                button.Width = (flowLayoutPanel1.Width / formNames.Count);
                button.Margin = new Padding(0);
                button.Height = flowLayoutPanel1.Height;
                button.Font = new System.Drawing.Font("微軟正黑體", 15.75F);
                button.TextAlign = ContentAlignment.MiddleCenter;
                button.Click += ChangePage_Click;
                button.Enabled = true;
                flowLayoutPanel1.Controls.Add(button);
            }
        }

        private void Navbar_SizeChanged(object sender, EventArgs e)
        {
            List<Button> buttons = flowLayoutPanel1.Controls.OfType<Button>().ToList();
            foreach (Button button in buttons)
            {
                button.Width = ((flowLayoutPanel1.Width - 3) / buttons.Count);
            }
        }
    }
}
