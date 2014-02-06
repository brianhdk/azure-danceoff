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

		public void Update(Dancer dancer)
		{
			Status = dancer.Status;

			LocationX = dancer.LocationX;
			LocationY = dancer.LocationY;

			Animation = dancer.Animation;
		}
	}
}