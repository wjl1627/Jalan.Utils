using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jalan.Utils.Attributes
{
    /// <summary>
    /// EXCEL导入列名称
    /// </summary>
    public class ExcelImportColumnNameAttribute : Attribute
    {
        public string ColumnName { get; set; }
        public ExcelImportColumnNameAttribute() { }
        public ExcelImportColumnNameAttribute(string columnName)
        {
            this.ColumnName = columnName;
        }
        public override string ToString()
        {
            return this.ColumnName;
        }
    }
}
