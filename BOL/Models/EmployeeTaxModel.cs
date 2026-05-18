using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class EmployeeTaxModel
    {
        public int compid { get; set; }
        public int emp_serial { get; set; }
        public int txYear_id { get; set; }
        public int dtl_invstment { get; set; }
        public string dtl_invstment_title { get; set; }
        public int dtl_advcTax { get; set; }
        public int dtl_TaxOnCR_invstment { get; set; }
        public int dtl_ttlTaxAble_Inc { get; set; }
        public int dtl_ttlTax_Inc { get; set; }
        public int dtl_ttlTaxPay_inc { get; set; }
        public int dtl_monthly_tax { get; set; }
        public int dtl_interestBnk_inc { get; set; }
        public string userName { get; set; }
        public List<EmployeeTaxChild> taxBenefits { get; set; }
    }
    public class EmployeeTaxChild
    {
        public string txbf_type { get; set; }
        public string txbf_title { get; set; }
        public decimal txbf_amt { get; set; }
    }
    public class EmployeeTaxBenefits : EmployeeTaxChild
    {
        public int txbf_empSerial { get; set; }
        public int txbf_compid { get; set; }
        public int txbf_year { get; set; }
        public string userName { get; set; }

        public static EmployeeTaxBenefits TaxBenefitsQuery(EmployeeTaxModel obj, EmployeeTaxChild loopObj)
        {
            var result = new EmployeeTaxBenefits
            {
                txbf_empSerial = obj.emp_serial,
                txbf_compid = obj.compid,
                txbf_year = obj.txYear_id,
                txbf_type = loopObj.txbf_type,
                txbf_title = loopObj.txbf_title,
                txbf_amt = loopObj.txbf_amt,
                userName = obj.userName
            };
            return result;
        }
    }
    public class EmployeeTaxReport
    {
        public string reportType { get; set; }
        public int compid { get; set; }
        public int emp_serial { get; set; }
        public int taxYear { get; set; }
        public string userName { get; set; }
    }
    public class EmployeeTaxReportRender
    {
        public string message { get; set; }
        public byte[] reportByte { get; set; }
    }
    public class EmployeeTaxChallan
    {
        public int challan_id { get; set; }
        public int compid { get; set; }
        public int taxYear { get; set; }
        public int empSerial { get; set; }
        public string challanNo { get; set; }
        public string challanFilePath { get; set; }
        public string depositDate { get; set; }
        public int bankid { get; set; }
        public int branch_id { get; set;}
        public decimal amt { get; set; }
        public decimal claim_amt { get; set; }
        public string userName { get; set; }
        public IFormFile file { get; set; }
    }
    public class BankBranch
    {
        [Key]
        public int bnc_id { get; set; }
        public int bnk_id { get; set; }
        public string bnc_name { get; set; }
        public string bnc_swift_code { get; set; }
        public string bnc_routeNo { get; set; }
        public string bnc_contractNo { get; set; }
        public string bnc_address { get; set; }
        public string bnc_entyBy { get; set; }
        public DateTime? bnc_entDt { get; set; }
        public string bnc_upBy { get; set; }
        public DateTime? bnc_upDt { get; set; }
    }
    public class BankBranchPayload
    {
        public int bnc_id { get; set; }
        public int bnk_id { get; set; }
        public string b_name { get; set; }
        public string b_swift_code { get; set; }
        public string b_routeNo { get; set; }
        public string b_contractNo { get; set; }
        public string b_address { get; set; }
        public string userName { get; set; }

        public static BankBranch PayloadToBankBranchDb_Obj(BankBranchPayload obj, BankBranch dbObj=null)
        {
            var result = new BankBranch
            {
                bnc_id = obj.bnc_id,
                bnk_id = obj.bnk_id,
                bnc_name = obj.b_name,
                bnc_swift_code = obj.b_swift_code,
                bnc_routeNo = obj.b_routeNo,
                bnc_contractNo = obj.b_contractNo,
                bnc_address = obj.b_address,
                bnc_entyBy = dbObj == null ? obj.userName : dbObj.bnc_entyBy,
                bnc_entDt = dbObj == null ? DateTime.Now : dbObj.bnc_entDt,
                bnc_upBy = dbObj == null ? null : obj.userName,
                bnc_upDt = dbObj == null ? null : DateTime.Now,
            };
            return result;
        }
    }
}