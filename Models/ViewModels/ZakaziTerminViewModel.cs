namespace BeSkSalon.Models.ViewModels
{
    public class ZakaziTerminViewModel
    {
        public int FrizerId { get; set; }
        public int UslugaId { get; set; }
        public DateTime Datum { get; set; }
        public TimeSpan VrijemeOd { get; set; }
        
        public List<Frizer> Frizeri { get; set; } = new List<Frizer>();
        public List<Usluga> Usluge { get; set; } = new List<Usluga>();
        public List<TimeSpan> SlobodniTermini { get; set; } = new List<TimeSpan>();
    }
}
