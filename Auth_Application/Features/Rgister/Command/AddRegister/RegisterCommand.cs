using Auth_Core;
using IdentityApplication.Models;
using MediatR;
namespace Auth_Application.Features
{
    public class RegisterCommand : RegisterBaseModel, IRequest<RegisterOutPut>
    {

        public override void ValidateRegisterModel()
        {
            base.ValidateRegisterModel();

            if (this.RegisterType == (int)Auth_Core.Enums.RegisterType.VerifyYakeenMobile)
            {
                if (string.IsNullOrEmpty(this.Password))
                    throw new AppException(ExceptionEnum.PasswordIsEmpty);
            }
            else if (this.RegisterType == (int)Auth_Core.Enums.RegisterType.VerifyYakeenNationalId)
            {
                if (this.BirthDateMonth < 1)
                    throw new AppException(ExceptionEnum.ErrorBirthMonth);
                if (this.BirthDateYear < 1)
                    throw new AppException(ExceptionEnum.ErrorBirthYear);
            }
            else if(this.RegisterType == (int)Auth_Core.Enums.RegisterType.VerifyOTP)
            {
                if (this.OTP<1)
                    throw new AppException(ExceptionEnum.NationalIdEmpty);
            }
            else
            {
                throw new AppException(ExceptionEnum.EmptyInputParameter);
            }
        }
    }
}
