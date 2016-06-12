using System.Threading.Tasks;

namespace Quizer.Services.MessageSending.Email
{
    public interface IEmailSender
    {
        IEmailDescriptor To(params string[] recipients);
        Task SendAsync(IEmailDescriptor emailDescriptor);
    }
}