using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainDump.Models.Auth;
using MongoDB.Driver;

namespace BrainDump.data {
    public class TokenRepository : ITokenRepository {
        private readonly IMongoCollection<AccessToken> _tokens;

        public TokenRepository(IMongoCollection<AccessToken> tokens) {
            _tokens = tokens;
        }

        public void Add(AccessToken token) {
            _tokens.InsertOne(token);
        }

        public AccessToken Find(long tokenId) {
            return _tokens.Find(r => r.TokenId == tokenId).FirstOrDefault();
        }

        public void Remove(long tokenId) {
            _tokens.DeleteOne(r => r.TokenId == tokenId);
        }
    }
}
