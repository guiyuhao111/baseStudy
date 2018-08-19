using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.IO;

namespace DSMTMALL.Core.Common
{
    public class ExcelHelper
    {
        private readonly int Excel_MaxRow = 65535;
        /// <summary>
        /// 读取Excel文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>DataTable</returns>
        public DataTable ReadExcel(string filePath)
        {
            DataTable dt = new DataTable();
            try
            {
                if (File.Exists(filePath))
                {
                    using (FileStream fileStream = File.OpenRead(filePath))
                    {
                        FileInfo fileInfo = new FileInfo(filePath);
                        IWorkbook iWorkbook = fileInfo.Extension.ToLower() == ".xls" ? (IWorkbook)new HSSFWorkbook(fileStream) : (IWorkbook)new XSSFWorkbook(fileStream);
                        ISheet iSheet = iWorkbook.GetSheetAt(0);
                        IRow iRow = iSheet.GetRow(0);
                        int cellCount = iRow.LastCellNum;
                        for (int i = iRow.FirstCellNum; i < cellCount; i++)
                        {
                            DataColumn dataColumn = new DataColumn(iRow.GetCell(i).StringCellValue.Trim());
                            dt.Columns.Add(dataColumn);
                        }
                        for (int i = (iSheet.FirstRowNum + 1); i <= iSheet.LastRowNum; i++)
                        {
                            IRow row = iSheet.GetRow(i);
                            DataRow dataRow = dt.NewRow();
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                {
                                    dataRow[j] = row.GetCell(j).ToString().Trim();
                                }
                            }
                            dt.Rows.Add(dataRow);
                        }
                    }
                }
            }
            catch { }
            return dt;
        }
        /// <summary>
        /// 将DataTable转换为Excel工作簿
        /// </summary>
        /// <param name="dtExcel">Excel数据源</param>
        /// <param name="excelType">Excel类型</param>
        /// <param name="sheetName">Sheet表名称</param>
        /// <returns>Excel工作簿</returns>
        public IWorkbook DataTableToExcel(DataTable dtExcel, string excelType, string sheetName)
        {
            IWorkbook iWorkbook = excelType == ".xls" ? (IWorkbook)new HSSFWorkbook() : (IWorkbook)new XSSFWorkbook();
            if (dtExcel.Rows.Count < Excel_MaxRow)
            {
                DataTableWriteToSheet(dtExcel, 0, dtExcel.Rows.Count - 1, iWorkbook, sheetName);
            }
            else
            {
                int sheetPage = dtExcel.Rows.Count / Excel_MaxRow;
                for (int i = 0; i < sheetPage; i++)
                {
                    int startRow = i * Excel_MaxRow;
                    int endRow = (i * Excel_MaxRow) + Excel_MaxRow - 1;
                    DataTableWriteToSheet(dtExcel, startRow, endRow, iWorkbook, sheetName + i.ToString());
                }
                int lastPageItemCount = dtExcel.Rows.Count % Excel_MaxRow;
                DataTableWriteToSheet(dtExcel, dtExcel.Rows.Count - lastPageItemCount, lastPageItemCount, iWorkbook, sheetName + sheetPage.ToString());
            }
            return iWorkbook;
        }
        /// <summary>
        /// 将DataTable转换为Excel工作簿并写入磁盘
        /// </summary>
        /// <param name="dtExcel">Excel数据源</param>
        /// <param name="excelType">Excel类型</param>
        /// <param name="sheetName">Sheet表名称</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>bool</returns>
        public bool DataTableToExcel(DataTable dtExcel, string excelType, string sheetName, string filePath)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                }
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    DataTableToExcel(dtExcel, excelType, sheetName).Write(fileStream);
                }
                return true;
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 将DataTable写入Sheet表
        /// </summary>
        /// <param name="dtExcel">Excel数据源</param>
        /// <param name="startRow">起始行</param>
        /// <param name="endRow">结束行</param>
        /// <param name="iWorkbook">Excel工作簿</param>
        /// <param name="sheetName">Sheet表名称</param>
        private void DataTableWriteToSheet(DataTable dtExcel, int startRow, int endRow, IWorkbook iWorkbook, string sheetName)
        {
            ISheet iSheet = iWorkbook.CreateSheet(sheetName);
            IRow iRowHeader = iSheet.CreateRow(0);
            for (int i = 0; i < dtExcel.Columns.Count; i++)
            {
                ICell iCell = iRowHeader.CreateCell(i);
                string strTemp = dtExcel.Columns[i].Caption ?? dtExcel.Columns[i].ColumnName;
                iCell.SetCellValue(strTemp);
            }
            int rowIndex = 1;
            for (int i = startRow; i <= endRow; i++)
            {
                DataRow dataRow = dtExcel.Rows[i];
                IRow iRow = iSheet.CreateRow(rowIndex++);
                for (int j = 0; j < dataRow.ItemArray.Length; j++)
                {
                    iRow.CreateCell(j).SetCellValue(dataRow[j].ToString());
                }
            }
        }
    }
}