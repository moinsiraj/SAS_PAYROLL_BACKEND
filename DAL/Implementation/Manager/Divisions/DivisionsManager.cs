using BLL.Interfaces.Manager.Divisions;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Divisions;
using EF.Core.Repository.Manager;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementation.Manager.Divisions
{
    public class DivisionsManager : CommonManager<Division_DbModel>,IDivisionsManager
    {
        private readonly dg_hrpayrollContext _context;
        public DivisionsManager(dg_hrpayrollContext context, Dg_Common dgCommon) : base(new DivisionsRepository(context))
        {
            _context = context;
        }
        public async Task<ReturnObject> AddOrEditDivision(DivisionPayload obj)
        {
            var result = new ReturnObject();
            try
            {
                var dbData = new Division_DbModel();
                if (obj.divID == 0)
                {
                    var isExists = GetFirstOrDefault(x => x.div_name == obj.divName);
                    if (isExists == null)
                    {
                        dbData = DivisionPayload.PayloadToDivisionDb_Obj(obj);
                        bool isSave = await AddAsync(dbData);
                        if (isSave)
                        {
                            result.Message = "Division Save Successfully !!";
                            result.IsSuccess = true;
                        }
                        else
                        {
                            result.Message = "Can Not Saved Division !!";
                            result.IsSuccess = false;
                        }
                    }
                    else
                    {
                        result.Message = "Division Already Exists !!";
                        result.IsSuccess = false;
                    }
                }
                else
                {
                    var isExists = GetFirstOrDefault(x => x.div_id == obj.divID);
                    dbData = DivisionPayload.PayloadToDivisionDb_Obj(obj, isExists);
                    bool isUpdate = await UpdateAsync(dbData);
                    if (isUpdate)
                    {
                        result.Message = "Division Update Successfully !!";
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.Message = "Can Not Update Division !!";
                        result.IsSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.Message = "Something Went Wrong !!";
                result.IsSuccess = false;
            }
            return result;
        }
        public async Task<ReturnObject<Division_DbModel>> GetAllDivisions()
        {
            var result = new ReturnObject<Division_DbModel>();
            try
            {
                var DivisionLs = await GetAllAsync();
                var userLs = await _context.Tbl_User.ToListAsync();
                var mainList = from a in DivisionLs
                join b in userLs on (!string.IsNullOrEmpty(a?.div_user) ? a?.div_user.Trim() : string.Empty) equals b?.FullName.Trim() into userFullName
                from a1 in userFullName.DefaultIfEmpty()
                join c in userLs on a?.div_updateBy equals c?.FullName.Trim() into userFullName2
                from a2 in userFullName2.DefaultIfEmpty()
                select (new Division_DbModel
                {
                    div_id = a.div_id,
                    div_name = a?.div_name,
                    div_name_bangla = a?.div_name_bangla,
                    div_groupid = a?.div_groupid,
                    div_user = a1?.UserFullname.Trim(),
                    div_udate = a?.div_udate,
                    div_updateBy = a2?.UserFullname.Trim(),
                    div_update = a?.div_update
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
                    result.Message = "Data Not Loaded !!";
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
        public async Task<ReturnObject<Division_DbModel>> GetDivisionById(int id)
        {
            var result = new ReturnObject<Division_DbModel>();
            try
            {
                if (id > 0)
                {
                    var data = await GetFirstOrDefaultAsync(x => x.div_id == id);
                    if (data != null)
                    {
                        result.IsSuccess = true;
                        result.Message = "Data Loaded Successfully !!";
                        result.SingleData = data;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Data Not Loaded !!";
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Division Id Not Found !!";
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
