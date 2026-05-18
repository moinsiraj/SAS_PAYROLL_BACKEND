using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class AnnualLeaveAllucation_DbModel
    {
        [Key]
        public int serial { get; set; }
        public int year { get; set; }
        public int comid { get; set; }
        public string comName { get; set; } = null;
        public int? groupid { get; set; } = null;
        public int? casual { get; set; } = null;
        public int? madical { get; set; } = null;
        public int? annual { get; set; } = null;
        public string enteredby { get; set; } = null;
        public string updatedby { get; set; } = null;
        public DateTime? addtime { get; set; } = null;
        public DateTime? updatedtime { get; set; } = null;
    }
    public class DgAnnualLeaveAllucation
    {
        public int Serial { get; set; }
        public int Year { get; set; }
        public int Comid { get; set; }
        public string ComName { get; set; } = null;
        public int? Groupid { get; set; } = null;
        public int? Casual { get; set; } = null;
        public int? Madical { get; set; } = null;
        public int? Annual { get; set; } = null;
        public string Enteredby { get; set; } = null;
        public string Updatedby { get; set; } = null;
        public DateTime? Addtime { get; set; } = null;
        public DateTime? Updatedtime { get; set; } = null;

        public static AnnualLeaveAllucation_DbModel CustonToDbModel(DgAnnualLeaveAllucation obj)
        {
            try
            {
                var dbModel = new AnnualLeaveAllucation_DbModel
                {
                    serial = obj.Serial,
                    year = obj.Year,
                    comid = obj.Comid,
                    comName = obj.ComName,
                    groupid = obj.Groupid,
                    casual = obj.Casual,
                    madical = obj.Madical,
                    annual = obj.Annual,
                    enteredby = obj.Enteredby,
                    updatedby = obj.Updatedby,
                    addtime = obj.Addtime,
                    updatedtime = obj.Updatedtime
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgAnnualLeaveAllucation DbToCustomModel(AnnualLeaveAllucation_DbModel obj)
        {
            try
            {
                var customModel = new DgAnnualLeaveAllucation
                {
                    Serial = obj.serial,
                    Year = obj.year,
                    Comid = obj.comid,
                    ComName = obj.comName,
                    Groupid = obj.groupid,
                    Casual = obj.casual,
                    Madical = obj.madical,
                    Annual = obj.annual,
                    Enteredby = obj.enteredby,
                    Updatedby = obj.updatedby,
                    Addtime = obj.addtime,
                    Updatedtime = obj.updatedtime
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
    public class AnnualLeave
    {
        public int compid { get; set; }
        public string genDate { get; set; }
        public int genType { get; set; }
        public string userName { get; set; }
        public List<AnnualLeaveChild> employeeInfos { get; set; }
    }
    public class AnnualLeaveChild
    {
        public int empserial { get; set; }
        public int empNo { get; set; }
    }
    public class AnnualLeaveResponse : ReturnObject
    {
        public int emp_no { get; set; }
    }
}
