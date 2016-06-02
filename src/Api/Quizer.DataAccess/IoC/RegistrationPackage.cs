using System;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using Quizer.DataAccess.DocumentDb;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Quizer.DataAccess.IoC
{
    public class RegistrationPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.RegisterSingleton<IStorage, DocumentDbStorage>();

            container.RegisterSingleton(() =>
            {
                var options = container.GetInstance<IOptions<DocumentDbOptions>>();
                var serviceEndpoint = new Uri(options.Value.ServiceEndpoint);

                return new DocumentClient(serviceEndpoint, options.Value.AuthKey);
            });
        }
    }
}