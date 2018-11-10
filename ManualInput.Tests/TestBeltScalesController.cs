using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CTS_Models;
using System.Data.Entity;
using Moq;
using System.Collections.Generic;
using CTS_Manual_Input.Models.Common;
using CTS_Manual_Input.Controllers;
using System.Web.Mvc;
using CTS_Manual_Input.Models;
using CTS_Models.DBContext;

namespace ManualInput.Tests
{
  [TestClass]
  public class TestBeltScalesController
  {

    static List<BeltTransfer> testTransfers = new List<BeltTransfer>()
    {
      new BeltTransfer()
      {
        ID = "1",
        LotName = "TestLot",
        LotQuantity = 1234,
        TotalQuantity = 1234,
        Comment = "TestComment",
        OperatorName = "TestOpertator",
        LasEditDateTime = DateTime.Now,
        TransferTimeStamp = DateTime.Now,
        EquipID = 1,
        IsValid = true,
        ItemID = 1,
        AnalysisID = 1,
        Equip = new BeltScale()
        {
          ID = 1,
        Name = "testName",
        LocationID ="test",
        FromInnerDestID = 1,
        ToInnerDestID = 1,
        NameEng = "testNameEng",
        Location = new Location()
        {
          ID = "test",
        LocationName = "Test",
        DomainName = "Test",
        }
        }
      }
    };

    static List<BeltScale> testBeltScales = new List<BeltScale>()
    {
      new BeltScale()
      {
        ID = 1,
        Name = "testName",
        LocationID ="test",
        FromInnerDestID = 1,
        ToInnerDestID = 1,
        NameEng = "testNameEng",
        Location = new Location()
        {
          ID = "test",
        LocationName = "Test",
        DomainName = "Test",
        }
      }
    };


    List<Location> testLocations = new List<Location>()
    {
      new Location()
      {
        ID = "test",
        LocationName = "Test",
        DomainName = "Test",
      }
    };

    //List<UserRole> testRole = new List<UserRole>()
    //{
    //  new UserRole()
    //  {
    //    ID = 1,
    //    SystemName = "DeleteUser",
    //    DomainRoleName = "Test"
    //  }
    //};

    Mock<CtsDbContext> cdbcontext;

    [TestInitialize]
    public void testInit()
    {
      var set = new Mock<DbSet<BeltTransfer>>()
        .SetupData(testTransfers);

      var scalesset = new Mock<DbSet<BeltScale>>()
        .SetupData(testBeltScales);

      var locationsset = new Mock<DbSet<Location>>()
        .SetupData(testLocations);

      //var roleset = new Mock<DbSet<UserRole>>()
      //  .SetupData(testRole);

      cdbcontext = new Mock<CtsDbContext>();
      cdbcontext.Setup(s => s.InternalTransfers).Returns(set.Object);
      cdbcontext.Setup(s => s.BeltScales).Returns(scalesset.Object);
      cdbcontext.Setup(s => s.Locations).Returns(locationsset.Object);
      cdbcontext.Setup(s => s.Locations).Returns(locationsset.Object);
      //cdbcontext.Setup(s => s.UserRoles).Returns(roleset.Object);

   }

    [TestMethod]
    public void TestAdd()
    {
      var controller = new BeltScalesController(cdbcontext.Object);
      var result = controller.Add(1,"test") as ViewResult;
      var resultmodel = (BeltTransfer)result.Model;

      Assert.AreEqual("Add", result.ViewName);
      Assert.IsTrue(resultmodel.EquipID == 1);
      Assert.IsTrue(resultmodel.LasEditDateTime.ToString("YYYY.MM.DD HH:mm") == (DateTime.Now.ToString("YYYY.MM.DD HH:mm")));
      Assert.IsNotNull(resultmodel.Equip);
      Assert.IsTrue(resultmodel.LotName.StartsWith("testName"));
      Assert.IsTrue(resultmodel.IsValid == true);
      Assert.IsTrue(resultmodel.ID.StartsWith("B1"));
    }
    [TestMethod]
    public void TestIndex()
    {
      var controller = new BeltScalesController(cdbcontext.Object);
      var result = controller.Index() as ViewResult;
      var resultmodel = (BeltScale_Transfer)result.Model;

      Assert.IsNotNull(result.Model);
      Assert.IsTrue(resultmodel.BeltScales.Count > 0);
      Assert.IsTrue(resultmodel.CanDelete);
      Assert.IsTrue(resultmodel.InternalTransfers.TotalItemCount > 0);
    }
  }
}
