using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Loan_DbModel
    {
        [Key]
        public int l_loanserial { get; set; }
        public int? l_empserial { get; set; } = null;
        public DateTime? l_date { get; set; } = null;
        public decimal? l_loan { get; set; } = null;
        public int? l_period { get; set; } = null;
        public decimal? l_rental { get; set; } = null;
        public decimal? l_balance { get; set; } = null;
        public int? l_cat { get; set; } = null;
        public DateTime? l_startdate { get; set; } = null;
        public bool? l_active { get; set; } = null;
        public string l_user { get; set; } = null;
        public DateTime? l_udate { get; set; } = null;
    }
    public class DgPayLoan
    {
        public int LLoanserial { get; set; }
        public int? LEmpserial { get; set; } = null;
        public DateTime? LDate { get; set; } = null;
        public decimal? LLoan { get; set; } = null;
        public int? LPeriod { get; set; } = null;
        public decimal? LRental { get; set; } = null;
        public decimal? LBalance { get; set; } = null;
        public int? LCat { get; set; } = null;
        public DateTime? LStartdate { get; set; } = null;
        public bool? LActive { get; set; } = null;
        public string LUser { get; set; } = null;
        public DateTime? LUdate { get; set; } = null;

        public static Loan_DbModel CustomToDbModel(DgPayLoan obj)
        {
            try
            {
                var dbModel = new Loan_DbModel
                {
                    l_loanserial = obj.LLoanserial,
                    l_empserial = obj.LEmpserial,
                    l_date = obj.LDate,
                    l_loan = obj.LLoan,
                    l_period = obj.LPeriod,
                    l_rental = obj.LRental,
                    l_balance = obj.LBalance,
                    l_cat = obj.LCat,
                    l_startdate = obj.LStartdate,
                    l_active = obj.LActive,
                    l_user = obj.LUser,
                    l_udate = obj.LUdate
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayLoan DbToCustomModel(Loan_DbModel obj)
        {
            try
            {
                var customModel = new DgPayLoan
                {
                    LLoanserial = obj.l_loanserial,
                    LEmpserial = obj.l_empserial,
                    LDate = obj.l_date,
                    LLoan = obj.l_loan,
                    LPeriod = obj.l_period,
                    LRental = obj.l_rental,
                    LBalance = obj.l_balance,
                    LCat = obj.l_cat,
                    LStartdate = obj.l_startdate,
                    LActive = obj.l_active,
                    LUser = obj.l_user,
                    LUdate = obj.l_udate
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