using BLL.Interfaces.Manager.SalaryCategories;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.SalaryCategories;
using EF.Core.Repository.Manager;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementation.Manager.SalaryCategories
{
    public class SalaryCategoriesManager : CommonManager<Salarycategory_DbModel>,ISalaryCategoriesManager
    {
        private readonly dg_hrpayrollContext _context;
        public SalaryCategoriesManager(dg_hrpayrollContext context):base(new SalaryCategoriesRepository(context))
        {
            _context = context;
        }

        public async Task<ReturnObject> AddOrEditSalaryCategory(SalarycategoryPayload obj)
        {
            var result = new ReturnObject();
            try
            {
                var dbData = new Salarycategory_DbModel();
                if (obj.salCatID == 0)
                {
                    var isExists = GetFirstOrDefault(x => x.cat_compid == obj.companyID && x.cat_name == obj.salCatName);
                    if (isExists == null)
                    {
                        dbData = SalarycategoryPayload.PayloadToSalaryCategoryDb_Obj(obj);
                        bool isSave = await AddAsync(dbData);
                        if (isSave)
                        {
                            result.IsSuccess = true;
                            result.Message = "Salary Category Save Successfully !!";
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = "Can Not Saved Salary Category !!";
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Salary Category Already Exists !!";
                    }
                }
                else
                {
                    var dbObj = GetFirstOrDefault(x => x.cat_id == obj.salCatID);
                    dbData = SalarycategoryPayload.PayloadToSalaryCategoryDb_Obj(obj, dbObj);
                    bool isUpdate = await UpdateAsync(dbData);
                    if (isUpdate)
                    {
                        result.IsSuccess = true;
                        result.Message = "Salary Category Update Successfully !!";
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Can Not Update Salary Category !!";
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
        public async Task<ReturnObject<Salarycategory_DbModel>> GetAllSalaryCategoryByCompany(int companyID)
        {
            var result = new ReturnObject<Salarycategory_DbModel>();
            try
            {
                var allowanceLs = await GetAsync(x => x.cat_compid == companyID);
                var userLs = await _context.Tbl_User.ToListAsync();
                var mainList = from a in allowanceLs
                join b in userLs on (!string.IsNullOrEmpty(a?.cat_createBy) ? a?.cat_createBy.Trim() : string.Empty) equals b?.FullName.Trim() into userFullName
                from a1 in userFullName.DefaultIfEmpty()
                join c in userLs on a.cat_updateBy equals c.FullName.Trim() into userFullName2
                from a2 in userFullName2.DefaultIfEmpty()
                select (new Salarycategory_DbModel
                {
                    cat_id = a.cat_id,
                    ct_groupid = a.ct_groupid,
                    cat_compid = a?.cat_compid,
                    cat_compname = a?.cat_compname,
                    cat_name = a?.cat_name,
                    cat_ot = a?.cat_ot,
                    cat_createBy = a1?.UserFullname.Trim(),
                    cat_createdt = a?.cat_createdt,
                    cat_updateBy = a2?.UserFullname.Trim(),
                    cat_updatedt = a?.cat_updatedt
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