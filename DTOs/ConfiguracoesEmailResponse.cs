using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace EnviaEmailSmtp.DTOs
{
    public class ConfiguracoesEmailResponse
    {
        public MailMessage Mail { get; set; }
        public IConfiguration Configuration { get; set; }
    }
}
