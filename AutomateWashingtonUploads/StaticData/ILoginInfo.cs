using System.Collections.Generic;

namespace AutomateWashingtonUploads.StaticData
{
    public interface ILoginInfo
    {
        string LoginUrl { get; set; }
        string Id { get; set; }
        string Password { get; set; }

        string MailerAddress { get; set; }
        string MailerPassword { get; set; }
        List<string> EmailRecipients { get; set; }
    }
}