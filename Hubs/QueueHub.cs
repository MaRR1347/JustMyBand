using Microsoft.AspNetCore.SignalR;
using BlazorApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Hubs
{
    public class QueueHub : Hub
    {
        public async Task ChangeSongs(Songs songs)
        {
            await Clients.All.SendAsync("SongsChanged", songs);
        }

        public async Task ChangeQueue(Queue queue)
        {
            await Clients.All.SendAsync("QueueChanged", queue);
        }
    }
}
