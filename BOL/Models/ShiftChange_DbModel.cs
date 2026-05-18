using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class ShiftChange_DbModel
    {
        [Key]
        public int sc_serial { get; set; }
        public decimal? sc_empserial { get; set; } = null;
        public DateTime? sc_start_date { get; set; } = null;
        public DateTime? sc_effect_date { get; set; } = null;
        public DateTime? sc_end_date { get; set; } = null;
        public int? sc_old_shift { get; set; } = null;
        public int? sc_new_shift { get; set; } = null;
        public string sc_transfer_type { get; set; } = null;
        public string sc_user { get; set; } = null;
        public DateTime? sc_udate { get; set; } = null;

        public static ShiftChange_DbModel CustomToDbModel(DgPayShiftChange obj)
        {
            try
            {
                var dbModel = new ShiftChange_DbModel
                {
                    sc_serial = obj.ScSerial,
                    sc_empserial = obj.ScEmpserial,
                    sc_start_date = obj.ScStartDate,
                    sc_effect_date = obj.ScEffectDate,
                    sc_end_date = obj.ScEndDate,
                    sc_old_shift = obj.ScOldShift,
                    sc_new_shift = obj.ScNewShift,
                    sc_transfer_type = obj.ScTransferType,
                    sc_user = obj.ScUser,
                    sc_udate = obj.ScUdate
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
    }
    public class DgPayShiftChange
    {
        public int ScSerial { get; set; }
        public decimal? ScEmpserial { get; set; } = null;
        public DateTime? ScStartDate { get; set; } = null;
        public DateTime? ScEffectDate { get; set; } = null;
        public DateTime? ScEndDate { get; set; } = null;
        public int? ScOldShift { get; set; } = null;
        public int? ScNewShift { get; set; } = null;
        public string ScTransferType { get; set; } = null;
        public string ScUser { get; set; } = null;
        public DateTime? ScUdate { get; set; } = null;

        public static DgPayShiftChange DbToCustomModel(ShiftChange_DbModel obj)
        {
            try
            {
                var customModel = new DgPayShiftChange
                {
                    ScSerial = obj.sc_serial,
                    ScEmpserial = obj.sc_empserial,
                    ScStartDate = obj.sc_start_date,
                    ScEffectDate = obj.sc_effect_date,
                    ScEndDate = obj.sc_end_date,
                    ScOldShift = obj.sc_old_shift,
                    ScNewShift = obj.sc_new_shift,
                    ScTransferType = obj.sc_transfer_type,
                    ScUser = obj.sc_user,
                    ScUdate = obj.sc_udate
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