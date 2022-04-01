using System.Threading.Tasks;

namespace MyIS.HTTP
{
    public interface IHttpServer
    {
        Task StartAsync();
        Task ResetAsync();
        void Stop();
    }
}
