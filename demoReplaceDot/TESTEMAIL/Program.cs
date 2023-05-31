// See https://aka.ms/new-console-template for more information
using MailKit;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

var userName = "THINHMAIL";
var senderEmail = "no-reply-cls40@cls.vn";
var passWord = "R0iZZFZS5IZv&";
var host = "14.225.5.136";
var port = 25;
var receipient = "thinhdangvan1997@gmail.com";

var message = new MimeMessage();
message.From.Add(new MailboxAddress(userName, senderEmail));
message.To.Add(new MailboxAddress("Recipient Name", receipient));
message.Subject = "TESTMAIL";
var bodyBuilder = new BodyBuilder
{
    HtmlBody = "TESTMAIL"
};
var htmlBody = bodyBuilder.ToMessageBody();
message.Body = new TextPart(TextFormat.Html) { Text = "TESTMAIL" };


try
{
    using (var client = new MailKit.Net.Smtp.SmtpClient())
    {
        //var mails = client.ReceiveEmail(host, senderEmail, passWord);

        client.CheckCertificateRevocation = false;
        client.Connect(host, port, false ? SecureSocketOptions.Auto : SecureSocketOptions.None);
        client.Authenticate(senderEmail, passWord);
        //client.DeliveryStatusNotificationType = DeliveryStatusNotificationType.Full;

        var abc = client.Verify(receipient);
        //var addr = new System.Net.Mail.MailAddress(email);


        client.Send(message);
        client.MessageSent += (s, e) =>
        {
        };
        client.Disconnect(true);
    }
    Thread.Sleep(1000);
}
catch (Exception e)
{
    
}


public class MySmtpClient : MailKit.Net.Smtp.SmtpClient
{
    readonly List<SmtpCommandException> exceptions = new List<SmtpCommandException>();

    protected override void OnSenderAccepted(MimeMessage message, MailboxAddress mailbox, SmtpResponse response)
    {
        exceptions.Clear();
    }

    protected override void OnRecipientNotAccepted(MimeMessage message, MailboxAddress mailbox, SmtpResponse response)
    {
        try
        {
            base.OnRecipientNotAccepted(message, mailbox, response);
        }
        catch (SmtpCommandException ex)
        {
            exceptions.Add(ex);
        }
    }

    protected override void OnNoRecipientsAccepted(MimeMessage message)
    {
        if (exceptions.Count == 1)
            throw exceptions[0];

        throw new AggregateException(exceptions.ToArray());
    }

    protected override DeliveryStatusNotification? GetDeliveryStatusNotifications(MimeMessage message, MailboxAddress mailbox)
    {
        if (!(message.Body is MultipartReport report) || report.ReportType == null || !report.ReportType.Equals("delivery-status", StringComparison.OrdinalIgnoreCase))
            return default;
        report.OfType<MessageDeliveryStatus>().ToList().ForEach(x => {
            x.StatusGroups.Where(y => y.Contains("Action") && y.Contains("Final-Recipient")).ToList().ForEach(z => {
                switch (z["Action"])
                {
                    case "failed":
                        Console.WriteLine("Delivery of message {0} failed for {1}", z["Action"], z["Final-Recipient"]);
                        break;
                    case "delayed":
                        Console.WriteLine("Delivery of message {0} has been delayed for {1}", z["Action"], z["Final-Recipient"]);
                        break;
                    case "delivered":
                        Console.WriteLine("Delivery of message {0} has been delivered to {1}", z["Action"], z["Final-Recipient"]);
                        break;
                    case "relayed":
                        Console.WriteLine("Delivery of message {0} has been relayed for {1}", z["Action"], z["Final-Recipient"]);
                        break;
                    case "expanded":
                        Console.WriteLine("Delivery of message {0} has been delivered to {1} and relayed to the the expanded recipients", z["Action"], z["Final-Recipient"]);
                        break;
                }
            });
        });
        return default;
    }

    public List<EmailMessage> ReceiveEmail(string host, string username, string pass, int maxCount = 1000)
    {
        using (var emailClient = new Pop3Client())
        {
            emailClient.Connect(host, 110, false ? SecureSocketOptions.Auto : SecureSocketOptions.None);

            //emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

            emailClient.Authenticate(username, pass);

            List<EmailMessage> emails = new List<EmailMessage>();
            var count = emailClient.GetMessageCount();
            for (int i = 0; i < emailClient.Count && i < maxCount; i++)
            {
                var message = emailClient.GetMessage(i);
                var emailMessage = new EmailMessage
                {
                    Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                    Subject = message.Subject
                };
                emailMessage.ToAddresses = new List<EmailAddress>();
                emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                emailMessage.FromAddresses = new List<EmailAddress>();
                emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                emails.Add(emailMessage);
            }

            return emails;
        }
    }
}

public class EmailMessage
{
    public string Content { get; set; }
    public string Subject { get; set; }
    public List<EmailAddress> ToAddresses { get; set; }
    public List<EmailAddress> FromAddresses { get; set; }

}

public class EmailAddress
{
    public string Address { get; set; }
    public string Name { get; set; }
}