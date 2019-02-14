using Jalan.Utils.Attributes;
using Jalan.Utils.Extension;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Jalan.Utils.Common
{
    public class ExcelUtil
    {
        private const string _excel_2007 = ".xlsx";
        private const string _excel_2003 = ".xls";
        /// <summary>
        /// 当前Sheet索引
        /// </summary>
        public int CurrentSheetIndex { get; set; }
        ///// <summary>
        ///// 当前SheetName
        ///// </summary>
        //public string CurrentSheetName { get; set; }
        private List<SelectOptionDto> _sheetNames = new List<SelectOptionDto>();
        /// <summary>
        /// 当前所有SheetName
        /// </summary>
        public List<SelectOptionDto> SheetNames { get { return _sheetNames; } }
        private int _skipRowNum = 0;
        /// <summary>
        /// 跳过多少行后读取
        /// </summary>
        public int SkipRowNum
        {
            get { return _skipRowNum; }
            set
            {
                if (value < 0)
                    _skipRowNum = 0;
                else
                    _skipRowNum = value;
            }
        }
        /// <summary>
        /// 忽略哪一行后的所有数据
        /// </summary>
        public int IgnoreNum { get; set; }
        private IWorkbook _workBook;
        private ISheet _currentSheet;
        public string FileName { get; set; }
        public Stream FileStream { get; set; }
        public Dictionary<int, string> Titles { get { return _columnIndexDict; } }
        private Dictionary<int, string> _columnIndexDict = new Dictionary<int, string>();
        public ExcelUtil(string fileName, Stream fileStream, int sheetIndex = 0)
        {
            this.FileName = fileName;
            this.FileStream = fileStream;
            if (sheetIndex < 0)
                this.CurrentSheetIndex = 0;
            else
                this.CurrentSheetIndex = sheetIndex;
            //this.Init();
        }
        public void Init()
        {
            InitWorkBook();
            InitSheet();
            InitExcelAllSheetNames();
            InitColumns();
        }
        /// <summary>
        /// 读取的Excle数据
        /// </summary>
        /// <param name="fileName">读取的文件名</param>
        /// <param name="fileStream">读取的文件流</param>
        /// <returns></returns>
        public List<T> Read<T>() where T : new()
        {
            List<T> list = new List<T>();
            T obj = default(T);
            var firstRowNum = _currentSheet.FirstRowNum;
            var lastRowNum = _currentSheet.LastRowNum;
            var type = typeof(T);
            var properties = type.GetProperties();
            var columnPropertyMapping = AttributeValueUtil.GetPropertyAttributeValueDict<ExcelImportColumnNameAttribute>(type);
            //rowIndex=1去掉标题行
            for (int rowIndex = 1; rowIndex <= lastRowNum; rowIndex++)
            {
                if (this.IgnoreNum > 0 && rowIndex >= this.IgnoreNum)
                    break;
                IRow row = _currentSheet.GetRow(rowIndex);
                if (row == null)
                    continue;
                var firstCellNum = row.FirstCellNum;
                var lastCellNum = row.LastCellNum;
                if (lastCellNum <= 0)
                    throw new Exception("没有任何数据需要导入");
                obj = new T();
                for (int cellIndex = firstCellNum; cellIndex < lastCellNum; cellIndex++)
                {
                    ICell cell = row.GetCell(cellIndex);
                    var cellName = _columnIndexDict[cellIndex];
                    if (cell != null && columnPropertyMapping.ContainsValue(cellName))
                    {
                        foreach (var prop in properties)
                        {
                            if (columnPropertyMapping[prop.Name] == cellName)
                            {
                                object objValue = null;
                                if (cell.CellType == CellType.Numeric)
                                    objValue = cell.NumericCellValue;
                                else if (cell.CellType == CellType.String)
                                    objValue = cell.StringCellValue.Trim();
                                if (prop.PropertyType == typeof(int))
                                    prop.SetValue(obj, objValue.TryParse<int>(int.TryParse), null);
                                else if (prop.PropertyType == typeof(decimal))
                                    prop.SetValue(obj, objValue.TryParse<decimal>(decimal.TryParse), null);
                                else if (prop.PropertyType == typeof(DateTime))
                                    prop.SetValue(obj, objValue == null ? cell.DateCellValue : objValue.TryParse<DateTime>(DateTime.TryParse), null);
                                else if (prop.PropertyType == typeof(bool))
                                    prop.SetValue(obj, objValue == null ? cell.BooleanCellValue : objValue.TryParse<bool>(bool.TryParse), null);
                                else
                                    prop.SetValue(obj, objValue.ToString().Trim(), null);
                                break;
                            }
                        }
                    }
                }
                list.Add(obj);
            }
            return list;
        }
        public DataTable Read()
        {
            DataTable table = new DataTable();
            foreach (var item in _columnIndexDict)
            {
                table.Columns.Add(item.Value + item.Key);
            }
            var firstRowNum = _currentSheet.FirstRowNum;
            var lastRowNum = _currentSheet.LastRowNum;
            DataRow dr;
            string strValue;
            for (int rowIndex = 0; rowIndex <= lastRowNum; rowIndex++)
            {
                if (this.IgnoreNum > 0 && rowIndex >= this.IgnoreNum)
                    break;
                //这里要跳过标题行
                if (rowIndex <= this.SkipRowNum)
                    continue;
                IRow row = _currentSheet.GetRow(rowIndex);
                if (row == null)
                    continue;
                var firstCellNum = row.FirstCellNum;
                var lastCellNum = row.LastCellNum;
                if (lastCellNum <= 0)
                    continue;//throw new BusinessInfoException("没有任何数据需要导入");
                dr = table.NewRow();
                bool isEmpty = true;
                for (int cellIndex = firstCellNum; cellIndex < lastCellNum; cellIndex++)
                {
                    if (_columnIndexDict.ContainsKey(cellIndex))
                    {
                        ICell cell = row.GetCell(cellIndex);
                        if (cell == null)
                            continue;
                        if (cell.CellType == CellType.Numeric)
                            strValue = cell.NumericCellValue.ToString();
                        else if (cell.CellType == CellType.String)
                            strValue = cell.StringCellValue.Trim();
                        else if (cell.CellType == CellType.Formula)
                            strValue = cell.NumericCellValue.ToString();
                        else
                            strValue = null;
                        dr[_columnIndexDict[cellIndex] + cellIndex] = strValue;
                        isEmpty = false;
                    }
                }
                if (!isEmpty)
                    table.Rows.Add(dr);
            }
            return table;
        }
        private void InitSheet()
        {
            _currentSheet = _workBook.GetSheetAt(this.CurrentSheetIndex);
            if (_currentSheet.LastRowNum <= 0)
            {
                throw new Exception("没有任何数据需要导入");
            }
        }

        private void InitWorkBook()
        {
            if (this.FileName.IndexOf(_excel_2007) > 0) // 2007版本
                _workBook = new XSSFWorkbook(this.FileStream);
            else if (this.FileName.IndexOf(_excel_2003) > 0) // 2003版本
                _workBook = new HSSFWorkbook(this.FileStream);

            int workBookSheetNum = _workBook.NumberOfSheets;
            if (workBookSheetNum <= 0)
            {
                throw new Exception("Excel文件中不存在Sheet");
            }
        }
        private void InitExcelAllSheetNames()
        {
            int x = _workBook.NumberOfSheets;
            for (int i = 0; i < x; i++)
            {
                _sheetNames.Add(new SelectOptionDto() { Name = _workBook.GetSheetName(i), Value = i });
            }
        }
        private void InitColumns()
        {
            IRow titleRow = _currentSheet.GetRow(this.SkipRowNum);
            if (titleRow == null)
                throw new Exception("没有任何行数据需要导入");
            var firstCellNum = titleRow.FirstCellNum;
            var lastCellNum = titleRow.LastCellNum;
            if (lastCellNum <= 0)
                throw new Exception("没有任何列数据需要导入");

            for (int cellIndex = firstCellNum; cellIndex <= lastCellNum; cellIndex++)
            {
                ICell cell = titleRow.GetCell(cellIndex);
                if (cell != null)
                {
                    string cellStrValue = null;
                    if (cell.TryGetCellStringValue(out cellStrValue))
                        _columnIndexDict.Add(cellIndex, cellStrValue);
                    //else
                    //    throw new BusinessInfoException("无法解析第" + (cellIndex + 1).ToString() + "列标题内容");
                }
            }
        }
    }
    public class SelectOptionDto
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
