namespace BeSkSalon.Models.ViewModels
{
    public class MojiTerminiViewModel
    {
        public List<TerminDetaljiViewModel> Termini { get; set; } = new List<TerminDetaljiViewModel>();
    }
    public class TerminDetaljiViewModel
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public TimeSpan VrijemeOd { get; set; }
        public TimeSpan VrijemeDo { get; set; }
        public string Status { get; set; } = string.Empty;
        public string FrizerIme { get; set; } = string.Empty;
        public string UslugaNaziv { get; set; } = string.Empty;
        public decimal UslugaCijena { get; set; }
        public int UslugaTrajanje { get; set; }
    }
}