using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Village_DbModel
    {
        [Key]
        public int vill_id { get; set; }
        public int? vill_division { get; set; } = null;
        public string vill_divname { get; set; } = null;
        public int vill_district { get; set; }
        public string vill_disname { get; set; }
        public int vill_thana { get; set; }
        public string vill_thananame { get; set; }
        public string vill_village_name { get; set; }
        public string vill_village_namebangla { get; set; }
        public string vill_user { get; set; } = null;
        public DateTime? vill_udate { get; set; } = null;
    }
}