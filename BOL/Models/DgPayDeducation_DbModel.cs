using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class DgPayDeducation_DbModel
    {
        [Key]
        public int d_serial { get; set; }
        public decimal? d_emp_serial { get; set; } = null;
        public int? d_groupid { get; set; } = null;
        public int? d_compid { get; set; } = null;
        public DateTime? d_date { get; set; } = null;
        public int? d_id { get; set; } = null;
        public decimal? d_value { get; set; } = null;
        public string d_user { get; set; } = null;
        public DateTime? d_udate { get; set; } = null;
    }
    public class DgPayDeducationPostPayload
    {
        public int DEmpSerial { get; set; }
        public int DGroupid { get; set; }
        public int DCompid { get; set; }
        public string DDate { get; set; }
        public List<DgPayDeducationPostPayloadChild> DeducationChild { get; set; }
        public string DUser { get; set; }
    }
    public class DgPayDeducationPostPayloadChild
    {
        public int DId { get; set; }
        public decimal? DValue { get; set; }
    }
    public class DgPayDeducation
    {
        public int DSerial { get; set; }
        public decimal? DEmpSerial { get; set; } = null;
        public int? DGroupid { get; set; } = null;
        public int? DCompid { get; set; } = null;
        public DateTime? DDate { get; set; } = null;
        public int? DId { get; set; } = null;
        public decimal? DValue { get; set; } = null;
        public string DUser { get; set; } = null;
        public DateTime? DUdate { get; set; } = null;

        public static DgPayDeducation_DbModel CustonToDbModel(DgPayDeducation obj)
        {
            try
            {
                var dbModel = new DgPayDeducation_DbModel
                {
                    d_serial = obj.DSerial,
                    d_emp_serial = obj.DEmpSerial,
                    d_groupid = obj.DGroupid,
                    d_compid = obj.DCompid,
                    d_date = obj.DDate,
                    d_id = obj.DId,
                    d_value = obj.DValue,
                    d_user = obj.DUser,
                    d_udate = obj.DUdate
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayDeducation DbToCustomModel(DgPayDeducation_DbModel obj)
        {
            try
            {
                var customModel = new DgPayDeducation
                {
                    DSerial = obj.d_serial,
                    DEmpSerial = obj.d_emp_serial,
                    DGroupid = obj.d_groupid,
                    DCompid = obj.d_compid,
                    DDate = obj.d_date,
                    DId = obj.d_id,
                    DValue = obj.d_value,
                    DUser = obj.d_user,
                    DUdate = obj.d_udate
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