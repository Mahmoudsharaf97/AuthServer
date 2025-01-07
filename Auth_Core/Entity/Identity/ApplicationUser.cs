using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
namespace Auth_Core
{
    [Table("Users")]
    public class ApplicationUser<T> : IdentityUser  
    {
		public string? FullNameAr { get; set; }
		public string? FullNameEn { get; set; }
		public string? DateOfBirthH { get; set; }
		public DateTime DateOfBirthG { get; set; }
		public string? ClientId { get; set; }       
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? LastSuccessLogin { get; set; }
        public DateTime? LastLoginDate { set; get; }
        public DateTime? LockoutEndDateUtc { set; get; }
        public bool IsDeleted { get; set; }=false;   
        public long? NationalId { get; set; }
        public bool IsPhoneVerifiedByYakeen { get; set; } = false;
        public bool IsYakeenNationalIdVerified { get; set; } = false;
        public  string? DateOfBirthYear { get; set; }
        public string? DateOfBirthMonth { get; set; }
        public string? Channel { get; set; }
        public string? DeviceToken { get; set; }
        public bool IsCorporateUser { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? LanguageId { get; set; }
    }
}
