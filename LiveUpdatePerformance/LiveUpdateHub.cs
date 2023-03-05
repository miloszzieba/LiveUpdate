using LiveUpdatePerformance.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LiveUpdatePerformance
{
    public class LiveUpdateHub : Hub
    {
        private readonly DataService _dataService;

        public LiveUpdateHub(DataService dataService)
        {
            this._dataService = dataService;
        }

        public async Task Init()
        {
            await Clients.Caller.SendAsync("Init", this._dataService.GetCurrentRows());
        }
    }
}
