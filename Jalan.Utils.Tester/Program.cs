using Jalan.Utils.Common;
using Jalan.Utils.Log4net;
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
            var key = Guid.NewGuid().ToString();
            var t = DESCodeUtil.ToDES_Encrypt("02734072", key);
            Console.WriteLine(t);
            Console.WriteLine(DESCodeUtil.ToMd5(t));
            t = DESCodeUtil.ToDES_Decrypt(t, key);
            Console.WriteLine(t);

            //var isTrue = true;
            //decimal value ;
            //while (isTrue)
            //{
            //    Console.WriteLine("请输入数字：");
            //    var input = Console.ReadLine();
            //    if (decimal.TryParse(input, out value))
            //    {
            //        var result = NumberToUpper.ConvertToUpper(value);
            //        Console.WriteLine(result);
            //    }
            //    else {
            //        break;
            //    }
            //}
            Console.Read();
        }
    }
}
