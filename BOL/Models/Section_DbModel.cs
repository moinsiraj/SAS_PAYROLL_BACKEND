using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Section_DbModel
    {
        [Key]
        public int nSectionID { get; set; }
        public string cSection_Description { get; set; } = null;
        public string secnamebangla { get; set; } = null;
        public int? nUserDept { get; set; } = null;
        public string nUserDeptName { get; set; } = null;
        public int? nCarder { get; set; } = null;
        public int? nCompanyID { get; set; } = null;
        public string nCompanyName { get; set; } = null;
        public int? nFloor { get; set; } = null;
        public string cSupervisor { get; set; } = null;
        public int? nMachine_Operator { get; set; } = null;
        public int? nHelper { get; set; } = null;
        public string cEntUser { get; set; } = null;
        public DateTime? dEntdt { get; set; } = null;
        public string cSectionBDname { get; set; } = null;
        public string cUpdateBy { get; set; } = null;
        public DateTime? cUpdatedt { get; set; } = null;
    }
    public class SectionPayload
    {
        [Required(ErrorMessage = "Section ID Is Required !!")]
        public int sectionID { get; set; }
        [Required(ErrorMessage = "Company ID Is Required !!")]
        public int companyID { get; set; }
        [Required(ErrorMessage = "Department ID Is Required !!")]
        public int departmentID { get; set; }
        [Required(ErrorMessage = "Section Name Is Required !!")]
        public string sectionName { get; set; }
        [Required(ErrorMessage = "Section Name Bangla Is Required !!")]
        public string sectionNameBD { get; set; }
        [Required(ErrorMessage = "Create User Name Is Required !!")]
        public string userName { get; set; }

        public static Section_DbModel PayloadToSectionDb_Obj(SectionPayload obj, Section_DbModel uObj = null)
        {
            var dbModel = new Section_DbModel
            {
                nSectionID = obj.sectionID,
                nCompanyID = obj.companyID,
                nUserDept = obj.departmentID,
                cSection_Description = obj.sectionName,
                secnamebangla = obj.sectionNameBD,
                cEntUser = uObj == null ? obj.userName : uObj.cEntUser,
                dEntdt = uObj == null ? DateTime.Now : uObj.dEntdt,
                cUpdateBy = uObj == null ? null : obj.userName,
                cUpdatedt = uObj == null ? null : DateTime.Now,
            };
            return dbModel;
        }
    }
    public class SectionList
    {
        public int sectionID { get; set; }
        public int? companyID { get; set; }
        public string companyName { get; set; }
        public int? departmentID { get; set; }
        public string departmentName { get; set; }
        public string sectionName { get; set; }
        public string sectionNameBangla { get; set; }
        public string createBy { get; set; }
        public DateTime? createDate { get; set; }
        public string updateBy { get; set; }
        public DateTime? updateDate { get; set; }
    }
    public class DgPaySection
    {
        public int NSectionId { get; set; }
        public string CSectionDescription { get; set; } = null;
        public string Secnamebangla { get; set; } = null;
        public int? NUserDept { get; set; } = null;
        public string NUserDeptName { get; set; } = null;
        public int? NCarder { get; set; } = null;
        public int? NCompanyId { get; set; } = null;
        public string NCompanyName { get; set; } = null;
        public int? NFloor { get; set; } = null;
        public string CSupervisor { get; set; } = null;
        public int? NMachineOperator { get; set; } = null;
        public int? NHelper { get; set; } = null;
        public string CEntUser { get; set; } = null;
        public DateTime? DEntdt { get; set; } = null;
        public string CSectionBdname { get; set; } = null;

        public static Section_DbModel CustonToDbModel(DgPaySection obj)
        {
            try
            {
                var dbModel = new Section_DbModel
                {
                    nSectionID = obj.NSectionId,
                    cSection_Description = obj.CSectionDescription,
                    secnamebangla = obj.Secnamebangla,
                    nUserDept = obj.NUserDept,
                    nUserDeptName = obj.NUserDeptName,
                    nCarder = obj.NCarder,
                    nCompanyID = obj.NCompanyId,
                    nCompanyName = obj.NCompanyName,
                    nFloor = obj.NFloor,
                    cSupervisor = obj.CSupervisor,
                    nMachine_Operator = obj.NMachineOperator,
                    nHelper = obj.NHelper,
                    cEntUser = obj.CEntUser,
                    dEntdt = obj.DEntdt,
                    cSectionBDname = obj.CSectionBdname
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPaySection DbToCustomModel(Section_DbModel obj)
        {
            try
            {
                var customModel = new DgPaySection
                {
                    NSectionId = obj.nSectionID,
                    CSectionDescription = obj.cSection_Description,
                    Secnamebangla = obj.secnamebangla,
                    NUserDept = obj.nUserDept,
                    NUserDeptName = obj.nUserDeptName,
                    NCarder = obj.nCarder,
                    NCompanyId = obj.nCompanyID,
                    NCompanyName = obj.nCompanyName,
                    NFloor = obj.nFloor,
                    CSupervisor = obj.cSupervisor,
                    NMachineOperator = obj.nMachine_Operator,
                    NHelper = obj.nHelper,
                    CEntUser = obj.cEntUser,
                    DEntdt = obj.dEntdt,
                    CSectionBdname = obj.cSectionBDname
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