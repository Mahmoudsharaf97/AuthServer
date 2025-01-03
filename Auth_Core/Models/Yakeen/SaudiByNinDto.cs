using Auth_Core.Enums;

namespace Auth_Core.Models.Yakeen
{
    public class SaudiByNinDto
    {
        public string dateOfBirthG { get; set; }

        public string dateOfBirthH { get; set; }

        public string englishFirstName { get; set; }

        public string englishLastName { get; set; }

        public string englishSecondName { get; set; }

        public string englishThirdName { get; set; }

        public string familyName { get; set; }

        public string fatherName { get; set; }

        public string firstName { get; set; }

        public string Egender { get; set; }
        public Gender gender { get; set; }

        public bool genderSpecified { get; set; }

        public string grandFatherName { get; set; }
        public string idExpiryDate { get; set; }

        public string idIssuePlace { get; set; }

        public int logId { get; set; }

        public string occupationCode { get; set; }

        public string socialStatusDetailedDesc { get; set; }

        public string subtribeName { get; set; }
        public string errorId { get; set; }
        public YakeenErrorDetailsDto errorDetail { get; set; }
    }
}
