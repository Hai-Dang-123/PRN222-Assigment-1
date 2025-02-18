using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizDAL.Entities;

namespace GroupBoizDAL.Repository.Interface
{
    public interface ITokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken> GetRefreshTokenByUserID(short userId);
        Task<RefreshToken?> GetRefreshTokenByKey(string refreshTokenKey);
    }
}
