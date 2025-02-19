﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GroupBoizBLL.Services.Interface;
using GroupBoizCommon.Setting;
using Microsoft.IdentityModel.Tokens;

namespace GroupBoizBLL.Services.Implement
{
    public class JWTProvide  
    {
        public static  string GenerateAccessToken(List<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(JWTSettingModel.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(JWTSettingModel.ExpireDayAccessToken),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = JWTSettingModel.Issuer, // Thêm Issuer
                Audience = JWTSettingModel.Audience // Thêm Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public static string GenerateRefreshToken(List<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(JWTSettingModel.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(JWTSettingModel.ExpireDayRefreshToken), // Thời gian hết hạn
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = JWTSettingModel.Issuer, // Thêm Issuer cho refresh token
                Audience = JWTSettingModel.Audience // Thêm Audience cho refresh token
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public static bool Validation(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(JWTSettingModel.SecretKey);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = JWTSettingModel.Issuer,
                    ValidateAudience = true,
                    ValidAudience = JWTSettingModel.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Không cho phép trễ thời gian
                }, out SecurityToken validatedToken);

                return true;
            }
            catch (SecurityTokenExpiredException)
            {
                return false; // Token hết hạn
            }
            catch (Exception)
            {
                return false; // Token không hợp lệ
            }
        }

        public static List<Claim> DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(JWTSettingModel.SecretKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var claims = jwtToken.Claims.ToList();

            return claims;
        }
        public static string GetValueFromToken(string token, string claimType)
        {
            var claims = DecodeToken(token);

            var claim = claims.FirstOrDefault(c => c.Type == claimType);

            return claim?.Value ?? string.Empty; // Trả về giá trị nếu claim tồn tại, nếu không thì trả về chuỗi rỗng
        }
    }
}
