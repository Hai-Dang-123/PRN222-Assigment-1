using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizDAL.Data;
using GroupBoizDAL.Entities;
using GroupBoizDAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace GroupBoizDAL.Repository.Implement
{
    public class TokenRepository : GenericRepository<RefreshToken>, ITokenRepository
    {
        private readonly FUNewsManagementContext _context;
        public TokenRepository(FUNewsManagementContext context) : base(context)
        {
            _context = context;
        }
        public async Task<RefreshToken> GetRefreshTokenByUserID(short userId)
        {
            // lấy token đúng id và chưa bị thu hồi
            return await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked)
                .FirstOrDefaultAsync();
        }
        public async Task<RefreshToken?> GetRefreshTokenByKey(string refreshTokenKey)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenKey))
            {
                throw new ArgumentException("Refresh token cannot be null or empty.", nameof(refreshTokenKey));
            }

            // Thực hiện truy vấn để tìm RefreshToken theo RefreshTokenKey
            var refreshTokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.RefreshTokenKey == refreshTokenKey);

            return refreshTokenEntity;
        }
    }
}
