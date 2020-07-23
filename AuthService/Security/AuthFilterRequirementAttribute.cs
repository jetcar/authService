using AuthService.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Security
{
    public class AuthFilterRequirementAttribute : TypeFilterAttribute
    {
        public AuthFilterRequirementAttribute() : base(typeof(AuthFilter))
        {
            
        }
    }
}