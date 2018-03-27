using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQMethods;

namespace kjnjem
{
    class Program
    {
        static void Main(string[] args)
        { int[] arr = { 9, 4, 4 };
            IEnumerable<int> e = arr;
            //  arr = e.ExtensionSelect(item => item * 2).ToArray();
            // arr = e.ExtensionWhere(item => item >5).ToArray();
            IOrderedEnumerable<int> a = e.ExtensionOrderByASCENDING(item => item +1);
            arr = a.ToArray();
              for (int i = 0; i < arr.Length; i++)
                  Console.WriteLine(arr[i]);
           
        }
    }
}
