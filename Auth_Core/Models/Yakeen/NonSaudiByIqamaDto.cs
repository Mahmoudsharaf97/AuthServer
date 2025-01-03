
using Auth_Core.Enums;

namespace Auth_Core.Models.Yakeen
{
    public class NonSaudiByIqamaDto
    {
        public string dateOfBirthG { get; set; }

        public string dateOfBirthH { get; set; }

        public string englishFirstName { get; set; }

        public string englishLastName { get; set; }

        public string englishSecondName { get; set; }

        public string englishThirdName { get; set; }

        public string firstName { get; set; }

        public string Egender { get; set; }
        public Gender gender { get; set; }

        public bool genderSpecified { get; set; }

        public string iqamaExpiryDateH { get; set; }

        public string iqamaIssuePlaceDesc { get; set; }

        public string lastName { get; set; }

        //public LicensesDto[] licensesListList { get; set; }

        public int logId { get; set; }

        public short nationalityCode { get; set; }

        public string nationalityMofaCode { get; set; }

        public string occupationDesc { get; set; }
        public string occupationCode { get; set; }

        public string secondName { get; set; }

        public string socialStatus { get; set; }

        public string thirdName { get; set; }
        public string errorId { get; set; }
        public YakeenErrorDetailsDto errorDetail { get; set; }

    }
}
