using BLL.Interfaces.Manager.Experince;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Experince;
using EF.Core.Repository.Manager;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.Experince
{
    public class ExperinceManager : CommonManager<EmpExperince>,IExperinceManager
    {
        private readonly Dg_Common _dgCommon;
        
        private readonly SqlConnection _connection;
        public ExperinceManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base(new ExperinceRepository(context))
        {
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<EmpExperinceView> GetExpcSingel(int EmpNo)
        {
            EmpExperinceView expcls = new EmpExperinceView();
            var dataTable = await _dgCommon.get_InformationDataTableAsync(@"select emp_id,pi_fullname,prv_companyName,Designation,fromDate,todate
                 from dg_pay_Employee left outer join dg_Pay_Emp_Experience_info on
				dg_pay_Employee.compid=dg_Pay_Emp_Experience_info.compid and dg_pay_Employee.emp_no=dg_Pay_Emp_Experience_info.emp_id
				where dg_pay_Employee.emp_no=" + EmpNo, _connection);
            foreach (DataRow dr in dataTable.Rows)
            {
                expcls.emp_id = Convert.ToInt32(!string.IsNullOrEmpty(dr["emp_id"].ToString()) ? dr["emp_id"].ToString() : 0);
                expcls.empName = !string.IsNullOrEmpty(dr["pi_fullname"].ToString()) ? dr["pi_fullname"].ToString() : "";
                expcls.prv_companyName = !string.IsNullOrEmpty(dr["prv_companyName"].ToString()) ? dr["prv_companyName"].ToString() : "";
                expcls.Designation = !string.IsNullOrEmpty(dr["Designation"].ToString()) ? dr["Designation"].ToString() : "";
                expcls.fromDate = !string.IsNullOrEmpty(dr["fromDate"].ToString()) ? dr["fromDate"].ToString() : "";
                expcls.todate = !string.IsNullOrEmpty(dr["todate"].ToString()) ? dr["todate"].ToString() : "";
            };
            return expcls;
        }
        public async Task<bool> UpdateExpcInfo(EmpExperince obj)
        {
            bool flag = false;
            try
            {
                await _connection.OpenAsync();
                SqlCommand cmd = new SqlCommand(@"update dg_Pay_Emp_Experience_info set prv_companyName=@prv_companyName,Designation=@Designation,
                    fromDate=@fromDate,todate=@todate,updatedby=@updatedby,updatetime=@updatetime where emp_id=" + obj.emp_id, _connection);
                cmd.Parameters.AddWithValue("@prv_companyName", obj.prv_companyName);
                cmd.Parameters.AddWithValue("@Designation", obj.Designation);
                cmd.Parameters.AddWithValue("@fromDate", obj.fromDate);
                cmd.Parameters.AddWithValue("@todate", obj.todate);
                cmd.Parameters.AddWithValue("@updatedby", obj.updatedby);
                cmd.Parameters.AddWithValue("@updatetime", DateTime.Now);
                await cmd.ExecuteNonQueryAsync();
                flag = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                flag = false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
            return flag;
        }        
    }
}
