using System;
using System.Runtime.InteropServices;
using Outlook = Microsoft.Office.Interop.Outlook;

public class OutlookEmailSender
{
    public void SendEmail(string recipient, string subject, string body)
    {
        try
        {
            Outlook.Application outlookApp = new Outlook.Application();
            Outlook.MailItem mailItem = (Outlook.MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);

            // Set the recipient, subject, and body
            mailItem.To = recipient;
            mailItem.Subject = subject;
            mailItem.Body = body;

            // Send the email
            mailItem.Send();

            // Release COM objects
            Marshal.ReleaseComObject(mailItem);
            Marshal.ReleaseComObject(outlookApp);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}

