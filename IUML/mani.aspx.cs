using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Net;

namespace IUML
{
    public partial class mani : System.Web.UI.Page
    {
        string strMsg;
        string _IUMLCon = Convert.ToString(ConfigurationManager.ConnectionStrings["IUMLCon"]);
        string returnurl;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindYear();
                Bind_grd_PDFList("");
            }
        }

        protected void btn_view_Click(object sender, EventArgs e)
        {
            string filename = "";
            string year = DateTime.Now.Year.ToString();
            string Month = DateTime.Now.Date.ToString("MMMM");
            using (SqlConnection con = new SqlConnection(_IUMLCon))
            {
                string qry;
                //qry = "SELECT top 1 FileName FROM iumltn_manichuder where Created_Year =YEAR(getdate())  and Created_Month  = DATENAME(month, GETDATE())   order by Created_Date desc";
                //qry = "SELECT top 1 FileName   FROM iumltn_manichuder order by Created_Date desc";
                qry = "select  top 1 FileName  from iumltn_manichuder where Created_Year='" + year.ToString() + "' and Created_Month='" + Month.ToString() + "' order by DayofMonth desc";
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    con.Open();
                    filename = Convert.ToString(cmd.ExecuteScalar());
                    con.Close();
                    string Desdaypdffullpath = Path.Combine(Server.MapPath("~/manichudar/" + filename));
                    if (File.Exists(Path.Combine(Desdaypdffullpath)))
                    {
                        Session["filepath"] = Desdaypdffullpath;
                        Response.Redirect("PDFView.aspx");
                        string FilePath = Desdaypdffullpath;
                        WebClient User = new WebClient();
                        Byte[] FileBuffer = User.DownloadData(FilePath);
                        if (FileBuffer != null)
                        {
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-length", FileBuffer.Length.ToString());
                            Response.BinaryWrite(FileBuffer);
                        }
                    }
                    else
                    {
                        lblMessage.Text = "File not found !.";
                        lblMessage.ForeColor = Color.Red;
                    }
                }
            }
        }
        void BindYear()
        {
            try
            {
                string _year = DateTime.Now.Date.Year.ToString();
                DataTable _dt = new DataTable();
                using (SqlConnection con = new SqlConnection(_IUMLCon))
                {
                    string qry = "SELECT distinct Created_Year FROM iumltn_manichuder   order by Created_Year desc ";
                    con.Open();
                    SqlDataAdapter _adapter = new SqlDataAdapter(qry, con);
                    _adapter.Fill(_dt);
                    _adapter.Dispose();
                    con.Close();
                    if (_dt.Rows.Count > 0)
                    {
                        ddl_year.DataSource = _dt;
                        ddl_year.DataTextField = "Created_Year";
                        ddl_year.DataValueField = "Created_Year";
                        ddl_month.SelectedValue = _year;
                        ddl_year.DataBind();
                        ddl_year.Items.Insert(0, new ListItem("Select", "")); //updated code
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        void BindMonth(string _year)
        {
            try
            {
                string monthname = DateTime.Now.Date.ToString("MMMM");
                DataTable _dt = new DataTable();
                int currentyear = DateTime.Now.Year;
                using (SqlConnection con = new SqlConnection(_IUMLCon))
                {
                    string qry;
                    if (string.IsNullOrEmpty(_year))
                    {
                        qry = "\r\nSELECT Created_Month\r\nFROM (\r\n    SELECT DISTINCT \r\n        Created_Month,\r\n        CASE Created_Month\r\n            WHEN 'December' THEN 1\r\n\t\t\t  WHEN 'November'  THEN 2\r\n\t\t\t   WHEN 'October'   THEN 3\r\n\t\t\t    WHEN 'September' THEN 4\r\n\t\t\t\t WHEN 'August'   THEN 5\r\n\t\t\t\t  WHEN 'July'     THEN 6\r\n\t\t\t\t  WHEN 'June'     THEN 7\r\n\t\t\t\t  WHEN 'May'      THEN 8\r\n\t\t\t\t  WHEN 'April'    THEN 9\r\n\t\t\t\t  WHEN 'March'    THEN 10\r\n\t\t\t\t  WHEN 'February' THEN 11\r\n            WHEN 'January'  THEN 12\r\n        END AS MonthOrder\r\n    FROM iumltn_manichuder\r\n) AS m\r\nORDER BY m.MonthOrder; ";

                    }
                    else
                    {
                        qry = "\r\nSELECT Created_Month\r\nFROM (\r\n    SELECT DISTINCT \r\n        Created_Month,\r\n        CASE Created_Month\r\n            WHEN 'December' THEN 1\r\n\t\t\t  WHEN 'November'  THEN 2\r\n\t\t\t   WHEN 'October'   THEN 3\r\n\t\t\t    WHEN 'September' THEN 4\r\n\t\t\t\t WHEN 'August'   THEN 5\r\n\t\t\t\t  WHEN 'July'     THEN 6\r\n\t\t\t\t  WHEN 'June'     THEN 7\r\n\t\t\t\t  WHEN 'May'      THEN 8\r\n\t\t\t\t  WHEN 'April'    THEN 9\r\n\t\t\t\t  WHEN 'March'    THEN 10\r\n\t\t\t\t  WHEN 'February' THEN 11\r\n            WHEN 'January'  THEN 12\r\n        END AS MonthOrder\r\n    FROM iumltn_manichuder\r\n where  created_year='" + _year + "' ) AS m\r\nORDER BY m.MonthOrder; ";
                    }

                    con.Open();
                    SqlDataAdapter _adapter = new SqlDataAdapter(qry, con);
                    _adapter.Fill(_dt);
                    _adapter.Dispose();
                    con.Close();
                    if (_dt.Rows.Count > 0)
                    {
                        ddl_month.DataSource = _dt;

                        ddl_month.DataTextField = "Created_Month";
                        ddl_month.DataValueField = "Created_Month";
                        //ddl_month.SelectedValue = monthname;
                        ddl_month.DataBind();

                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        protected void ddl_year_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            if (ddl_year.SelectedValue == null || ddl_year.SelectedValue == "")
            {

            }
            else
            {
                BindMonth(ddl_year.Text.Trim());
                Bind_grd_PDFList(ddl_year.Text.Trim()); ;
            }



        }
        protected void ddl_month_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            Bind_grd_PDFList(ddl_month.Text.Trim()); ;
        }
        public String LoadLatestPage()
        {

            DataTable _dt = new DataTable();

            string _url = "";
            #region SQL Connection

            String _Query = "";
            _Query = "exec STP_GetLatestURL";
            SqlConnection _sqlCon = new SqlConnection(_IUMLCon);
            _sqlCon.Open();
            SqlCommand _sqlCom = new SqlCommand(_Query, _sqlCon);
            _sqlCom.CommandType = CommandType.Text;
            SqlDataAdapter _sqlAdapter = new SqlDataAdapter(_sqlCom);
            _sqlAdapter.Fill(_dt);
            _dt.AcceptChanges();
            if (_dt.Rows.Count > 0)
            {
                _url = _dt.Rows[0][0].ToString();
            }
            _sqlCon.Close();

            #endregion


            return _url;
        }

        #region

        public void Bind_grd_PDFList(string filter)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_IUMLCon))
                {
                    DataTable _dt = new DataTable();
                    ViewState["PDFList"] = _dt;
                    string qry = "";
                    if (filter != null)
                    {
                        qry = "SELECT id,Created_Year,FileName,cast (( cast(dayofmonth as varchar)+'-'+Created_Month+'-'+ cast(Created_Year as varchar)) as varchar) AS AsofDate FROM iumltn_manichuder where Created_Year ='" + ddl_year.Text.Trim() + "'  and Created_Month  = '" + ddl_month.Text.Trim() + "'   order by Created_Date desc ";
                        //qry = "SELECT id,Created_Year,FileName,convert(vaRCHAR(10),Created_Date,103) AS AsofDate FROM iumltn_manichuder where FileName like '%" + filter+ "%'  or Created_Date like '%"+filter+ "%'   order by Created_Date desc ";
                    }
                    else
                    {
                        qry = "SELECT id,Created_Year,FileName,cast (( cast(dayofmonth as varchar)+'-'+Created_Month+'-'+ cast(Created_Year as varchar)) as varchar) AS AsofDate FROM iumltn_manichuder where Created_Year ='" + ddl_year.Text.Trim() + "'  and Created_Month  = '" + ddl_month.Text.Trim() + "'   order by Created_Date desc ";
                        //qry = "SELECT id,Created_Year,FileName,convert(vaRCHAR(10),Created_Date,103) AS AsofDate FROM iumltn_manichuder order by Created_Date desc ";
                    }

                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        con.Open();
                        SqlDataAdapter _adapter = new SqlDataAdapter(cmd);
                        _adapter.Fill(_dt);
                        _adapter.Dispose();
                        if (_dt.Rows.Count > 0)
                        {
                            ViewState["PDFList"] = _dt;
                        }


                        con.Close();
                    }
                    Bind_grdP_PTCPDFList_Design();
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            grd_PDFList.PageIndex = 0; // reset to first page
            Bind_grd_PDFList("");
        }
        protected void grd_PDFList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd_PDFList.PageIndex = e.NewPageIndex;
            Bind_grd_PDFList("");
        }
        protected void Bind_grdP_PTCPDFList_Design()
        {
            if (ViewState["PDFList"] != null)
            {
                grd_PDFList.Attributes.Add("data-page-size", "10");
                grd_PDFList.Attributes.Add("data-filter", "#txtfilter_grd_PDFList");
                grd_PDFList.Attributes.Add("data-filter-text-only", "true");
                grd_PDFList.DataSource = (DataTable)ViewState["PDFList"];
                grd_PDFList.DataBind();

                if (grd_PDFList.HeaderRow != null)
                {
                    if (grd_PDFList.HeaderRow.Cells.Count > 0)
                    {
                        grd_PDFList.HeaderRow.Cells[1].Attributes["data-class"] = "expand";
                        grd_PDFList.HeaderRow.Cells[3].Attributes["data-hide"] = "expand";
                        grd_PDFList.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";
                        //grd_PDFList.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
                        grd_PDFList.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";

                        //grd_PDFList.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";
                    }
                    grd_PDFList.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                //grd_PDFList.FooterRow.TableSection = TableRowSection.TableFooter;

                grd_PDFList.HeaderRow.TableSection = TableRowSection.TableHeader;
                if (grd_PDFList.TopPagerRow != null)
                {
                    grd_PDFList.TopPagerRow.TableSection = TableRowSection.TableHeader;
                }
                if (grd_PDFList.BottomPagerRow != null)
                {
                    grd_PDFList.BottomPagerRow.TableSection = TableRowSection.TableFooter;
                }
            }
        }
        protected void grd_PDFList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                lblMessage.Text = "";
                if (e.CommandName == "Select")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = grd_PDFList.Rows[rowIndex];
                    Label lbl_ID = (row.Cells[0].FindControl("lblID") as Label);
                    Session["ID"] = lbl_ID.Text;
                    DataTable dt = new DataTable();
                    using (SqlConnection con = new SqlConnection(_IUMLCon))
                    {

                        using (SqlCommand cmd = new SqlCommand("STP_GetPDFPath", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id", lbl_ID.Text);
                            con.Open();
                            SqlDataAdapter _adapter = new SqlDataAdapter(cmd);
                            _adapter.Fill(dt);
                            con.Close();
                        }
                        if (dt.Rows.Count > 0)
                        {
                            string filename = dt.Rows[0][0].ToString();
                            string Desdaypdffullpath = Path.Combine(Server.MapPath("~/manichudar/" + filename));
                            if (File.Exists(Path.Combine(Desdaypdffullpath)))
                            {
                                Session["filepath"] = Desdaypdffullpath;
                                Response.Redirect("PDFView.aspx");
                                string FilePath = Desdaypdffullpath;
                                WebClient User = new WebClient();
                                Byte[] FileBuffer = User.DownloadData(FilePath);
                                if (FileBuffer != null)
                                {
                                    Response.ContentType = "application/pdf";
                                    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                                    Response.BinaryWrite(FileBuffer);
                                }
                            }
                            else
                            {
                                lblMessage.Text = "File not found !.";
                                lblMessage.ForeColor = Color.Red;
                            }
                            //string[] daypdf = dt.Rows[0][0].ToString().Split('\\');
                            //string Desdaypdffullpath = Path.Combine(Server.MapPath("~/manichudar/" + daypdf[1].ToString() + "/" + daypdf[2].ToString() + "/" + daypdf[3].ToString() + "/" + daypdf[4].ToString()));
                            //if (File.Exists(Path.Combine(Desdaypdffullpath)))
                            //{
                            //    Session["filepath"] = Desdaypdffullpath;
                            //    Response.Redirect("PDFView.aspx");
                            //    string FilePath = Desdaypdffullpath;
                            //    WebClient User = new WebClient();
                            //    Byte[] FileBuffer = User.DownloadData(FilePath);
                            //    if (FileBuffer != null)
                            //    {
                            //        Response.ContentType = "application/pdf";
                            //        Response.AddHeader("content-length", FileBuffer.Length.ToString());
                            //        Response.BinaryWrite(FileBuffer);
                            //    }
                            //}
                            //else
                            //{
                            //    lblMessage.Text = "File not found !.";
                            //    lblMessage.ForeColor= Color.Red;
                            //}

                        }

                    }

                }
                if (e.CommandName == "download")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = grd_PDFList.Rows[rowIndex];
                    Label lbl_ID = (row.Cells[0].FindControl("lblID") as Label);
                    Session["ID"] = lbl_ID.Text;
                    DataTable dt = new DataTable();
                    using (SqlConnection con = new SqlConnection(_IUMLCon))
                    {

                        using (SqlCommand cmd = new SqlCommand("STP_GetPDFPath", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id", lbl_ID.Text);
                            con.Open();
                            SqlDataAdapter _adapter = new SqlDataAdapter(cmd);
                            _adapter.Fill(dt);
                            con.Close();
                        }
                        if (dt.Rows.Count > 0)
                        {
                            string filename = dt.Rows[0][0].ToString();
                            string Desdaypdffullpath = Path.Combine(Server.MapPath("~/manichudar/" + filename));
                            //string[] daypdf = dt.Rows[0][0].ToString().Split('\\');
                            //string Desdaypdffullpath = Path.Combine(Server.MapPath("~/manichudar/" + daypdf[1].ToString() + "/" + daypdf[2].ToString() + "/" + daypdf[3].ToString() + "/" + daypdf[4].ToString()));
                            if (File.Exists(Path.Combine(Desdaypdffullpath)))
                            {
                                byte[] bytes = File.ReadAllBytes(Desdaypdffullpath);

                                Response.ClearHeaders();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.Clear();
                                Response.ContentType = "application/pdf";
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                                Response.BinaryWrite(bytes);
                                //Response.Flush();
                                Response.End();
                            }
                            else
                            {

                                lblMessage.Text = "File not found !.";
                                lblMessage.ForeColor = Color.Red;
                            }

                        }

                    }

                }
                Bind_grd_PDFList("");

            }
            catch (Exception ex)
            {

            }

        }
        protected void grd_PDFList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {


                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //LinkButton btn = (LinkButton)e.Row.FindControl("lnkdownload");
                    //ScriptManager.GetCurrent(this).RegisterPostBackControl(btn);
                }



            }

            catch (Exception ex)
            {

            }
        }
        protected void grd_PDFList_RowCreate(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int colSpan = e.Row.Cells.Count;

                for (int i = (e.Row.Cells.Count - 1); i >= 1; i -= 1)
                {
                    e.Row.Cells.RemoveAt(i);
                    e.Row.Cells[0].ColumnSpan = colSpan;
                }

                e.Row.Cells[0].Controls.Add(new LiteralControl("<div class=\"pagination pagination-centered\"></div>"));
            }
        }


        #endregion
        #region IUML

        public class Iuml_month
        {

            public string Monthname { get; set; }

            public Iuml_month(string _Monthname)
            {

                Monthname = _Monthname;

            }
        }

        //public class Iuml_DaysOfmonth
        //{
        //    public string fullpath { get; set; }
        //    public string PDFfile { get; set; }
        //        public Iuml_DaysOfmonth(string _fullpath, string _PDFfile)
        //        {

        //            fullpath = _fullpath;
        //            PDFfile = _PDFfile;

        //        }

        //}

        public class Iuml_DaysOfmonth
        {
            public string fullpath { get; set; }
            public string PDFfile { get; set; }
            public string Days { get; set; }

            public string createddate { get; set; }


        }

        [WebMethod]
        public static String Check_Login(String _UserName, String _Password)
        {
            DataTable _dt = new DataTable();
            string _IUMLCon = Convert.ToString(ConfigurationManager.ConnectionStrings["IUMLCon"]);

            #region SQL Connection

            String _Query = "";
            _Query = "SELECT * FROM Login_Details WHERE UserName = '" + Convert.ToString(_UserName) + "' AND UserPassword = '" + Convert.ToString(_Password) + "' ORDER BY UserName ASC";
            SqlConnection _sqlCon = new SqlConnection(_IUMLCon);
            _sqlCon.Open();
            SqlCommand _sqlCom = new SqlCommand(_Query, _sqlCon);
            _sqlCom.CommandType = CommandType.Text;
            SqlDataAdapter _sqlAdapter = new SqlDataAdapter(_sqlCom);
            _sqlAdapter.Fill(_dt);
            _dt.AcceptChanges();
            _sqlCon.Close();

            #endregion

            String _JsonString = JsonConvert.SerializeObject(_dt);
            return _JsonString;
        }
        [WebMethod]
        public static String GetPDFFileUrl(int _year, string _month)
        {
            string _IUMLCon = Convert.ToString(ConfigurationManager.ConnectionStrings["IUMLCon"]);
            DataTable _dt = new DataTable();
            String _JsonString = string.Empty;
            Dictionary<string, object> data = new Dictionary<string, object>();
            List<Iuml_DaysOfmonth> _lst = new List<Iuml_DaysOfmonth>();
            string _concate = string.Empty;
            #region SQL Connection

            String _Query = "";
            _Query = "STP_GetPDFPath";
            SqlConnection _sqlCon = new SqlConnection(_IUMLCon);
            _sqlCon.Open();

            using (SqlCommand cmd = new SqlCommand(_Query, _sqlCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@YEAR", SqlDbType.VarChar).Value = _year;
                cmd.Parameters.Add("@MONTH", SqlDbType.VarChar).Value = _month;

                SqlDataAdapter _sqlAdapter = new SqlDataAdapter(cmd);
                _sqlAdapter.Fill(_dt);
                _dt.AcceptChanges();
            }

            if (_dt.Rows.Count > 0)
            {


                foreach (DataRow _row in _dt.Rows)
                {
                    Iuml_DaysOfmonth _Iuml_DaysOfmonth = new Iuml_DaysOfmonth();
                    _Iuml_DaysOfmonth.Days = _row["days"].ToString();
                    _Iuml_DaysOfmonth.fullpath = _row["fullpath"].ToString();
                    _Iuml_DaysOfmonth.PDFfile = _row["pdffile"].ToString();
                    _Iuml_DaysOfmonth.createddate = _row["createddate"].ToString();
                    // data.Add(_row["days"].ToString(), new Iuml_DaysOfmonth(_row["fullpath"].ToString(),_row["pdffile"].ToString()));
                    _lst.Add(_Iuml_DaysOfmonth);

                }

                _JsonString = "[" + JsonConvert.SerializeObject(_lst) + "]";


            }
            _sqlCon.Close();

            #endregion
            return _JsonString;
        }

        [WebMethod]
        public static string GetTreeview()
        {
            DataTable _dt = new DataTable();
            jsonconvert _jsonconvert = new jsonconvert();
            string _IUMLCon = Convert.ToString(ConfigurationManager.ConnectionStrings["IUMLCon"]);
            String _JsonString = string.Empty;
            Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> data1 = new Dictionary<string, List<string>>();
            string _concate = "";
            #region SQL Connection

            String _Query = "";
            _Query = "exec STP_BindTreeView";
            SqlConnection _sqlCon = new SqlConnection(_IUMLCon);
            _sqlCon.Open();
            SqlCommand _sqlCom = new SqlCommand(_Query, _sqlCon);
            _sqlCom.CommandType = CommandType.Text;
            SqlDataAdapter _sqlAdapter = new SqlDataAdapter(_sqlCom);
            List<string> lstyear = new List<string>();
            List<string> lstmont = new List<string>();
            _sqlAdapter.Fill(_dt);
            _dt.AcceptChanges();
            if (_dt.Rows.Count > 0)
            {
                int i = 0;

                foreach (DataRow _row in _dt.Rows)
                {

                    if (_row["manichuder_Year"].ToString() != "")
                    {
                        if (_concate.Length > 0)
                        {

                            data.Add("months", lstmont);
                            i++;
                            _JsonString = _JsonString + _jsonconvert.WriteDictionaryAsJson_v2(data) + ",";
                        }



                        _concate = "";

                        data = new Dictionary<string, List<string>>();
                        lstyear = new List<string>();
                        lstmont = new List<string>();

                        lstyear.Add(_row["manichuder_Year"].ToString());
                        data.Add("year", lstyear);
                        //data.Add("title", _row["manichuder_Year"].ToString());

                    }
                    lstmont.Add(_row["manichuder_MonthName"].ToString());
                    _concate = string.Join(",", '"' + _row["manichuder_MonthName"].ToString() + '"') + _concate;


                }
                data.Add("months", lstmont);

                _JsonString = _JsonString + _jsonconvert.WriteDictionaryAsJson_v2(data) + ",";

            }
            _sqlCon.Close();

            #endregion


            return "[" + _JsonString.Substring(0, _JsonString.Length - 1) + "]";
        }
        public class jsonconvert
        {
            public string WriteDictionaryAsJson_v2(Dictionary<string, List<string>> myDict)
            {
                string str_json = "";
                DataContractJsonSerializerSettings setting =
                    new DataContractJsonSerializerSettings()
                    {
                        UseSimpleDictionaryFormat = true
                    };

                DataContractJsonSerializer js =
                    new DataContractJsonSerializer(typeof(Dictionary<string, List<string>>), setting);

                using (MemoryStream ms = new MemoryStream())
                {
                    // Serializer the object to the stream.  
                    js.WriteObject(ms, myDict);
                    str_json = Encoding.Default.GetString(ms.ToArray());

                }
                return str_json;
            }
            #endregion
        }


        [WebMethod]
        public String Get_ImageList_by_ImageType_Ongoing(String _ImageType, String _UserType)
        {


            #region Sql Connection

            DataTable _dt = new DataTable();
            if (!String.IsNullOrEmpty(Convert.ToString(_ImageType).Trim().TrimEnd().TrimStart()))
            {

                String _ConnectionString = Convert.ToString(ConfigurationManager.ConnectionStrings["IUMLCon"]);

                String _ImageTypeIN = "";
                List<String> _imageTypeList = new List<String>();
                _imageTypeList = Convert.ToString(_ImageType).Split(',').ToList();
                _ImageTypeIN = Convert.ToString(String.Join("','", _imageTypeList));



                String _Query = "";
                //_Query = "SELECT " + _Top + " * FROM Image_List WHERE Image_Type IN ('" + _ImageTypeIN + "') ORDER BY CreatedOn DESC, ID DESC";
                _Query = "SELECT  * FROM Administrator_Image_List WHERE Image_Type IN ('" + _ImageTypeIN + "') ORDER BY CreatedOn DESC, ID DESC";
                SqlConnection _sqlCon = new SqlConnection(_ConnectionString);
                _sqlCon.Open();
                SqlCommand _sqlCom = new SqlCommand(_Query, _sqlCon);
                _sqlCom.CommandType = CommandType.Text;
                SqlDataAdapter _sqlAdapter = new SqlDataAdapter(_sqlCom);
                _sqlAdapter.Fill(_dt);
                _dt.AcceptChanges();
                _sqlCon.Close();
            }
            String _JsonString = JsonConvert.SerializeObject(_dt);
            return _JsonString;

            #endregion
        }


    }
}