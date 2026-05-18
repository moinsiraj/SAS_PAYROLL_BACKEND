using BLL.Interfaces.Manager.Thanas;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Thanas;
using EF.Core.Repository.Manager;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementation.Manager.Thanas
{
    public class ThanasManager : CommonManager<Thana_DbModel>,IThanasManager
    {
        private readonly dg_hrpayrollContext _context;
        public ThanasManager(dg_hrpayrollContext context):base(new ThanasRepository(context))
        {
            _context = context;
        }
        
        public async Task<ReturnObject> AddOrEditThana(ThanaPayload obj)
        {
            var result = new ReturnObject();
            try
            {
                var dbModel = new Thana_DbModel();
                if (obj.thanaID == 0)
                {
                    var isExists = GetFirstOrDefault(x => x.th_division == obj.divisionID && x.th_district == obj.districtID && x.th_name == obj.thanaName && x.th_nameBangla == obj.thanaNameBangla);
                    if (isExists == null)
                    {
                        dbModel = ThanaPayload.PayloadToThanaDb_Obj(obj);
                        bool isSave = await AddAsync(dbModel);
                        if (isSave)
                        {
                            result.IsSuccess = true;
                            result.Message = "Thana Save Successfully !!";
                        }
                        else
                        {
                            result.Message = "Can Not Saved Thana !!";
                        }
                    }
                    else
                    {
                        result.Message = "Thana Already Exists !!";
                    }
                }
                else
                {
                    var dbObj = GetFirstOrDefault(x => x.th_id == obj.thanaID);
                    dbModel = ThanaPayload.PayloadToThanaDb_Obj(obj, dbObj);
                    bool isUpdate = await UpdateAsync(dbModel);
                    if (isUpdate)
                    {
                        result.IsSuccess = true;
                        result.Message = "Thana Update Successfully !!";
                    }
                    else
                    {
                        result.Message = "Can Not Update Thana !!";
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.Message = "Something Went Wrong !!";
            }
            return result;
        }
        public async Task<ReturnObject<Thana_DbModel>> GetAllThana()
        {
            var result = new ReturnObject<Thana_DbModel>();
            try
            {
                var thanaLS = await GetAllAsync();
                var userLs = await _context.Tbl_User.ToListAsync();
                var mainList = from a in thanaLS
                join b in userLs on (!string.IsNullOrEmpty(a?.th_user) ? a?.th_user.Trim() : string.Empty) equals b?.FullName.Trim() into userFullName
                from a1 in userFullName.DefaultIfEmpty()
                join c in userLs on a?.th_updateBy equals c?.FullName.Trim() into userFullName2
                from a2 in userFullName2.DefaultIfEmpty()
                select (new Thana_DbModel
                {
                    th_id = a.th_id,
                    th_groupid = a.th_groupid,
                    th_division = a.th_division,
                    th_divname = a.th_divname,
                    th_district = a.th_district,
                    th_disname = a.th_disname,
                    th_name = a.th_name,
                    th_nameBangla = a.th_nameBangla,
                    th_user = a1?.UserFullname,
                    th_udate = a?.th_udate,
                    th_updateBy = a2?.UserFullname,
                    th_upDate = a?.th_upDate
                });
                if (mainList.ToList() != null)
                {
                    result.IsSuccess = true;
                    result.Message = "Data Loaded Successfully !!";
                    result.ListData = mainList.ToList();
                }
                else
                {
                    result.Message = "Data Loaded Fail !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.Message = "Something Went Wrong !!";
            }
            return result;
        }
    }
}
