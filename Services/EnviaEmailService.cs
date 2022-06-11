using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using EnviaEmailSmtp.Interface;
using EnviaEmailSmtp.Interface.Services;
using Microsoft.Extensions.Configuration;

namespace EnviaEmailSmtp.Services
{
    public class EnviaEmailService : IEnviaEmailService
    {
        public async Task EnviarEmail()
        {
            var verificaConfiguração = await ObterConfiguracoesEmail();
            if (verificaConfiguração == true)
                Console.WriteLine("Email enviado com sucesso.");
            else
                Console.WriteLine("Email não foi enviado");
        }
        private string MensagemDoEmail()
        {
            string body = "";

            body += $@"

                    <div style='background-color: #fff; width: 90%; font-family: sans-serif;'>
                        <div style='text-align: center; padding-top: 10px;'>
                            <img src='https://dkrn4sk0rn31v.cloudfront.net/uploads/2020/06/Logo-Visual-Studio.png' style='width: 120px; height: auto;'>
                        </div>
                        <div style='width: 601px; margin: auto; margin-top: 20px; background-color: #fff; text-align: center;'>
                            <div style='padding: 30px;text-align: justify'>                              
                                Olá 
                                <p>Isso é apenas um teste para o protocolo smtp .</p>
                            </div>
                            <div>
                            </div>
                            <div style='background-color: #005DB9;'>
                                <p style='font-size: 14px; color: #fff; padding: 10px 0;'>Por favor não retorne esse email!</p>
                            </div>                        
                        </div>
                    </div>                   
                    <br>";
            return body;

        }
        private async Task<bool> ObterConfiguracoesEmail()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                                               .SetBasePath(Directory.GetCurrentDirectory())
                                               .AddJsonFile("appSettings.json")
                                               .Build();

            try
            {
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(configuration["FromEmail"])
                };

                mail.To.Add(new MailAddress(configuration["FromEmail"]));
                mail.Bcc.Add(new MailAddress(configuration["ToEmail"]));

                mail.Subject = $"Teste envio de email por smtp";
                mail.Body = MensagemDoEmail();
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient(configuration["PrimaryDomain"], Int32.Parse(configuration["PrimaryPort"])))
                {
                    smtp.Credentials = new NetworkCredential(configuration["UsernameEmail"], configuration["UsernamePassword"]);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
    }
}

