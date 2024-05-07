using ChatThing.Lib.Constants;
using ChatThing.Lib.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatThing.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async void SendMessage(string user, byte[] message, DateTime time, Guid id)
        {
            await Clients.All.SendAsync(HubConstants.RECEIVE_MESSAGE, user, message, time, id);
        }
        public async void Connect(ClientModel model)
        {
            await Clients.Others.SendAsync(HubConstants.RECEIVE_NEW_USER_INFO, model);
        }
        public async void SendAes(ConnectionModel model)
        {
            await Clients.Others.SendAsync(HubConstants.GET_AES, model);
        }
        public async void HandshakeAes(ConnectionModel model)
        {
            await Clients.Others.SendAsync(HubConstants.ACK_AES, model);
        }
    }
}
