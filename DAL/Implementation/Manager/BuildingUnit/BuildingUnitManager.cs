using BLL.Interfaces.Manager.BuildingUnit;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.BuildingUnit;
using EF.Core.Repository.Manager;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.BuildingUnit
{
    public class BuildingUnitManager : CommonManager<BuildingUnit_DbModel>,IBuildingUnitManager
    {
        private readonly dg_SpecFoContext _context;
        private readonly dg_hrpayrollContext _payContext;
        private readonly Dg_Common _dgCommon;
        SqlConnection _connection;
        public BuildingUnitManager(dg_SpecFoContext context, dg_hrpayrollContext payContext, Dg_Common dgCommon) : base( new BuildingUnitRepository(context))
        {
            _context = context;
            _payContext = payContext;
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.SpecFoCon);
        }

        public async Task<List<DgPayBuildingUnit>> GetBuildingUnitsByUser(string userName)
        {
            var lst = new List<DgPayBuildingUnit>();
            var data = await _dgCommon.get_InformationDataTableAsync("select * from Smt_BuildingUnit inner join dg_hrpayroll.dbo.dg_pay_companyaccess as compAccs on Smt_BuildingUnit.Company_ID=compAccs.ca_compid where compAccs.ca_accessuser='"+ userName + "'", _connection);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                lst.Add(new DgPayBuildingUnit()
                {
                    UnitCode = Convert.ToInt32(data.Rows[i]["Unit_Code"]),
                    UnitName = data.Rows[i]["Unit_Name"].ToString(),
                    CompanyId = Convert.ToInt32(data.Rows[i]["Company_ID"]),
                    CompanyName = data.Rows[i]["Company_Name"].ToString(),
                    CreatedBy = data.Rows[i]["CreatedBy"].ToString(),
                    Createddt = Convert.ToDateTime(data.Rows[i]["Createddt"].ToString())
                });
            }
            return lst;
        }
        public async Task<ReturnObject> AddOrEditBuildingUnit(BuildingUnitPayload obj)
        {
            var result = new ReturnObject();
            try
            {
                var dbData = new BuildingUnit_DbModel();
                if (obj.unitCode == 0)
                {
                    var isExists = GetFirstOrDefault(x => x.Company_ID == obj.companyID && x.Unit_Name == obj.unitName);
                    if (isExists == null)
                    {
                        dbData = BuildingUnitPayload.PayloadToBuildingUnitDb_Obj(obj);
                        bool isSave = await AddAsync(dbData);
                        if (isSave)
                        {
                            result.IsSuccess = true;
                            result.Message = "BuildingUnit Save Successfully !!";
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = "Can Not Saved BuildingUnit !!";
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "BuildingUnit Already Exists !!";
                    }
                }
                else
                {
                    var dbObj = GetFirstOrDefault(x => x.Unit_Code == obj.unitCode);
                    dbData = BuildingUnitPayload.PayloadToBuildingUnitDb_Obj(obj, dbObj);
                    bool isUpdate = await UpdateAsync(dbData);
                    if (isUpdate)
                    {
                        result.IsSuccess = true;
                        result.Message = "BuildingUnit Update Successfully !!";
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Can Not Update BuildingUnit !!";
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
        public async Task<ReturnObject<BuildingUnit_DbModel>> GetAllBuildingUnitByCompany(int compnayID)
        {
            var result = new ReturnObject<BuildingUnit_DbModel>();
            try
            {
                var buildingUnitLs = await GetAsync(x => x.Company_ID == compnayID);
                var userLs = await _payContext.Tbl_User.ToListAsync();
                var compList = await _context.Smt_Company.ToListAsync();
                var mainList = from a in buildingUnitLs
                join b in userLs on (!string.IsNullOrEmpty(a?.CreatedBy) ? a?.CreatedBy.Trim() : string.Empty) equals b?.FullName.Trim() into userFullName
                from a1 in userFullName.DefaultIfEmpty()
                join c in userLs on a.UpdatedBy equals c.FullName.Trim() into userFullName2
                join com in compList on a.Company_ID equals com.nCompanyID
                from a2 in userFullName2.DefaultIfEmpty()
                select (new BuildingUnit_DbModel
                {
                    Unit_Code = a.Unit_Code,
                    Unit_Name = a.Unit_Name,
                    Company_ID = a?.Company_ID,
                    Company_Name = com?.cCmpName,
                    CreatedBy = a1?.UserFullname.Trim(),
                    Createddt = a?.Createddt,
                    UpdatedBy = a2?.UserFullname.Trim(),
                    Updateddt = a?.Updateddt
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