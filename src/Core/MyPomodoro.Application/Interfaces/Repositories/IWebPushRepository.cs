using System.Threading.Tasks;
using MyPomodoro.Domain.Entities;

namespace MyPomodoro.Application.Interfaces.Repositories
{
    public interface IWebPushRepository : IGenericRepository<WebPushSubscription>
    {
        public string GetVapidPublicKey();
        public Task<WebPushSubscription> GetPushSubscriptionByKey(string userId, string key);
    }
}