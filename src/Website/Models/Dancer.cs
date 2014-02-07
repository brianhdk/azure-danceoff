using System;

namespace Website.Models
{
	public class Dancer : Connectable
	{
		public Dancer(string id)
		{
			Id = id;
		}

		public string Id { get; private set; }
		public string Status { get; set; }
		public ushort LocationX { get; set; }
		public ushort LocationY { get; set; }
		public ushort LocationZ { get; set; }
		public string Animation { get; set; }

		public void Update(Dancer remote)
		{
			if (remote == null) throw new ArgumentNullException("remote");

			if (!String.Equals(Id, remote.Id))
				throw new InvalidOperationException("ID mismatch.");

			Status = remote.Status;

			LocationX = remote.LocationX;
			LocationY = remote.LocationY;
			LocationZ = remote.LocationZ;

			Animation = remote.Animation;
		}

		public void Restart()
		{
			LocationX = default(ushort);
			LocationY = default(ushort);

			Animation = null;
		}
	}
}