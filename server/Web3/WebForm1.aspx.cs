using System;
using System.Collections.Generic;
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
			NameLabel.Text = EndPoint.Text + " - " + StartPoint.Text;
		}
	}
}