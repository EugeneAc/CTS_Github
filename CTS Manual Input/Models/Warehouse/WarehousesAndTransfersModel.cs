using CTS_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTS_Manual_Input.Models
{
	public class WarehousesAndTransfersModel
	{
		public List<Warehouse> Warehouses { get; set; }
		public PagedList.IPagedList<WarehouseMeasure> WarehouseMeasures { get; set; }
	}
}