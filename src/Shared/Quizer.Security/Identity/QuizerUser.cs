using Newtonsoft.Json;
using Quizer.Domain.Attributes;
using Quizer.Domain.Contracts;

// ReSharper disable VirtualMemberCallInConstructor

namespace Quizer.Security.Identity
{
    [DbCollection("identities")]
    [JsonObject(MemberSerialization.OptIn)]
    public class QuizerUser : IdentityUserBase<string, IdentityUserClaim<string>, string, IdentityUserLogin<string>>, IDocument
    {
        public override string Email => Id;

        public QuizerUser(string email) : base(email)
        {
            Id = email;
            UserName = email;
        }
    }
}