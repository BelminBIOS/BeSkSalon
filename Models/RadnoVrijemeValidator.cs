namespace BeSkSalon.Models
{
    public static class RadnoVrijemeValidator
    {
        
        public static readonly TimeSpan RadnoVrijemeOd = new TimeSpan(8, 0, 0);  
        public static readonly TimeSpan RadnoVrijemeDo = new TimeSpan(20, 0, 0); 
        
        public static (bool IsValid, string ErrorMessage) ValidateRadnoVrijeme(DateTime datum, TimeSpan vrijemeOd, TimeSpan vrijemeDo)
        {
            
            if (datum.DayOfWeek == DayOfWeek.Sunday)
            {
                return (false, "Salon ne radi nedjeljom.");
            }
            
            if (vrijemeOd < RadnoVrijemeOd)
            {
                return (false, $"Salon počinje raditi u {RadnoVrijemeOd:hh\\:mm}.");
            }
            
            if (vrijemeDo > RadnoVrijemeDo)
            {
                return (false, $"Salon završava sa radom u {RadnoVrijemeDo:hh\\:mm}.");
            }
            
            var terminDatumVrijeme = datum.Date.Add(vrijemeOd);
            if (terminDatumVrijeme < DateTime.Now)
            {
                return (false, "Ne možete zakazati termin u prošlosti.");
            }
            return (true, string.Empty);
        }
        
        public static bool IsRadniDan(DateTime datum)
        {
            return datum.DayOfWeek != DayOfWeek.Sunday;
        }
    }
}
