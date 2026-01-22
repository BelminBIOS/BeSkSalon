namespace BeSkSalon.Models
{
    public class Usluga
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public int Trajanje { get; set; }
        public decimal Cijena { get; set; }
        public int FrizerId { get; set; }
        public Frizer Frizer { get; set; } = null!;
        public ICollection<Termin> Termini { get; set; } = new List<Termin>();
    }
}