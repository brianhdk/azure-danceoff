using Microsoft.AspNet.SignalR;

namespace Website.Hubs
{
	[Authorize]
	public class ChatHub : Hub
	{
		public void Send(string message)
		{
			Clients.All.broadcastMessage(Context.User.Identity.Name, message);
		}
	}
}