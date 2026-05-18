using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class EmployeeTransferModel
    {
        [Required(ErrorMessage="Enter Transfer Type")]
        public string transferType { get; set; }

        [Required(ErrorMessage ="Select Old Company")]
        public int fromCompanyID { get; set; }

        public int toCompanyID { get; set; }

        [Required(ErrorMessage ="Select Old Employee No")]
        public int employeeID { get; set; }

        [Required(ErrorMessage = "Enter Transfer Date")]
        public string transferDate { get; set; }

        public int newEmployeeID { get; set; }
        public string newProxid { get; set; }
        public int salaryCatID { get; set; }

        [Required(ErrorMessage = "Select New Department")]
        public int departmentID { get; set; }

        [Required(ErrorMessage = "Select New Section")]
        public int sectionID { get; set; }

        [Required(ErrorMessage = "Select New Building")]
        public int buildingID { get; set; }

        [Required(ErrorMessage = "Select New Floor")]
        public int floorID { get; set; }

        [Required(ErrorMessage = "Select New Line")]
        public int lineID { get; set; }

        [Required(ErrorMessage = "Select New Shift")]
        public int shiftID { get; set; }
        public string reason { get; set; }

        [Required(ErrorMessage = "Enter UserName")]
        public string userName { get; set; }
    }
}