using Jalan.Utils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jalan.Utils.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var isTrue = true;
            decimal value ;
            while (isTrue)
            {
                Console.WriteLine("请输入数字：");
                var input = Console.ReadLine();
                if (decimal.TryParse(input, out value))
                {
                    var result = NumberToUpper.ConvertToUpper(value);
                    Console.WriteLine(result);
                }
                else {
                    break;
                }
            }
            Console.Read();
        }
    }
}
