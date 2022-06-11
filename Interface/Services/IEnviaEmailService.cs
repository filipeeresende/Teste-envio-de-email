using System.Threading.Tasks;

namespace EnviaEmailSmtp.Interface.Services
{
    public interface IEnviaEmailService
    {
        Task EnviarEmail();
    }

}
