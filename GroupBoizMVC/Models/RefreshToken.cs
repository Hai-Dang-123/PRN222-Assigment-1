using System.ComponentModel.DataAnnotations;

namespace GroupBoizMVC.Models
{
    public class RefreshToken
    {
        [Key] // Đánh dấu là khóa chính
        public Guid RefreshTokenId { get; set; } // Khóa chính cho RefreshToken

        [Required(ErrorMessage = "UserId is required")]
        public Guid UserId { get; set; } // Khóa ngoại liên kết đến người dùng

        [Required(ErrorMessage = "RefreshTokenId is required")]
        public string RefreshTokenKey { get; set; } // Giá trị refresh token
        public bool IsRevoked { get; set; }

        public DateTime CreatedAt { get; set; } // Thời gian tạo refresh token
    }

}
