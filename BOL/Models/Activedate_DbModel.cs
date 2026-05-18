using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Activedate_DbModel
    {
        [Key]
        public int sl { get; set; }
        public DateTime? pa_date { get; set; } = null;
        public int? pa_groupid { get; set; } = null;
        public string pa_user { get; set; } = null;
        public DateTime? pa_udate { get; set; } = null;
    }
}
