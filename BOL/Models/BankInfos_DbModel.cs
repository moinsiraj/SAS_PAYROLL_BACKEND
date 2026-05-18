using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class BankInfos_DbModel
    {
        [Key]
        public int Bank_Code { get; set; }
        public string Bank_Name { get; set; } = null;
        public string Bank_Name_Bangla { get; set; } = null;
        public string Bank_Address { get; set; } = null;
        public string Telephone_No { get; set; } = null;
        public string Fax { get; set; } = null;
        public int? Country_Code { get; set; } = null;
        public string Swift_Code { get; set; } = null;
        public DateTime? Created_Date { get; set; } = null;
        public string Created_By { get; set; } = null;
        public string Bshortname { get; set; } = null;
        public bool Showing_permission { get; set; } = false;
    }
    public class DgPayBankInfo
    {
        public int BankCode { get; set; }
        public string BankName { get; set; } = null;
        public string BankNameBangla { get; set; } = null;
        public string BankAddress { get; set; } = null;
        public string TelephoneNo { get; set; } = null;
        public string Fax { get; set; } = null;
        public int? CountryCode { get; set; } = null;
        public string SwiftCode { get; set; } = null;
        public DateTime? CreatedDate { get; set; } = null;
        public string CreatedBy { get; set; } = null;
        public string Bshortname { get; set; } = null;
        public bool ShowingPermission { get; set; } = false;

        public static BankInfos_DbModel CustonToDbModel(DgPayBankInfo obj)
        {
            try
            {
                var dbModel = new BankInfos_DbModel
                {
                    Bank_Code = obj.BankCode,
                    Bank_Name = obj.BankName,
                    Bank_Name_Bangla = obj.BankNameBangla,
                    Bank_Address = obj.BankAddress,
                    Telephone_No = obj.TelephoneNo,
                    Fax = obj.Fax,
                    Country_Code = obj.CountryCode,
                    Swift_Code = obj.SwiftCode,
                    Created_Date = obj.CreatedDate,
                    Created_By = obj.CreatedBy,
                    Bshortname = obj.Bshortname,
                    Showing_permission = obj.ShowingPermission
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayBankInfo DbToCustomModel(BankInfos_DbModel obj)
        {
            try
            {
                var customModel = new DgPayBankInfo
                {
                    BankCode = obj.Bank_Code,
                    BankName = obj.Bank_Name,
                    BankNameBangla = obj.Bank_Name_Bangla,
                    BankAddress = obj.Bank_Address,
                    TelephoneNo = obj.Telephone_No,
                    Fax = obj.Fax,
                    CountryCode = obj.Country_Code,
                    SwiftCode = obj.Swift_Code,
                    CreatedDate = obj.Created_Date,
                    CreatedBy = obj.Created_By,
                    Bshortname = obj.Bshortname,
                    ShowingPermission = obj.Showing_permission
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