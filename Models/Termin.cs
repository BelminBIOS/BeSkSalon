using Microsoft.AspNetCore.Identity;
namespace BeSkSalon.Models
{
    public class Termin
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public TimeSpan VrijemeOd { get; set; }
        public TimeSpan VrijemeDo { get; set; }
        public string Status { get; set; } = "Zakazan"; 
        public int FrizerId { get; set; }
        public Frizer Frizer { get; set; } = null!;
        public int UslugaId { get; set; }
        public Usluga Usluga { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;
        public IdentityUser User { get; set; } = null!;
    }
}
