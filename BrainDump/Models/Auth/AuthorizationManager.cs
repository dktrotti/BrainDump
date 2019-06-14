using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BrainDump.data;
using BrainDump.util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace BrainDump.Models.Auth {
    public class AuthorizationManager {
        private static readonly TimeSpan ExpirationTime = TimeSpan.FromMinutes(30);

        private readonly IUserRepository _users;
        private readonly IConfiguration _configuration;
        private readonly RNGCryptoServiceProvider _secureRandom;
        private readonly Random _random;

        public AuthorizationManager(
            IConfiguration configuration,
            MongoDataAccess dataAccess,
            RNGCryptoServiceProvider secureRandom,
            Random random) {
            _users = dataAccess.GetUserRepository();
            _configuration = configuration;
            _secureRandom = secureRandom;
            _random = random;
        }

        public static void ConfigureAccessBearer(
            JwtBearerOptions options,
            IConfiguration configuration) {
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidAudience = configuration["domain"],
                // Tokens are being created locally, so there should be zero skew
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                SaveSigninToken = true,
                ValidateActor = false,
                ValidateAudience = true,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(configuration["authSigningKey"]))
            };
        }

        public JwtSecurityToken CreateUser(string username, string password) {
            byte[] salt = new byte[128 / 8];
            _secureRandom.GetBytes(salt);

            byte[] hash = hashPassword(password, salt);

            try {
                _users.Add(new User(
                    username,
                    _random.NextPositiveLong(),
                    Convert.ToBase64String(salt),
                    Convert.ToBase64String(hash)));
            } catch (MongoWriteException) {
                throw new DuplicateUserException();
            }

            return Login(username, password);
        }

        public JwtSecurityToken Login(string username, string password) {
            var user = _users.Find(username);
            if (user == null) {
                throw new InvalidCredentialsException();
            }

            var userHash = Convert.FromBase64String(user.Hash);
            var salt = Convert.FromBase64String(user.Salt);
            if (userHash.slowEquals(hashPassword(password, salt))) {
                return CreateAccessToken(user);
            } else {
                throw new InvalidCredentialsException();
            }
        }

        public async Task<JwtSecurityToken> RefreshAccessToken(HttpContext context) {
            var token = new JwtSecurityToken(await context.GetTokenAsync("access_token"));

            if (token.ValidTo < DateTime.UtcNow) {
                throw new SecurityTokenExpiredException();
            }

            return new JwtSecurityToken(
                claims: token.Claims,
                expires: DateTime.UtcNow.Add(ExpirationTime),
                signingCredentials: GetSigningCredentials());
        }

        private JwtSecurityToken CreateAccessToken(User user) {
            var claims = new List<Claim> {
                new Claim(CustomClaimTypes.Username, user.UserName),
                new Claim(CustomClaimTypes.UserId, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, _random.NextPositiveLong().ToString())
            };

            return new JwtSecurityToken(
                claims: claims,
                audience: _configuration["domain"],
                expires: DateTime.UtcNow.Add(ExpirationTime),
                signingCredentials: GetSigningCredentials());
        }

        private SigningCredentials GetSigningCredentials() {
            var key = new SymmetricSecurityKey(Convert.FromBase64String(_configuration["authSigningKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            return creds;
        }

        private byte[] hashPassword(string password, byte[] salt) {
            return KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                10000,
                256 / 8);
        }
    }

    public static class CustomClaimTypes {
        public const string Username = "BrainDumpUsername";
        public const string UserId = "BrainDumpUserId";
    }

    public class InvalidCredentialsException : Exception { }
    public class DuplicateUserException : Exception { }
}
