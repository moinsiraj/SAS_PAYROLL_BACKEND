using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Department_DbModel
    {
        [Key]
        public int nUserDept { get; set; }
        public int? nCompanyID { get; set; } = null;
        public string nCompany_name { get; set; } = null;
        public string cDeptname { get; set; } = null;
        public string dptnamebangla { get; set; } = null;
        public string cDptnameBD { get; set; } = null;
        public string cEntUser { get; set; } = null;
        public DateTime? dEntdt { get; set; } = null;
        public string cUpdateBy { get; set; } = null;
        public DateTime? cUpdatedt { get; set; } = null;
    }
    public class DepartmentPayload
    {
        [Required(ErrorMessage = "Department ID Is Required !!")]
        public int departmentID { get; set; }
        [Required(ErrorMessage = "Company ID Is Required !!")]
        public int companyID { get; set; }
        [Required(ErrorMessage = "Company Name Is Required !!")]
        public string companyName { get; set; }
        [Required(ErrorMessage = "Department Name Is Required !!")]
        public string departmentName { get; set; }
        [Required(ErrorMessage = "Department Name Bangla Is Required !!")]
        public string departmentNameBN { get; set; }
        [Required(ErrorMessage = "Create User Name Is Required !!")]
        public string userName { get; set; }

        public static Department_DbModel PayloadToDepartmentDb_Obj(DepartmentPayload obj, Department_DbModel uObj=null)
        {
            var dbModel = new Department_DbModel
            {
                nUserDept = obj.departmentID,
                nCompanyID = obj.companyID,
                nCompany_name = obj.companyName,
                cDeptname = obj.departmentName,
                dptnamebangla = obj.departmentNameBN,
                cDptnameBD = "-",
                cEntUser = uObj == null ? obj.userName : uObj.cEntUser,
                dEntdt = uObj == null ? DateTime.Now : uObj.dEntdt,
                cUpdateBy = uObj == null ? null : obj.userName,
                cUpdatedt = uObj == null ? null : DateTime.Now,
            };
            return dbModel;
        }
    }
    public class DgPayDepartment
    {
        //public int Sl { get; set; }
        public int NUserDept { get; set; }
        public string CDeptname { get; set; } = null;
        public string Dptnamebangla { get; set; } = null;
        public int? NCompanyId { get; set; } = null;
        public string NCompanyName { get; set; } = null;
        public string CEntUser { get; set; } = null;
        public DateTime? DEntdt { get; set; } = null;
        public string CDptnameBd { get; set; } = null;

        public static Department_DbModel CustomToDbModel(DgPayDepartment obj)
        {
            try
            {
                var dbModel = new Department_DbModel
                {
                    nUserDept = obj.NUserDept,
                    cDeptname = obj.CDeptname,
                    dptnamebangla = obj.Dptnamebangla,
                    nCompanyID = obj.NCompanyId,
                    cEntUser = obj.CEntUser,
                    dEntdt = obj.DEntdt,
                    cDptnameBD = obj.CDptnameBd,
                    nCompany_name = obj.NCompanyName
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayDepartment DbToCustomModel(Department_DbModel obj)
        {
            try
            {
                var customModel = new DgPayDepartment
                {
                    NUserDept = obj.nUserDept,
                    CDeptname = obj.cDeptname,
                    Dptnamebangla = obj.dptnamebangla,
                    NCompanyId = obj.nCompanyID,
                    CEntUser = obj.cEntUser,
                    DEntdt = obj.dEntdt,
                    CDptnameBd = obj.cDptnameBD,
                    NCompanyName = obj.nCompany_name
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