using Energy.Core;
using Novell.Directory.Ldap;
using System;

namespace Energy.Net.Features.Authentication
{
    public class Ldap
    {
        private ApplicationOptions _options;
        private Administrator _admin;

        public Ldap(ApplicationOptions options,
            Administrator administrator)
        {
            _options = options;
            _admin = administrator;
        }

        public bool Validate(string employeeDn, string password)
        {
            try
            {
                if (employeeDn == _options.AdministratorDistinguishedName)
                {
                    return BCrypt.Net.BCrypt.Verify(password, _admin.HashedPassword);
                }

                if (string.IsNullOrWhiteSpace(employeeDn))
                {
                    return false;
                }
                else
                {
                    LdapConnection conn = new LdapConnection();
                    conn.Connect(_options.Server, _options.Port);
                    conn.Bind(employeeDn, password);
                    conn.Disconnect();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
