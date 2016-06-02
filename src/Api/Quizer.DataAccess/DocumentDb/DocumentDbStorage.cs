using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;
using Quizer.Domain.Attributes;

namespace Quizer.DataAccess.DocumentDb
{
    public sealed class DocumentDbStorage : IStorage
    {
        #region Fields

        private readonly IOptions<DocumentDbOptions> dbOptions;

        private readonly DocumentClient documentClient;

        private readonly ConcurrentDictionary<string, Task<Database>> databases =
            new ConcurrentDictionary<string, Task<Database>>();

        #endregion Fields

        public DocumentDbStorage(DocumentClient documentClient,
                                 IOptions<DocumentDbOptions> dbOptions)
        {
            this.dbOptions = dbOptions;
            this.documentClient = documentClient;
        }

        public async Task<IStorageCollection<T>> CollectionAsync<T>(string dbId)
        {
            dbId = dbId ?? dbOptions.Value.DefaultDbId;

            Task<Database> dbQuery;
            if (!databases.TryGetValue(dbId, out dbQuery))
            {
                if (string.IsNullOrEmpty(dbId))
                {
                    throw new ArgumentNullException(nameof(dbId));
                }

                Db(dbId);

                if (!databases.TryGetValue(dbId, out dbQuery))
                {
                    throw new InvalidOperationException("Could not get database: " + dbId);
                }
            }

            var db = await dbQuery;
            var collection = documentClient.CreateDocumentCollectionQuery(db.SelfLink)
                .AsEnumerable()
                .First(dc => dc.Id == GetCollectionId(typeof(T)));

            return new DocumentDbStorageCollection<T>(documentClient, collection);
        }

        public IStorage Db(string id)
        {
            id = id ?? dbOptions.Value.DefaultDbId;
            databases.AddOrUpdate(id, GetOrCreateDatabaseAsync, (key, database) => database);

            return this;
        }

        #region Private Stuff

        private async Task<Database> GetOrCreateDatabaseAsync(string id)
        {
            return documentClient.CreateDatabaseQuery().Where(db => db.Id == id).ToArray().FirstOrDefault() ??
                   await documentClient.CreateDatabaseAsync(new Database {Id = id});
        }

        private static string GetCollectionId(MemberInfo resourceType)
        {
            var dbCollectionAttribute = resourceType.GetCustomAttribute<DbCollectionAttribute>();

            return dbCollectionAttribute != null
                ? dbCollectionAttribute.Id
                : resourceType.Name.Camelize().Pluralize();
        }

        #endregion Private Stuff

        #region IDisposable

        public void Dispose()
        {
            try
            {
                documentClient.Dispose();
            }
            catch
            {
                //ignore
            }
        }

        #endregion
    }
}