using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainDump.Models.Auth;

namespace BrainDump.data {
    public interface ITokenRepository {
        void Add(AccessToken token);
        AccessToken Find(long tokenId);
        void Remove(long tokenId);
    }
}
