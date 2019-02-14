using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jalan.Utils.Common
{

    public class NumberToUpper
    {
        private static string[] _numbersUpper = new string[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
        private static string[] _pointValues = new string[] { "", "拾", "佰", "仟" };
        private static string[] _units = new string[] { "元", "万", "亿" };
        public static string ConvertToUpper(decimal money)
        {
            if ((money * 100 - Math.Floor(money * 100) != 0) || money > decimal.MaxValue)
            {
                return "只保留两位小数";//只要两位小数
            }
            var retulstring = string.Empty;
            //文本化
            string value = Math.Floor(money * 100).ToString();
            string pointRigjtLowerValue = value.Substring(value.Length - 2, 2);
            string pointLeftLowerValue = value.Substring(0, value.Length - 2);
            //取小数
            string pointRigjtUpperValue = string.Empty;
            if (pointRigjtLowerValue == "00")
            {
                pointRigjtUpperValue = "整";
            }
            else
            {
                var j = _numbersUpper[Convert.ToInt16(pointRigjtLowerValue.Substring(0, 1))];
                var f = _numbersUpper[Convert.ToInt16(pointRigjtLowerValue.Substring(1, 1))];
                if (j != "0")
                    pointRigjtUpperValue = j + "角";
                if (f != "0")
                    pointRigjtUpperValue += f + "分";
            }
            //4位一取，保证元、万、亿
            List<string> pointLeftValues = new List<string>();
            while (pointLeftLowerValue.Length > 4)
            {
                pointLeftValues.Add(pointLeftLowerValue.Substring(pointLeftLowerValue.Length - 4, 4));
                pointLeftLowerValue = pointLeftLowerValue.Substring(0, pointLeftLowerValue.Length - 4);
            }
            pointLeftValues.Add(pointLeftLowerValue);
            //循环每个4位
            for (int a = 0; a < pointLeftValues.Count; a++)
            {
                string numberChar = pointLeftValues[a];
                string str = "";
                int number;
                //个十百千
                for (int i = 0; i < numberChar.Length; i++)
                {
                    var temp = numberChar.Substring(numberChar.Length - 1 - i, 1);
                    //找出来，数字化
                    int.TryParse(temp, out number);
                    if (number != 0)//如果非0，就加一千二百三十四个
                    {
                        str = str.Insert(0, (_numbersUpper[number] + _pointValues[i]));
                    }
                    else//如果0，就加零
                    {
                        if (temp == "-")
                            str = str.Insert(0, "负");
                        else
                            str = str.Insert(0, "零");
                    }
                }
                //多于的0去掉
                str = str + "X";
                str = str.Replace("零零零零", "零");
                str = str.Replace("零零零", "零");
                str = str.Replace("零零", "零");
                str = str.Replace("零X", "");
                str = str.Replace("X", "");
                str = str + _units[a];
                retulstring = retulstring.Insert(0, str);
            }
            retulstring = retulstring.Replace("亿万", "亿");
            var result = retulstring + pointRigjtUpperValue;
            return result;
        }
    }
}
