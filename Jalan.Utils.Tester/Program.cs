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

            Logger logger = new Logger();
            //logger.Error<Program>("fdsafdsa");
            logger.WriteInfo("aaaaaaaaaaaa");
            try
            {
                throw new NullReferenceException("zzzz");
            }
            catch (Exception ex)
            {
                logger.WriteError(ex);
            }

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
