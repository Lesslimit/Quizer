﻿using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Quizer.DataAccess.AzureStorage;
using Quizer.DataAccess.DocumentDb;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace Quizer.Websiite.IoC
{
    public class RegistrationPackage : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.RegisterPackages(new[]
            {
                typeof(IStorage).Assembly
            });

            container.Register(() =>
            {
                var storageOpts = container.GetInstance<IOptions<AzureStorageOptions>>().Value;

                return CloudStorageAccount.Parse(storageOpts.ConnectionString);
            }, Lifestyle.Singleton);

            container.Register(() =>
            {
                var storageAccount = container.GetInstance<CloudStorageAccount>();

                return storageAccount.CreateCloudBlobClient();
            }, Lifestyle.Singleton);
        }
    }
}