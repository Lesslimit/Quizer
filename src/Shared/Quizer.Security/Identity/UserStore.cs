using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Quizer.DataAccess.DocumentDb;
using Quizer.Security.Options;

namespace Quizer.Security.Identity
{
    public class UserStore : IUserLoginStore<QuizerUser>, IUserRoleStore<QuizerUser>, IUserClaimStore<QuizerUser>, IUserPasswordStore<QuizerUser>, IUserEmailStore<QuizerUser>
    {
        private readonly IStorage storage;
        private readonly ILookupNormalizer idNormalizer;
        private readonly IOptions<UserIdentityOptions> identityOptions;

        public UserStore(IStorage storage,
                         ILookupNormalizer idNormalizer,
                         IOptions<UserIdentityOptions> identityOptions)
        {
            this.storage = storage;
            this.idNormalizer = idNormalizer;
            this.identityOptions = identityOptions;
        }

        #region Implementation Of IUserLoginStore<QuizerUser>

        public Task<string> GetUserIdAsync(QuizerUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(idNormalizer.Normalize(user.Id));
        }

        public async Task<string> GetUserNameAsync(QuizerUser user, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(user.UserName))
            {
                return user.UserName;
            }

            if (string.IsNullOrEmpty(user.Id))
            {
                return null;
            }

            var identities = await CollectionAsync().ConfigureAwait(false);
            var id = idNormalizer.Normalize(user.Id);

            return identities.Query()
                             .Where(qu => qu.Id == id)
                             .Select(qu => qu.UserName)
                             .FirstOrDefault();
        }

        public Task SetUserNameAsync(QuizerUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;

            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(QuizerUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(QuizerUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.UserName = normalizedName;

            return Task.CompletedTask;
        }

        public async Task<IdentityResult>  CreateAsync(QuizerUser user, CancellationToken cancellationToken)
        {
            try
            {
                var identities = await CollectionAsync().ConfigureAwait(false);

                await identities.AddAsync(user, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError {Description = ex.ToString()});
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(QuizerUser user, CancellationToken cancellationToken)
        {
            try
            {
                var identities = await CollectionAsync().ConfigureAwait(false);

                await identities.UpdateAsync(user).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError {Description = ex.ToString()});
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(QuizerUser user, CancellationToken cancellationToken)
        {
            try
            {
                var identities = await CollectionAsync().ConfigureAwait(false);

                await identities.DeleteAsync(user).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.ToString() });
            }

            return IdentityResult.Success;
        }

        public async Task<QuizerUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var identities = await CollectionAsync().ConfigureAwait(false);
            var id = idNormalizer.Normalize(userId);

            return identities.Query()
                             .FirstOrDefault(iu => iu.Id == id);
        }

        public async Task<QuizerUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var identities = await CollectionAsync().ConfigureAwait(false);

            return identities.Query()
                             .FirstOrDefault(qu => qu.UserName == normalizedUserName);
        }

        public Task AddLoginAsync(QuizerUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            return default(Task);
        }

        public Task RemoveLoginAsync(QuizerUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return default(Task);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(QuizerUser user, CancellationToken cancellationToken)
        {
            return default(Task<IList<UserLoginInfo>>);
        }

        public Task<QuizerUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return default(Task<QuizerUser>);
        }

        #endregion

        #region Implementation Of IUserRoleStore<QuizerUser>

        public Task AddToRoleAsync(QuizerUser user, string roleName, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            user.Roles.Add(roleName);

            return Task.CompletedTask;
        }

        public Task RemoveFromRoleAsync(QuizerUser user, string roleName, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            user.Roles.Remove(roleName);

            return Task.CompletedTask;
        }

        public async Task<IList<string>> GetRolesAsync(QuizerUser user, CancellationToken cancellationToken)
        {
            var identities = await CollectionAsync().ConfigureAwait(false);
            var id = idNormalizer.Normalize(user.Id);

            var roles = identities.Query()
                                  .Where(qu => qu.Id == id)
                                  .SelectMany(qu => qu.Roles)
                                  .ToList();

            return roles;
        }

        public async Task<bool> IsInRoleAsync(QuizerUser user, string roleName, CancellationToken cancellationToken)
        {
            var identities = await CollectionAsync().ConfigureAwait(false);

            var result = identities.Query()
                                   .FirstOrDefault(qu => qu.Roles.Contains(roleName));

            return result != null;
        }

        public async Task<IList<QuizerUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var identities = await CollectionAsync().ConfigureAwait(false);

            return identities.Query()
                             .Where(qu => qu.Roles.Contains(roleName))
                             .ToList();
        }

        #endregion

        #region Implementation Of IUserClaimStore<QuizerUser>

        public async Task<IList<Claim>> GetClaimsAsync(QuizerUser user, CancellationToken cancellationToken)
        {
            var identities = await CollectionAsync().ConfigureAwait(false);
            var id = idNormalizer.Normalize(user.Id);

            return identities.Query()
                             .Where(qu => qu.Id == id)
                             .SelectMany(qu => qu.Claims)
                             .AsEnumerable()
                             .Select(uc => uc.ToClaim())
                             .ToList();
        }

        public Task AddClaimsAsync(QuizerUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task RemoveClaimsAsync(QuizerUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task<IList<QuizerUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task ReplaceClaimAsync(QuizerUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            return null;
        }
 
        #endregion

        #region Implementation Of IUserPasswordStore<QuizerUser>

        public Task SetPasswordHashAsync(QuizerUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;

            return Task.CompletedTask;
        }

        public async Task<string> GetPasswordHashAsync(QuizerUser user, CancellationToken cancellationToken)
        {
            var identities = await CollectionAsync().ConfigureAwait(false);
            var id = idNormalizer.Normalize(user.Id);

            return identities.Query()
                             .Where(qu => qu.Id == id)
                             .Select(qu => qu.PasswordHash)
                             .FirstOrDefault();
        }

        public async Task<bool> HasPasswordAsync(QuizerUser user, CancellationToken cancellationToken)
        {
            var identities = await CollectionAsync().ConfigureAwait(false);
            var id = idNormalizer.Normalize(user.Id);

            return identities.Query()
                             .Where(iu => iu.Id == id)
                             .Select(qu => string.IsNullOrEmpty(qu.PasswordHash))
                             .FirstOrDefault();
        }

        #endregion

        #region Implementation Of IUserEmailStore<QuizerUser>

        public Task SetEmailAsync(QuizerUser user, string email, CancellationToken cancellationToken)
        {
            user.Id = email;

            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(QuizerUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(idNormalizer.Normalize(user.Id));
        }

        public async Task<bool> GetEmailConfirmedAsync(QuizerUser user, CancellationToken cancellationToken)
        {
            var identities = await CollectionAsync().ConfigureAwait(false);
            var id = idNormalizer.Normalize(user.Id);

            return identities.Query()
                             .Where(qu => qu.Id == id)
                             .Select(qu => qu.EmailConfirmed)
                             .FirstOrDefault();
        }

        public Task SetEmailConfirmedAsync(QuizerUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;

            return Task.CompletedTask;
        }

        public async Task<QuizerUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await FindByIdAsync(normalizedEmail, cancellationToken).ConfigureAwait(false);
        }

        public Task<string> GetNormalizedEmailAsync(QuizerUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task SetNormalizedEmailAsync(QuizerUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.Id = normalizedEmail;

            return Task.CompletedTask;
        }

        #endregion

        #region Implementation Of IDisposable

        public void Dispose()
        {
            storage.Dispose();
        }

        #endregion

        #region Private Stuff

        private async Task<IStorageCollection<QuizerUser>> CollectionAsync()
        {
            return await storage.Db(identityOptions.Value.DbName)
                                .CollectionAsync<QuizerUser>(identityOptions.Value.CollectionName)
                                .ConfigureAwait(false);
        }

        #endregion
    }
}