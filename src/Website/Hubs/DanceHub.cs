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
				Owner(o => o.Add(dancer));
			
			return dancer;
		}

		private void Owner(Action<dynamic> owner)
		{
			if (Ring.Owner != null)
				owner(Clients.Clients(Ring.Owner.Connections.ToList()));
		}

		[Authorize(Users = Models.User.Brianh)]
		public Dancer[] Manage()
		{
			Dancer dancer = Enter();

			Ring.Owner = dancer;

			return Ring.Dancers;
		}

		public void Update(Dancer dancer)
		{
			Ring.Update(ref dancer);

			Clients.Clients(dancer.Connections.ToList()).Update(dancer);

			Owner(o => o.Update(dancer));
		}

		public override Task OnDisconnected()
		{
			Dancer dancer;
			if (Ring.Leave(ConnectionId, out dancer))
				Owner(o => o.Remove(dancer));

			return base.OnDisconnected();
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
			get { return _dancers.Values.Where(x => x.HasAnyConnections()).ToArray(); }
		}

		public Dancer Owner { get; set; }

		public Dancer Enter(string id, string connectionId, out bool newDancer)
		{
			Dancer dancer;
			newDancer = !_dancers.TryGetValue(id, out dancer) || !dancer.HasAnyConnections();

			if (dancer == null)
			{
				dancer = new Dancer(id);
				_dancers.Add(id, dancer);
			}

			dancer.Connect(connectionId);

			return dancer;
		}

		public void Update(ref Dancer dancer)
		{
			if (dancer == null) throw new ArgumentNullException("dancer");

			Dancer actualDancer = ActualDancer(dancer);

			actualDancer.Update(dancer);

			dancer = actualDancer;
		}

		private Dancer ActualDancer(Dancer dancer)
		{
			if (!_dancers.TryGetValue(dancer.Id, out dancer))
				throw new InvalidOperationException("Dancer not found.");

			return dancer;
		}

		public bool Leave(string connectionId, out Dancer dancer)
		{
			dancer = _dancers.Values.SingleOrDefault(x => x.HasConnection(connectionId));

			if (dancer != null)
			{
				dancer.Disconnect(connectionId);

				// Same dancer can be connected from multiple devices/sessions at the same time
				// se we don't remove the dancer before the last connection has lost
				if (!dancer.HasAnyConnections())
				{
					if (dancer == Owner)
						Owner = null;

					return true;
				}
			}

			return false;
		}
	}

	public class Dancer
	{
		private readonly HashSet<string> _connections;
  
		public Dancer(string id)
		{
			Id = id;
			_connections = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		}

		public string Id { get; private set; }
		public string Status { get; set; }
		public ushort LocationX { get; set; }
		public ushort LocationY { get; set; }

		public void Connect(string connectionId)
		{
			_connections.Add(connectionId);
		}

		public IEnumerable<string> Connections
		{
			get { return _connections; }
		}

		public bool HasConnection(string connectionId)
		{
			return _connections.Contains(connectionId);
		}

		public void Disconnect(string connectionId)
		{
			_connections.Remove(connectionId);
		}

		public bool HasAnyConnections()
		{
			return _connections.Count > 0;
		}

		public void Update(Dancer dancer)
		{
			Status = dancer.Status;
			LocationX = dancer.LocationX;
			LocationY = dancer.LocationY;
		}
	}
}