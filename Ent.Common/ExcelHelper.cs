using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ent.Common
{
  public  class ExcelHelper
    {

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <typeparam name="T">泛型动态T</typeparam>
        /// <param name="exportData">导出数据</param>
        /// <param name="cellData">列名称对应的列索引</param>
        ///  <param name="dic">列索引对应实体属性</param>
        ///  <param name="spicial">特殊类型数据显示转化</param>
        /// <param name="excelName">导出excel名称</param>
        /// <returns></returns>
      public MemoryStream Export<T>(IList<T> exportData, IDictionary<int, string> cellData, IDictionary<string, int> dic, IDictionary<string, Dictionary<object, object>> special, string excelName) where T : class,new()
        {
           
            HSSFWorkbook workbook = new HSSFWorkbook();
            #region 创建excel表结构
            ISheet sheet = workbook.CreateSheet(excelName);


            #region 表头标题 样式
            IRow rowHeaderTitle = sheet.CreateRow(0);//表标题
            rowHeaderTitle.CreateCell(0, CellType.String).SetCellValue(excelName);
            rowHeaderTitle.HeightInPoints = 29;//行高

            ICellStyle style = workbook.CreateCellStyle();
            //设置单元格的样式：水平对齐居中
            style.Alignment = HorizontalAlignment.Center;
            //新建一个字体样式对象
            IFont font = workbook.CreateFont();
            //设置字体加粗样式
            font.Boldweight = short.MaxValue;
            //使用SetFont方法将字体样式添加到单元格样式中 
            font.FontName = "宋体";
            font.FontHeightInPoints = 20;//字体大小
            style.SetFont(font);
            //将新的样式赋给单元格
            rowHeaderTitle.Cells[0].CellStyle = style;

            //起始行 结束行 起始列 结束列
            rowHeaderTitle.Sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, cellData.Count()-1));
            #endregion

            #region 列名样式
            for (int i = 0; i < cellData.Count(); i++) {

                sheet.SetColumnWidth(i, 22 * 256);
            
             }
            #endregion

            IRow rowHeader = sheet.CreateRow(1);//表头行

            foreach (KeyValuePair<int, string> item in cellData) {//列名称

                rowHeader.CreateCell(item.Key, CellType.String).SetCellValue(item.Value);
            
            }

            int rowIndex = 1;
            #endregion
            if (exportData.Count() > 0)
            {

                #region 表数据

                for (int i = 0; i < exportData.Count(); i++)
                {
                    rowIndex++;
                    IRow row = sheet.CreateRow(rowIndex);

                    T tModel = exportData[i];
                    Type typeName = tModel.GetType();
                    PropertyInfo[] pList = typeName.GetProperties();

                    foreach (PropertyInfo p in pList)
                    {
                       
                            if (dic.Keys.Contains(p.Name))
                            {
                                if (special != null)
                                {
                                    if (special.Keys.Contains(p.Name))
                                    {
                                        Dictionary<object, object> tempDic = special[p.Name];
                                        row.CreateCell(dic[p.Name], CellType.String).SetCellValue(Convert.ToString(tempDic[p.GetValue(tModel, null)]));
                                    }
                                    else
                                    {
                                        row.CreateCell(dic[p.Name], CellType.String).SetCellValue(Convert.ToString(p.GetValue(tModel, null)));
                                    }
                                }


                            }
                      
                       
                    }

                }

                IRow rowEndInfo = sheet.CreateRow(rowIndex + 1);
                rowEndInfo.CreateCell(0, CellType.String).SetCellValue("总计:" + exportData.Count());
                rowEndInfo.Sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, cellData.Count()));

                #endregion

                #region 输出表
                string filename = excelName + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
                HttpContext.Current.Response.Clear();

                ////create a entry of DocumentSummaryInformation
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "NPOI";
                workbook.DocumentSummaryInformation = dsi;

                ////create a entry of SummaryInformation
                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Subject = excelName;
                workbook.SummaryInformation = si;
                MemoryStream file = new MemoryStream();
                workbook.Write(file);
                return file;
               
                #endregion

            }
            else
            {
                return new MemoryStream() ;
            }

        }

      /// <summary>
      /// 导入excel 
      /// </summary>
      /// <param name="stream">导入的excel内存流</param>
      /// <param name="dic">excel对应列一一key ：对应的泛型T的属性名称 value：是属性名称对应的excel索引</param>
      /// <param name="errorList">错误行和列的记录，用于验证转换对应数据类型时候错误集合</param>
      /// <returns></returns>
      public List<T> Import<T>(Stream stream,IDictionary<string,int> dic,out Dictionary<int,int> errorList) where T:class,new() {

          errorList = new Dictionary<int, int>();
         List<T> list = new List<T>();
          #region Excel操作读取
          using (FileStream fs = (FileStream)stream)
          {
              //读取指定的Excel文件，把文件的内容读出到控制台上
              //1.创建一个Workbook对象（根据指定的文件流fs,创建一个Workbook对象，会自动把指定的Excel加载到该Workbook对象中）

              IWorkbook wk = null;

              if (fs.Name.ToLower().Contains(".xlsx"))
              {//先过滤高级，再次验证低版本，支持07、10版本以上
                  wk = new XSSFWorkbook(fs);
              }
              else if (fs.Name.ToLower().Contains(".xls"))
              {//支持03，07
                  wk = new HSSFWorkbook(fs);
              }

              //获取第一个工作表
              ISheet sheet = wk.GetSheetAt(0);

              //最后一行的索引为1，也就是第三行数据为空，前面两行是标题和列名
              if (sheet.LastRowNum == 1)
              {              
                  return new List<T>();
              }
              else
              {
                  //获取当前工作表下的所有的行
                  //循环每一样
                  //数据从第三行开始，索引为2 
                  #region 循环获取数据，并校验数据
                  for (int r = 2; r <= sheet.LastRowNum; r++)
                  {
                      //获取当前行
                      IRow row = sheet.GetRow(r);
                      //获取每个单元格

                      T tempModel = new T();
                      Type typeName = tempModel.GetType();

                      PropertyInfo[] prop = typeName.GetProperties();

                      foreach(PropertyInfo p in prop){

                          //判断 model属性是否在对应的列字典中，如果有，表示有excel的列，进行动态赋值；没有，自动为null
                          if (dic.Keys.Contains(p.Name)) {

                           #region 数据填充
		                    string tempValue = row.GetCell(dic[p.Name]) == null ? "" : row.GetCell(dic[p.Name]).ToString().Trim();
                              //动态转换数据类型再次赋值（不然会有数据类型异常）
                              if (tempValue == "")
                              {

                                          p.SetValue(tempModel, null);//为空的时候赋值为null

                              }
                              else
                              {//不为空的时候，动态转换成对应数据类型的数据，再进行赋值,这里一定要保证对应的数据可以转换为对应的数据类型
                                  try
                                  {
                                      p.SetValue(tempModel, Convert.ChangeType(tempValue, p.PropertyType));
                                  }
                                  catch (Exception ex)
                                  {
                                      int tempCellIndex = dic[p.Name];
                                      p.SetValue(tempModel, null);
                                      errorList.Add(r, tempCellIndex);
                                      continue;
                                  }

                              } 
	#endregion
                          }
                        
                      }

                          list.Add(tempModel);
                  }
                  #endregion
              }

              return list;
          }

          #endregion

      }


    
    }
}
