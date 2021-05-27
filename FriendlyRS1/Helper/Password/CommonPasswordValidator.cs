using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FriendlyRS1
{
    public class CommonPasswordValidator<TUser> : IPasswordValidator<TUser>
         where TUser : class
    {
        static HashSet<string> Passwords { get; } = PasswordLists.LoadPasswordList("FriendlyRS1.Files.common_passwords.txt");

        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager,
                                                    TUser user,
                                                    string password)
        {
            if (Passwords.Contains(password))
            {
                var result = IdentityResult.Failed(new IdentityError
                {
                    Code = "CommonPassword",
                    Description = "The password you chose is too common."
                });
                return Task.FromResult(result);
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }

    internal static class PasswordLists
    {
        public static HashSet<string> LoadPasswordList(string resourceName)
        {
            HashSet<string> hashset;

            var assembly = Assembly.GetEntryAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    hashset = new HashSet<string>(
                        GetLines(streamReader),
                        StringComparer.OrdinalIgnoreCase);
                }
            }
            return hashset;
        }

        private static IEnumerable<string> GetLines(StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                yield return reader.ReadLine();
            }
        }
    }
}
