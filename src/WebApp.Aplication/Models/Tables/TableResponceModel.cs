namespace WebApp.Aplication.Models.Tables
{
    public class TableResponceModel
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; } = "available";
        public string Section { get; set; } = "Asosiy zal";
    }
}
