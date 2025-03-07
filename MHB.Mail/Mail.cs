using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace MHB.Mail
{
    public class Mail
    {
        #region Constants

        private const int MHB_API_KEY_LENGTH = 32;
        private const string MHB_DEFAULT_EMAIL = "support@myhomebills.info";
        private const string MHB_DEFAULT_MAIL_SERVER_ADDRESS = "mail.myhomebills.info";
        private const string MHB_DEFAULT_MAIL_SERVER_PASSWORD = "mitko123";
        public const string MHB_DEFAULT_IMPORT_RESPONSE_EMAIL_SUBJECT = "Imported from email";

        #endregion Constants

        private static bool _isRunning = false;

        #region Properties

        public string DefaultEmail => MHB_DEFAULT_EMAIL;

        private string _defaultMailServerAddress = MHB_DEFAULT_MAIL_SERVER_ADDRESS;

        public string DefaultMailServerAddress
        {
            get
            {
                return this._defaultMailServerAddress;
            }
            set
            {
                this._defaultMailServerAddress = value;
            }
        }

        private string _defaultMailServerPassword = MHB_DEFAULT_MAIL_SERVER_PASSWORD;

        public string DefaultMailServerPassword
        {
            get
            {
                return this._defaultMailServerPassword;
            }
            set
            {
                this._defaultMailServerPassword = value;
            }
        }

        #endregion Properties

        public Mail()
        {
        }

        public static bool SendMail(string message, string subject, string recipientsAddress, string senderAddress = MHB_DEFAULT_EMAIL)
        {
            bool result;

            try
            {
                MailMessage msg = new MailMessage(senderAddress, recipientsAddress);

                msg.Body = message;
                msg.IsBodyHtml = true;
                msg.BodyEncoding = Encoding.UTF8;
                msg.Subject = subject;

                SmtpClient smtp = new SmtpClient();

                smtp.Send(msg);

                result = true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Mail.SendMail:{ex.Message}", ex);
            }

            return result;
        }

        public static bool SendWelcomeMail(string recipientsAddress, string messagePath)
        {
            try
            {
                string message = File.ReadAllText(messagePath);

                SendMail(recipientsAddress, "[Support MyHomeBills] Welcome!", recipientsAddress);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Mail.SendWelcomeMail:{ex.Message}");
            }
        }

        public static IEnumerable<ImportMessage> GetImportEmail()
        {
            List<ImportMessage> results = new List<ImportMessage>();

            Message message = new Message(new byte[] { });

            Pop3Client pop3Client = new Pop3Client();

            try
            {
                if (_isRunning == false)
                {
                    _isRunning = true;

                    pop3Client.Connect(MHB_DEFAULT_MAIL_SERVER_ADDRESS, 110, false);
                    pop3Client.Authenticate(MHB_DEFAULT_EMAIL, MHB_DEFAULT_MAIL_SERVER_PASSWORD);

                    var totalMessagesCount = pop3Client.GetMessageCount();

                    if (totalMessagesCount > 0)
                    {
                        for (int messageNumber = 1; messageNumber <= totalMessagesCount; messageNumber++)
                        {
                            message = pop3Client.GetMessage(messageNumber);

                            ImportMessage importMessage = new ImportMessage();

                            if (message.Headers.Subject.Length == MHB_API_KEY_LENGTH && message.MessagePart.MessageParts.Any())
                            {
                                pop3Client.DeleteMessage(messageNumber);

                                List<ImportMessageItem> importMessageItems = new List<ImportMessageItem>();

                                string messageBody = message.MessagePart.MessageParts[0].GetBodyAsText();

                                MatchCollection matches = Regex.Matches(messageBody, @"(.*:)\s+?["",'](.*)["",']\s+?(\d+[.,]\d+|\d+)\s+(\d+[.,]\d+|\d+)?");

                                foreach (Match match in matches)
                                {
                                    Group[] groups = match.Groups.Cast<Group>().Where(g => !string.IsNullOrWhiteSpace(g.Value)).ToArray();

                                    if (groups.Length > 2)
                                    {
                                        string categoryName = groups[1].Value.Trim(':', '"', '\'');
                                        string productName = groups[2].Value.Trim('"', '\'');

                                        decimal value = 0, quantity = 0;

                                        if (groups.Length > 3)
                                        {
                                            decimal.TryParse(groups[3].Value.Replace(",", "."), out value);

                                            if (groups.Length > 4)
                                            {
                                                decimal.TryParse(groups[4].Value.Replace(",", "."), out quantity);
                                            }
                                        }

                                        importMessageItems.Add(new ImportMessageItem(categoryName, productName, value, quantity));
                                    }
                                }

                                importMessage.MessageItems = importMessageItems;
                                importMessage.SenderEmailAddress = message.Headers.From.Address;
                                importMessage.ApiKey = message.Headers.Subject.Trim();
                                importMessage.DateSent = message.Headers.DateSent;

                                results.Add(importMessage);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (message?.Headers?.From?.Address != null)
                    SendMail($"Failed to import data:\n{ex.Message}", "[Support MyHomeBills] Failed to import data!", message?.Headers?.From?.Address);
            }
            finally
            {
                _isRunning = false;
                pop3Client.Disconnect();
            }

            return results;
        }
    }
}