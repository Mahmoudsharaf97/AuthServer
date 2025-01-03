using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
namespace Auth_Core
{
    [Table("Users")]
    public class ApplicationUser<T> : IdentityUser  
    {
        public override string UserName { get; set; }
		public string FullNameAr { get; set; }
		public string FullNameEn { get; set; }
		public string DateOfBirthH { get; set; }
		public DateTime DateOfBirthG { get; set; }
		public string? ClientId { get; set; }       
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? LastSuccessLogin { set; get; }
        public bool IsDeleted { get; set; }=false;   
        public long? NationalId { get; set; }
        public bool IsPhoneVerifiedByYakeen { get; set; } = false;
        public  string? DateOfBirthYear { get; set; }
        public string? DateOfBirthMonth { get; set; }
        public string FullNameAr { get; set; }
        public string Channel { get; set; }
        public bool IsCorporateUser { get; set; }
    }
}
