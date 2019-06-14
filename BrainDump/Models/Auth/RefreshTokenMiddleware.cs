using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace BrainDump.Models.Auth {
    public class RefreshTokenMiddleware {
        private readonly AuthorizationManager _authorizationManager;
        private readonly RequestDelegate _next;

        public RefreshTokenMiddleware(AuthorizationManager authorizationManager, RequestDelegate next) {
            _authorizationManager = authorizationManager;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            var newToken = await _authorizationManager.RefreshAccessToken(context);
            await context.Response.WriteAsync();

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
