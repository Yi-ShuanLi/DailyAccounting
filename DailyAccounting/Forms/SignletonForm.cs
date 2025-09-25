using DailyAccounting.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DailyAccounting.Forms
{
    internal static class SignletonForm
    {
        public static Form lastForm = null;
        public static Dictionary<string, Form> forms = new Dictionary<string, Form>();
        public static Form GetForm(String formName)
        {
            if (lastForm != null)
            {
                lastForm.Hide();
            }
            if (forms.ContainsKey(formName))
            {
                lastForm = forms[formName];
            }
            else
            {
                string pageType = "DailyAccounting.Forms." + formName;
                Type type = Type.GetType(pageType);
                lastForm = (Form)Activator.CreateInstance(type);
                forms.Add(formName, lastForm);
            }

            Navbar navbar = lastForm.Controls.OfType<Navbar>().FirstOrDefault();
            navbar?.DisableButton(formName);

            return lastForm;

        }
        //public static void DisableButton(Form form, string formName)
        //{
        //    Navbar navbar = form.Controls.OfType<Navbar>().FirstOrDefault();
        //    if (navbar != null)
        //    {
        //        FlowLayoutPanel flowLayoutPanel = navbar.Controls.OfType<FlowLayoutPanel>().FirstOrDefault();
        //        Button button = flowLayoutPanel.Controls.OfType<Button>().First(x => x.Text == formName);
        //        button.Enabled = false;
        //    }

        //}

    }
}
