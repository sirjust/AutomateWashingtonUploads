using System.Collections.Generic;

namespace AutomateWashingtonUploads.StaticData
{
    public class LoginInfo : ILoginInfo
    {
        public string LoginUrl { get; set; }  = "https://secureaccess.wa.gov/myAccess/saw/select.do";
        public string Id { get; set; } = "1599";
        public string Password { get; set; } = "1SecureaccessforWAinfo2";
        public string MailerAddress { get; set; } = "anytimecemailer@gmail.com";
        public string MailerPassword { get; set; } = "22@@WASHINGTONuploads";
        public List<string> EmailRecipients { get; set; } = new List<string> { "jmooretenor@gmail.com" };
    }
}