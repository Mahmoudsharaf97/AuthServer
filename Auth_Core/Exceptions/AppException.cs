

namespace Auth_Core
{
    public class AppException : Exception
    {
        public readonly ExceptionEnum ErrorNumber;
        public string ErrorMessageEn = "";
        public string ErrorMessageAr = "";
        public AppException(string errorMessage) : base(errorMessage)
        {
            ErrorMessageEn = errorMessage;
            ErrorMessageAr = errorMessage;
            ErrorNumber = ExceptionEnum.ModelValidationError;
        }
        public AppException(ExceptionEnum errorNumber, params object?[] args) : base()
        {
            this.ErrorNumber = errorNumber;
            string[] tempMsg;
            AppExceptions.ExceptionMessages.TryGetValue((int)this.ErrorNumber, out tempMsg);
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
