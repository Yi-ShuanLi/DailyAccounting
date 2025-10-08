using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DailyAccounting.Extensions
{
    internal static class FormExtension
    {
        static System.Threading.Timer timer = null;
        static Action action = null;
        public static void DebounceTime(this Form form, Action actionInput)
        {
            if (timer != null && action.Equals(actionInput))
            {
                timer.Change(400, -1);
            }
            else
            {
                action = actionInput;
                // 延後執行的時間(毫秒),  >=1 loop的間隔毫秒，0或-1為停止loop
                timer = new System.Threading.Timer((data) =>
                {
                    form.Invoke(actionInput);  // 把委派切回 UI 執行緒
                }, "Hello", 400, -1);
            }
        }


    }
}
