using Auth_Core.Enums;
using System;
namespace Auth_Core.UseCase
{
  
    public class CustomerIdYakeenInfoDto
    {
        public CustomerIdYakeenInfoDto()
        {
            Error = new YakeenErrorDto();
        }

        public bool Success { get; set; }

        public YakeenErrorDto Error { get; set; }

        public bool IsCitizen { get; set; }

        public int LogId { get; set; }

        public string IdIssuePlace { get; set; }

        public Gender Gender { get; set; }

        public short NationalityCode { get; set; }

        public DateTime DateOfBirthG { get; set; }

        public string DateOfBirthH { get; set; }
        public string IdExpiryDate { get; set; }

        public string SocialStatus { get; set; }

        public string OccupationCode { get; set; }

        public string EnglishFirstName { get; set; }

        public string EnglishLastName { get; set; }

        public string EnglishSecondName { get; set; }

        public string EnglishThirdName { get; set; }

        public string LastName { get; set; }

        public string SecondName { get; set; }

        public string FirstName { get; set; }

        public string ThirdName { get; set; }

        public string SubtribeName { get; set; }

        public string OccupationDesc { get; set; }

    }
}