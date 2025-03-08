using GroupBoizCommon.DTO;
using Microsoft.AspNetCore.SignalR;

namespace GroupBoizBLL.Hubs
{
    public class NewsHub : Hub
    {
        public async Task SendNews(NewsArticleDTO newArticle)
        {
            await Clients.All.SendAsync("ReceiveNews", newArticle);
        }


        public async Task JoinCategoryGroup(string categoryId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"category-{categoryId}");
        }

        public async Task JoinTagGroup(string tagId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"tag-{tagId}");
        }
    }
}
