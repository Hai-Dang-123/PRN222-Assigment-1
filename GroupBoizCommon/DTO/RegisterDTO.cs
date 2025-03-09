using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBoizCommon.DTO
{
    public class RegisterDTO
    {
        public string AccountEmail { get; set; }
        public string AccountName { get; set; }
        public string AccountPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public int AccountRole { get; set; } = 2; // Mặc định là Lecturer
        public bool IsEnable { get; set; } = true;
    }
}
