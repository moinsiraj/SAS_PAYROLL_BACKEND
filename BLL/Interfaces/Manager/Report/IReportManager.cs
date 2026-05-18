using BOL.Models;

namespace BLL.Interfaces.Manager.Report
{
    public interface IReportManager
    {
        #region"Employee"
        byte[] Export_Report_Employee_Details_InActive(string reportType, int companyID, string userName);
        byte[] Export_Report_Employee_Details_Active(string reportType, int companyID, string userName);
        byte[] Dg_ActiveEmployeesWithImage(string reportType, int companyID, string userName);
        byte[] Export_Report_Employee_Details_Active_With_Image(string reportType, int companyID, string userName);
        byte[] Export_Report_Employee_Details_In_Active_With_Image(string reportType, int companyID, string userName);
        byte[] Dg_JoinDateWise_Employee_Details_Active(string reportType, int companyID, string userName);
        byte[] Dg_CreateReportFileIDCARD(string reportType, int companyID, string userName);
        byte[] Dg_CreateReportFileIDCARD_BN(string reportType, int companyID, string userName);
        byte[] Dg_EmpAgeCertificate(string reportType, int companyID, string userName);
        byte[] Dg_IncrementLetter(string reportType, int companyID, string userName);
        byte[] Dg_PromotionWithIncrementLetter(string reportType, int companyID, string userName);
        byte[] Dg_IncrementLetter_Bangla(string reportType, int companyID, string userName);
        byte[] Dg_PromotionWithIncrementLetter_Bangla(string reportType, int companyID, string userName);
        byte[] Dg_AppointmentLetter(string reportType, int companyID, string userName);
        byte[] Dg_AppointmentLetter_IFL(string reportType, int companyID, string userName);
        byte[] Dg_AppointmentLetter_StaffBangla(string reportType, int companyID, string userName);
        byte[] Dg_AppointmentLetter_EN(string reportType, int companyID, string userName);
        byte[] Dg_NoticeLetter_1st(string reportType, int companyID, string userName);
        byte[] Dg_NoticeLetter_2nd(string reportType, int companyID, string userName);
        byte[] Dg_voluntarily_resign_letter(string reportType, int companyID, string userName);
        byte[] Dg_MaleFemaleDetails(string reportType, int companyID, string userName);
        byte[] Dg_MaleFemaleSummary(string reportType, int companyID, string userName);
        byte[] Dg_EmployeeDetailsRegligion(string reportType, int companyID, string userName);
        byte[] Dg_ProximityCardChecklist(string reportType, int companyID, string userName);
        byte[] Dg_EmployeewiseIncrementDetails(string reportType, int companyID, string userName);
        byte[] Dg_EmployeewiseIncremen_Sheet_Details(string reportType, int companyID, string userName);
        byte[] Dg_CutoffDatewiseIncrementPendingList(string reportType, int companyID, string userName);
        byte[] Dg_CutoffDatewiseIncrementApprovedList(string reportType, int companyID, string userName);
        byte[] Dg_EmployeeWiseDetailedInformation(string reportType, int companyID, string userName);
        byte[] Dg_EmployeeInformation_BankAccForm(string reportType, int companyID, string userName);
        byte[] Dg_Emp_TiffinBillStatus(string reportType, int companyID, string userName);
        byte[] Dg_Emp_NightBillStatus(string reportType, int companyID, string userName);
        byte[] Export_Report_Employee_shiftchange_history(string reportType, int companyID, string userName);
        byte[] Export_Report_Employee_shiftchange_history_New(ReportParameterModel obj);
        byte[] Dg_EmpShiftGroupChecklist(string reportType, int companyID, string userName);
        byte[] EmployeeNominationForm(string reportType, int companyID, string userName);
        byte[] EmployeeJoiningLetter(string reportType, int companyID, string userName);
        byte[] EmployeeJobApplication(string reportType, int companyID, string userName);
        byte[] EmployeeDetailsExcel(string reportType, int companyID, string userName);
        byte[] EmployeeBackgroundCheck(string reportType, int companyID, string userName);
        byte[] EmployeeIvelatution_From(string reportType, int companyID, string userName);
        byte[] EmployeeBudgetReport(string reportType, int companyID, string userName);
        byte[] EmployeeServiceBookReport(string reportType, int companyID, string userName);
        byte[] Dg_CreateReportFile_Ecard_Date_To_Date_2hour(string reportType, int companyID, string userName);
        byte[] Dg_CreateReportFile_Ecard_Date_To_Date_4hour(string reportType, int companyID, string userName);
        byte[] Report_In_Activeemp_Date_wise_Excel(string reportType, int companyID, string userName);
        byte[] Report_EmployeeInternalTransfer_info_D2D(string reportType, int companyID, string userName);
        byte[] Export_Report_Employee_info_bangla(string reportType, int companyID, string userName);

        //Audit
        byte[] Report_preriodical_present_absent_leave_Weekly_holiday_special_holiday(string reportType, int companyID, string userName);
        byte[] Report_Salary_info_Audit_Excel(string reportType, int companyID, string userName);
        byte[] Report_Joindate_wise_info_Audit_Excel(string reportType, int companyID, string userName);
        byte[] Report_manual_attendsance_Audit_Excel(string reportType, int companyID, string userName);
        byte[] Report_TiffinBill_Audit_Excel(string reportType, int companyID, string userName);
        byte[] Report_Night_Audit_Excel(string reportType, int companyID, string userName);
        byte[] Report_Leave_transction_Audit_Excel(string reportType, int companyID, string userName);
        byte[] Report_In_Activeemp_Date_wise_Audit_Excel(string reportType, int companyID, string userName);
        byte[] Date_wise_total_ot_details_Audit_Excel(string reportType, int companyID, string userName);
        byte[] Report_Employee_envelope(string reportType, int companyID, string userName);
        byte[] Report_TiffinBill_Audit_date_to_date_Excel(string reportType, int companyID, string userName);
        byte[] Report_manual_attendsance_Audit_in_out_Excel(string reportType, int companyID, string userName);
        //Audit
        #endregion

        #region"Leave"
        byte[] Dg_LeaveBalances(string reportType,int companyID,string userName);
        byte[] Dg_LeaveTransactions(string reportType, int companyID, string userName);
        byte[] Dg_IndividualLeaveStatement(string reportType, int companyID, string userName);
        byte[] Dg_MaternityLeaveList(string reportType, int companyID, string userName);
        byte[] Dg_MaternityLeavePayment(string reportType, int companyID, string userName);
        byte[] Dg_LeaveForm(string reportType, int companyID, string userName);
        byte[] Dg_LeaveFormEmpWise(int empSerial, int leaveID, string reportType);
        byte[] Dg_IndividualLeave_Register(string reportType, int companyID, string userName);
        #endregion

        #region"Attendance"
        byte[] CreateReportFile_Attendance_Present(string reportType, int companyID, string userName);
        byte[] Dg_Att_PresentWithImages(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Attendance_Absent(string reportType, int companyID, string userName);
        byte[] Dg_Att_AbsentWithImages(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Attendance_Late(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Attendance_Innotpunch(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Attendance_Outnotpunch(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Attendance_InOutnotpunch(string reportType, int companyID, string userName);
        byte[] Dg_Att_SectionWiseSummary(string reportType, int companyID, string userName);
        byte[] Dg_CreateReportFile_Ecard_u1(string reportType, int companyID, string userName);
        byte[] Dg_CreateReportFile_Ecard_Date_To_Date(string reportType, int companyID, string userName);
        byte[] Dg_CreateReportFile_Ecard_complaince_2hour(string reportType, int companyID, string userName);
        byte[] Dg_CreateReportFile_Ecard_bayer_4hour(string reportType, int companyID, string userName);
        byte[] Dg_Att_MonthlyAttendance(string reportType, int companyID, string userName);
        byte[] Dg_Att_MonthlyAttendance_Absent(string reportType, int companyID, string userName);
        byte[] EmployeeMenualAttnList(string reportType, int companyID, string userName);
        byte[] Date_wise_total_ot_details(string reportType, int companyID, string userName);
        byte[] Floor_wise_manpower_comparison(string reportType, int companyID, string userName);
        byte[] EmployeeAutoShiftHistory(string reportType, int companyID, string userName);       
        byte[] Report_preriodical_present_absent_leave_Weekly_holiday_special_holidays(string reportType, int companyID, string userName);
        byte[] Dg_Att_hourly_manpower(string reportType, int companyID, string userName);
        byte[] Dg_Att_hourly_manpower_d2d(string reportType, int companyID, string userName);
        byte[] Dg_Att_hourly_manpower_otandvalue(string reportType, int companyID, string userName);
        byte[] Dg_Att_single_hourwise_manpower_otandvalue(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Attendance_LunchOut(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Weekly_Warning(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Continuous_Absenteeism(string reportType, int companyID, string userName);
        #endregion

        #region"Salary"
        byte[] CreateReportFile_salarysheet_D(string reportType, int companyID, string userName);
        byte[] Sal_salarysheet_Details(string reportType, int companyID, string userName);
        byte[] Sal_salarysheet_Details_Excel(string reportType, int companyID, string userName);
        byte[] Sal_salarysheet_DetailsReport(string reportType, int companyID, string userName);
        byte[] Sal_salarysheet_ReportDetails(string reportType, int companyID, string userName);
        byte[] SalaryShert_LineWise_Summary(string reportType, int companyID, string userName);
        byte[] CreateReportFile_salarysheet_D_53(string reportType, int companyID, string userName);
        byte[] CreateReportFile_salarysheet_D_53_2(string reportType, int companyID, string userName);
        byte[] CreateReportFile_salarysheet_D_53_4(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Payslip(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Payslip_bangl(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Payslip_complaince_2hour(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Payslip_bayer_2hour_Bangla(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Payslip_bayer_4hour(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Payslip_bayer_4hour_Bangla(string reportType, int companyID, string userName);
        byte[] CreateReportFile_SalarySheetBank(string reportType, int companyID, string userName);
        byte[] CreateReportFile_SalarySheetEXOT(string reportType, int companyID, string userName);
        byte[] CreateReportFile_SalarySheetOTEXOT(string reportType, int companyID, string userName);
        byte[] CreateReportFile_OTDetails(string reportType, int companyID, string userName);
        byte[] CreateReportFile_OTSummary(string reportType, int companyID, string userName);
        byte[] CreateReportFile_SalarySheetSummary(string reportType, int companyID, string userName);
        byte[] CreateReportFile_SalarySheetSummaryYearly(string reportType, int companyID, string userName);
        byte[] SalarySheetSummarySalCategoryWise(string reportType, int companyID, string userName);
        byte[] SalarySheetSummary_Floor_SalCategoryWise(string reportType, int companyID, string userName);
        byte[] SalarySheetSummary_FloorAndLine_SalCategoryWise(string reportType, int companyID, string userName);
        byte[] CreateReportFile_SalarySheetOtExotSummary(string reportType, int companyID, string userName);
        byte[] Sal_SalarySheetBankExcel(string reportType, int companyID, string userName);
        byte[] Sal_SalarySheetAccount(string reportType, int companyID, string userName);
        byte[] Sal_EmployeeTiffinBillSummary(string reportType, int companyID, string userName);
        byte[] Sal_EmployeeNightBillSummary(string reportType, int companyID, string userName);
        byte[] Sal_EmployeeTiffinBillAmount(string reportType, int companyID, string userName);
        byte[] Sal_EmployeeNightBillAmount(string reportType, int companyID, string userName);
        byte[] Sal_EmployeeNightBillAmount_date_to_date(string reportType, int companyID, string userName);
        byte[] Sal_Eid_bonus(string reportType, int companyID, string userName);
        byte[] Sal_Eid_bonus_excel(string reportType, int companyID, string userName);
        byte[] Sal_EidBonusSummaryLineWise(string reportType, int companyID, string userName);
        byte[] Sal_Eid_bonus_bank(string reportType, int companyID, string userName);
        byte[] Sal_EidBonusSummaryHead(string reportType, int companyID, string userName);
        byte[] Sal_SalaryAdvance(string reportType, int companyID, string userName);
        byte[] CreateReportFile_SalarySheet_advanceBank(string reportType, int companyID, string userName);
        byte[] CreateReportFile_SalarySheet_Advance_Summary(string reportType, int companyID, string userName);
        byte[] SalaryShert_LineWise_Advance_Summary(string reportType, int companyID, string userName);
        byte[] CreateReportFile_SalarySheetcomplaince2hour(string reportType, int companyID, string userName);
        byte[] CreateReportFile_SalarySheetcomplaince4hour(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Date_TO_Date_OTSummary(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Date_TO_Date_OT_4_hour(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Date_TO_Date_OTSummary_subscetion_wise(string reportType, int companyID, string userName);
        byte[] CreateReportFile_SalarySheetSummary_2hour(string reportType, int companyID, string userName);
        byte[] CreateReportFile_SalarySheetSummary_4hour(string reportType, int companyID, string userName);
        byte[] SalaryShert_LineWise_Summary_2hour(string reportType, int companyID, string userName);
        byte[] SalaryShert_LineWise_Summary_4hour(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Date_TO_Date_2hourorabove_2hourOT(string reportType, int companyID, string userName);
        byte[] CreateReportFile_SalarySheetBank_2hrs(string reportType, int companyID, string userName);
        byte[] CreateReportFile_SalarySheetBank_4hrs(string reportType, int companyID, string userName);
        byte[] Sal_SalarySheetBank_Forwarding(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Date_TO_Date_4hourorabove_OT(string reportType, int companyID, string userName);
        byte[] CreateReportFile_R_OT(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Date_TO_Date_4hourorabove_OT_linewise_sum(string reportType, int companyID, string userName);
        byte[] Report_Anual_Leave_payment_rpt(string reportType, int companyID, string userName);
        byte[] Sal_EmployeeNightBillAmount_date_to_date_General_Shift(string reportType, int companyID, string userName);
        byte[] Comparative_Salary_Summary_2Month(string reportType, int companyID, string userName);
        byte[] Sal_EmpNightBillAmount_date_to_date_General_Shift_Bank(string reportType, int companyID, string userName);
        byte[] Sal_EmployeeNightBillAmount_date_to_date_General_Shift_Summary(string reportType, int companyID, string userName);
        byte[] Sal_EidBonusSummaryLineWiseExcel(string reportType, int companyID, string userName);
        #endregion

        #region"Others"
        byte[] CreateReportFile_salarysheet_D_Bank(string reportType, int companyID, string userName);
        byte[] CreateReportFile_Ecard(string reportType, int companyID, string userName);       
        byte[] CreateReportFile_Payslip_U1(string reportType, int companyID, string userName);
        byte[] Export_Report_SalarySheet(string reportType, int companyID, string userName);   
        byte[] testBarcode(string reportType, int companyID, string userName);
        #endregion

        Task<byte[]> PostPayReportView(PostReportViewPayload obj);
    }
}