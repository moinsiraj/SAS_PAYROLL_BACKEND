using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class AttcoveringDay_DbModel
    {
        [Key]
        public int cd_serial { get; set; }
        public DateTime? cd_hday { get; set; } = null;
        public DateTime? cd_covday { get; set; } = null;
        public int cd_compid { get; set; }
        public string cd_compName { get; set; } = null;
        public int? cd_groupid { get; set; } = null;
        public string cd_user { get; set; } = null;
        public DateTime? cd_udate { get; set; } = null;
    }
    public class EmpWiseCoveringdayPayload
    {
        public int companyID { get; set; }
        public List<EmployeeInfoCoveringday> empInfos { get; set; }
        public string formDate { get; set; }
        public string toDate { get; set; }
        public string acHoliday { get; set; }
        public string userName { get; set; }
    }
    public class EmployeeInfoCoveringday
    {
        public int emp_serial { get; set; }
        public int emp_no { get; set; }
    }
    public class DeleteCoveringdayPayload
    {
        public int compID { get; set; }
        public string formDate { get; set; }
        public string toDate { get; set; }
        public List<DeleteCoveringdayChild> delChilds { get; set; }
    }
    public class DeleteCoveringdayChild
    {
        public int empSerial { get; set; }
        public int empNumber { get; set; }
    }
    public class DgPayAttcoveringDay
    {
        public int CdSerial { get; set; }
        public DateTime? CdHday { get; set; } = null;
        public DateTime? CdCovday { get; set; } = null;
        public int CdCompid { get; set; }
        public string CdCompName { get; set; } = null;
        public int? CdGroupid { get; set; } = null;
        public string CdUser { get; set; } = null;
        public DateTime? CdUdate { get; set; } = null;

        public static AttcoveringDay_DbModel CustonToDbModel(DgPayAttcoveringDay obj)
        {
            try
            {
                var dbModel = new AttcoveringDay_DbModel
                {
                    cd_serial = obj.CdSerial,
                    cd_hday = obj.CdHday,
                    cd_covday = obj.CdCovday,
                    cd_compid = obj.CdCompid,
                    cd_compName = obj.CdCompName,
                    cd_groupid = obj.CdGroupid,
                    cd_user = obj.CdUser,
                    cd_udate = obj.CdUdate,
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayAttcoveringDay DbToCustomModel(AttcoveringDay_DbModel obj)
        {
            try
            {
                var customModel = new DgPayAttcoveringDay
                {
                    CdSerial = obj.cd_serial,
                    CdHday = obj.cd_hday,
                    CdCovday = obj.cd_covday,
                    CdCompid = obj.cd_compid,
                    CdCompName = obj.cd_compName,
                    CdGroupid = obj.cd_groupid,
                    CdUser = obj.cd_user,
                    CdUdate = obj.cd_udate
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

    public class AttcoveringDayPayloadList
    {
        public int comp_id { get; set; }
        public int emp_serial_id { get; set; }
        public int emp_no { get; set; }
        public string shiftDate { get; set; }
        public string fDate { get; set; }
        public string tDate { get; set; }
        public string acHoliday { get; set; }
        public bool isGov { get; set; }
        public string user { get; set; }
    }
    public class AttcoveringDayEcardSum
    {
        public int comp_id { get; set; }
        public int emp_id { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Late { get; set; }
        public int WeekHoliday { get; set; }
        public int SpecialHoliday { get; set; }
        public string shDate { get; set; }
    }
}