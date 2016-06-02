using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace Quizer.DataAccess.DocumentDb
{
    public class DocumentDbStorageCollection<T> : IStorageCollection<T>
    {
        private readonly DocumentClient documentClient;
        private readonly DocumentCollection collection;

        public DocumentDbStorageCollection(DocumentClient documentClient, DocumentCollection collection)
        {
            this.documentClient = documentClient;
            this.collection = collection;
        }

        public IQueryable<T> Query()
        {
            return documentClient.CreateDocumentQuery<T>(collection.DocumentsLink);
        }
    }
}