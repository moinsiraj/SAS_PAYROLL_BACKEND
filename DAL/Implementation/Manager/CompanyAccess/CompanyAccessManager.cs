using BLL.Interfaces.Manager.CompanyAccess;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.CompanyAccess;
using EF.Core.Repository.Manager;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.CompanyAccess
{
    public class CompanyAccessManager : CommonManager<CompanyAccess_DbModel>,ICompanyAccessManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _connection;
        public CompanyAccessManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base( new CompanyAccessRepository(context))
        {
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<DataSet> User_List()
        {
            var data = await _dgCommon.get_InformationDtasetAsync("User_List", _connection);
            return data;
        }
        public async Task<DataSet> User_List_New()
        {
            var data = await _dgCommon.get_InformationDtasetAsync("User_List_New", _connection);
            return data;
        }
        public async Task<DataSet> User_Listfrom_ACCESS(string user)
        {
            var data = await _dgCommon.get_InformationDtasetAsync("User_List_from_access_table '"+ user + "'", _connection);
            return data;
        }

        //New Action 8/15/2024
        public ReturnObject GetAllCompanyAccess()
        {
            var result = new ReturnObject();
            var data = GetAll().Select(data => DgPayCompanyaccess.DbToCustomModel(data)).ToList();
            if (data.Count > 0)
            {
                result.IsSuccess = true;
                result.Message = "Data Loaded !!";
                result.dataTable = data;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> GetAccessByCompanyUser(int companyID, string userName)
        {
            var result = new ReturnObject();
            var dataAccess = await GetFirstOrDefaultAsync(x => x.ca_compid ==  companyID && x.ca_accessuser == userName);
            if (dataAccess != null)
            {
                result.IsSuccess = true;
                result.Message = "Data Loaded !!";
                result.dataTable = dataAccess;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject<DgPayCompanyaccess>> AddOrEditCompanyAccess(DgPayCompanyAccessPayload obj)
        {
            var result = new ReturnObject<DgPayCompanyaccess>();
            var dbData = DgPayCompanyAccessPayload.CompanyAccessDB(obj);
            bool flag;
            var isExists = GetFirstOrDefault(x => x.ca_compid == obj.companyID && x.ca_accessuser == obj.accessUser);
            if (isExists == null)
            {
                flag = await AddAsync(dbData);
                if (flag)
                {
                    result.IsSuccess = flag;
                    result.Message = "Save Successfully !!";
                    result.ListData = GetAll().Select(data => DgPayCompanyaccess.DbToCustomModel(data)).ToList();
                    return result;
                }
                result.Message = "Data Save Fail !!";
                return result;
            }
            flag = await UpdateAsync(dbData);
            if (flag)
            {
                result.IsSuccess = flag;
                result.Message = "Update Successfully !!";
                result.ListData = GetAll().Select(data => DgPayCompanyaccess.DbToCustomModel(data)).ToList();
                return result;
            }
            result.Message = "Data Update Fail !!";
            return result;
        }
    }
}
