using LiveUpdatePerformance.Services;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace LiveUpdatePerformance
{
    public class Sender : BackgroundService
    {
        private readonly IHubContext<LiveUpdateHub> _hubContext;
        private readonly DataService _dataService;

        public Sender(IHubContext<LiveUpdateHub> hubContext, DataService dataService)
        {
            _hubContext = hubContext;
            _dataService = dataService;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await DoWork(cancellationToken);
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000);
                var data = this._dataService.GetLatestChanges();
                await this._hubContext.Clients.All.SendAsync("Update", data);
            }
        }
    }
}
