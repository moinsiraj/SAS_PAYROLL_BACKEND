using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Stampcharge_DbModel
    {
        [Key]
        public int Id { get; set; }
        public int comID { get; set; }
        public string comName { get; set; } = null;
        public int? stamp { get; set; } = null;
    }
    public class DgStampcharge
    {
        public int Id { get; set; }
        public int ComId { get; set; }
        public string ComName { get; set; } = null;
        public int? Stamp { get; set; } = null;

        public static Stampcharge_DbModel CustomToDbModel(DgStampcharge obj)
        {
            try
            {
                var dbModel = new Stampcharge_DbModel
                {
                    Id = obj.Id,
                    comID = obj.ComId,
                    comName = obj.ComName,
                    stamp = obj.Stamp
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgStampcharge DbToCustomModel(Stampcharge_DbModel obj)
        {
            try
            {
                var customModel = new DgStampcharge
                {
                    Id = obj.Id,
                    ComId = obj.comID,
                    ComName = obj.comName,
                    Stamp = obj.stamp
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