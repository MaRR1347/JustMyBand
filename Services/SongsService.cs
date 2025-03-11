using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using BlazorApp1.DBContext;
using BlazorApp1.Hubs;
using BlazorApp1.Models;

namespace BlazorApp1.Services
{
    public class SongService
    {
        private readonly IHubContext<QueueHub> hubContext;
        public event Action<Songs>? OnSongsChanged;

        public SongService(IHubContext<QueueHub> _hubContext)
        {
            hubContext = _hubContext;
        }

        public async Task AddNewSong(Songs song)
        {
            using (var context = new AppDBContext()) { 
                await context.songs.AddAsync(song);
                await context.SaveChangesAsync();
            }

            OnSongsChanged?.Invoke(song);
            await hubContext.Clients.All.SendAsync("SongsChanged", song);
        }

        public async Task DeleteSong(int songId)
        {
            using (var context = new AppDBContext())
            {
                var oldSong = await context.songs.FindAsync(songId);
                if (oldSong != null) { 
                    context.songs.Remove(oldSong);
                    await context.SaveChangesAsync();

                    OnSongsChanged?.Invoke(oldSong);
                    await hubContext.Clients.All.SendAsync("SongsChanged", oldSong);
                }
            }
        }

        public async Task EditSong(Songs song)
        {
            using (var context = new AppDBContext())
            {
                context.songs.Update(song);
                await context.SaveChangesAsync();
            }

            OnSongsChanged?.Invoke(song);
            await hubContext.Clients.All.SendAsync("SongsChanged", song);
        }

        public async Task<List<Songs>> GetAll()
        {
            using (var context = new AppDBContext())
            {
                return await context.songs.ToListAsync();
            }
        }
    }
}
