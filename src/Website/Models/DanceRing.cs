using System;
using System.Collections.Generic;
using System.Linq;

namespace Website.Models
{
	public class DanceRing
	{
		private readonly Dictionary<string, Dancer> _dancers;

		public DanceRing()
		{
			Owner = new Owner();
			_dancers = new Dictionary<string, Dancer>(StringComparer.OrdinalIgnoreCase);
		}

		public Owner Owner { get; private set; }

		public Dancer[] Dancers
		{
			get { return _dancers.Values.Where(x => x.HasAnyConnections()).ToArray(); }
		}

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

		public void Update(string id, ref Dancer remote)
		{
			if (remote == null) throw new ArgumentNullException("remote");

			Dancer local = GetLocalOf(id);

			local.Update(remote);
			
			remote = local;
		}

		private Dancer GetLocalOf(string id)
		{
			if (String.IsNullOrWhiteSpace(id)) throw new ArgumentException("Value cannot be null or empty.", "id");

			Dancer local;
			if (!_dancers.TryGetValue(id, out local))
				throw new InvalidOperationException(String.Format("Dancer with ID '{0}' not found.", id));

			return local;
		}

		public bool Leave(string connectionId, out Dancer dancer)
		{
			dancer = null;

			if (Owner.HasConnection(connectionId))
			{
				Owner.Disconnect(connectionId);
				if (!Owner.HasAnyConnections())
					return false;
			}

			dancer = _dancers.Values.SingleOrDefault(x => x.HasConnection(connectionId));

			if (dancer != null)
			{
				dancer.Disconnect(connectionId);

				// Same dancer can be connected from multiple devices/sessions at the same time
				// se we don't remove the dancer before the last connection has lost
				if (!dancer.HasAnyConnections())
				{
					return true;
				}
			}

			return false;
		}
	}
}