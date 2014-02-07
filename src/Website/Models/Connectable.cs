using System;
using System.Collections.Generic;

namespace Website.Models
{
	public abstract class Connectable
	{
		private readonly HashSet<string> _connections;

		protected Connectable()
		{
			_connections = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		}

		public IEnumerable<string> Connections
		{
			get { return _connections; }
		}

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

		public bool HasAnyConnections()
		{
			return _connections.Count > 0;
		}
	}
}