using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChy.Frame.Common.Helper
{
    public class ExcelHelper
    {
        public static DataTable ReadFile(string excelPath, string sheetName = "sheet1")
        {
            var dtGbPatient = new DataTable();
            var filepath = IOFiles.GetFileMapPath(excelPath);
            const string strConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='{0}';Extended Properties='Excel 8.0;HDR=YES;IMEX=1';";
            string strConnection = string.Format(strConn, filepath);
            var conn = new OleDbConnection(strConnection);
            conn.Open();
            var oada = new OleDbDataAdapter("select * from [" + sheetName + "$]", strConnection);

            dtGbPatient.TableName = "gbPatientInfo";
            oada.Fill(dtGbPatient);//获得datatable
            conn.Close();
            return dtGbPatient;
        }
    }
}
