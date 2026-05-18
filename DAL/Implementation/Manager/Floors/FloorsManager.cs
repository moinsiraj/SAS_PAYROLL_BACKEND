using BLL.Interfaces.Manager.Floors;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Floors;
using EF.Core.Repository.Manager;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.Floors
{
    public class FloorsManager : CommonManager<Floor_DbModel>,IFloorsManager
    {
        private readonly dg_SpecFoContext _context;
        private readonly dg_hrpayrollContext _payContext;
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _connection;
        public FloorsManager(dg_SpecFoContext context, dg_hrpayrollContext payContext, Dg_Common dgCommon) : base(new FloorsRepository(context))
        {
            _context = context;
            _payContext = payContext;
            _dgCommon = dgCommon;
            _connection = new SqlConnection(Getway.SpecFoCon);
        }

        public async Task<List<DgPayFloor>> GetFloorByUserName(string userName)
        {
            var lst = new List<DgPayFloor>();
            var data = await _dgCommon.get_InformationDataTableAsync("select * from Smt_Floor inner join dg_hrpayroll.dbo.dg_pay_companyaccess as compAccs on Smt_Floor.CompanyID=compAccs.ca_compid where compAccs.ca_accessuser='"+ userName + "'", _connection);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                lst.Add(new DgPayFloor()
                {
                    NFloor = Convert.ToInt32(data.Rows[i]["nFloor"]),
                    CFloorDescriptin = data.Rows[i]["cFloor_Descriptin"].ToString(),
                    CFloorDescriptinBangla = data.Rows[i]["cFloor_Descriptin_bangla"].ToString(),
                    CEntUser = data.Rows[i]["cEntUser"].ToString(),
                    DEntdt = Convert.ToDateTime(data.Rows[i]["dEntdt"].ToString()),
                    BuildingUid = Convert.ToInt32(data.Rows[i]["BuildingUID"]),
                    BuildingUiname = data.Rows[i]["BuildingUIName"].ToString(),
                    CompanyId = Convert.ToInt32(data.Rows[i]["CompanyID"]),
                    CompanyName = data.Rows[i]["CompanyName"].ToString(),
                    CFloorDescriptinBd = data.Rows[i]["cFloor_DescriptinBD"].ToString().Trim()
                });
            }
            return lst;
        }
        public async Task<ReturnObject> AddOrEditFloor(FloorPayload obj)
        {
            var result = new ReturnObject();
            try
            {
                var dbData = new Floor_DbModel();
                if (obj.floorID == 0)
                {
                    var isExists = GetFirstOrDefault(x => x.CompanyID == obj.companyID && x.BuildingUID == obj.buildingID && x.cFloor_Descriptin == obj.floorName && x.cFloor_Descriptin_bangla == obj.floorNameBD);
                    if (isExists == null)
                    {
                        dbData = FloorPayload.PayloadToFloorDb_Obj(obj);
                        bool isSave = await AddAsync(dbData);
                        if (isSave)
                        {
                            result.IsSuccess = true;
                            result.Message = "Floor Save Successfully !!";
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = "Can Not Saved Floor !!";
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Floor Already Exists !!";
                    }
                }
                else
                {
                    var dbObj = GetFirstOrDefault(x => x.nFloor == obj.floorID);
                    dbData = FloorPayload.PayloadToFloorDb_Obj(obj, dbObj);
                    bool isUpdate = await UpdateAsync(dbData);
                    if (isUpdate)
                    {
                        result.IsSuccess = true;
                        result.Message = "Floor Update Successfully !!";
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Can Not Update Floor !!";
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
        public async Task<ReturnObject<Floor_DbModel>> GetAllFloorByCompany(int compnayID)
        {
            var result = new ReturnObject<Floor_DbModel>();
            try
            {
                var floor = await GetAsync(x => x.CompanyID == compnayID);
                var userLs = await _payContext.Tbl_User.ToListAsync();
                var compList = await _context.Smt_Company.ToListAsync();
                var buildingList = await _context.Smt_BuildingUnit.ToListAsync();
                var mainList = from a in floor
                join b in userLs on (!string.IsNullOrEmpty(a?.cEntUser) ? a?.cEntUser.Trim() : string.Empty) equals b?.FullName.Trim() into userFullName
                join comp in compList on a.CompanyID equals comp.nCompanyID
                join build in buildingList on a.BuildingUID equals build.Unit_Code
                from a1 in userFullName.DefaultIfEmpty()
                join c in userLs on a.cUpdateBy equals c.FullName.Trim() into userFullName2
                from a2 in userFullName2.DefaultIfEmpty()
                select (new Floor_DbModel
                {
                    nFloor = a.nFloor,
                    cFloor_Descriptin = a.cFloor_Descriptin,
                    cFloor_Descriptin_bangla = !string.IsNullOrEmpty(a?.cFloor_Descriptin_bangla) ? a?.cFloor_Descriptin_bangla.Trim() : null,
                    CompanyID = a?.CompanyID,
                    CompanyName = comp?.cCmpName,
                    BuildingUID = a?.BuildingUID,
                    BuildingUIName = build?.Unit_Name,
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
