using SME_Core;

namespace Auth_Core
{
    public  class YakeenException :Exception
    {
        public readonly ExceptionEnum ErrorNumber;
        public string ErrorMessageEn = "";
        public string ErrorMessageAr = "";
        public YakeenException(string errorMessage) : base(errorMessage)
        {
            ErrorMessageEn = errorMessage;
            ErrorMessageAr = errorMessage;
            ErrorNumber = ExceptionEnum.ModelValidationError;
        }
        public YakeenException(ExceptionEnum errorNumber, params object?[] args) : base()
        {
            this.ErrorNumber = errorNumber;
            string[] tempMsg;
            YakeenExceptions.ExceptionMessages.TryGetValue((int)this.ErrorNumber, out tempMsg);
            try
            {
                ErrorMessageEn = string.Format(tempMsg[0], args);
                ErrorMessageAr = string.Format(tempMsg[1], args);
            }
            catch
            {

            }
        }
    }
}
