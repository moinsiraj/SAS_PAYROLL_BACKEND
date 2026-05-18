using BLL.Interfaces.Manager.Sections;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Sections;
using EF.Core.Repository.Manager;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.Sections
{
    public class SectionsManager : CommonManager<Section_DbModel>,ISectionsManager
    {
        //private readonly dg_SpecFoContext _context;
        private readonly dg_hrpayrollContext _payContext;
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _sqlConnection;
        public SectionsManager(dg_SpecFoContext context, dg_hrpayrollContext payContext, Dg_Common dgCommon) : base(new SectionsRepository(context))
        {
            //_context = context;
            _payContext = payContext;
            _dgCommon = dgCommon;
            _sqlConnection = new SqlConnection(Getway.SpecFoCon);
        }

        public async Task<List<DgPaySection>> GetSectionByUserPermission(string userName)
        {
            var lst = new List<DgPaySection>();
            var data = await _dgCommon.get_InformationDataTableAsync("select distinct nSectionID,cSection_Description,secnamebangla,nUserDept,nUserDeptName,nCompanyID,nCompanyName,cEntUser,dEntdt from Smt_Section inner join dg_hrpayroll.dbo.dg_pay_companyaccess as access on Smt_Section.nCompanyID=access.ca_compid where access.ca_accessuser='" + userName + "'", _sqlConnection);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                lst.Add(new DgPaySection()
                {
                    NSectionId = Convert.ToInt32(data.Rows[i]["nSectionID"]),
                    CSectionDescription = !string.IsNullOrEmpty(data.Rows[i]["cSection_Description"].ToString())? data.Rows[i]["cSection_Description"].ToString():string.Empty,
                    Secnamebangla = !string.IsNullOrEmpty(data.Rows[i]["secnamebangla"].ToString())? data.Rows[i]["secnamebangla"].ToString().Trim():string.Empty,
                    NUserDept = Convert.ToInt32(!string.IsNullOrEmpty(data.Rows[i]["nUserDept"].ToString())? data.Rows[i]["nUserDept"]:string.Empty),
                    NUserDeptName = !string.IsNullOrEmpty(data.Rows[i]["nUserDeptName"].ToString())? data.Rows[i]["nUserDeptName"].ToString():string.Empty,
                    NCompanyId = Convert.ToInt32(!string.IsNullOrEmpty(data.Rows[i]["nCompanyID"].ToString())? data.Rows[i]["nCompanyID"]:string.Empty),
                    NCompanyName = !string.IsNullOrEmpty(data.Rows[i]["nCompanyName"].ToString())? data.Rows[i]["nCompanyName"].ToString():string.Empty,
                    CEntUser = !string.IsNullOrEmpty(data.Rows[i]["cEntUser"].ToString())? data.Rows[i]["cEntUser"].ToString():string.Empty,
                    DEntdt = Convert.ToDateTime(!string.IsNullOrEmpty(data.Rows[i]["dEntdt"].ToString())? data.Rows[i]["dEntdt"]:DateTime.Now)
                });
            }
            return lst;
        }

        public async Task<ReturnObject> AddOrEditSection(SectionPayload obj)
        {
            var result = new ReturnObject();
            try
            {
                var dbData = new Section_DbModel();
                if (obj.sectionID == 0)
                {
                    var isExists = GetFirstOrDefault(x => x.nCompanyID == obj.companyID && x.nUserDept == obj.departmentID && x.cSection_Description == obj.sectionName && x.secnamebangla == obj.sectionNameBD);
                    if (isExists == null)
                    {
                        dbData = SectionPayload.PayloadToSectionDb_Obj(obj);
                        bool isSave = await AddAsync(dbData);
                        if (isSave)
                        {
                            result.IsSuccess = true;
                            result.Message = "Section Save Successfully !!";
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = "Can Not Saved Section !!";
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Section Already Exists !!";
                    }
                }
                else
                {
                    var dbObj = GetFirstOrDefault(x => x.nSectionID == obj.sectionID);
                    dbData = SectionPayload.PayloadToSectionDb_Obj(obj, dbObj);
                    bool isUpdate = await UpdateAsync(dbData);
                    if (isUpdate)
                    {
                        result.IsSuccess = true;
                        result.Message = "Section Update Successfully !!";
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Can Not Update Section !!";
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
        public async Task<ReturnObject<SectionList>> GetAllSectionByCompany(int compnayID)
        {
            var result = new ReturnObject<SectionList>();
            try
            {
                var sectionLs = await GetAsync(x => x.nCompanyID == compnayID);
                var userLs = await _payContext.Tbl_User.ToListAsync();
                var mainList = from a in sectionLs
                join b in userLs on (!string.IsNullOrEmpty(a?.cEntUser) ? a?.cEntUser.Trim() : string.Empty) equals b?.FullName.Trim() into userFullName
                from a1 in userFullName.DefaultIfEmpty()
                join c in userLs on a.cUpdateBy equals c.FullName.Trim() into userFullName2
                from a2 in userFullName2.DefaultIfEmpty()
                select (new SectionList
                {
                    sectionID = a.nSectionID,
                    companyID = a.nCompanyID,
                    companyName = a?.nCompanyName,
                    departmentID = a?.nUserDept,
                    departmentName = a?.nUserDeptName,
                    sectionName = a?.cSection_Description,
                    sectionNameBangla = !string.IsNullOrEmpty(a?.secnamebangla) ? a?.secnamebangla.Trim():null,
                    createBy = a1?.UserFullname.Trim(),
                    createDate = a?.dEntdt,
                    updateBy = a2?.UserFullname.Trim(),
                    updateDate = a?.cUpdatedt
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