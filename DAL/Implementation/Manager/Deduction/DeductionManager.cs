using BLL.Interfaces.Manager.Deduction;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Deduction;
using EF.Core.Repository.Manager;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementation.Manager.Deduction
{
    public class DeductionManager : CommonManager<Deduction_DbModel>,IDeductionManager
    {
        private readonly dg_hrpayrollContext _context;
        public DeductionManager(dg_hrpayrollContext context) : base( new DeductionRepository(context))
        {
            _context = context;
        }

        public async Task<ReturnObject> AddOrEditDeduction(DeductionPayload obj)
        {
            var result = new ReturnObject();
            try
            {
                var dbData = new Deduction_DbModel();
                if (obj.deductionID == 0)
                {
                    var isExists = GetFirstOrDefault(x => x.d_description == obj.deductionName);
                    if (isExists == null)
                    {
                        dbData = DeductionPayload.PayloadToAllowanceDb_Obj(obj);
                        bool isSave = await AddAsync(dbData);
                        if (isSave)
                        {
                            result.IsSuccess = true;
                            result.Message = "Deduction Save Successfully !!";
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = "Can Not Saved Deduction !!";
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Deduction Already Exists !!";
                    }
                }
                else
                {
                    var dbObj = GetFirstOrDefault(x => x.d_code == obj.deductionID);
                    dbData = DeductionPayload.PayloadToAllowanceDb_Obj(obj, dbObj);
                    bool isUpdate = await UpdateAsync(dbData);
                    if (isUpdate)
                    {
                        result.IsSuccess = true;
                        result.Message = "Deduction Update Successfully !!";
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Can Not Update Deduction !!";
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
        public async Task<ReturnObject<Deduction_DbModel>> GetAllDeduction()
        {
            var result = new ReturnObject<Deduction_DbModel>();
            try
            {
                var allowanceLs = await GetAllAsync();
                var userLs = await _context.Tbl_User.ToListAsync();
                var mainList = from a in allowanceLs
                join b in userLs on (!string.IsNullOrEmpty(a?.d_user) ? a?.d_user.Trim() : string.Empty) equals b?.FullName.Trim() into userFullName
                from a1 in userFullName.DefaultIfEmpty()
                join c in userLs on a.d_updateBy equals c.FullName.Trim() into userFullName2
                from a2 in userFullName2.DefaultIfEmpty()
                select (new Deduction_DbModel
                {
                    d_code = a.d_code,
                    d_groupid = a.d_groupid,
                    d_description = a?.d_description,
                    d_type = a?.d_type,
                    d_user = a1?.UserFullname.Trim(),
                    d_udate = a?.d_udate,
                    d_updateBy = a2?.UserFullname.Trim(),
                    d_updatedt = a?.d_updatedt
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
