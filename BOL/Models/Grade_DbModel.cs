using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Grade_DbModel
    {
        [Key]
        public int Grd_id { get; set; }
        public string Grd_Name { get; set; } = null;
        public int? Grd_Groupid { get; set; } = null;
        public string Grd_CreateBy { get; set; } = null;
        public DateTime? Grd_Createdt { get; set; } = null;
        public string Grd_UpdateBy { get; set; } = null;
        public DateTime? Grd_Updatedt { get; set; } = null;
    }
    public class GradePayload
    {
        [Required(ErrorMessage = "Grade ID Is Required !!")]
        public int gradeID { get; set; }
        [Required(ErrorMessage = "Grade Name Is Required !!")]
        public string gradeName { get; set; }
        [Required(ErrorMessage = "Create User Name Is Required !!")]
        public string userName { get; set; }

        public static Grade_DbModel PayloadToSalaryGradeDb_Obj(GradePayload obj, Grade_DbModel uObj=null)
        {
            var dbModel = new Grade_DbModel
            {
                Grd_id = obj.gradeID,
                Grd_Name = obj.gradeName,
                Grd_Groupid = 11,
                Grd_CreateBy = uObj == null ? obj.userName : uObj.Grd_CreateBy,
                Grd_Createdt = uObj == null ? DateTime.Now : uObj.Grd_Createdt,
                Grd_UpdateBy = uObj == null ? null : obj.userName,
                Grd_Updatedt = uObj == null ? null : DateTime.Now,
            };
            return dbModel;
        }
    }
    public class DgPayGrade
    {
        public int GrdId { get; set; }
        public string GrdName { get; set; } = null;
        public int? GrdGroupid { get; set; } = null;

        public static Grade_DbModel CustonToDbModel(DgPayGrade obj)
        {
            try
            {
                var dbModel = new Grade_DbModel
                {
                    Grd_id = obj.GrdId,
                    Grd_Name = obj.GrdName,
                    Grd_Groupid = obj.GrdGroupid
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayGrade DbToCustomModel(Grade_DbModel obj)
        {
            try
            {
                var customModel = new DgPayGrade
                {
                    GrdId = obj.Grd_id,
                    GrdName = obj.Grd_Name,
                    GrdGroupid = obj.Grd_Groupid
                };
                return customModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
    }
}