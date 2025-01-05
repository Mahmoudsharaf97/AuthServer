namespace Auth_Core.Models.Yakeen
{
    public class YakeenErrorDetailsDto
    {
        public string errorCode { get; set; }
        public string errorTitle { get; set; }
        public string errorMessage { get; set; }
        public string timestamp { get; set; }
        public string error { get; set; }
        public int status { get; set; }
        public string path { get; set; }
    }
}
