namespace BOL.Models
{
    public class Employee_Personal_Info
    {
        public int emp_serial { get; set; }
        public int? groupid { get; set; } = null;
        public int? compid { get; set; } = null;
        public int? emp_no { get; set; } = null;
        public string emp_ref { get; set; } = null;
        public string emp_proxid { get; set; } = null;
        public string pi_firstname { get; set; } = null;
        public string pi_middlename { get; set; } = null; //new
        public string pi_lastname { get; set; } = null;
        public string pi_fullname { get; set; } = null;
        public string pi_nameinbangla { get; set; } = null;
        public string pi_farthersname { get; set; } = null;
        public string pi_Fathernamebangla { get; set; } = null;
        public string pi_fcontactno { get; set; } = null;
        public string pi_mothersname { get; set; } = null;
        public string pi_Mothernamebangla { get; set; } = null;
        public string pi_mcontactno { get; set; } = null;
        public string pi_birthdate { get; set; } = null;
        public string pi_bloodgroup { get; set; } = null;
        public string pi_sex { get; set; } = null;
        public string pi_maritalstatus { get; set; } = null;
        public int? pi_noofchild { get; set; } = null;
        public string pi_spouse { get; set; } = null;
        public string pi_nic { get; set; } = null;
        public string pi_birth_certificate_no { get; set; } = null;
        public string Pi_tin { get; set; } = null;
        public string pi_religoin { get; set; } = null;
        public string pi_nationality { get; set; } = null;
        public string pi_officecontactno { get; set; } = null; //new
        public string pi_empcontactno { get; set; } = null;
        public string pi_email { get; set; } = null; //new
        public string pi_emergencyno { get; set; } = null;
        public int? pi_present_Division { get; set; } = null;
        public int? pi_division { get; set; } = null;
        public int? pi_present_District { get; set; } = null;
        public int? pi_district { get; set; } = null;
        public int? pi_present_Thana { get; set; } = null;
        public int? pi_thana { get; set; } = null;

        public string pi_present_postofficeName { get; set; } = null;
        public string pi_permanent_postofficeName { get; set; } = null;
        public string pi_present_villageName { get; set; } = null;
        public string pi_permanent_villageName { get; set; } = null;


        //public int? pi_present_postoffice { get; set; } = null;
        //public int? pi_postoffice { get; set; } = null;
        //public int? pi_present_add2_id { get; set; } = null; //present_village_id
        //public int? pi_permanent_add2_id { get; set; } = null; //permanent_village_id

        public string pi_present_PO_Bgl { get; set; } = null;
        public string pi_postoffice_bangla { get; set; } = null;
        public string pi_present_village_Bgl { get; set; } = null;
        public string pi_permanent_add2_bangla { get; set; } = null; //permanent_village_bangla
        public string pi_present_add1 { get; set; } = null; //present_add(full)
        public string pi_permanent_add1 { get; set; } = null; //permanent_add(full)
        public bool isNidDuplicate { get; set; }
        public bool isBirthNoDuplicate { get; set; }
        public string userName { get; set; } = null;
    }
    public class Employee_Personal_Info_View : Employee_Personal_Info
    {
        public string comp_name { get; set; } = null;
        public string pi_present_Division_name { get; set; } = null;
        public string pi_division_name { get; set; } = null;
        public string pi_present_District_name { get; set; } = null;
        public string pi_district_name { get; set; } = null;
        public string pi_present_Thana_name { get; set; } = null;
        public string pi_thana_name { get; set; } = null;
        public string pi_present_postoffice_name { get; set; } = null;
        public string pi_postoffice_name { get; set; } = null;
        public string pi_present_add2 { get; set; } = null;
        public string pi_permanent_add2 { get; set; } = null;
    }
    public class Employee_Office_Info
    {
        public int emp_serial { get; set; }
        public string oi_joineddate { get; set; } = null;
        public string oi_ConfDate { get; set; } = null;
        public string oi_ConfDate_Extend { get; set; } = null;
        public int? oi_department { get; set; } = null;
        public int? oi_section { get; set; } = null;
        public int? oi_bulding { get; set; } = null;
        public int? oi_floor { get; set; } = null;
        public int? oi_line { get; set; } = null;
        public int? oi_shift { get; set; } = null;
        public int? oi_shift_groupid { get; set; } = null;
        public string oi_weeklyholiday { get; set; } = null;
        public string oi_ProcessName_Bangla { get; set; } = null; //oi_worktype_Bangla
        public int? oi_garde { get; set; } = null;
        public int? oi_designation { get; set; } = null;
        public int? oi_salcategory { get; set; } = null;
        public int? oi_bank { get; set; } = null;
        public string oi_ProcessName { get; set; } = null; //oi_bankName_bangla
        public string oi_bankacno { get; set; } = null;
        public decimal? oi_grossalary { get; set; } = null;
        public decimal? oi_cashPayAmt { get; set; } = null;
        public int? oi_cashID { get; set; } = null;
        public int? oi_cashBank { get; set; } = null;
        public string oi_cashBankAcno { get; set; } = null;
        public string oi_grossalaryBnWords { get; set; } = null;
        public decimal? oi_basicsalary { get; set; } = null;
        public decimal? oi_houserent { get; set; } = null;
        public decimal? oi_medical { get; set; } = null;
        public decimal? oi_traveling { get; set; } = null;
        public decimal? oi_food { get; set; } = null;
        public decimal? oi_providentfund { get; set; } = null;
        public bool? oi_ot_status { get; set; } = null;
        public bool? oi_tiffin_bill_status { get; set; } = null;
        public bool? oi_night_bill_status { get; set; } = null;
        public bool? oi_active { get; set; } = null;
        public string userName { get; set; } = null;
    }
    public class Employee_Office_Info_View : Employee_Office_Info
    {
        public string pi_fullname { get; set; } = null;
        public string oi_departmente_name { get; set; } = null;
        public string oi_section_name { get; set; } = null;
        public string oi_bulding_name { get; set; } = null;
        public string oi_floor_name { get; set; } = null;
        public string oi_line_name { get; set; } = null;
        public string oi_shift_name { get; set; } = null;
        public string sRGrpDescription { get; set; } = null;
        public string oi_garde_name { get; set; } = null;
        public string oi_designation_name { get; set; } = null;
        public string oi_salcategory_name { get; set; } = null;
        public string oi_cashBankName { get; set; } = null;
        public int cat_ot { get; set; } = 0;
        public string oi_bank_name { get; set; } = null;
    }
    public class Employee_Nominee_Info
    {
        public int emp_serial { get; set; }
        public string pi_nominee { get; set; } = null;
        public string pi_nominee_ad1 { get; set; } = null; // pi_nominee_ad1(Full)
        public string pi_nominee_ad2 { get; set; } = null; // pi_nominee_ad2(relation)
        public string pi_nomineeNIC { get; set; } = null;
        public string ni_dateofbirth { get; set; } = null; //new
        public string ni_fatherName { get; set; } = null; //new
        public string ni_motherName { get; set; } = null; //new
        public string ni_contactNo { get; set; } = null; //new
        public string ni_birthCertificate { get; set; } = null; //new
        public int? ni_present_division { get; set; } = null; //new
        public int? ni_permanent_division { get; set; } = null; //new
        public int? ni_present_district { get; set; } = null; //new
        public int? ni_permanent_district { get; set; } = null; //new
        public int? ni_present_thana { get; set; } = null; //new
        public int? ni_permanent_thana { get; set; } = null; //new

        public string ni_present_postofficeName { get; set; } = null;
        public string ni_permanent_postofficeName { get; set; } = null;
        public string ni_present_villageName { get; set; } = null;
        public string ni_permanent_villageName { get; set; } = null;

        //public int? ni_present_postoffice { get; set; } = null; //new
        //public int? ni_permanent_postoffice { get; set; } = null; //new
        //public int? ni_present_village { get; set; } = null; //new
        //public int? ni_permanent_village { get; set; } = null; //new

        public string userName { get; set; } = null;
    }
    public class Employee_Nominee_Info_View : Employee_Nominee_Info
    {
        public string pi_fullname { get; set; } = null;
        public string ni_present_division_name { get; set; } = null;
        public string ni_permanent_division_name { get; set; } = null;
        public string ni_present_district_name { get; set; } = null;
        public string ni_permanent_district_name { get; set; } = null;
        public string ni_present_thana_name { get; set; } = null;
        public string ni_permanent_thana_name { get; set; } = null;
        public string ni_present_postoffice_name { get; set; } = null;
        public string ni_permanent_postoffice_name { get; set; } = null;
        public string ni_present_village_name { get; set; } = null;
        public string ni_permanent_village_name { get; set; } = null;
    }
    public class Employee_Education_Info
    {
        public int sid { get; set; }
        public int? comid { get; set; } = null;
        public int? empno { get; set; } = null;
        public int? emptitle_id { get; set; } = null;
        public string subject { get; set; } = null;
        public string institute_name { get; set; } = null;
        public int? board_id { get; set; } = null;
        public string result { get; set; } = null;
        public string duration { get; set; } = null;
        public int? passyear { get; set; } = null;
        public string userName { get; set; } = null;
    }
    public class Employee_Education_Info_View : Employee_Education_Info
    {
        public string pi_fullname { get; set; } = null;
        public string emptitle { get; set; } = null;       
        public string board_univer { get; set; } = null;
    }
    public class Employee_Experince_Info
    {
        public int expid { get; set; }
        public int? emp_id { get; set; } = null;
        public int? compid { get; set; } = null;
        public string prv_companyName { get; set; } = null;
        public string comp_business { get; set; } = null;
        public string Designation { get; set; } = null;
        public string dept_name { get; set; } = null;
        public string fromDate { get; set; } = null;
        public string todate { get; set; } = null;
        public string priod { get; set; } = null;
        public string userName { get; set; } = null;
    }
    public class Employee_Experince_Info_View : Employee_Experince_Info
    {
        public string pi_fullname { get; set; } = null;        
    }
    public class Employee_FullName_View
    {
        public string pi_fullname { get; set; } = null;
        public string emp_image { get; set; }
        public string emp_signature_image { get; set; }
    }
}