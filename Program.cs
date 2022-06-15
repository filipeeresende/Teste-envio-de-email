using System;
using System.IO;
using System.Threading.Tasks;
using EnviaEmailSmtp.Interface.Services;
using EnviaEmailSmtp.Services;
using Microsoft.Extensions.Configuration;

namespace EnviaEmailSmtp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Inicio da aplicação de teste envio de email.");

            var enviandoEmail = new EnviaEmailService();
            await enviandoEmail.EnviarEmail();

            Console.WriteLine("Fim da aplicação de teste envio de email.");
        }
    }
}



