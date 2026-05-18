using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class AllProcess_DbModel
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; } = null;
        public DateTime? Date { get; set; } = null;
    }
    public class AllProcess
    {
        public int Id { get; set; }
        public string Name { get; set; } = null;
        public DateTime? Date { get; set; } = null;

        public static AllProcess_DbModel CustonToDbModel(AllProcess obj)
        {
            try
            {
                var dbModel = new AllProcess_DbModel
                {
                    ID = obj.Id,
                    Name = obj.Name,
                    Date = obj.Date
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static AllProcess DbToCustomModel(AllProcess_DbModel obj)
        {
            try
            {
                var customModel = new AllProcess
                {
                    Id = obj.ID,
                    Name = obj.Name,
                    Date = obj.Date
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
    public class SingleSalaryProcess
    {
        public int[] emp_serial { get; set; }
        public int groupid { get; set; }
        public int compid { get; set; }
        public string pDate { get; set; }
    }
}
