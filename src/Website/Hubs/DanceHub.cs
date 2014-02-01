using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Website.Hubs
{
	[Authorize]
	public class DanceHub : Hub
	{
		private static readonly DanceRing Ring = new DanceRing();

		public Dancer Enter()
		{
			Dancer dancer = Ring.Enter(User, ConnectionId);

			UpdateClients();

			return dancer;
		}

		public void Update(Dancer dancer)
		{
			Ring.Update(dancer);

			UpdateClients();
		}

		public override Task OnDisconnected()
		{
			Ring.Leave(ConnectionId);

			UpdateClients();

			return base.OnDisconnected();
		}

		private void UpdateClients()
		{
			Clients.All.Update(Ring);
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

	public class DanceRing
	{
		private readonly Dictionary<string, Dancer> _dancers;

		public DanceRing()
		{
			_dancers = new Dictionary<string, Dancer>(StringComparer.OrdinalIgnoreCase);
		}

		public Dancer[] Dancers
		{
			get { return _dancers.Values.ToArray(); }
		}

		public Dancer Enter(string id, string connectionId)
		{
			Dancer dancer;
			if (!_dancers.TryGetValue(id, out dancer))
			{
				dancer = new Dancer(id);
				_dancers.Add(id, dancer);
			}

			dancer.Connect(connectionId);

			return dancer;
		}

		public void Update(Dancer dancer)
		{
			Dancer actualDancer;
			if (_dancers.TryGetValue(dancer.Id, out actualDancer))
				actualDancer.Update(dancer);
		}

		public void Leave(string connectionId)
		{
			Dancer dancer = _dancers.Values.SingleOrDefault(x => x.HasConnection(connectionId));

			if (dancer != null)
			{
				dancer.Disconnect(connectionId);

				if (!dancer.IsAlive())
					_dancers.Remove(dancer.Id);
			}
		}
	}

	public class Dancer
	{
		private readonly HashSet<string> _connections;
		private DateTimeOffset _lastPulse;
  
		public Dancer(string id)
		{
			Id = id;
			_connections = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		}

		public string Id { get; private set; }
		public string Status { get; set; }

		public void Connect(string connectionId)
		{
			_connections.Add(connectionId);
		}

		public bool HasConnection(string connectionId)
		{
			return _connections.Contains(connectionId);
		}

		public void Disconnect(string connectionId)
		{
			_connections.Remove(connectionId);
		}

		public bool IsAlive()
		{
			return _connections.Count > 0;
		}

		public void Update(Dancer dancer)
		{
			Status = dancer.Status;
		}
	}
}