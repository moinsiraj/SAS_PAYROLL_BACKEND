using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class PostOffice_DbModel
    {
        [Key]
        public int po_id { get; set; }
        public int? po_division { get; set; } = null;
        public string po_divname { get; set; } = null;
        public int? po_district { get; set; } = null;
        public string po_disname { get; set; } = null;
        public int? po_thana { get; set; } = null;
        public string po_thananame { get; set; } = null;
        public string po_postoffice_name { get; set; } = null;
        public string po_postoffice_name_bangla { get; set; } = null;
        public string po_user { get; set; } = null;
        public DateTime? po_udate { get; set; } = null;
    }
}
