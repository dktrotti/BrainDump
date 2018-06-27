using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainDump.Models.Auth;
using Microsoft.AspNetCore.Http;

namespace BrainDump.util {
    static class HttpContextExtensions {
        public static long? GetUserId(this HttpContext context) {
            var userIdStr = context.User.Claims
                .FirstOrDefault(c => c.Type.Equals(CustomClaims.BraindumpUserId))
                ?.Value;
            return long.TryParse(userIdStr, out var tempVal) ? tempVal : (long?)null;
        }
    }
}
