using BLL.Interfaces.Manager.Grades;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Grades;
using EF.Core.Repository.Manager;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementation.Manager.Grades
{
    public class GradesManager : CommonManager<Grade_DbModel>,IGradesManager
    {
        private readonly dg_hrpayrollContext _context;
        public GradesManager(dg_hrpayrollContext context) : base(new GradesRepository(context))
        {
            _context = context;
        }

        public async Task<ReturnObject> AddOrEditGrade(GradePayload obj)
        {
            var result = new ReturnObject();
            try
            {
                var dbData = new Grade_DbModel();
                if (obj.gradeID == 0)
                {
                    var isExists = GetFirstOrDefault(x => x.Grd_Name == obj.gradeName);
                    if (isExists == null)
                    {
                        dbData = GradePayload.PayloadToSalaryGradeDb_Obj(obj);
                        bool isSave = await AddAsync(dbData);
                        if (isSave)
                        {
                            result.IsSuccess = true;
                            result.Message = "Grade Save Successfully !!";
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = "Can Not Saved Grade !!";
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Grade Already Exists !!";
                    }
                }
                else
                {
                    var dbObj = GetFirstOrDefault(x => x.Grd_id == obj.gradeID);
                    dbData = GradePayload.PayloadToSalaryGradeDb_Obj(obj, dbObj);
                    bool isUpdate = await UpdateAsync(dbData);
                    if (isUpdate)
                    {
                        result.IsSuccess = true;
                        result.Message = "Grade Update Successfully !!";
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Can Not Update Grade !!";
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
        public async Task<ReturnObject<Grade_DbModel>> GetAllGrade()
        {
            var result = new ReturnObject<Grade_DbModel>();
            try
            {
                var allowanceLs = await GetAllAsync();
                var userLs = await _context.Tbl_User.ToListAsync();
                var mainList = from a in allowanceLs
                join b in userLs on (!string.IsNullOrEmpty(a?.Grd_CreateBy) ? a?.Grd_CreateBy.Trim() : string.Empty) equals b?.FullName.Trim() into userFullName
                from a1 in userFullName.DefaultIfEmpty()
                join c in userLs on a.Grd_UpdateBy equals c.FullName.Trim() into userFullName2
                from a2 in userFullName2.DefaultIfEmpty()
                select (new Grade_DbModel
                {
                    Grd_id = a.Grd_id,
                    Grd_Name = a.Grd_Name,
                    Grd_Groupid = a?.Grd_Groupid,
                    Grd_CreateBy = a1?.UserFullname.Trim(),
                    Grd_Createdt = a?.Grd_Createdt,
                    Grd_UpdateBy = a2?.UserFullname.Trim(),
                    Grd_Updatedt = a?.Grd_Updatedt
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
