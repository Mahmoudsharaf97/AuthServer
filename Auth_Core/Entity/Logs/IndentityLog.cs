using Auth_Core.Entities;
namespace Auth_Core.Entity
{
    public  class IndentityLog : BaseLog
    {

        public IndentityLog() : base()
        {

        }
        public IndentityLog(
          string QuiryParams
        , string Request
        , DateTime CreateDate
        , string CreateUser
        , string ServerIP
        , string Channel
        , string UserIP
        , string ServiceResponseTimeInSeconds
        , string ErrorCode
        , string ErrorDescription
        , string InnerException
        )
        {
            this.QuiryParams = QuiryParams;
            this.Request = Request;
            this.CreateDate = CreateDate;
            this.CreateUser = CreateUser;
            this.ServerIP = ServerIP;
            this.Channel = Channel;
            this.UserIP = UserIP;
            this.ServiceResponseTimeInSeconds = ServiceResponseTimeInSeconds;
            this.ErrorCode = ErrorCode;
            this.ErrorDescription = ErrorDescription;
            this.InnerException = InnerException;
        }
        public string? QuiryParams { get; set; }
        public string? Request { get; set; }
        public DateTime CreateDate { get; set; }
        public string? CreateUser { get; set; }
        public string? ServerIP { get; set; }
        public string? Channel { get; set; }
        public string? UserIP { get; set; }
        public string? ServiceResponseTimeInSeconds { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorDescription { get; set; }
        public string? InnerException { get; set; }
    }
}
