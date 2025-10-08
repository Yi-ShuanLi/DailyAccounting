using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 反射練習
{
    internal class Program
    {
        static void Main(string[] args)
        {

            FileAccess fileAccess = FileAccess.Read | FileAccess.Execute;

            FileAccess[] accesses = { FileAccess.Read, FileAccess.Write, FileAccess.Execute };

            foreach (var ac in accesses)
            {
                if ((fileAccess & ac) == 0)
                    throw new Exception($"未擁有此{ac.ToString()}權限");
            }


            Student student = Generate<Student>();
            PrintAllProps(student);
        }
        static void PrintAllProps(Object data)
        {
            var props = data.GetType().GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                Console.Write(props[i].ToString() + " " + props[i].Name + " ");
                Console.WriteLine(props[i].GetValue(data));
            }
        }
        static T Generate<T>() where T : class, new()
        {
            T t = new T();
            Type type = typeof(T);
            var props = type.GetProperties();
            string[] datas = new[] { "1", "Jenny", "jenny@gmail.com" };
            for (int i = 0; i < props.Length; i++)
            {
                props[i].SetValue(t, datas[i]);
            }
            return t;
        }
    }
}
