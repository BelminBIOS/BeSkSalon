namespace BeSkSalon.Models
{
    public class Frizer
    {
        public int Id { get; set; }
        public string Ime { get; set; } = string.Empty;
        public string Tip { get; set; } = string.Empty;
        public ICollection<Usluga> Usluge { get; set; } = new List<Usluga>();
        public ICollection<Termin> Termini { get; set; } = new List<Termin>();
    }
}