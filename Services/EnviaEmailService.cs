using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using EnviaEmailSmtp.DTOs;
using EnviaEmailSmtp.Interface.Services;
using Microsoft.Extensions.Configuration;

namespace EnviaEmailSmtp.Services
{
    public class EnviaEmailService : IEnviaEmailService
    {
        public async Task EnviarEmail()
        {
            try
            {
                ConfiguracoesEmailResponse configuracao = await ObterConfiguracoesEmail();

                using (var smtp = new SmtpClient(configuracao.Configuration["PrimaryDomain"],
                    Int32.Parse(configuracao.Configuration["PrimaryPort"])))
                {
                    smtp.Credentials = new NetworkCredential(configuracao.Configuration["UsernameEmail"],
                        configuracao.Configuration["UsernamePassword"]);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(configuracao.Mail);
                }

                Console.WriteLine("Email enviado com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email não foi enviado");
                Console.WriteLine(ex.Message);
            }
        }

        private async Task<ConfiguracoesEmailResponse> ObterConfiguracoesEmail()
        {
            string workingDirectory = Environment.CurrentDirectory;

            IConfiguration configuration = new ConfigurationBuilder()
                                               .SetBasePath(Directory.GetParent(workingDirectory).Parent.Parent.FullName)
                                               .AddJsonFile("appSettings.json")
                                               .Build();

            var mail = new MailMessage()
            {
                From = new MailAddress(configuration["FromEmail"])
            };

            mail.To.Add(new MailAddress(configuration["FromEmail"]));
            mail.Bcc.Add(new MailAddress(configuration["ToEmail"]));

            mail.Subject = $"Teste envio de email por smtp";
            mail.Body = MontarMensagemDoEmail();
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            return new ConfiguracoesEmailResponse
            {
                Configuration = configuration,
                Mail = mail
            };
        }

        private static string MontarMensagemDoEmail()
        {
            var body = new StringBuilder();

            body.Append("<div style='background-color: #fff; width: 90%; font-family: sans-serif;'>");
            body.Append("<div style='text-align: center; padding-top: 10px;'>");
            body.Append("<img src='https://dkrn4sk0rn31v.cloudfront.net/uploads/2020/06/Logo-Visual-Studio.png' style='width: 120px; height: auto;'>");
            body.Append("</div>");
            body.Append("<div style='width: 601px; margin: auto; margin-top: 20px; background-color: #fff; text-align: center;'>");
            body.Append("<div style='padding: 30px;text-align: justify'>");
            body.Append("Olá");
            body.Append("<p>Isso é apenas um teste para o protocolo smtp .</p>");
            body.Append("</div>");
            body.Append("<div>");
            body.Append("</div>");
            body.Append("<div style='background-color: #005DB9;'>");
            body.Append("<p style='font-size: 14px; color: #fff; padding: 10px 0;'>Por favor não retorne esse email!</p>");
            body.Append("</div>");
            body.Append("</div>");
            body.Append("</div>");
            body.Append("<br>");

            return body.ToString();
        }
    }
}

