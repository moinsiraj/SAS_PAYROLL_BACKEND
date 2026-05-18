using BLL.Interfaces.Manager.Departments;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Departments;
using EF.Core.Repository.Manager;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.Departments
{
    //From SpecFo
    public class DepartmentsManager : CommonManager<Department_DbModel>, IDepartmentsManager
    {
        //private readonly dg_SpecFoContext _context;
        private readonly dg_hrpayrollContext _payContext;
        private readonly Dg_Common _dgCommon;
        private SqlConnection _connection;
        public DepartmentsManager(dg_SpecFoContext context, dg_hrpayrollContext payContext, Dg_Common dgCommon) : base(new DepartmentsRepository(context))
        {
            //_context = context;
            _payContext = payContext;
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.SpecFoCon);
        }

        public async Task<List<DgPayDepartment>> GetDepartmentByUserName(string userName)
        {
            var lst = new List<DgPayDepartment>();
            var data = await _dgCommon.get_InformationDataTableAsync("select * from Smt_Department INNER JOIN dg_hrpayroll.dbo.dg_pay_companyaccess AS compAcc ON Smt_Department.nCompanyID=compAcc.ca_compid where compAcc.ca_accessuser='" + userName + "'", _connection);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                lst.Add(new DgPayDepartment()
                {
                    NUserDept = Convert.ToInt32(data.Rows[i]["nUserDept"]),
                    CDeptname = data.Rows[i]["cDeptname"].ToString(),
                    Dptnamebangla = data.Rows[i]["dptnamebangla"].ToString().Trim(),
                    NCompanyId = Convert.ToInt32(data.Rows[i]["nCompanyID"]),
                    CEntUser = data.Rows[i]["cEntUser"].ToString(),
                    DEntdt = Convert.ToDateTime(data.Rows[i]["dEntdt"]),
                    CDptnameBd = data.Rows[i]["cDptnameBD"].ToString().Trim(),
                    NCompanyName = data.Rows[i]["nCompany_name"].ToString()
                });
            }
            return lst;
        }

        public async Task<ReturnObject> AddOrEditDepartment(DepartmentPayload obj)
        {
            var result = new ReturnObject();
            try
            {
                var dbData = new Department_DbModel();
                if (obj.departmentID == 0)
                {
                    var isExists = GetFirstOrDefault(x => x.nCompanyID == obj.companyID && x.cDeptname == obj.departmentName && x.dptnamebangla == obj.departmentNameBN);
                    if (isExists == null)
                    {
                        dbData = DepartmentPayload.PayloadToDepartmentDb_Obj(obj);
                        bool isSave = await AddAsync(dbData);
                        if (isSave)
                        {
                            result.IsSuccess = true;
                            result.Message = "Department Save Successfully !!";
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = "Can Not Saved Department !!";
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Department Already Exists !!";
                    }
                }
                else
                {
                    var dbObj = GetFirstOrDefault(x => x.nUserDept == obj.departmentID);
                    dbData = DepartmentPayload.PayloadToDepartmentDb_Obj(obj, dbObj);
                    bool isUpdate = await UpdateAsync(dbData);
                    if (isUpdate)
                    {
                        result.IsSuccess = true;
                        result.Message = "Department Update Successfully !!";
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Can Not Update Department !!";
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.IsSuccess = false;
                result.Message = "Something Went Wrong !!";
            }
            return result;
        }
        public async Task<ReturnObject<Department_DbModel>> GetAllDepartmentByCompany(int compnayID)
        {
            var result = new ReturnObject<Department_DbModel>();
            try
            {
                var departmentLs = await GetAsync(x => x.nCompanyID == compnayID);
                var userLs = await _payContext.Tbl_User.ToListAsync();
                var mainList = from a in departmentLs
                join b in userLs on (!string.IsNullOrEmpty(a?.cEntUser) ? a?.cEntUser.Trim() : string.Empty) equals b?.FullName.Trim() into userFullName
                from a1 in userFullName.DefaultIfEmpty()
                join c in userLs on a.cUpdateBy equals c.FullName.Trim() into userFullName2
                from a2 in userFullName2.DefaultIfEmpty()
                select (new Department_DbModel
                {
                    nUserDept = a.nUserDept,
                    nCompanyID = a?.nCompanyID,
                    nCompany_name = a?.nCompany_name,
                    cDeptname = a?.cDeptname,
                    dptnamebangla = !string.IsNullOrEmpty(a?.dptnamebangla)? a?.dptnamebangla.Trim():null,
                    cEntUser = a1?.UserFullname.Trim(),
                    dEntdt = a?.dEntdt,
                    cUpdateBy = a2?.UserFullname.Trim(),
                    cUpdatedt = a?.cUpdatedt
                });
                if (mainList.ToList() != null)
                {
                    result.IsSuccess = true;
                    result.Message = "Data Loaded Successfully !!";
                    result.ListData = mainList.ToList();
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Data Loaded Fail !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.IsSuccess = false;
                result.Message = "Something Went Wrong !!";
            }
            return result;
        }
    }
}