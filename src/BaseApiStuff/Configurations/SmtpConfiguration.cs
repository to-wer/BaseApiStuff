namespace BaseApiStuff.Configurations;

public class SmtpConfiguration
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpUsername { get; set; }
    public string SmtpPassword { get; set; }
    public bool SmtpEnableSsl { get; set; }
    public string SmtpFromAddress { get; set; }
}