using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Net.Mime;
namespace IUML.admin
{
    public partial class upload : System.Web.UI.Page
    {
        string _IUMLCon = Convert.ToString(ConfigurationManager.ConnectionStrings["IUMLCon"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtDate.Value = DateTime.Now.ToString("dd-MM-yyyy");
                    BindYear();
                    Bind_grd_PDFList("");

                }


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "temp", "<script language='javascript'>OpenAlertPopup('" + ex.Message + "','danger');</script>", false);

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
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDate.Value))
                {
                    lblMessage.Text = "Please select a date.";
                    lblMessage.ForeColor = Color.Red;
                    return;
                }

                if (!fileUpload.HasFile)
                {
                    lblMessage.Text = "Please select a PDF file!";
                    lblMessage.ForeColor = Color.Red;
                    Bind_grd_PDFList("");
                    return;
                }

                if (Path.GetExtension(fileUpload.FileName).ToLower() != ".pdf")
                {
                    lblMessage.Text = "Only PDF files allowed!";
                    lblMessage.ForeColor = Color.Red;
                    Bind_grd_PDFList("");
                    return;
                }

                string fileName = fileUpload.FileName;
                
                DateTime _currentdate = DateTime.Parse(txtDate.Value); ;
                // Read file bytes
                string _file = Path.Combine(Server.MapPath("~/manichudar/"), fileName);
                //string _file = Path.Combine(Server.MapPath("~/manichudar/PDFFile/"), fileName);
                fileUpload.SaveAs(_file);
               
                DataTable dt = new DataTable();
               
                using (SqlConnection con = new SqlConnection(_IUMLCon))
                {

                    using (SqlCommand cmd = new SqlCommand("InsertPDF", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@filename", fileName);
                        cmd.Parameters.AddWithValue("@AsOnDate", _currentdate);
                      
                        con.Open();
                        SqlDataAdapter _adapter = new SqlDataAdapter(cmd);
                        _adapter.Fill(dt);
                        con.Close();
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][0].ToString().Contains("Alredy") == false)
                            {
                              
                                string[] daypdf = dt.Rows[0][0].ToString().Split('\\');
                                bool existsyear = System.IO.Directory.Exists(Server.MapPath("~/manichudar/"+ daypdf[1]));
                                if (!existsyear)
                                    System.IO.Directory.CreateDirectory(Server.MapPath("~/manichudar/"+ daypdf[1]));
                                bool existsmonth = System.IO.Directory.Exists(Server.MapPath("~/manichudar/" + daypdf[1]+"/"+ daypdf[2]));
                                if (!existsmonth)
                                    System.IO.Directory.CreateDirectory(Server.MapPath("~/manichudar/" + daypdf[1] +"/"+ daypdf[2]));

                                bool existsday = System.IO.Directory.Exists(Server.MapPath("~/manichudar/" + daypdf[1] + "/" + daypdf[2]+"/"+ daypdf[3]));
                                if (!existsday)
                                    System.IO.Directory.CreateDirectory(Server.MapPath("~/manichudar/" + daypdf[1] + "/" + daypdf[2]+"/"+ daypdf[3]));

                                string fullpath= Server.MapPath("~/manichudar/" + daypdf[1] + "/" + daypdf[2] + "/" + daypdf[3]+"/"+fileName);
                                fileUpload.SaveAs(fullpath);
                               

                                lblMessage.Text = "Successfully uploaded !";
                                lblMessage.ForeColor = Color.Green;
                            }
                            else
                            {
                                lblMessage.Text = "Already uploaded PDF for this date!";
                                lblMessage.ForeColor = Color.Red;
                            }

                        }

                    }
                }
                Bind_grd_PDFList("");
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.ForeColor = Color.Red;


            }
        }

        #region
        protected void grd_PDFList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd_PDFList.PageIndex = e.NewPageIndex;
            Bind_grd_PDFList("");
        }
        public void Bind_grd_PDFList(string filter)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_IUMLCon))
                {
                    DataTable _dt = new DataTable();
                    ViewState["PDFList"] = _dt;
                    string Qry = "";
                    if (filter != null)
                    {
                        Qry = "SELECT top 50 id,fulllPath,FileName,Created_Year,Created_Month,convert(vaRCHAR(10),Created_Date,103) AS AsofDate FROM iumltn_manichuder where Created_Year ='" + ddl_year.Text.Trim() + "'  and Created_Month  = '" + ddl_month.Text.Trim() + "'   order by asofdate desc  ";
                    }
                    else
                    {
                        Qry = "SELECT top 50 id,fulllPath,FileName,Created_Year,Created_Month,convert(vaRCHAR(10),Created_Date,103) AS AsofDate FROM iumltn_manichuder where Created_Year ='" + ddl_year.Text.Trim() + "'  and Created_Month  = '" + ddl_month.Text.Trim() + "'  order by asofdate desc  ";
                    }
                        using (SqlCommand cmd = new SqlCommand(Qry, con))
                    {
                        con.Open();
                        SqlDataAdapter _adapter = new SqlDataAdapter(cmd);
                        _adapter.Fill(_dt);
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
                lblMessage.Text = ex.Message;
                lblMessage.ForeColor = Color.Red;
            }
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

                if (e.CommandName == "Select")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = grd_PDFList.Rows[rowIndex];
                    Label lbl_ID = (row.Cells[0].FindControl("lblID") as Label);
                    Session["ID"] = lbl_ID.Text;
                    DataTable dt = new DataTable();
                    using (SqlConnection con = new SqlConnection(_IUMLCon))
                    {

                        using (SqlCommand cmd = new SqlCommand("DeletePDF", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Id", lbl_ID.Text);
                            con.Open();
                            SqlDataAdapter _adapter = new SqlDataAdapter(cmd);
                            _adapter.Fill(dt);
                            con.Close();
                        }
                        if(dt.Rows.Count > 0) 
                        {
                            string[] daypdf = dt.Rows[0][0].ToString().Split('\\');
                            

                          
                            string Desdaypdffullpath = Path.Combine(Server.MapPath("~/manichudar/" + daypdf[1].ToString() + "/" + daypdf[2].ToString() + "/" + daypdf[3].ToString() + "/" + daypdf[4].ToString()));
                            File.Delete(Desdaypdffullpath);

                        }
                        lblMessage.Text = "PDF file Deleted !.";
                        lblMessage.ForeColor = Color.Green;
                    }
                    
                }
                Bind_grd_PDFList("");

            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.ForeColor = Color.Red;
            }

        }
        protected void ddl_month_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            Bind_grd_PDFList(ddl_month.Text.Trim()); ;
        }
        protected void grd_PDFList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {


                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                }



            }

            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.ForeColor = Color.Red;
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
    }
}