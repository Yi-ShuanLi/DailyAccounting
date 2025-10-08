using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 反射練習
{
    [Flags]
    internal enum FileAccess
    {
        Read = 1, //   0001
        Write = 2, //  0010
        //Read+Write => 0011
        Execute = 4 // 0100
        //Read+Write+Execute => 0111

        // 0111
        // 0001
        // 0001 => 不為0


        // 0001
        // 0010
        // 0000 => 不具備全縣
    }
}
