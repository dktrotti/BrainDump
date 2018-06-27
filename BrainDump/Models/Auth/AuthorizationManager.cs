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
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BrainDump.Models.Auth {
    public class AuthorizationManager {
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

        public JwtSecurityToken CreateUser(string username, string password) {
            byte[] salt = new byte[128 / 8];
            _secureRandom.GetBytes(salt);

            byte[] hash = hashPassword(password, salt);

            _users.Add(new User(
                username,
                _random.NextPositiveLong(),
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash)));

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
                return createSecurityToken(user);
            } else {
                throw new InvalidCredentialsException();
            }
        }

        private byte[] hashPassword(string password, byte[] salt) {
            return KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
        }

        private JwtSecurityToken createSecurityToken(User user) {
            var key = new SymmetricSecurityKey(Convert.FromBase64String(_configuration["authSigningKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                claims: new List<Claim> {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(CustomClaims.BraindumpUserId, user.UserId.ToString())
                },
                signingCredentials: creds);
        }
    }

    public static class CustomClaims {
        public const String BraindumpUserId = "bdUserId";
    }

    public class InvalidCredentialsException : Exception {
        public InvalidCredentialsException() : base() { }
    }
}
