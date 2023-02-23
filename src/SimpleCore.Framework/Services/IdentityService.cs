using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCore.Framework.Services
{
    public interface IIdentityService 
    {
        IEnumerable<Claim> GetClaims();
    }

    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public IdentityService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public IEnumerable<Claim> GetClaims()
        {
            return _contextAccessor?.HttpContext?.User?.Claims;
        }
    }
}
