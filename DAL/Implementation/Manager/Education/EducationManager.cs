using BLL.Interfaces.Manager.Education;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Education;
using EF.Core.Repository.Manager;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.Education
{
    public class EducationManager : CommonManager<EmpEducation>, IEducationManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _connection;
        public EducationManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base(new EducationRepository(context))
        {
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<EmpEducation> GetEduSingel(int EmpNo)
        {
            EmpEducation eduls = new EmpEducation();
            var dataTable = await _dgCommon.get_InformationDataTableAsync(@"select empno, emptitle,[subject],result,passyear,board_univer,pi_fullname from dg_pay_Employee left outer join dg_Pay_EmployeeEducation on dg_Pay_EmployeeEducation.comid=dg_pay_Employee.compid and dg_Pay_EmployeeEducation.empno=dg_pay_Employee.emp_no where dg_pay_Employee.emp_no=" + EmpNo, _connection);
            foreach (DataRow dr in dataTable.Rows)
            {
                eduls.empno = Convert.ToInt32(!string.IsNullOrEmpty(dr["empno"].ToString()) ? dr["empno"].ToString() : 0);
                eduls.empName = !string.IsNullOrEmpty(dr["pi_fullname"].ToString()) ? dr["pi_fullname"].ToString() : "";
                eduls.emptitle = !string.IsNullOrEmpty(dr["emptitle"].ToString()) ? dr["emptitle"].ToString() : "";
                eduls.subject = !string.IsNullOrEmpty(dr["subject"].ToString()) ? dr["subject"].ToString() : "";
                eduls.result = !string.IsNullOrEmpty(dr["result"].ToString()) ? dr["result"].ToString() : "";
                eduls.passyear = Convert.ToInt32(!string.IsNullOrEmpty(dr["passyear"].ToString()) ? dr["passyear"].ToString() : 0);
                eduls.board_univer = !string.IsNullOrEmpty(dr["board_univer"].ToString()) ? dr["board_univer"].ToString() : "";
            };
            return eduls;
        }
        public async Task<bool> UpdateEduInfo(EmpEducation obj)
        {
            bool flag = false;
            try
            {
                await _connection.OpenAsync();
                SqlCommand cmd = new SqlCommand(@"update dg_Pay_EmployeeEducation set emptitle=@emptitle,subject=@subject,result=@result,passyear=@passyear,board_univer=@board_univer,updatedby=@updatedby,updatetime=@updatetime where empno=" + obj.empno, _connection);
                cmd.Parameters.AddWithValue("@emptitle", obj.emptitle);
                cmd.Parameters.AddWithValue("@subject", obj.subject);
                cmd.Parameters.AddWithValue("@result", obj.result);
                cmd.Parameters.AddWithValue("@passyear", obj.passyear);
                cmd.Parameters.AddWithValue("@board_univer", obj.board_univer);
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
