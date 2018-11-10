using CTS_Analytics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using System.Net.Http;
using CTS_Models.DBContext;

namespace CTS_Analytics.Controllers
{

	//[AllowCrossSiteJson]
	public class GrafanaController : ApiController
	{
		CtsDbContext cdb = new CtsDbContext();

		[System.Web.Http.HttpGet]
		public HttpResponseMessage Index()
		{
			var connection = new SqlConnection(cdb.Database.Connection.ConnectionString);
			try
			{
				connection.Open();
				if (connection.State == ConnectionState.Open)
				{
					connection.Close();
					return new HttpResponseMessage(HttpStatusCode.OK);
				}
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
			}
			return new HttpResponseMessage(HttpStatusCode.InternalServerError);
		}

		public IHttpActionResult query(QueryModel model)
		{
			if (model != null)
			{
				var resopnsemodels = new Iresponsemodel[model.targets.Count()];
				int i = 0;
				foreach (var target in model.targets)
				{

					if (target.type == "table")
					{
						resopnsemodels[i] = buildtable(target, model.range);
					}
					if (target.type == "timeserie")
					{
						resopnsemodels[i] = gettimeserie(target, model.range, model.interval);
					}
					i++;
				}
				return Json(resopnsemodels);
			}
			return Json(new QueryResponseTableModel[1]); ;
		}

		private QueryResponseTimeSerieModel gettimeserie(target_ target, range range, string interval)
		{
			var _target = "";
			string targetquery = target.target;
			if (target.target.Split(Convert.ToChar(";")).Count() > 1)
			{
				targetquery = target.target.Split(Convert.ToChar(";"))[0];
				_target = target.target.Split(Convert.ToChar(";"))[1];

			}

			DateTime from = Convert.ToDateTime(range.from);
			DateTime to = Convert.ToDateTime(range.to);

			string query = targetquery;

			query = Regex.Replace(query, "where", (" WHERE TransferTimeStamp>='" + from.ToString("yyyy-MM-dd HH:mm:ss") + "'") + (" AND TransferTimeStamp<='" + to.ToString("yyyy-MM-dd HH:mm:ss") + "' and "), RegexOptions.IgnoreCase);

			var connection = new SqlConnection(cdb.Database.Connection.ConnectionString);
			var command = new SqlCommand(query, connection);
			connection.Open();
			DataTable dt = new DataTable();
			dt.Load(command.ExecuteReader());
			connection.Close();

			var returnmodel = new QueryResponseTimeSerieModel();
			returnmodel.datapoints = new double[dt.Rows.Count][];
			for (int i = 0; i < returnmodel.datapoints.Count(); i++)
			{
				float value = 0;
				float.TryParse(dt.Rows[i].ItemArray[0].ToString(), out value);
				ulong time = 0;
				DateTime d = new DateTime();
				if (dt.Columns.Count > 1)
					if (DateTime.TryParse((dt.Rows[i].ItemArray[1].ToString()), out d))
						UInt64.TryParse((DateTime.Parse(dt.Rows[i].ItemArray[1].ToString()).AddHours(-6).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds).ToString(), out time);
				returnmodel.datapoints[i] = new double[2] { value, time };
			}
			returnmodel.target = _target;
			return returnmodel;
		}

		private QueryResponseTableModel buildtable(target_ m, range range)
		{

			DateTime from = Convert.ToDateTime(range.from);
			DateTime to = Convert.ToDateTime(range.to);

			var resopnsemodel = new QueryResponseTableModel();
			string query = m.target;

			query = Regex.Replace(query, "where", (" WHERE TransferTimeStamp>='" + from.ToString("yyyy-MM-dd HH:mm:ss") + "'") + (" AND TransferTimeStamp<='" + to.ToString("yyyy-MM-dd HH:mm:ss") + "' and "), RegexOptions.IgnoreCase);

			var connection = new SqlConnection(cdb.Database.Connection.ConnectionString);
			var command = new SqlCommand(query, connection);
			connection.Open();
			DataTable dt = new DataTable();
			dt.Load(command.ExecuteReader());
	
			connection.Close();

			resopnsemodel.columns = new column[dt.Columns.Count];
			resopnsemodel.rows = new object[dt.Rows.Count][];

			for (int colnum = 0; colnum < resopnsemodel.columns.Count(); colnum++)
			{
				resopnsemodel.columns[colnum] = new column();
				resopnsemodel.columns[colnum].text = dt.Columns[colnum].ColumnName;

				for (int rownum = 0; rownum < resopnsemodel.rows.Count(); rownum++)
				{
					if (colnum == 0)
						resopnsemodel.rows[rownum] = new object[dt.Columns.Count];
					string s = dt.Rows[rownum].ItemArray[colnum].ToString();
					if (DateTime.TryParse(s, out var dateresult))
					{
						resopnsemodel.columns[colnum].type = "time";
						UInt64.TryParse((DateTime.Parse(s).AddHours(-6).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds).ToString(), out ulong value);
						resopnsemodel.rows[rownum][colnum] = value;
					}
					else if (double.TryParse(s, out var doubleresult))
					{
						resopnsemodel.columns[colnum].type = "number";
						resopnsemodel.rows[rownum][colnum] = doubleresult;
					}
					else
					{
						resopnsemodel.columns[colnum].type = "string";
						resopnsemodel.rows[rownum][colnum] = s;
					}
				}
			}
			resopnsemodel.type = "table";
			return resopnsemodel;
		}



		public IHttpActionResult search(SearchModel model)
		{
			var sets = from p in typeof(CtsDbContext).GetProperties()
						   //where p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>)
						   //let entityType = p.PropertyType.GetGenericArguments().First()
					   select p.Name;
			Regex regex = new Regex(@"\s(\w+)$", RegexOptions.IgnoreCase);
			Match match = regex.Match(model.target);

			List<string> returnarray = new List<string>();

			if (match.Success)

				foreach (string set in sets)
				{
					Regex r = new Regex(set, RegexOptions.IgnoreCase);
					Match m = r.Match(set);
					if (m.Success)
						returnarray.Add(set);
				}
			return Json(returnarray);
		}

		public IHttpActionResult annotations(SearchModel model)
		{

			return Json(false);
		}
	}
}


