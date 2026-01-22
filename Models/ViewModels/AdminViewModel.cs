using System.ComponentModel.DataAnnotations;
namespace BeSkSalon.Models.ViewModels
{
    public class FrizerViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ime je obavezno")]
        [Display(Name = "Ime frizera")]
        public string Ime { get; set; } = string.Empty;
        [Required(ErrorMessage = "Tip je obavezan")]
        [Display(Name = "Tip frizera")]
        public string Tip { get; set; } = string.Empty;
    }
    public class UslugaViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naziv je obavezan")]
        [Display(Name = "Naziv usluge")]
        public string Naziv { get; set; } = string.Empty;
        [Required(ErrorMessage = "Trajanje je obavezno")]
        [Display(Name = "Trajanje (min)")]
        [Range(15, 300, ErrorMessage = "Trajanje mora biti između 15 i 300 minuta")]
        public int Trajanje { get; set; }
        [Required(ErrorMessage = "Cijena je obavezna")]
        [Display(Name = "Cijena (KM)")]
        [Range(0.01, 10000, ErrorMessage = "Cijena mora biti pozitivan broj")]
        public decimal Cijena { get; set; }
        [Required(ErrorMessage = "Frizer je obavezan")]
        [Display(Name = "Frizer")]
        public int FrizerId { get; set; }
        public List<Frizer> Frizeri { get; set; } = new List<Frizer>();
    }
}
