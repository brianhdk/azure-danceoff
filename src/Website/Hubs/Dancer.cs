namespace Website.Hubs
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
		public string Animation { get; set; }

		public void Update(Dancer remote)
		{
			Status = remote.Status;

			LocationX = remote.LocationX;
			LocationY = remote.LocationY;

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