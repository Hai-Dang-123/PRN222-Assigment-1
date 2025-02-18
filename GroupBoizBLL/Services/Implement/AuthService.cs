using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.Constants;
using GroupBoizCommon.DTO;
using GroupBoizDAL.Entities;
using GroupBoizDAL.UnitOfWork;

namespace GroupBoizBLL.Services.Implement
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService (IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseDTO> Login(LoginDTO loginDTO)
        {
            // Kiếm người dùng
            var user = await _unitOfWork.AccountRepo.FindByEmailAsync(loginDTO.AccountEmail);
            if (user == null)
            {
                return new ResponseDTO("User not found", 404, false);
            }

            // Kiểm tra nếu là admin
            if (loginDTO.AccountEmail == "admin@FUNewsManagementSystem.org" && loginDTO.AccountPassword == "@@abc123@@")
            {
                // Khởi tạo claim cho admin, đổi tên thành `adminClaims` để tránh trùng lặp với phạm vi bên ngoài
                var adminClaims = new List<Claim>();

                // Thêm email
                adminClaims.Add(new Claim(JWTConstants.KeyClaim.Email, user.AccountEmail));

                // Thêm id
                adminClaims.Add(new Claim(JWTConstants.KeyClaim.userId, user.AccountId.ToString()));

                // Thêm tên
                adminClaims.Add(new Claim(JWTConstants.KeyClaim.fullName, user.AccountName));

                // Tạo refresh token và access token cho admin
                var adminRefreshTokenKey = JWTProvide.GenerateRefreshToken(adminClaims);
                var adminAccessTokenKey = JWTProvide.GenerateAccessToken(adminClaims);

                // Trả về response cho admin
                return new ResponseDTO("Hello Admin", 200, true, new
                {
                    AccessToken = adminRefreshTokenKey,
                    RefreshToken = adminAccessTokenKey
                });
            }

            // Kiểm tra refreshToken hiện tại của người dùng
            var exitsRefreshToken = await _unitOfWork.TokenRepo.GetRefreshTokenByUserID(user.AccountId);
            if (exitsRefreshToken != null)
            {
                // Nếu có refreshToken thì thu hồi
                exitsRefreshToken.IsRevoked = true;
                await _unitOfWork.TokenRepo.UpdateAsync(exitsRefreshToken); // Cập nhật refreshToken
            }

            // Khởi tạo claims cho người dùng, đổi tên thành `userClaims`
            var userClaims = new List<Claim>();

            // Thêm email
            userClaims.Add(new Claim(JWTConstants.KeyClaim.Email, user.AccountEmail));

            // Thêm id
            userClaims.Add(new Claim(JWTConstants.KeyClaim.userId, user.AccountId.ToString()));

            // Thêm tên
            userClaims.Add(new Claim(JWTConstants.KeyClaim.fullName, user.AccountName));

            // Tạo refresh token và access token
            var refreshTokenKey = JWTProvide.GenerateRefreshToken(userClaims);
            var accessTokenKey = JWTProvide.GenerateAccessToken(userClaims);

            // Lấy giá trị Max RefreshTokenId hiện tại (kiểu short)
            var maxRefreshTokenID = await _unitOfWork.TokenRepo.GetMaxIdByShort("RefreshTokenId");

            // Gán RefreshTokenId = maxRefreshTokenID + 1
            var newRefreshTokenId = (short)(maxRefreshTokenID.HasValue ? Math.Min(maxRefreshTokenID.Value + 1, short.MaxValue) : (short)1);

            // Tạo mới đối tượng RefreshToken
            var refreshToken = new RefreshToken
            {
                RefreshTokenId = newRefreshTokenId,
                UserId = user.AccountId,
                RefreshTokenKey = refreshTokenKey,
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };

            // Thêm RefreshToken mới vào repo
            _unitOfWork.TokenRepo.Add(refreshToken);

            try
            {
                await _unitOfWork.SaveChangeAsync(); // Lưu thay đổi
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error saving refresh token: {ex.Message}", 500, false);
            }

            // Trả về response tùy theo role của người dùng
            if (user.AccountRole == 1)
            {
                return new ResponseDTO("Hello Staff", 200, true, new
                {
                    AccessToken = accessTokenKey,
                    RefreshToken = refreshToken
                });
            }
            else if (user.AccountRole == 2)
            {
                return new ResponseDTO("Hello Lecturer", 200, true, new
                {
                    AccessToken = accessTokenKey,
                    RefreshToken = refreshToken
                });
            }
            else
            {
                return new ResponseDTO("User role not recognized", 400, false);
            }
        }



        public Task<ResponseDTO> LogoutAsync(string refreshTokenKey)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> RefreshBothTokens(string oldAccessToken, string refreshTokenKey)
        {
            throw new NotImplementedException();
        }

        
    }
}
