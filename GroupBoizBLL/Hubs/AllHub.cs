using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace GroupBoizBLL.Hubs
{
    public class AllHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext?.Request.Query["userId"].ToString();

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
                Console.WriteLine($"🟢 User {userId} đã kết nối SignalR.");
            }
            else
            {
                Console.WriteLine("⚠️ Không tìm thấy userId trong query string.");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var userId = httpContext?.Request.Query["userId"].ToString();

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
                Console.WriteLine($"🔴 User {userId} đã ngắt kết nối.");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
