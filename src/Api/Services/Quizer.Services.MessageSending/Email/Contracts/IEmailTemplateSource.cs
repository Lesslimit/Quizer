using System.Threading;
using System.Threading.Tasks;

namespace Quizer.Services.MessageSending.Email
{
    public interface IEmailTemplateSource
    {
        Task<IEmailTemplate> RetriveAsync(string key, CancellationToken cancelToken = default(CancellationToken));
    }
}