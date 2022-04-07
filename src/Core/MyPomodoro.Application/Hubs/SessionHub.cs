using System;
using Microsoft.AspNetCore.SignalR;

namespace MyPomodoro.Application.Hubs
{
    public class SessionHub : Hub
    {
        public string getConnectionId() => Context.ConnectionId;
    }
}