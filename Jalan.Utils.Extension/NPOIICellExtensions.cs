using NPOI.SS.UserModel;
using System;

namespace Jalan.Utils.Extension
{
    public static class NPOIICellExtensions
    {
        public static bool TryGetCellDateValue(this ICell cell, out DateTime date)
        {
            bool successFlag = false;
            date = default(DateTime);
            if (cell.CellType.Equals(CellType.Numeric))
            {
                date = cell.DateCellValue;
                successFlag = true;
            }

            else if (cell.CellType.Equals(CellType.String))
            {
                string dateStr = cell.StringCellValue;
                successFlag = DateTime.TryParse(dateStr, out date);
            }

            return successFlag;
        }

        public static bool TryGetCellStringValue(this ICell cell, out string str)
        {
            bool successFlag = false;
            str = default(string);

            if (cell.CellType.Equals(CellType.Numeric))
            {
                str = cell.NumericCellValue.ToString();
                successFlag = true;
            }

            else if (cell.CellType.Equals(CellType.String))
            {
                str = cell.StringCellValue;
                successFlag = true;
            }

            return successFlag;
        }

        public static bool TryGetCellDoubleValue(this ICell cell, out double dou)
        {
            bool successFlag = false;
            dou = default(double);
            if (cell.CellType.Equals(CellType.Numeric))
            {
                dou = cell.NumericCellValue;
                successFlag = true;
            }

            else if (cell.CellType.Equals(CellType.String))
            {
                string douStr = cell.StringCellValue;
                successFlag = Double.TryParse(douStr, out dou);
            }

            return successFlag;
        }
    }
}
