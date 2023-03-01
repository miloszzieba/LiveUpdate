namespace LiveUpdatePerformance.Models
{
    public class Row
    {
        public int Id { get; set; }
        public string Ticker { get; set; } = "";
        public decimal LastValue { get; set; }
        public DateTime LastValueDate { get; set; }
        public decimal HighestBuyValue { get; set; }
        public int HighestBuyVolume { get; set; }
        public decimal LowestSellValue { get; set; }
        public int LowestSellVolume { get; set; }
    }
}
