using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using BlazorApp1.DBContext;
using BlazorApp1.Hubs;
using BlazorApp1.Models;
using System.Diagnostics;

namespace BlazorApp1.Services
{
    public class QueueService
    {
        private readonly IHubContext<QueueHub> hubContext;
        public event Action<Queue>? OnQueueChanged;

        public QueueService(IHubContext<QueueHub> _hubContext)
        {
            hubContext = _hubContext;
        }

        public async Task AddToQueue(Queue queue)
        {
            using (var context = new AppDBContext()) {
                await context.queue.AddAsync(queue);
                await context.SaveChangesAsync();
            }

            OnQueueChanged?.Invoke(queue);
            await hubContext.Clients.All.SendAsync("QueueChanged", queue);
        }

        public async Task DeleteFromQueue(int queueId)
        {
            using (var context = new AppDBContext())
            {
                var oldQueue = await context.queue.FindAsync(queueId);
                if (oldQueue != null) { 
                    context.queue.Remove(oldQueue);
                    await context.SaveChangesAsync();

                    OnQueueChanged?.Invoke(oldQueue);
                    await hubContext.Clients.All.SendAsync("QueueChanged", oldQueue);
                }
            }
        }

        public async Task<List<Queue>> GetAll()
        {
            using (var context = new AppDBContext())
            {
                return await context.queue.ToListAsync();
            }
        }
    }
}
