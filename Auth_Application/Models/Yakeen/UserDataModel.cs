using Newtonsoft.Json;

namespace Auth_Application.Models
{
    public  class UserDataModel 
    {
        [JsonProperty("iS_FullName_Exist")]
        public bool IsExist { get; set; }

        [JsonProperty("aR_FullName")]
        public string FullNameAr { get; set; }

        [JsonProperty("en_FullName")]
        public string FullNameEn { get; set; }
    }
}