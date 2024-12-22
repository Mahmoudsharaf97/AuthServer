namespace Auth_Core.Global
{
    public class GlobalInfo
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber   { get; set; }
        public string ClientId      { set; get; }
        public string Token         { get; set; }
        public string SessionID        { get; set; }
        public string ClientDateOfBirth { set; get; }
        public static string lang { get; set; }
        public static string Channel { get; set; }

        public void SetValues(string UserName, string UserId, string lang, string channel ,
             string EmailAddress,
             string PhoneNumber,
             string ClientId,
             string Token,
             string SessionID ,
             string birthdate)
        {
            this.UserName = UserName;
            this.UserId = UserId;
            this.EmailAddress= EmailAddress;
            this.PhoneNumber = PhoneNumber;
            this.ClientId = ClientId;
            this.Token = Token;
            this.SessionID = SessionID;
            this.ClientDateOfBirth = birthdate;
            GlobalInfo.lang = lang;
            GlobalInfo.Channel = channel;

        }
    }
}
