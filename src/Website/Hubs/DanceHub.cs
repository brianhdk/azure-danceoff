using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Website.Hubs
{
	[Authorize]
	public class DanceHub : Hub
	{
		private static readonly DanceRing Ring = new DanceRing();
		private static readonly object SyncRoot = new object();

		public Dancer Enter()
		{
			bool newDancer;
			Dancer dancer;

			lock (SyncRoot)
			{
				dancer = Ring.Enter(User, ConnectionId, out newDancer);
			}

			if (newDancer)
				Owner(Ring).Add(dancer);
			
			return dancer;
		}

		[Authorize(Users = Models.User.Brianh)]
		public Dancer[] Manage()
		{
			Ring.Owner.Connect(ConnectionId);

			return Ring.Dancers;
		}

		public void Update(Dancer dancer)
		{
			Ring.Update(ref dancer);

			Dancer(dancer).Update(dancer);

			Owner(Ring).Update(dancer);
		}

		public override Task OnDisconnected()
		{
			Dancer dancer;
			if (Ring.Leave(ConnectionId, out dancer))
				Owner(Ring).Remove(dancer);

			return base.OnDisconnected();
		}

		private dynamic Owner(DanceRing ring)
		{
			if (ring == null) throw new ArgumentNullException("ring");

			return ClientsOf(Ring.Owner);
		}

		private dynamic Dancer(Dancer dancer)
		{
			return ClientsOf(dancer);
		}

		private dynamic ClientsOf(Connectable connectable)
		{
			if (connectable == null) throw new ArgumentNullException("connectable");

			return Clients.Clients(connectable.Connections.ToList());
		}

		private string User
		{
			get { return Context.User.Identity.Name; }
		}

		private string ConnectionId
		{
			get { return Context.ConnectionId; }
		}
	}
}