namespace WebApp.Aplication.Models.Tables
{
    public class TableCreateModel
    {
        public int TableNumber { get; set; }
        public int Capacity { get; set; } = 4;
        public string Section { get; set; } = "Asosiy zal";
        public string TableStatus { get; set; } = "Available";
    }
}
