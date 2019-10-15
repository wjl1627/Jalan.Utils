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

        public static void CacheTest()
        {
            //查询数据源
            List<Person> personData = new List<Person>();
            personData.Add(new Person() { Id = Guid.NewGuid(), Code = "101", Name = "张三" });

            //放进缓存 默认2分半钟自动释放
            var cachePersonData = new CacheThread<List<Person>>(personData);

            //从缓存取数据
            personData = cachePersonData.DictionaryData;
        }
    }
}
