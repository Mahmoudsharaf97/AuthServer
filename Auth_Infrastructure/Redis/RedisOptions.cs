namespace SME_Infrastructure.Redis
{
    public class RedisOptions
    {
        public bool Enabled { get; set; }
        public string? ConnectionString { get; set; }
        public string? Instance { get; set; }
        
    }
}