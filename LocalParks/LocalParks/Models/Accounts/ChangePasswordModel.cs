namespace LocalParks.Models
{
    public class ChangePasswordModel
    {
        public string Old { get; set; }
        public string New { get; set; }
        public string NewConfirmed { get; set; }
    }
}
