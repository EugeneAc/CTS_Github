using System;
using CTS_RoleAdmin.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoleAdmin.Handlers;

namespace RoleAdmin.Tests
{
	[TestClass]
	public class HomeControllerTests
	{
		[TestMethod]
		public void CheckFindDomainUser()
		{

			var user = DomainUsersHandler.FindUser("y.aniskina", "kazprom");
			Assert.IsNotNull(user);
		}
	}
}
