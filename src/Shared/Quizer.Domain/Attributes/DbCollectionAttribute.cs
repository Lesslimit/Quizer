using System;

namespace Quizer.Domain.Attributes
{
    public class DbCollectionAttribute : Attribute
    {
        public string Id { get; private set; }

        public DbCollectionAttribute(string id)
        {
            this.Id = id;
        }
    }
}