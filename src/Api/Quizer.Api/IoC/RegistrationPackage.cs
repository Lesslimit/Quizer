using Quizer.DataAccess.DocumentDb;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Quizer.Api.IoC
{
    public class RegistrationPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.RegisterPackages(new[]
            {
                typeof(IStorage).Assembly
            });
        }
    }
}