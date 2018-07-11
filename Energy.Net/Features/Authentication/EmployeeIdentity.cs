using System.Security.Principal;

namespace Energy.Net.Features.Authentication
{
    public class EmployeeIdentity : IIdentity
    {
        public string AuthenticationType { get; } 

        public bool IsAuthenticated { get; } 

        public string Name { get; }

        public EmployeeIdentity(string name)
        {
            AuthenticationType = "ad";
            IsAuthenticated = true;
            Name = name;
        }
    }
}
