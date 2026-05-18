using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Deduction_DbModel
    {
        [Key]
        public int d_code { get; set; }
        public int? d_groupid { get; set; } = null;
        public string d_description { get; set; } = null;
        public string d_type { get; set; } = null;
        public string d_user { get; set; } = null;
        public DateTime? d_udate { get; set; } = null;
        public string d_updateBy { get; set; } = null;
        public DateTime? d_updatedt { get; set; } = null;
    }
    public class DeductionPayload
    {
        [Required(ErrorMessage = "Deduction ID Is Required !!")]
        public int deductionID { get; set; }
        [Required(ErrorMessage = "Deduction Name Is Required !!")]
        public string deductionName { get; set; }
        [Required(ErrorMessage = "Deduction Type Is Required !!")]
        public string deductionType { get; set; }
        [Required(ErrorMessage = "Create User Name Is Required !!")]
        public string userName { get; set; }

        public static Deduction_DbModel PayloadToAllowanceDb_Obj(DeductionPayload obj, Deduction_DbModel uObj=null)
        {
            var dbModel = new Deduction_DbModel
            {
                d_code = obj.deductionID,
                d_groupid = 11,
                d_description = obj.deductionName,
                d_type = obj.deductionType,
                d_user = uObj == null ? obj.userName : uObj.d_user,
                d_udate = uObj == null ? DateTime.Now : uObj.d_udate,
                d_updateBy = uObj == null ? null : obj.userName,
                d_updatedt = uObj == null ? null : DateTime.Now,
            };
            return dbModel;
        }
    }
    public class DgPayDeductionsde
    {
        public int DCode { get; set; }
        public string DDescription { get; set; } = null;
        public string DType { get; set; } = null;
        public string DUser { get; set; } = null;
        public DateTime? DUdate { get; set; } = null;
        public int? DGroupid { get; set; } = null;

        public static Deduction_DbModel CustonToDbModel(DgPayDeductionsde obj)
        {
            try
            {
                var dbModel = new Deduction_DbModel
                {
                    d_code = obj.DCode,
                    d_description = obj.DDescription,
                    d_type = obj.DType,
                    d_user = obj.DUser,
                    d_udate = obj.DUdate,
                    d_groupid = obj.DGroupid
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayDeductionsde DbToCustomModel(Deduction_DbModel obj)
        {
            try
            {
                var customModel = new DgPayDeductionsde
                {
                    DCode = obj.d_code,
                    DDescription = obj.d_description,
                    DType = obj.d_type,
                    DUser = obj.d_user,
                    DUdate = obj.d_udate,
                    DGroupid = obj.d_groupid
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