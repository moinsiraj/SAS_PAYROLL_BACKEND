using BLL.Interfaces.Manager.Allowancesde;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Allowancesde;
using EF.Core.Repository.Manager;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementation.Manager.Allowancesde
{
    public class AllowancesdeManager : CommonManager<Allowancesde_DbModel>, IAllowancesdeManager
    {
        private readonly dg_hrpayrollContext _context;
        public AllowancesdeManager(dg_hrpayrollContext context) : base(new AllowancesdeRepository(context))
        {
            _context = context;
        }

        public async Task<ReturnObject> AddOrEditAllowance(AllowancesdepPayload obj)
        {
            var result = new ReturnObject();
            try
            {
                var dbData = new Allowancesde_DbModel();
                if (obj.allowanceID == 0)
                {
                    var isExists = GetFirstOrDefault(x => x.al_description == obj.allowanceName);
                    if (isExists == null)
                    {
                        dbData = AllowancesdepPayload.PayloadToAllowanceDb_Obj(obj);
                        bool isSave = await AddAsync(dbData);
                        if (isSave)
                        {
                            result.IsSuccess = true;
                            result.Message = "Allowance Save Successfully !!";
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = "Can Not Saved Allowance !!";
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Allowance Already Exists !!";
                    }
                }
                else
                {
                    var dbObj = GetFirstOrDefault(x => x.al_code == obj.allowanceID);
                    dbData = AllowancesdepPayload.PayloadToAllowanceDb_Obj(obj, dbObj);
                    bool isUpdate = await UpdateAsync(dbData);
                    if (isUpdate)
                    {
                        result.IsSuccess = true;
                        result.Message = "Allowance Update Successfully !!";
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Can Not Update Allowance !!";
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
        public async Task<ReturnObject<Allowancesde_DbModel>> GetAllAllowances()
        {
            var result = new ReturnObject<Allowancesde_DbModel>();
            try
            {
                var allowanceLs = await GetAllAsync();
                var userLs = await _context.Tbl_User.ToListAsync();
                var mainList = from a in allowanceLs
                join b in userLs on (!string.IsNullOrEmpty(a?.al_user) ? a?.al_user.Trim() : string.Empty) equals b?.FullName.Trim() into userFullName
                from a1 in userFullName.DefaultIfEmpty()
                join c in userLs on a.al_updateBy equals c.FullName.Trim() into userFullName2
                from a2 in userFullName2.DefaultIfEmpty()
                select (new Allowancesde_DbModel
                {
                    al_code = a.al_code,
                    al_groupid = a.al_groupid,
                    al_description = a?.al_description,
                    al_type = a?.al_type,
                    al_user = a1?.UserFullname.Trim(),
                    al_udate = a?.al_udate,
                    al_updateBy = a2?.UserFullname.Trim(),
                    al_updatedt = a?.al_updatedt
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
