using Microsoft.AspNetCore.Http;

namespace BOL.Models
{
    public class ExcelToDb
    {
        public int countryID { get; set; }
        public string countryName { get; set; }
    }
    public class UploadExcelFile
    {
        public int CompanyID { get; set; }
        public int actionTypeID { get; set; }
        public IFormFile excelFile { get; set; }
        public string remarks { get; set; } = null;
        public string userName { get; set; }
    }
    public class EmployeeExcelPayload
    {
        //Personal
        public string comp_name { get; set;}
        public int emp_no { get; set; }
        public string emp_proxid { get; set; }
        public string pi_fullname { get; set; }
        public string pi_farthersname { get; set; }
        public string pi_mothersname { get; set; }
        public string pi_birthdate { get; set; }
        public string pi_bloodgroup { get; set; }
        public string pi_sex { get; set; }
        public string pi_nic { get; set; }
        public string pi_birth_certificate_no { get; set; }
        public string Pi_tin { get; set; }
        public string pi_religoin { get; set; }
        public string pi_empcontactno { get; set;}

        //Official
        public string oi_joineddate { get; set; }
        public string oi_ConfDate { get; set; }        
        public string oi_department_name { get; set; }
        public string oi_section_name { get; set; }
        public string oi_floor_name { get; set; }
        public string oi_line_name { get; set; }
        public string oi_shift_name { get; set; }
        public string oi_garde_name { get; set; }
        public string oi_designation_name { get; set; }
        public string oi_salcategory_name { get; set; }
        public int oi_grossalary { get; set; }
        public string oi_bank_shCode { get; set; }
        public string oi_bankacno { get; set; }
        public int oi_tiffin_bill_status { get; set; }
        public int oi_night_bill_status { get; set; }
        public int oi_ot_status { get; set; }
    }
}