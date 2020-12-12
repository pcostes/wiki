using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web3
{
	public partial class WebForm1 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void Submit_Click1(object sender, EventArgs e)
		{
			string[] result = Helper.dummyBFS(StartPoint.Text, EndPoint.Text);
			NameLabel.Text = "Length: " + result.Length.ToString();
			System.Data.DataTable data = new DataTable();
			data.Columns.Add("Path");
			foreach (var r in result)
			{
				data.Rows.Add(r);
			}

			PathList.DataSource = data;
			PathList.DataBind();
		}
	}
}