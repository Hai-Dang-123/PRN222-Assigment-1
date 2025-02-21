using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizBLL.Services.Implement;
using Microsoft.AspNetCore.Http;

namespace GroupBoizBLL.Utilities
{
    public class UserUtility
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserUtility(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        // lấy role từ token
        public string? GetRoleFromToken()
        {

            try
            {
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(token))
                    return ""; // Không có token => Trả về mặc định

                var claims = JWTProvide.DecodeToken(token);
                var tokenClaim = claims.FirstOrDefault(c => c.Type == "role");
                
                return tokenClaim?.Value; // Nếu không có FullName, trả về "User"
            }
            catch
            {
                return ""; // Nếu lỗi, trả về mặc định
            }

        }
        // lấy tên
        public string? GetFullNameFromToken()
        {
            try
            {
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(token))
                    return ""; // Không có token => Trả về mặc định

                var claims = JWTProvide.DecodeToken(token);
                var fullNameClaim = claims.FirstOrDefault(c => c.Type == "fullName");
                
                return fullNameClaim?.Value; // Nếu không có FullName, trả về "User"
            }
            catch
            {
                return ""; // Nếu lỗi, trả về mặc định
            }


        }
        public short GetUserIDFromToken()
        {
            try
            {
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];
                if (string.IsNullOrEmpty(token))
                    return -1; // Trả về -1 nếu không có token

                var claims = JWTProvide.DecodeToken(token);
                var userIdClaim = claims.FirstOrDefault(c => c.Type == "userId" || c.Type == "sub");

                if (userIdClaim != null && short.TryParse(userIdClaim.Value, out short userId))
                {
                    return userId;
                }

                return -1; // Trả về -1 nếu không lấy được UserId
            }
            catch
            {
                return -1; // Trả về -1 nếu có lỗi
            }
        }

    }
}
