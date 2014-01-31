using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Website.Models
{
	public class NonStoringUserStore : IUserStore<User>
	{
		public void Dispose()
		{
		}

		public Task CreateAsync(User user)
		{
			throw new NotSupportedException();
		}

		public Task UpdateAsync(User user)
		{
			throw new NotSupportedException();
		}

		public Task DeleteAsync(User user)
		{
			throw new NotSupportedException();
		}

		public Task<User> FindByIdAsync(string userId)
		{
			throw new NotSupportedException();
		}

		public Task<User> FindByNameAsync(string userName)
		{
			throw new NotSupportedException();
		}
	}
}