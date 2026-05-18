using BLL.Interfaces.Manager.Districts;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Districts;
using EF.Core.Repository.Manager;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementation.Manager.Districts
{
    public class DistrictsManager : CommonManager<District_DbModel>,IDistrictsManager
    {
        private readonly dg_hrpayrollContext _context;
        public DistrictsManager(dg_hrpayrollContext context) : base(new DistrictsRepository(context))
        {
            _context = context;
        }

        public async Task<ReturnObject> AddOrEditDistrict(DistrictPayload obj)
        {
            var result = new ReturnObject();
            try
            {
                var dbData = new District_DbModel();
                if (obj.dis_id == 0)
                {
                    var isExists = GetFirstOrDefault(x => x.di_divid == obj.div_id && x.di_name == obj.dis_name && x.di_nameBangla == obj.dis_nameBN);
                    if (isExists == null)
                    {
                        dbData = DistrictPayload.PayloadToDistrictDb_Obj(obj);
                        bool isSave = await AddAsync(dbData);
                        if (isSave)
                        {
                            result.IsSuccess = true;
                            result.Message = "District Save Successfully !!";                            
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = "Can Not Saved District !!";
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "District Already Exists !!";
                    }
                }
                else
                {
                    var dbObj = GetFirstOrDefault(x => x.di_id == obj.dis_id);
                    dbData = DistrictPayload.PayloadToDistrictDb_Obj(obj, dbObj);
                    bool isUpdate = await UpdateAsync(dbData);
                    if (isUpdate)
                    {
                        result.IsSuccess = true;
                        result.Message = "District Update Successfully !!";
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Can Not Update District !!";
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
        public async Task<ReturnObject<District_DbModel>> GetAllDistrict()
        {
            var result = new ReturnObject<District_DbModel>();
            try
            {
                var districtLs = await GetAllAsync();
                var userLs = await _context.Tbl_User.ToListAsync();
                var mainList = from a in districtLs
                join b in userLs on (!string.IsNullOrEmpty(a?.di_user) ? a?.di_user.Trim() : string.Empty) equals b?.FullName.Trim() into userFullName
                from a1 in userFullName.DefaultIfEmpty()
                join c in userLs on a.di_updateBy equals c.FullName.Trim() into userFullName2
                from a2 in userFullName2.DefaultIfEmpty()
                select (new District_DbModel
                {
                    di_id = a.di_id,
                    di_groupid = a?.di_groupid,
                    di_divid = a?.di_divid,
                    di_divname = a?.di_divname,
                    di_name = a?.di_name,
                    di_nameBangla = a?.di_nameBangla,
                    di_user = a1?.UserFullname.Trim(),
                    di_udate = a?.di_udate,
                    di_updateBy = a2?.UserFullname.Trim(),
                    di_updateDate = a?.di_updateDate
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