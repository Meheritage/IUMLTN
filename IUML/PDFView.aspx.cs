using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IUML
{
    public partial class PDFView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Convert.ToString(Session["filepath"])))
            {
                //string pass = Session["filepath"].ToString(); ; ;
                //iframeDiv.Controls.Add(new LiteralControl("<iframe src=\"" + pass + "\" style='height: 700px;; width: 100%; border: none'></iframe><br />"));
                string FilePath = Convert.ToString(Session["filepath"]);
                WebClient User = new WebClient();
                Byte[] FileBuffer = User.DownloadData(FilePath);
                if (FileBuffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                    Response.BinaryWrite(FileBuffer);
                }
            }

        }
    }
}