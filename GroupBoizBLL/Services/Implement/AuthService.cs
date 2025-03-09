using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.Constants;
using GroupBoizCommon.DTO;
using GroupBoizDAL.Entities;
using GroupBoizDAL.UnitOfWork;
using Microsoft.AspNetCore.Http;

namespace GroupBoizBLL.Services.Implement
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService (IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseDTO> Login(LoginDTO loginDTO)
        {
            try
            {
                // Kiểm tra nếu là admin
                if (loginDTO.AccountEmail == "admin@FUNewsManagementSystem.org" && loginDTO.AccountPassword == "@@abc123@@")
                {
                    var adminClaims = new List<Claim>
            {
                new Claim(JWTConstants.KeyClaim.Email, "admin@FUNewsManagementSystem.org"),
                new Claim(JWTConstants.KeyClaim.userId, "-1"),
                new Claim(JWTConstants.KeyClaim.fullName, "Admin"),
                new Claim(ClaimTypes.Role, "Admin")
            };

                    // Tạo token cho Admin
                    var adminAccessToken = JWTProvide.GenerateAccessToken(adminClaims);
                    var adminRefreshToken = JWTProvide.GenerateRefreshToken(adminClaims);

                    return new ResponseDTO("Hello Admin", 200, true, new TokenDTO
                    {
                        AccessToken = adminAccessToken,
                        RefreshToken = adminRefreshToken
                    });
                }

                // Kiếm người dùng trong DB
                var user = await _unitOfWork.AccountRepo.FindByEmailAsync(loginDTO.AccountEmail);
                if (user == null)
                {
                    return new ResponseDTO("User not found", 404, false);
                }

                // Nếu tài khoản bị khóa, không cho đăng nhập
                if (!user.IsEnable)
                {
                    return new ResponseDTO("Your account has been blocked. Please contact admin.", 403, false);
                }

                // **So sánh trực tiếp mật khẩu nhập vào với mật khẩu trong DB**
                if (loginDTO.AccountPassword != user.AccountPassword)
                {
                    return new ResponseDTO("Incorrect password", 401, false);
                }

                // Kiểm tra refreshToken hiện tại của người dùng
                var existingRefreshToken = await _unitOfWork.TokenRepo.GetRefreshTokenByUserID(user.AccountId);
                if (existingRefreshToken != null)
                {
                    existingRefreshToken.IsRevoked = true;
                    await _unitOfWork.TokenRepo.UpdateAsync(existingRefreshToken);
                }

                // Tạo claims cho người dùng
                var userClaims = new List<Claim>
        {
            new Claim(JWTConstants.KeyClaim.Email, user.AccountEmail),
            new Claim(JWTConstants.KeyClaim.userId, user.AccountId.ToString()),
            new Claim(JWTConstants.KeyClaim.fullName, user.AccountName),
            new Claim(ClaimTypes.Role, user.AccountRole switch
            {
                1 => "Staff",
                2 => "Lecturer",
                _ => "User"
            })
        };

                // Tạo Access Token & Refresh Token
                var accessToken = JWTProvide.GenerateAccessToken(userClaims);
                var refreshToken = JWTProvide.GenerateRefreshToken(userClaims);

                // Lưu RefreshToken vào DB
                var newRefreshToken = new RefreshToken
                {
                    AccountId = user.AccountId,
                    RefreshTokenKey = refreshToken,
                    IsRevoked = false,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.TokenRepo.AddAsync(newRefreshToken);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO($"Hello {userClaims.First(c => c.Type == ClaimTypes.Role).Value}", 200, true, new TokenDTO
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                });
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }



        public async Task<ResponseDTO> LogoutAsync()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context != null)
            {
                // Lấy refreshToken từ Cookie
                var refreshToken = context.Request.Cookies["RefreshToken"];

                // Kiểm tra nếu có refreshToken thì thu hồi
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    var token = await _unitOfWork.TokenRepo.GetRefreshTokenByKey(refreshToken);
                    if (token != null)
                    {
                        token.IsRevoked = true;
                        _unitOfWork.TokenRepo.UpdateAsync(token);
                        await _unitOfWork.SaveChangeAsync();
                        Console.WriteLine($"[AuthService] RefreshToken {refreshToken} đã bị thu hồi!");
                    }
                }

                //// Xóa session
                //context.Session.Clear();

                // Xóa cookies (AccessToken & RefreshToken)
                context.Response.Cookies.Delete("AccessToken");
                context.Response.Cookies.Delete("RefreshToken");

                Console.WriteLine("[AuthService] Đăng xuất thành công!");
            }


            return new ResponseDTO("Logout successful", 200, true);

        }

        public Task<ResponseDTO> RefreshBothTokens(string oldAccessToken, string refreshTokenKey)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDTO> Register(RegisterDTO registerDTO)
        {
            try
            {
                // Kiểm tra xem email đã tồn tại chưa
                var existingUser = await _unitOfWork.AccountRepo.FindByEmailAsync(registerDTO.AccountEmail);
                if (existingUser != null)
                {
                    return new ResponseDTO("Email already exists", 400, false);
                }

                // Lấy AccountId lớn nhất + 1
                short maxId = await _unitOfWork.AccountRepo.GetMaxAccountIdAsync();
                short newAccountId = (short)(maxId + 1); // Tăng giá trị lớn nhất lên 1

                // Chuyển DTO thành Entity SystemAccount
                var newUser = new SystemAccount
                {
                    AccountId = newAccountId, // Gán AccountId mới
                    AccountEmail = registerDTO.AccountEmail,
                    AccountName = registerDTO.AccountName,
                    AccountPassword = registerDTO.AccountPassword, // Cần hash mật khẩu sau này
                    AccountRole = 2, // Mặc định là Lecturer
                    IsEnable = true
                };

                // Lưu vào DB
                await _unitOfWork.AccountRepo.AddAsync(newUser);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO("Registration successful", 201, true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO($"Error: {ex.Message}", 500, false);
            }
        }

    }
}
