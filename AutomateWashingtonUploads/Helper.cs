using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace AutomateWashingtonUploads
{
    public static class Helper
    {

        public static List<Completion> ListToCompletionList(List<string> stringList)
        {
            List<string[]> dividedStrings = new List<string[]>();
            List<Completion> myCompletionList = new List<Completion>();
            foreach (string rawCompletion in stringList)
            {
                string[] itemArray = rawCompletion.Split('|');
                for (int i = 0; i < itemArray.Length; i++)
                {
                    itemArray[i] = itemArray[i].Trim();
                }
                dividedStrings.Add(itemArray);
            }

            for (int i = 0; i < dividedStrings.Count - 1; i++)
            {
                Completion completion = new Completion();
                completion.Course = dividedStrings[i][0];
                completion.Date = dividedStrings[i][1];
                completion.License = dividedStrings[i][2];
                completion.Name = dividedStrings[i][3];
                myCompletionList.Add(completion);
            }
            return myCompletionList;
        }

        public static void SendEmail(StreamReader reader, LoginInfo info)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(info.MailerAddress);
                mail.To.Add(info.EmailRecipient);
                mail.Subject = $"Washington Uploads: {DateTime.Today}";
                mail.Body = "mail with attachment";

                Attachment attachment;
                attachment = new Attachment(Logger.StreamLocation);
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(info.MailerAddress, info.MailerPassword);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                Console.WriteLine("Email sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static List<string> ConvertDataToStringList()
        {
            string s = "1";
            List<string> result = new List<string> { };
            while (!string.IsNullOrEmpty(s))
            {
                s = Console.ReadLine();
                result.Add(s);
            }
            return result;
        }

        public static string ChangeSecondToLastCharacter(string s)
        {
            char[] license = s.ToCharArray();
            license[10] = 'O';
            return new string(license);
        }

        public static bool IsLicenseTwelveCharacters(string license) => license.Length == 12;
    }
}
