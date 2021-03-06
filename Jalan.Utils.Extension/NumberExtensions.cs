﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jalan.Utils.Extension
{
    public static class NumberExtensions
    {
        private static string[] _numbersUpper = new string[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
        private static string[] _pointValues = new string[] { "", "拾", "佰", "仟" };
        private static string[] _units = new string[] { "元", "万", "亿" };
        private static string[] _numberArr = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", };
        private static string[] _numberCnArr = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九", };
        private static string[] _unitArr = { "", "十", "百", "千", "万", "十", "百", "千", "亿", "十", "百", "千", "万", "兆" };
        /// <summary>
        /// 整数转中文数字
        /// 例如：125->一百二十五，37->三十七
        /// </summary>
        /// <param name="value">整数</param>
        /// <returns>返回一个中文数字</returns>
        public static string NumberToCN(this int value)
        {
            char[] tempArr = value.ToString().ToArray();
            string temp = string.Empty;
            int count = 0;
            StringBuilder tmpVal = new StringBuilder();
            for (int i = 0; i < tempArr.Length; i++)
            {
                var numberCn = _numberCnArr[tempArr[i] - 48];
                if (tempArr.Length == 1)
                    return numberCn;
                var unit = _unitArr[tempArr.Length - 1 - i];//ASCII编码 0为48
                if (numberCn == _numberCnArr[0])
                {
                    count++;
                }
                if (numberCn != _numberCnArr[0])
                {
                    if ((unit == "" && temp == _numberCnArr[0]) || count > 1)
                        tmpVal.Append(_numberCnArr[0]);
                    tmpVal.Append(numberCn);
                    tmpVal.Append(unit);
                    count = 0;
                }
                else if (i > 0 && ((tempArr.Length - i - 1) % 4) == 0)
                {
                    tmpVal.Append(unit);
                }
                temp = numberCn;
            }
            var result = tmpVal.ToString();
            //把一十开头改成十开头
            if (result.StartsWith("一十"))
            {
                return result.Substring(1);
            }
            return result;
        }
        /// <summary>
        /// 金额转财务大写金额
        /// 例如：12.34->拾贰圆叁角肆分，102->一百零贰圆整
        /// </summary>
        /// <param name="money">一个双精度金额</param>
        /// <returns>返回一个财务大写金额</returns>
        public static string MoneyToUpper(this decimal money)
        {
            if (money == 0)
            {
                return "零元整";
            }
            if (money > decimal.MaxValue)
            {
                return "金额超过存储最大值";
            }
            if ((money * 10000 - Math.Floor(money * 10000) != 0))
            {
                return "只保留四位小数";//只要两位小数
            }
            var retulstring = string.Empty;
            //文本化
            string value = Math.Floor(money * 10000).ToString();
            string pointRightLowerValue = string.Empty;
            string pointLeftLowerValue = string.Empty;

            if (value.Length >= 4)
            {
                pointRightLowerValue = value.Substring(value.Length - 4);
                pointLeftLowerValue = value.Substring(0, value.Length - 4);
            }
            //取小数
            StringBuilder pointRigjtUpperValue = new StringBuilder();
            if (pointRightLowerValue.PadLeft(4, '0') == "0000")
            {
                pointRigjtUpperValue.Append("整");
            }
            else
            {
                var array = pointRightLowerValue.ToArray();
                for (int i = 0; i < array.Length; i++)
                {
                    var temp = array[i];
                    if (temp == '0')
                    {
                        //pointRigjtUpperValue.Append("零");
                        continue;
                    }
                    pointRigjtUpperValue.Append(_numbersUpper[Convert.ToInt32(temp.ToString())]);
                    switch (i)
                    {
                        case 0:
                            pointRigjtUpperValue.Append("角");
                            break;
                        case 1:
                            pointRigjtUpperValue.Append("分");
                            break;
                        case 2:
                            pointRigjtUpperValue.Append("毫");
                            break;
                        case 3:
                            pointRigjtUpperValue.Append("厘");
                            break;
                        default:
                            break;
                    }
                }
                //pointRigjtUpperValue = pointRigjtUpperValue.Replace("零零零", "零");
                //pointRigjtUpperValue = pointRigjtUpperValue.Replace("零零", "零");
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
                if (string.IsNullOrEmpty(str))
                {
                    continue;
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
