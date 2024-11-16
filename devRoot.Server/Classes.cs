namespace devRoot.Server
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

    public class Tag : TagRequest
    {
        public int? ID { get; set; }
    }

    public class TagRequest
    {
        public string? Name { get; set; }
    }
}
