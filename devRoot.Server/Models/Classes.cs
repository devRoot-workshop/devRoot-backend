namespace devRoot.Server.Models
{
    public class Rat
    {
        public enum RatType
        {
            Lab,
            Rex,
            Hairless
        }

        public string Name { get; set; }
        public RatType Type { get; set; }
    }
}
