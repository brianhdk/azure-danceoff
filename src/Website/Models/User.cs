using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Website.Models
{
	public class User : IUser
	{
		public User(ExternalLoginInfo login)
		{
			if (login == null) throw new ArgumentNullException("login");

			Id = login.Login.ProviderKey;
			UserName = login.Login.ProviderKey;
		}

		public string Id { get; private set; }
		public string UserName { get; set; }
	}
}