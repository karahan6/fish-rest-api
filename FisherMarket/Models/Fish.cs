namespace FisherMarket.Models
{
    public class Fish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Definition { get; set; }
        public double Price { get; set; }
        public byte [] Photo { get; set; }
        public string ContentType { get; set; }
    }
}
