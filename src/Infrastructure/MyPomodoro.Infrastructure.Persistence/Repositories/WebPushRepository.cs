using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyPomodoro.Application.Interfaces.Repositories;
using MyPomodoro.Domain.Entities;
using MyPomodoro.Infrastructure.Persistence.Contexts;
using MyPomodoro.Infrastructure.Persistence.Dapper;
using MyPomodoro.Infrastructure.Persistence.Settings;

namespace MyPomodoro.Infrastructure.Persistence.Repositories
{
    public class WebPushRepository : GenericRepository<WebPushSubscription>, IWebPushRepository
    {
        IDapper dapper;
        IdentityContext _context;
        private APIAppSettings _apiSettings;
        public WebPushRepository(IDapper dapper, IdentityContext dbContext, IOptions<APIAppSettings> apiSettings) : base(dbContext)
        {
            this.dapper = dapper;
            this._context = dbContext;
            _apiSettings = apiSettings.Value;
        }

        public async Task<WebPushSubscription> GetPushSubscriptionByKey(string userId, string key)
        {
            return await _context.WebPushSubscriptions.FirstOrDefaultAsync(s => s.UserId == userId && s.P256Dh == key);
        }

        public string GetVapidPublicKey()
        {
            return _apiSettings.VapidSettings.PublicKey;
        }
    }
}