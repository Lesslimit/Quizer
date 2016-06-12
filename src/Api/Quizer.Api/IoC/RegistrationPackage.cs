﻿using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Quizer.DataAccess.AzureStorage;
using Quizer.DataAccess.DocumentDb;
using Quizer.Services.MessageSending.Email;
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
                typeof(IStorage).Assembly,
                typeof(IEmailSender).Assembly
            }.Distinct());

            container.Register(() =>
            {
                var storageOpts = container.GetService<IOptions<AzureStorageOptions>>().Value;

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