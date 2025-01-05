namespace Auth_Core.Models.Yakeen
{
    public  class YakeenOutput
    {
        public enum ErrorCodes
        {
            Success = 1,
            NullResponse,
            UnspecifiedError,
            ServiceError,
            NullRequest,
            ServiceException,
            YakeenServiceException,
            DateOfBirthGIsEmpty,
            NoAddressFound,
            CommunicationException,
            TimeoutException,
            ModelYearIsNull,
            SequenceNumberIsNullOrEmpty,
            DateOfBirthEmpty
        }
        public ErrorCodes ErrorCode
        {
            get;
            set;
        }
        public string ErrorDescription
        {
            get;
            set;
        }
        public NonSaudiByIqamaDto NonSaudiByIqamaDto { get; set; }
        public SaudiByNinDto SaudiByNinDto { get; set; }
    }
}
