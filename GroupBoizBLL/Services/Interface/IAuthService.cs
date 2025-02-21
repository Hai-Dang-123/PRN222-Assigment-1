using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBoizCommon.DTO;

namespace GroupBoizBLL.Services.Interface
{
    public interface IAuthService
    {
        Task<ResponseDTO> Login(LoginDTO loginDTO);
        
        Task<ResponseDTO> RefreshBothTokens(string oldAccessToken, string refreshTokenKey);
        Task<ResponseDTO> LogoutAsync();
    }
}
