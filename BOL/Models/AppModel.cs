namespace BOL.Models
{
    public class AppModel
    {
        public int total_employee { get; set; }
        public int total_Gross { get; set; }
        public int total_Staff { get; set; }
        public int total_Staff_gross { get; set; }
        public int total_Worker { get; set; }
        public int total_Worker_gross { get; set; }
        public int total_male { get; set; }
        public int total_female { get; set; }
        public int thismonth_join { get; set; }
        public int totalpresent3dayago { get; set; }
        public int totalAbsent3dayago { get; set; }
        public int totallate3dayago { get; set; }
        public int totalpresent2dayago { get; set; }
        public int totalAbsent2dayago { get; set; }
        public int totallate2dayago { get; set; }
        public int totalpresent1dayago { get; set; }
        public int totalAbsent1dayago { get; set; }
        public int totallate1dayago { get; set; }
        public int totalpresentToday { get; set; }
        public int totalAbsentToday { get; set; }
        public int totallateToday { get; set; }

        public int totallLeave3dayago { get; set; }
        public int totallLeave2dayago { get; set; }
        public int totallLeave1dayago { get; set; }
        public int totallLeaveToday { get; set; }
        public String First_Month_Name { get; set; }
        public int Salary_Frist_Month { get; set; }
        public String Second_Month_Name { get; set; }
        public int Salary_Second_Month { get; set; }
        public String Third_Month_Name { get; set; }
        public int Salary_Third_Month { get; set; }

    }
}
