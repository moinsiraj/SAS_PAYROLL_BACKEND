using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL.Models
{
    public class Employee_DbModel
    {
        [Key]
        public int emp_serial { get; set; }
        public int? groupid { get; set; } = null;
        public int? compid { get; set; } = null;
        public string comp_name { get; set; } = null;
        public int? emp_no { get; set; } = null;
        public string emp_ref { get; set; } = null;
        public string emp_proxid { get; set; } = null;
        public string pi_firstname { get; set; } = null;
        public string pi_lastname { get; set; } = null;
        public string pi_fullname { get; set; } = null;
        public string pi_nameinbangla { get; set; } = null;
        public string pi_permanent_add1 { get; set; } = null;
        public int? pi_permanent_add2_id { get; set; } = null;
        public string pi_permanent_add2 { get; set; } = null;
        public string pi_permanent_add2_temp { get; set; } = null; //not use
        public string pi_permanent_add1_bangla { get; set; } = null; //not use
        public string pi_permanent_add2_bangla { get; set; } = null;
        public string pi_present_add1 { get; set; } = null;
        public int? pi_present_add2_id { get; set; } = null;
        public string pi_present_add2 { get; set; } = null;
        public string pi_present_add2_temp { get; set; } = null; //not use
        public int? pi_postoffice { get; set; } = null;
        public string pi_postoffice_name { get; set; } = null;
        public string pi_postoffice_bangla { get; set; } = null;
        public string pi_postoffice_name_temp { get; set; } = null;
        public string pi_emergencyno { get; set; } = null;
        public string pi_empcontactno { get; set; } = null;
        public int? pi_division { get; set; } = null;
        public string pi_division_name { get; set; } = null;
        public int? pi_district { get; set; } = null;
        public string pi_district_name { get; set; } = null;
        public int? pi_thana { get; set; } = null;
        public string pi_thana_name { get; set; } = null;
        public string pi_sex { get; set; } = null;
        public DateTime? pi_birthdate { get; set; } = null;
        public string pi_bloodgroup { get; set; } = null;
        public string pi_nic { get; set; } = null;
        public string Pi_tin { get; set; } = null;
        public string pi_maritalstatus { get; set; } = null;
        public int? pi_noofchild { get; set; } = null;
        public string pi_farthersname { get; set; } = null;
        public string pi_fcontactno { get; set; } = null;
        public string pi_mothersname { get; set; } = null;
        public string pi_mcontactno { get; set; } = null;
        public string pi_religoin { get; set; } = null;
        public string pi_nationality { get; set; } = null;
        public string pi_nominee { get; set; } = null;
        public string pi_nominee_ad1 { get; set; } = null;
        public string pi_nominee_ad2 { get; set; } = null;
        public string pi_nomineeNIC { get; set; } = null;
        public string pi_spouse { get; set; } = null;
        public string pi_birth_certificate_no { get; set; } = null;
        public string pi_Fathernamebangla { get; set; } = null;
        public string pi_Mothernamebangla { get; set; } = null;
        public string Gross_salary_Bangla { get; set; } = null;
        public string pi_present_village_Bgl { get; set; } = null;
        public int? pi_present_postoffice { get; set; } = null;
        public string pi_present_postoffice_name { get; set; } = null;
        public string pi_present_PO_Bgl { get; set; } = null;
        public string pi_present_postoffice_name_temp { get; set; } = null;
        public int? pi_present_Division { get; set; } = null;
        public string pi_present_Division_name { get; set; } = null;
        public int? pi_present_District { get; set; } = null;
        public string pi_present_District_name { get; set; } = null;
        public int? pi_present_Thana { get; set; } = null;
        public string pi_present_Thana_name { get; set; } = null;
        public DateTime? oi_joineddate { get; set; } = null;
        public int? oi_designation { get; set; } = null;
        public string oi_designation_name { get; set; } = null;
        public int? oi_garde { get; set; } = null;
        public string oi_garde_name { get; set; } = null;
        public int? oi_salcategory { get; set; } = null;
        public string oi_salcategory_name { get; set; } = null;
        public int? oi_department { get; set; } = null;
        public string oi_departmente_name { get; set; } = null;
        public int? oi_section { get; set; } = null;
        public string oi_section_name { get; set; } = null;
        public int? oi_bulding { get; set; } = null;
        public string oi_bulding_name { get; set; } = null;
        public int? oi_floor { get; set; } = null;
        public string oi_floor_name { get; set; } = null;
        public int? oi_line { get; set; } = null;
        public string oi_line_name { get; set; } = null;
        public int? oi_shift { get; set; } = null;
        public string oi_shift_name { get; set; } = null;
        public int? oi_shift_old { get; set; } = null;
        public decimal? oi_basicsalary { get; set; } = null;
        public decimal? oi_grossalary { get; set; } = null;
        public decimal? oi_medical { get; set; } = null;
        public decimal? oi_traveling { get; set; } = null;
        public decimal? oi_food { get; set; } = null;
        public decimal? oi_houserent { get; set; } = null;
        public decimal? oi_spacola { get; set; } = null;
        public decimal? oi_providentfund { get; set; } = null;
        public int? oi_bank { get; set; } = null;
        public string oi_bank_name { get; set; } = null;
        public string oi_bankacno { get; set; } = null;
        public string oi_weeklyholiday { get; set; } = null;
        public string oi_ProcessName { get; set; } = null;
        public string oi_ProcessName_Bangla { get; set; } = null;
        public DateTime? oi_ConfDate { get; set; } = null;
        public bool? oi_active { get; set; } = null;
        public DateTime? oi_inactive_date { get; set; } = null;
        public string oi_inactive_reson { get; set; } = null;
        public bool? oi_tiffin_bill_status { get; set; } = null;
        public bool? oi_night_bill_status { get; set; } = null;
        public string emp_lastedituser { get; set; } = null;
        public DateTime? emplasteditudate { get; set; } = null;
        public string emp_user { get; set; } = null;
        public DateTime? emp_udate { get; set; } = null;        
        public DateTime? shift_rostered_date { get; set; } = null;
    }
    public class DgPayEmployee
    {
        public int EmpSerial { get; set; }
        public int? Groupid { get; set; } = null;
        public int? Compid { get; set; } = null;
        public string CompName { get; set; } = null;
        public int? EmpNo { get; set; } = null;
        public string EmpRef { get; set; } = null;
        public string EmpProxid { get; set; } = null;
        public string PiFirstname { get; set; } = null;
        public string PiLastname { get; set; } = null;
        public string PiFullname { get; set; } = null;
        public string PiNameinbangla { get; set; } = null;
        public string PiPermanentAdd1 { get; set; } = null;
        public int? PiPermanentAdd2Id { get; set; } = null;
        public string PiPermanentAdd2 { get; set; } = null;
        public string PiPermanentAdd2Temp { get; set; } = null;
        public string PiPermanentAdd1Bangla { get; set; } = null;
        public string PiPermanentAdd2Bangla { get; set; } = null;
        public string PiPresentAdd1 { get; set; } = null;
        public int? PiPresentAdd2Id { get; set; } = null;
        public string PiPresentAdd2 { get; set; } = null;
        public string PiPresentAdd2Temp { get; set; } = null;
        public int? PiPostoffice { get; set; } = null;
        public string PiPostofficeName { get; set; } = null;
        public string PiPostofficeBangla { get; set; } = null;
        public string PiPostofficeNameTemp { get; set; } = null;
        public string PiEmergencyno { get; set; } = null;
        public string PiEmpcontactno { get; set; } = null;
        public int? PiDivision { get; set; } = null;
        public string PiDivisionName { get; set; } = null;
        public int? PiDistrict { get; set; } = null;
        public string PiDistrictName { get; set; } = null;
        public int? PiThana { get; set; } = null;
        public string PiThanaName { get; set; } = null;
        public string PiSex { get; set; } = null;
        public DateTime? PiBirthdate { get; set; } = null;
        public string PiBloodgroup { get; set; } = null;
        public string PiNic { get; set; } = null;
        public string PiTin { get; set; } = null;
        public string PiMaritalstatus { get; set; } = null;
        public int? PiNoofchild { get; set; } = null;
        public string PiFarthersname { get; set; } = null;
        public string PiFcontactno { get; set; } = null;
        public string PiMothersname { get; set; } = null;
        public string PiMcontactno { get; set; } = null;
        public string PiReligoin { get; set; } = null;
        public string PiNationality { get; set; } = null;
        public string PiNominee { get; set; } = null;
        public string PiNomineeAd1 { get; set; } = null;
        public string PiNomineeAd2 { get; set; } = null;
        public string PiNomineeNic { get; set; } = null;
        public string PiSpouse { get; set; } = null;
        public string PiBirthCertificateNo { get; set; } = null;
        public DateTime? OiJoineddate { get; set; } = null;
        public int? OiDesignation { get; set; } = null;
        public string OiDesignationName { get; set; } = null;
        public int? OiGarde { get; set; } = null;
        public string OiGardeName { get; set; } = null;
        public int? OiSalcategory { get; set; } = null;
        public string OiSalcategoryName { get; set; } = null;
        public int? OiDepartment { get; set; } = null;
        public string OiDepartmenteName { get; set; } = null;
        public int? OiSection { get; set; } = null;
        public string OiSectionName { get; set; } = null;
        public int? OiBulding { get; set; } = null;
        public string OiBuldingName { get; set; } = null;
        public int? OiFloor { get; set; } = null;
        public string OiFloorName { get; set; } = null;
        public int? OiLine { get; set; } = null;
        public string OiLineName { get; set; } = null;
        public int? OiShift { get; set; } = null;
        public string OiShiftName { get; set; } = null;
        public int? oi_shift_old { get; set; } = null;
        public decimal? OiBasicsalary { get; set; } = null;
        public decimal? OiGrossalary { get; set; } = null;
        public decimal? OiMedical { get; set; } = null;
        public decimal? OiTraveling { get; set; } = null;
        public decimal? OiFood { get; set; } = null;
        public decimal? OiHouserent { get; set; } = null;
        public decimal? OiSpacola { get; set; } = null;
        public decimal? OiProvidentfund { get; set; } = null;
        public int? OiBank { get; set; } = null;
        public string OiBankName { get; set; } = null;
        public string OiBankacno { get; set; } = null;
        public string OiWeeklyholiday { get; set; } = null;
        public string OiProcessName { get; set; } = null;
        public string OiProcessNameBangla { get; set; } = null;
        public DateTime? OiConfDate { get; set; } = null;
        public bool? OiActive { get; set; } = null;
        public DateTime? OiInactiveDate { get; set; } = null;
        public string OiInactiveReson { get; set; } = null;
        public bool? OiTiffinBillStatus { get; set; } = null;
        public bool? OiNightBillStatus { get; set; } = null;
        public string EmpLastedituser { get; set; } = null;
        public DateTime? Emplasteditudate { get; set; } = null;
        public string EmpUser { get; set; } = null;
        public DateTime? EmpUdate { get; set; } = null;
        public string PiFathernamebangla { get; set; } = null;
        public string PiMothernamebangla { get; set; } = null;
        public string GrossSalaryBangla { get; set; } = null;
        public string PiPresentVillageBgl { get; set; } = null;
        public int? PiPresentPostoffice { get; set; } = null;
        public string PiPresentPostofficeName { get; set; } = null;
        public string PiPresentPoBgl { get; set; } = null;
        public string PiPresentPostofficeNameTemp { get; set; } = null;
        public int? PiPresentDivision { get; set; } = null;
        public string PiPresentDivisionName { get; set; } = null;
        public int? PiPresentDistrict { get; set; } = null;
        public string PiPresentDistrictName { get; set; } = null;
        public int? PiPresentThana { get; set; } = null;
        public string PiPresentThanaName { get; set; } = null;
        public DateTime? ShiftRosteredDate { get; set; } = null;

        public static Employee_DbModel CustomToDb(DgPayEmployee obj)
        {
            try
            {
                var DbModel = new Employee_DbModel
                {
                    emp_serial = obj.EmpSerial,
                    groupid = obj.Groupid,
                    compid = obj.Compid,
                    comp_name = !string.IsNullOrEmpty(obj.CompName) ? obj.CompName.Trim() : obj.CompName,
                    emp_no = obj.EmpNo,
                    emp_ref = !string.IsNullOrEmpty(obj.EmpRef) ? obj.EmpRef.Trim() : obj.EmpRef,
                    emp_proxid = !string.IsNullOrEmpty(obj.EmpProxid) ? obj.EmpProxid.Trim() : obj.EmpProxid,
                    pi_firstname = !string.IsNullOrEmpty(obj.PiFirstname) ? obj.PiFirstname.Trim() : obj.PiFirstname,
                    pi_lastname = !string.IsNullOrEmpty(obj.PiLastname) ? obj.PiLastname.Trim() : obj.PiLastname,
                    pi_fullname = !string.IsNullOrEmpty(obj.PiFullname) ? obj.PiFullname.Trim() : obj.PiFullname,
                    pi_nameinbangla = !string.IsNullOrEmpty(obj.PiNameinbangla) ? obj.PiNameinbangla.Trim() : obj.PiNameinbangla,
                    pi_permanent_add1 = !string.IsNullOrEmpty(obj.PiPermanentAdd1) ? obj.PiPermanentAdd1.Trim() : obj.PiPermanentAdd1,
                    pi_permanent_add2_id = obj.PiPermanentAdd2Id,
                    pi_permanent_add2 = !string.IsNullOrEmpty(obj.PiPermanentAdd2) ? obj.PiPermanentAdd2.Trim() : obj.PiPermanentAdd2,
                    pi_permanent_add2_temp = !string.IsNullOrEmpty(obj.PiPermanentAdd2Temp) ? obj.PiPermanentAdd2Temp.Trim() : obj.PiPermanentAdd2Temp,
                    pi_permanent_add1_bangla = !string.IsNullOrEmpty(obj.PiPermanentAdd1Bangla) ? obj.PiPermanentAdd1Bangla.Trim() : obj.PiPermanentAdd1Bangla,
                    pi_permanent_add2_bangla = !string.IsNullOrEmpty(obj.PiPermanentAdd2Bangla) ? obj.PiPermanentAdd2Bangla.Trim() : obj.PiPermanentAdd2Bangla,
                    pi_present_add1 = !string.IsNullOrEmpty(obj.PiPresentAdd1) ? obj.PiPresentAdd1.Trim() : obj.PiPresentAdd1,
                    pi_present_add2_id = obj.PiPresentAdd2Id,
                    pi_present_add2 = !string.IsNullOrEmpty(obj.PiPresentAdd2) ? obj.PiPresentAdd2.Trim() : obj.PiPresentAdd2,
                    pi_present_add2_temp = !string.IsNullOrEmpty(obj.PiPresentAdd2Temp) ? obj.PiPresentAdd2Temp.Trim() : obj.PiPresentAdd2Temp,
                    pi_postoffice = obj.PiPostoffice,
                    pi_postoffice_name = !string.IsNullOrEmpty(obj.PiPostofficeName) ? obj.PiPostofficeName.Trim() : obj.PiPostofficeName,
                    pi_postoffice_bangla = !string.IsNullOrEmpty(obj.PiPostofficeBangla) ? obj.PiPostofficeBangla.Trim() : obj.PiPostofficeBangla,
                    pi_postoffice_name_temp = !string.IsNullOrEmpty(obj.PiPostofficeNameTemp) ? obj.PiPostofficeNameTemp.Trim() : obj.PiPostofficeNameTemp,
                    pi_emergencyno = !string.IsNullOrEmpty(obj.PiEmergencyno) ? obj.PiEmergencyno.Trim() : obj.PiEmergencyno,
                    pi_empcontactno = !string.IsNullOrEmpty(obj.PiEmpcontactno) ? obj.PiEmpcontactno.Trim() : obj.PiEmpcontactno,
                    pi_division = obj.PiDivision,
                    pi_division_name = !string.IsNullOrEmpty(obj.PiDivisionName) ? obj.PiDivisionName.Trim() : obj.PiDivisionName,
                    pi_district = obj.PiDistrict,
                    pi_district_name = !string.IsNullOrEmpty(obj.PiDistrictName) ? obj.PiDistrictName.Trim() : obj.PiDistrictName,
                    pi_thana = obj.PiThana,
                    pi_thana_name = !string.IsNullOrEmpty(obj.PiThanaName) ? obj.PiThanaName.Trim() : obj.PiThanaName,
                    pi_sex = !string.IsNullOrEmpty(obj.PiSex) ? obj.PiSex.Trim() : obj.PiSex,
                    pi_birthdate = obj.PiBirthdate,
                    pi_bloodgroup = !string.IsNullOrEmpty(obj.PiBloodgroup) ? obj.PiBloodgroup.Trim() : obj.PiBloodgroup,
                    pi_nic = !string.IsNullOrEmpty(obj.PiNic) ? obj.PiNic.Trim() : obj.PiNic,
                    Pi_tin = !string.IsNullOrEmpty(obj.PiTin) ? obj.PiTin.Trim() : obj.PiTin,
                    pi_maritalstatus = !string.IsNullOrEmpty(obj.PiMaritalstatus) ? obj.PiMaritalstatus.Trim() : obj.PiMaritalstatus,
                    pi_noofchild = obj.PiNoofchild,
                    pi_farthersname = !string.IsNullOrEmpty(obj.PiFarthersname) ? obj.PiFarthersname.Trim() : obj.PiFarthersname,
                    pi_fcontactno = !string.IsNullOrEmpty(obj.PiFcontactno) ? obj.PiFcontactno.Trim() : obj.PiFcontactno,
                    pi_mothersname = !string.IsNullOrEmpty(obj.PiMothersname) ? obj.PiMothersname.Trim() : obj.PiMothersname,
                    pi_mcontactno = !string.IsNullOrEmpty(obj.PiMcontactno) ? obj.PiMcontactno.Trim() : obj.PiMcontactno,
                    pi_religoin = !string.IsNullOrEmpty(obj.PiReligoin) ? obj.PiReligoin.Trim() : obj.PiReligoin,
                    pi_nationality = !string.IsNullOrEmpty(obj.PiNationality) ? obj.PiNationality.Trim() : obj.PiNationality,
                    pi_nominee = !string.IsNullOrEmpty(obj.PiNominee) ? obj.PiNominee.Trim() : obj.PiNominee,
                    pi_nominee_ad1 = !string.IsNullOrEmpty(obj.PiNomineeAd1) ? obj.PiNomineeAd1.Trim() : obj.PiNomineeAd1,
                    pi_nominee_ad2 = !string.IsNullOrEmpty(obj.PiNomineeAd2) ? obj.PiNomineeAd2.Trim() : obj.PiNomineeAd2,
                    pi_nomineeNIC = !string.IsNullOrEmpty(obj.PiNomineeNic) ? obj.PiNomineeNic.Trim() : obj.PiNomineeNic,
                    pi_spouse = !string.IsNullOrEmpty(obj.PiSpouse) ? obj.PiSpouse.Trim() : obj.PiSpouse,
                    pi_birth_certificate_no = !string.IsNullOrEmpty(obj.PiBirthCertificateNo) ? obj.PiBirthCertificateNo.Trim() : obj.PiBirthCertificateNo,
                    oi_joineddate = obj.OiJoineddate,
                    oi_designation = obj.OiDesignation,
                    oi_designation_name = !string.IsNullOrEmpty(obj.OiDesignationName) ? obj.OiDesignationName.Trim() : obj.OiDesignationName,
                    oi_garde = obj.OiGarde,
                    oi_garde_name = !string.IsNullOrEmpty(obj.OiGardeName) ? obj.OiGardeName.Trim() : obj.OiGardeName,
                    oi_salcategory = obj.OiSalcategory,
                    oi_salcategory_name = !string.IsNullOrEmpty(obj.OiSalcategoryName) ? obj.OiSalcategoryName.Trim() : obj.OiSalcategoryName,
                    oi_department = obj.OiDepartment,
                    oi_departmente_name = !string.IsNullOrEmpty(obj.OiDepartmenteName) ? obj.OiDepartmenteName.Trim() : obj.OiDepartmenteName,
                    oi_section = obj.OiSection,
                    oi_section_name = !string.IsNullOrEmpty(obj.OiSectionName) ? obj.OiSectionName.Trim() : obj.OiSectionName,
                    oi_bulding = obj.OiBulding,
                    oi_bulding_name = !string.IsNullOrEmpty(obj.OiBuldingName) ? obj.OiBuldingName.Trim() : obj.OiBuldingName,
                    oi_floor = obj.OiFloor,
                    oi_floor_name = !string.IsNullOrEmpty(obj.OiFloorName) ? obj.OiFloorName.Trim() : obj.OiFloorName,
                    oi_line = obj.OiLine,
                    oi_line_name = !string.IsNullOrEmpty(obj.OiLineName) ? obj.OiLineName.Trim() : obj.OiLineName,
                    oi_shift = obj.OiShift,
                    oi_shift_name = !string.IsNullOrEmpty(obj.OiShiftName) ? obj.OiShiftName.Trim() : obj.OiShiftName,
                    oi_shift_old = obj.oi_shift_old,
                    oi_basicsalary = obj.OiBasicsalary,
                    oi_grossalary = obj.OiGrossalary,
                    oi_medical = obj.OiMedical,
                    oi_traveling = obj.OiTraveling,
                    oi_food = obj.OiFood,
                    oi_houserent = obj.OiHouserent,
                    oi_spacola = obj.OiSpacola,
                    oi_providentfund = obj.OiProvidentfund,
                    oi_bank = obj.OiBank,
                    oi_bank_name = !string.IsNullOrEmpty(obj.OiBankName) ? obj.OiBankName.Trim() : obj.OiBankName,
                    oi_bankacno = !string.IsNullOrEmpty(obj.OiBankacno) ? obj.OiBankacno.Trim() : obj.OiBankacno,
                    oi_weeklyholiday = !string.IsNullOrEmpty(obj.OiWeeklyholiday) ? obj.OiWeeklyholiday.Trim() : obj.OiWeeklyholiday,
                    oi_ProcessName = !string.IsNullOrEmpty(obj.OiProcessName) ? obj.OiProcessName.Trim() : obj.OiProcessName,
                    oi_ProcessName_Bangla = !string.IsNullOrEmpty(obj.OiProcessNameBangla) ? obj.OiProcessNameBangla.Trim() : obj.OiProcessNameBangla,
                    oi_ConfDate = obj.OiConfDate,
                    oi_active = obj.OiActive,
                    oi_inactive_date = obj.OiInactiveDate,
                    oi_inactive_reson = !string.IsNullOrEmpty(obj.OiInactiveReson) ? obj.OiInactiveReson.Trim() : obj.OiInactiveReson,
                    oi_tiffin_bill_status = obj.OiTiffinBillStatus,
                    oi_night_bill_status = obj.OiNightBillStatus,
                    emp_lastedituser = !string.IsNullOrEmpty(obj.EmpLastedituser) ? obj.EmpLastedituser.Trim() : obj.EmpLastedituser,
                    emplasteditudate = obj.Emplasteditudate,
                    emp_user = !string.IsNullOrEmpty(obj.EmpUser) ? obj.EmpUser.Trim() : obj.EmpUser,
                    emp_udate = obj.EmpUdate,
                    pi_Fathernamebangla = !string.IsNullOrEmpty(obj.PiFathernamebangla) ? obj.PiFathernamebangla.Trim() : obj.PiFathernamebangla,
                    pi_Mothernamebangla = !string.IsNullOrEmpty(obj.PiMothernamebangla) ? obj.PiMothernamebangla.Trim() : obj.PiMothernamebangla,
                    Gross_salary_Bangla = !string.IsNullOrEmpty(obj.GrossSalaryBangla) ? obj.GrossSalaryBangla.Trim() : obj.GrossSalaryBangla,
                    pi_present_village_Bgl = !string.IsNullOrEmpty(obj.PiPresentVillageBgl) ? obj.PiPresentVillageBgl.Trim() : obj.PiPresentVillageBgl,
                    pi_present_postoffice = obj.PiPresentPostoffice,
                    pi_present_postoffice_name = !string.IsNullOrEmpty(obj.PiPresentPostofficeName) ? obj.PiPresentPostofficeName.Trim() : obj.PiPresentPostofficeName,
                    pi_present_PO_Bgl = obj.PiPresentPoBgl.Trim(),
                    pi_present_postoffice_name_temp = !string.IsNullOrEmpty(obj.PiPresentPostofficeNameTemp) ? obj.PiPresentPostofficeNameTemp.Trim() : obj.PiPresentPostofficeNameTemp,
                    pi_present_Division = obj.PiPresentDivision,
                    pi_present_Division_name = !string.IsNullOrEmpty(obj.PiPresentDivisionName) ? obj.PiPresentDivisionName.Trim() : obj.PiPresentDivisionName,
                    pi_present_District = obj.PiPresentDistrict,
                    pi_present_District_name = !string.IsNullOrEmpty(obj.PiPresentDistrictName) ? obj.PiPresentDistrictName.Trim() : obj.PiPresentDistrictName,
                    pi_present_Thana = obj.PiPresentThana,
                    pi_present_Thana_name = !string.IsNullOrEmpty(obj.PiPresentThanaName) ? obj.PiPresentThanaName.Trim() : obj.PiPresentThanaName,
                    shift_rostered_date = obj.ShiftRosteredDate
                };
                return DbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayEmployee DbToCustom(Employee_DbModel obj)
        {
            try
            {
                var CustomModel = new DgPayEmployee
                {
                    EmpSerial = obj.emp_serial,
                    Groupid = obj.groupid,
                    Compid = obj.compid,
                    CompName = obj.comp_name,
                    EmpNo = obj.emp_no,
                    EmpRef = obj.emp_ref,
                    EmpProxid = obj.emp_proxid,
                    PiFirstname = obj.pi_firstname,
                    PiLastname = obj.pi_lastname,
                    PiFullname = obj.pi_fullname,
                    PiNameinbangla = obj.pi_nameinbangla,
                    PiPermanentAdd1 = obj.pi_permanent_add1,
                    PiPermanentAdd2Id = obj.pi_permanent_add2_id,
                    PiPermanentAdd2 = obj.pi_permanent_add2,
                    PiPermanentAdd2Temp = obj.pi_permanent_add2_temp,
                    PiPermanentAdd1Bangla = obj.pi_permanent_add1_bangla,
                    PiPermanentAdd2Bangla = obj.pi_permanent_add2_bangla,
                    PiPresentAdd1 = obj.pi_present_add1,
                    PiPresentAdd2Id = obj.pi_present_add2_id,
                    PiPresentAdd2 = obj.pi_present_add2,
                    PiPresentAdd2Temp = obj.pi_present_add2_temp,
                    PiPostoffice = obj.pi_postoffice,
                    PiPostofficeName = obj.pi_postoffice_name,
                    PiPostofficeBangla = obj.pi_postoffice_bangla,
                    PiPostofficeNameTemp = obj.pi_postoffice_name_temp,
                    PiEmergencyno = obj.pi_emergencyno,
                    PiEmpcontactno = obj.pi_empcontactno,
                    PiDivision = obj.pi_division,
                    PiDivisionName = obj.pi_division_name,
                    PiDistrict = obj.pi_district,
                    PiDistrictName = obj.pi_district_name,
                    PiThana = obj.pi_thana,
                    PiThanaName = obj.pi_thana_name,
                    PiSex = obj.pi_sex,
                    PiBirthdate = obj.pi_birthdate,
                    PiBloodgroup = obj.pi_bloodgroup,
                    PiNic = obj.pi_nic,
                    PiTin = obj.Pi_tin,
                    PiMaritalstatus = obj.pi_maritalstatus,
                    PiNoofchild = obj.pi_noofchild,
                    PiFarthersname = obj.pi_farthersname,
                    PiFcontactno = obj.pi_fcontactno,
                    PiMothersname = obj.pi_mothersname,
                    PiMcontactno = obj.pi_mcontactno,
                    PiReligoin = obj.pi_religoin,
                    PiNationality = obj.pi_nationality,
                    PiNominee = obj.pi_nominee,
                    PiNomineeAd1 = obj.pi_nominee_ad1,
                    PiNomineeAd2 = obj.pi_nominee_ad2,
                    PiNomineeNic = obj.pi_nomineeNIC,
                    PiSpouse = obj.pi_spouse,
                    PiBirthCertificateNo = obj.pi_birth_certificate_no,
                    OiJoineddate = obj.oi_joineddate,
                    OiDesignation = obj.oi_designation,
                    OiDesignationName = obj.oi_designation_name,
                    OiGarde = obj.oi_garde,
                    OiGardeName = obj.oi_garde_name,
                    OiSalcategory = obj.oi_salcategory,
                    OiSalcategoryName = obj.oi_salcategory_name,
                    OiDepartment = obj.oi_department,
                    OiDepartmenteName = obj.oi_departmente_name,
                    OiSection = obj.oi_section,
                    OiSectionName = obj.oi_section_name,
                    OiBulding = obj.oi_bulding,
                    OiBuldingName = obj.oi_bulding_name,
                    OiFloor = obj.oi_floor,
                    OiFloorName = obj.oi_floor_name,
                    OiLine = obj.oi_line,
                    OiLineName = obj.oi_line_name,
                    OiShift = obj.oi_shift,
                    OiShiftName = obj.oi_shift_name,
                    oi_shift_old = obj.oi_shift_old,
                    OiBasicsalary = obj.oi_basicsalary,
                    OiGrossalary = obj.oi_grossalary,
                    OiMedical = obj.oi_medical,
                    OiTraveling = obj.oi_traveling,
                    OiFood = obj.oi_food,
                    OiHouserent = obj.oi_houserent,
                    OiSpacola = obj.oi_spacola,
                    OiProvidentfund = obj.oi_providentfund,
                    OiBank = obj.oi_bank,
                    OiBankName = obj.oi_bank_name,
                    OiBankacno = obj.oi_bankacno,
                    OiWeeklyholiday = obj.oi_weeklyholiday,
                    OiProcessName = obj.oi_ProcessName,
                    OiProcessNameBangla = obj.oi_ProcessName_Bangla,
                    OiConfDate = obj.oi_ConfDate,
                    OiActive = obj.oi_active,
                    OiInactiveDate = obj.oi_inactive_date,
                    OiInactiveReson = obj.oi_inactive_reson,
                    OiTiffinBillStatus = obj.oi_tiffin_bill_status,
                    OiNightBillStatus = obj.oi_night_bill_status,
                    EmpLastedituser = obj.emp_lastedituser,
                    Emplasteditudate = obj.emplasteditudate,
                    EmpUser = obj.emp_user,
                    EmpUdate = obj.emp_udate,
                    PiFathernamebangla = obj.pi_Fathernamebangla,
                    PiMothernamebangla = obj.pi_Mothernamebangla,
                    GrossSalaryBangla = obj.Gross_salary_Bangla,
                    PiPresentVillageBgl = obj.pi_present_village_Bgl,
                    PiPresentPostoffice = obj.pi_present_postoffice,
                    PiPresentPostofficeName = obj.pi_present_postoffice_name,
                    PiPresentPoBgl = obj.pi_present_PO_Bgl,
                    PiPresentPostofficeNameTemp = obj.pi_present_postoffice_name_temp,
                    PiPresentDivision = obj.pi_present_Division,
                    PiPresentDivisionName = obj.pi_present_Division_name,
                    PiPresentDistrict = obj.pi_present_District,
                    PiPresentDistrictName = obj.pi_present_District_name,
                    PiPresentThana = obj.pi_present_Thana,
                    PiPresentThanaName = obj.pi_present_Thana_name,
                    ShiftRosteredDate = obj.shift_rostered_date
                };
                return CustomModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
    }

    public class EmpEducation
    {
        [Key]
        public int sid { get; set; }
        public int? comid { get; set; } = null;
        public int? empno { get; set; } = null;
        [NotMapped]
        public string empName { get; set; } = null;
        public string emptitle { get; set; } = null;
        public string subject { get; set; } = null;
        public string result { get; set; } = null;
        public int? passyear { get; set; } = null;
        public string board_univer { get; set; } = null;
        public string enteredby { get; set; } = null;
        public string updatedby { get; set; } = null;
        public DateTime? addtime { get; set; } = null;
        public DateTime? updatetime { get; set; } = null;
    }
    public class EmpExperince
    {
        [Key]
        public int expid { get; set; }
        public int? emp_id { get; set; } = null;
        public int? compid { get; set; } = null;
        [NotMapped]
        public string empName { get; set; } = null;
        public string prv_companyName { get; set; } = null;
        public string Designation { get; set; } = null;
        public DateTime? fromDate { get; set; } = null;
        public DateTime? todate { get; set; } = null;
        public string enteredby { get; set; } = null;
        public string updatedby { get; set; } = null;
        public DateTime? addtime { get; set; } = null;
        public DateTime? updatetime { get; set; } = null;
    }
    public class EmpExperinceView
    {
        public int emp_id { get; set; }
        public string empName { get; set; } = null;
        public string prv_companyName { get; set; } = null;
        public string Designation { get; set; } = null;
        public string fromDate { get; set; } = null;
        public string todate { get; set; } = null;
    }

    public class EmpQRCodeGenerator
    {
        public int CompanyID { get; set; }
        public int EmpNo { get; set; }
        public string FullName { get; set; }
        public DateTime JoinDate { get; set; }
        public string DesignationName { get; set; }

    }

    public class EmployeeFilterPayload
    {
        public int compid { get; set; }
        public int[] department { get; set; }
        public int[] section { get; set; }
        public int building { get; set; }
        public int[] floor { get; set; }
        public int[] line { get; set; }
        public int[] salCat { get; set; }
    }
    public class EmployeeLeaveFilterPayload
    {
        public int compid { get; set; }
        public int[] department { get; set; }
        public int[] section { get; set; }
        public int building { get; set; }
        public int[] floor { get; set; }
        public int[] line { get; set; }
        public int[] salCat { get; set; }
        public string leave_type { get; set; }
        public string leave_days { get; set; }
        public DateTime? fromDate { get; set; } = null;
        public DateTime? toDate { get; set; } = null;
    }
}
