namespace CodeChallenge.Models
{
    public class ReportingStructure
    {
        public int NumberOfReports { get; set; }
        public Employee Employee { get; set; }
        public ReportingStructure(Employee employee, int numberOfReports) 
        {
            Employee = employee;
            NumberOfReports = numberOfReports;
        }
    }
}
