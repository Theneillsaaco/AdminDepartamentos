namespace AdminDepartamentos.Models
{
    public class InquilinoModel
    {
        public int IdInquilino { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string cedula { get; set; }
        public int NumDepartamento { get; set; }
        public int NumInquilino { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
