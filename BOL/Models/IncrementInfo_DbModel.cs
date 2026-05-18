using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class IncrementInfo_DbModel
    {        
        [Key]
        public int inc_id { get; set; }
        public int com_code { get; set; }
        public int? emp_id { get; set; } = null;
        public int emp_serial { get; set; }
        public DateTime? inc_date { get; set; } = null;
        public string inc_type { get; set; } = null;
        public int? previous_Designation { get; set; } = null;
        public int? Promoted_Desig { get; set; } = null;
        public decimal? previos_gross { get; set; } = null;
        public decimal? previous_basic { get; set; } = null;
        public string depend_on { get; set; } = null;
        public decimal? inc_gross { get; set; } = null;
        public decimal? inc_basic { get; set; } = null;
        public decimal? current_gross { get; set; } = null;
        public decimal? current_basic { get; set; } = null;
        public string status { get; set; } = null;
        public string enteredby { get; set; } = null;
        public string updatedby { get; set; } = null;
        public DateTime? addtime { get; set; } = null;
        public DateTime? updatetime { get; set; } = null;
        public string approveBy { get; set; } = null;
        public DateTime? approvetime { get; set; } = null;
        public string DependGBasic { get; set; } = null;
        public int? housrent { get; set; } = null;
        public string inc_tk_person { get; set; } = null;
        public decimal? inc_percent_gss { get; set; } = null;
        public decimal? inc_percent_Basic { get; set; } = null;
        public DateTime? cut_of_date { get; set; } = null;
        public int? PVS_housrent { get; set; } = null;
    }
    public class EmployeeIncrementPayload
    {
        public int companyID { get; set; }
        public int employeeNo { get; set; }
        public int employeeSl { get; set;}
        public string incrementType { get; set; }
        public string dependOn { get; set; }
        public string incrementMethod { get; set; }
        public int incrementAmount { get; set; }
        public int incrementPersent { get; set; }
        public int newDesignation { get; set; }
        public int newSalaryCat { get; set; }
        public int newgarde { get; set; }
        public bool newOtStatus { get; set; }
        public bool newTiffinStatus { get; set; }
        public bool newNightStatus { get; set; }
        public string cutOfDate { get;set; }
        public string remarks { get; set; }
        public string userName { get; set; }
    }

    public class DgEmpIncrementInfo
    {
        public int ComCode { get; set; }
        public int IncId { get; set; }
        public int? EmpId { get; set; } = null;
        public int EmpSerial { get; set; }
        public DateTime? IncDate { get; set; } = null;
        public string IncType { get; set; } = null;
        public int? PreviousDesignation { get; set; } = null;
        public int? PromotedDesig { get; set; } = null;
        public decimal? PreviosGross { get; set; } = null;
        public decimal? PreviousBasic { get; set; } = null;
        public string DependOn { get; set; } = null;
        public decimal? IncGross { get; set; } = null;
        public decimal? IncBasic { get; set; } = null;
        public decimal? CurrentGross { get; set; } = null;
        public decimal? CurrentBasic { get; set; } = null;
        public string Status { get; set; } = null;
        public string Enteredby { get; set; } = null;
        public string Updatedby { get; set; } = null;
        public DateTime? Addtime { get; set; } = null;
        public DateTime? Updatetime { get; set; } = null;
        public string ApproveBy { get; set; } = null;
        public DateTime? Approvetime { get; set; } = null;
        public string DependGbasic { get; set; } = null;
        public int? Housrent { get; set; } = null;
        public string IncTkPerson { get; set; } = null;
        public decimal? IncPercentGss { get; set; } = null;
        public decimal? IncPercentBasic { get; set; } = null;
        public DateTime? CutOfDate { get; set; } = null;
        public int? PvsHousrent { get; set; } = null;

        public static IncrementInfo_DbModel CustomToDbModel(DgEmpIncrementInfo obj)
        {
            try
            {
                var dbModel = new IncrementInfo_DbModel
                {
                    com_code = obj.ComCode,
                    inc_id = obj.IncId,
                    emp_id = obj.EmpId,
                    emp_serial = obj.EmpSerial,
                    inc_date = obj.IncDate,
                    inc_type = obj.IncType,
                    previous_Designation = obj.PreviousDesignation,
                    Promoted_Desig = obj.PromotedDesig,
                    previos_gross = obj.PreviosGross,
                    previous_basic = obj.PreviousBasic,
                    depend_on = obj.DependOn,
                    inc_gross = obj.IncGross,
                    inc_basic = obj.IncBasic,
                    current_gross = obj.CurrentGross,
                    current_basic = obj.CurrentBasic,
                    status = obj.Status,
                    enteredby = obj.Enteredby,
                    updatedby = obj.Updatedby,
                    addtime = obj.Addtime,
                    updatetime = obj.Updatetime,
                    approveBy = obj.ApproveBy,
                    approvetime = obj.Approvetime,
                    DependGBasic = obj.DependGbasic,
                    housrent = obj.Housrent,
                    inc_tk_person = obj.IncTkPerson,
                    inc_percent_gss = obj.IncPercentGss,
                    inc_percent_Basic = obj.IncPercentBasic,
                    cut_of_date = obj.CutOfDate,
                    PVS_housrent = obj.PvsHousrent
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgEmpIncrementInfo DbToCustomModel(IncrementInfo_DbModel obj)
        {
            try
            {
                var customModel = new DgEmpIncrementInfo
                {
                    ComCode = obj.com_code,
                    IncId = obj.inc_id,
                    EmpId = obj.emp_id,
                    EmpSerial = obj.emp_serial,
                    IncDate = obj.inc_date,
                    IncType = obj.inc_type,
                    PreviousDesignation = obj.previous_Designation,
                    PromotedDesig = obj.Promoted_Desig,
                    PreviosGross = obj.previos_gross,
                    PreviousBasic = obj.previous_basic,
                    DependOn = obj.depend_on,
                    IncGross = obj.inc_gross,
                    IncBasic = obj.inc_basic,
                    CurrentGross = obj.current_gross,
                    CurrentBasic = obj.current_basic,
                    Status = obj.status,
                    Enteredby = obj.enteredby,
                    Updatedby = obj.updatedby,
                    Addtime = obj.addtime,
                    Updatetime = obj.updatetime,
                    ApproveBy = obj.approveBy,
                    Approvetime = obj.approvetime,
                    DependGbasic = obj.DependGBasic,
                    Housrent = obj.housrent,
                    IncTkPerson = obj.inc_tk_person,
                    IncPercentGss = obj.inc_percent_gss,
                    IncPercentBasic = obj.inc_percent_Basic,
                    CutOfDate = obj.cut_of_date,
                    PvsHousrent = obj.PVS_housrent
                };
                return customModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public class EmpIncrementApprovePayload
        {
            public int incID { get; set; }
            public string userName { get; set; }
        }
    }
}