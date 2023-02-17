namespace ToDoApi.Models
{
    public class AdmissionModel
    {
        public int AdmissionId { get; set; }
        public int PersonId { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public DateTime DateOfDischarge { get; set; }

    }
}
