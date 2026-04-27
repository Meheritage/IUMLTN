using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IUML.admin
{
    public partial class ImgUpload : System.Web.UI.Page
    {
        string _IUMLCon = Convert.ToString(ConfigurationManager.ConnectionStrings["IUMLCon"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Bind_ImageType();
                    Bind_grd_ImageListList();
                    lblMessage.Text = "";

                }
                Page.Form.Attributes.Add("enctype", "multipart/form-data");

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "temp", "<script language='javascript'>OpenAlertPopup('" + ex.Message + "','danger');</script>", false);

            }
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                string ext = System.IO.Path.GetExtension(fileUpload.FileName).ToLower();
                string[] allowedExt = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                if (ddl_imagetype.SelectedValue == "0")
                {
                    lblMessage.Text = "Please select the Image Type.";
                    lblMessage.ForeColor = Color.Red;
                    return;
                }
                if (string.IsNullOrEmpty(txt_position.Text))
                {
                    lblMessage.Text = "Please Enter the Position.";
                    lblMessage.ForeColor = Color.Red;
                    return;
                }
                if (string.IsNullOrEmpty(txt_place.Text))
                {
                    lblMessage.Text = "Please Enter the City/Place.";
                    lblMessage.ForeColor = Color.Red;
                    return;
                }
                if (ddl_imageorder.SelectedValue == "0")
                {
                    lblMessage.Text = "Please select the Order.";
                    lblMessage.ForeColor = Color.Red;
                    return;
                }
                if (!fileUpload.HasFile)
                {
                    lblMessage.Text = "Please select a Image file!";
                    lblMessage.ForeColor = Color.Red;
                    Bind_grd_ImageListList();
                    return;
                }

                if (!allowedExt.Contains(ext))
                {
                    lblMessage.Text = "Only image files are allowed.!";
                    lblMessage.ForeColor = Color.Red;
                    Bind_grd_ImageListList();
                    return;
                }

                string fileName = fileUpload.FileName;
                string _imageSubType = string.Empty;
                if (ddl_imageSubtype.SelectedValue == "Select")
                {
                    _imageSubType = "0";
                }
                else
                {
                    _imageSubType = ddl_imageSubtype.SelectedValue;
                }
                // Read file bytes
                //string _file = Path.Combine(Server.MapPath("~/manichudar/PDFFile/"), fileName);
                //fileUpload.SaveAs(_file);

                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(_IUMLCon))
                {

                    using (SqlCommand cmd = new SqlCommand("InsertImage", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Image_TypeId", ddl_imagetype.SelectedValue);
                        cmd.Parameters.AddWithValue("@Image_SubTypeId", _imageSubType);
                        cmd.Parameters.AddWithValue("@Image_URL", fileName);
                        cmd.Parameters.AddWithValue("@Name", txt_name.Text.Trim());
                        cmd.Parameters.AddWithValue("@Image_Position", txt_position.Text.Trim());
                        cmd.Parameters.AddWithValue("@Image_City", txt_place.Text.Trim());
                        cmd.Parameters.AddWithValue("@Image_Order", ddl_imageorder.SelectedValue);
                        con.Open();
                        SqlDataAdapter _adapter = new SqlDataAdapter(cmd);
                        _adapter.Fill(dt);
                        con.Close();
                        if (dt.Rows.Count > 0)
                        {

                            string[] daypdf = dt.Rows[0][0].ToString().Split('\\');
                            bool isexists = System.IO.Directory.Exists(Server.MapPath("~/assets/" + daypdf[1]));
                            if (!isexists)
                                System.IO.Directory.CreateDirectory(Server.MapPath("~/assets/" + daypdf[1]));
                            string fullpath = Server.MapPath("~/assets/" + daypdf[1] + "/" + fileName);
                            fileUpload.SaveAs(fullpath);
                            lblMessage.Text = "Successfully uploaded !";
                            lblMessage.ForeColor = Color.Green;


                        }

                    }
                }
                Bind_grd_ImageListList();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.ForeColor = Color.Red;


            }
        }

        #region
        protected void grd_ImageListList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd_ImageListList.PageIndex = e.NewPageIndex;
            Bind_grd_ImageListList();
        }
        public void Bind_grd_ImageListList()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_IUMLCon))
                {
                    DataTable _dt = new DataTable();
                    ViewState["PDFList"] = _dt;
                    string Query = "";
                    string _imageSubType = string.Empty;
                    con.Open();
                    if (ddl_imagetypeforearch.SelectedValue.ToString().Trim() == "Select")
                    {
                        Query = "select l.id,t.Image_TypeName,l.image_url,l.Name, l.Image_Position,l.Image_City from Image_List l inner join Image_Type t on l.Image_TypeId=t.ID order by CreatedOn desc  ";
                    }
                    else if (ddl_imageSubtypeforsearch.SelectedValue.ToString().Trim() == "Select")
                    {
                        Query = "select l.id,t.Image_TypeName,l.image_url,l.Name, l.Image_Position,l.Image_City from Image_List l inner join Image_Type t on l.Image_TypeId=t.ID where l.Image_TypeId= " + ddl_imagetypeforearch.SelectedValue.ToString().Trim() + "   order by CreatedOn desc  ";
                    }
                    else
                    {
                        Query = "select l.id,t.Image_TypeName,l.image_url,l.Name, l.Image_Position,l.Image_City from Image_List l inner join Image_Type t on l.Image_TypeId=t.ID where l.Image_TypeId= " + ddl_imagetypeforearch.SelectedValue.ToString().Trim() + " and l.Image_SubTypeId= " + ddl_imageSubtypeforsearch.SelectedValue.ToString().Trim() + "   order by CreatedOn desc  ";

                    }
                    using (SqlCommand cmd = new SqlCommand(Query, con))
                    {

                        SqlDataAdapter _adapter = new SqlDataAdapter(cmd);
                        _adapter.Fill(_dt);
                        if (_dt.Rows.Count > 0)
                        {
                            ViewState["PDFList"] = _dt;
                            lbl_TotalCount.Text = "Total :" + _dt.Rows.Count.ToString();
                        }

                    }
                    con.Close();
                    Bind_grdP_PTCPDFList_Design();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.ForeColor = Color.Red;
            }
        }
        protected void grd_ImageListList_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            //Finding the controls from Gridview for the row which is going to update
           
            Bind_grd_ImageListList();
        }
        protected void grd_ImageListList_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            //NewEditIndex property used to determine the index of the row being edited.
            grd_ImageListList.EditIndex = e.NewEditIndex;
            Bind_grd_ImageListList(); 
        }
        protected void grd_ImageListList_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            //Setting the EditIndex property to -1 to cancel the Edit mode in Gridview
            grd_ImageListList.EditIndex = -1;
            Bind_grd_ImageListList();
        }
        protected void ddl_imagetypeforearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ddl_imagetypeforearch"] = ddl_imagetypeforearch.SelectedValue.Trim();
            Bind_ImageSubTypeSearch(ddl_imagetypeforearch.SelectedValue.Trim());
            Bind_grd_ImageListList();
        }
        protected void ddl_imageSubtypeforsearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ddl_imageSubtypeforsearch"] = ddl_imageSubtypeforsearch.SelectedValue.Trim();
            Bind_grd_ImageListList();
        }
        void Bind_ImageSubType(string _Type)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_IUMLCon))
                {
                    DataTable _dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand("select * from Image_SubType where image_typeid=" + _Type + "", con))
                    {
                        con.Open();
                        SqlDataAdapter _adapter = new SqlDataAdapter(cmd);
                        _adapter.Fill(_dt);
                        if (_dt.Rows.Count > 0)
                        {
                            ddl_imageSubtype.DataSource = _dt;
                            ddl_imageSubtype.DataBind();


                            ddl_imageSubtype.DataValueField = "ID";
                            ddl_imageSubtype.DataTextField = "image_subTypename";
                            ddl_imageSubtype.DataBind();
                            ddl_imageSubtype.Items.Insert(0, "Select");

                        }
                        else
                        {
                            ddl_imageSubtype.DataSource = _dt;
                            ddl_imageSubtype.DataBind();
                            ddl_imageSubtype.DataBind();
                            ddl_imageSubtype.Items.Insert(0, "Select");
                        }


                        con.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.ForeColor = Color.Red;
            }
        }
        void Bind_ImageSubTypeSearch(string _Type)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_IUMLCon))
                {
                    DataTable _dtsubtype = new DataTable();
                    using (SqlCommand cmd = new SqlCommand("select * from Image_SubType where image_typeid=" + _Type + "", con))
                    {
                        con.Open();
                        SqlDataAdapter _adapter = new SqlDataAdapter(cmd);
                        _adapter.Fill(_dtsubtype);
                        if (_dtsubtype.Rows.Count > 0)
                        {
                            ddl_imageSubtypeforsearch.DataSource = _dtsubtype;
                            ddl_imageSubtypeforsearch.DataBind();


                            ddl_imageSubtypeforsearch.DataValueField = "ID";
                            ddl_imageSubtypeforsearch.DataTextField = "image_subTypename";
                            ddl_imageSubtypeforsearch.DataBind();
                            ddl_imageSubtypeforsearch.Items.Insert(0, "Select");

                        }
                        else
                        {
                            ddl_imageSubtypeforsearch.DataSource = _dtsubtype;
                            ddl_imageSubtypeforsearch.DataBind();

                            ddl_imageSubtypeforsearch.Items.Insert(0, "Select");
                        }


                        con.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.ForeColor = Color.Red;
            }
        }
        protected void ddl_imagetype_SelectedIndexChanged(object sender, EventArgs e)
        {
            string img_type = ddl_imagetype.SelectedValue;
            Bind_ImageSubType(img_type);
        }

        public void Bind_ImageType()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_IUMLCon))
                {
                    DataTable _dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand("select ID,image_Typename from Image_Type", con))
                    {
                        con.Open();
                        SqlDataAdapter _adapter = new SqlDataAdapter(cmd);
                        _adapter.Fill(_dt);
                        if (_dt.Rows.Count > 0)
                        {
                            ddl_imagetype.DataSource = _dt;
                            ddl_imagetype.DataBind();


                            ddl_imagetype.DataValueField = "ID";
                            ddl_imagetype.DataTextField = "image_Typename";
                            ddl_imagetype.DataBind();
                            ddl_imagetype.Items.Insert(0, "Select");

                            ddl_imagetypeforearch.DataSource = _dt;
                            ddl_imagetypeforearch.DataBind();
                            ddl_imagetypeforearch.DataValueField = "ID";
                            ddl_imagetypeforearch.DataTextField = "image_Typename";
                            ddl_imagetypeforearch.DataBind();
                            ddl_imagetypeforearch.Items.Insert(0, "Select");
                        }


                        con.Close();
                    }

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
                grd_ImageListList.Attributes.Add("data-page-size", "10");
                grd_ImageListList.Attributes.Add("data-filter", "#txtfilter_grd_ImageListList");
                grd_ImageListList.Attributes.Add("data-filter-text-only", "true");
                grd_ImageListList.DataSource = (DataTable)ViewState["PDFList"];
                grd_ImageListList.DataBind();

                if (grd_ImageListList.HeaderRow != null)
                {
                    if (grd_ImageListList.HeaderRow.Cells.Count > 0)
                    {
                        grd_ImageListList.HeaderRow.Cells[1].Attributes["data-class"] = "expand";
                        grd_ImageListList.HeaderRow.Cells[3].Attributes["data-hide"] = "expand";
                        grd_ImageListList.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";
                        //grd_ImageListList.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
                        grd_ImageListList.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";

                        //grd_ImageListList.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";
                    }
                    grd_ImageListList.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                //grd_ImageListList.FooterRow.TableSection = TableRowSection.TableFooter;

                grd_ImageListList.HeaderRow.TableSection = TableRowSection.TableHeader;
                if (grd_ImageListList.TopPagerRow != null)
                {
                    grd_ImageListList.TopPagerRow.TableSection = TableRowSection.TableHeader;
                }
                if (grd_ImageListList.BottomPagerRow != null)
                {
                    grd_ImageListList.BottomPagerRow.TableSection = TableRowSection.TableFooter;
                }
            }
        }
        protected void grd_ImageListList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "Select")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = grd_ImageListList.Rows[rowIndex];
                    Label lbl_ID = (row.Cells[0].FindControl("lblID") as Label);
                    Session["ID"] = lbl_ID.Text;
                    DataTable dt = new DataTable();
                    using (SqlConnection con = new SqlConnection(_IUMLCon))
                    {

                        using (SqlCommand cmd = new SqlCommand("DeleteImage", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Id", lbl_ID.Text);
                            con.Open();
                            SqlDataAdapter _adapter = new SqlDataAdapter(cmd);
                            _adapter.Fill(dt);
                            con.Close();
                        }
                        if (dt.Rows.Count > 0)
                        {
                            string[] daypdf = dt.Rows[0][0].ToString().Split('\\');



                            string Desdaypdffullpath = Path.Combine(Server.MapPath("~/" + dt.Rows[0][0].ToString().Trim()));
                            File.Delete(Desdaypdffullpath);

                        }
                        lblMessage.Text = "Image file Deleted !.";
                        lblMessage.ForeColor = Color.Green;
                    }

                }
                else if (e.CommandName == "Update")
                {
                    if (ddl_imagetypeforearch.SelectedValue.ToString().Trim() == "Select")
                    {
                        lblMessage.Text = "Please select the Type from geid !";
                        lblMessage.ForeColor = Color.Red;
                        return;
                    }
                        int rowIndex = Convert.ToInt32(e.CommandArgument);
                    GridViewRow row = grd_ImageListList.Rows[rowIndex];
                    Label id = (row.Cells[0].FindControl("lblID") as Label);
                    TextBox name = (row.Cells[0].FindControl("txt_Name") as TextBox);
                    TextBox city = (row.Cells[0].FindControl("txt_City") as TextBox);
                    TextBox Position = (row.Cells[0].FindControl("txt_Position") as TextBox);
                    FileUpload fu = (FileUpload)row.Cells[0].FindControl("file_image_url");
                    //FileUpload fu = (row.Cells[0].FindControl("file_image_url") as FileUpload);
                    HiddenField hf = (row.Cells[0].FindControl("hfOldFile") as HiddenField);
                    String[] splitfile = hf.Value.Split('\\');
                    string fileName = splitfile[2].Trim(); // default old file

                    if (fu.HasFile)
                    {
                        string Desdaypdffullpath = Path.Combine(Server.MapPath("~/" + hf.Value));
                        File.Delete(Desdaypdffullpath);
                        fileName = Path.GetFileName(fu.FileName);
                        //fu.SaveAs(Server.MapPath("~/Uploads/" + fileName));
                    }

                    using (SqlConnection con = new SqlConnection(_IUMLCon))
                    {
                        using (SqlCommand cmd = new SqlCommand("UpdateImage", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id", id.Text.Trim());
                            cmd.Parameters.AddWithValue("@Image_TypeId", ddl_imagetypeforearch.SelectedItem.Value);
                            cmd.Parameters.AddWithValue("@Image_URL", fileName);
                            cmd.Parameters.AddWithValue("@Name", name.Text.Trim());
                            cmd.Parameters.AddWithValue("@Image_Position", Position.Text.Trim());
                            cmd.Parameters.AddWithValue("@Image_City", city.Text.Trim());
                            DataTable dt = new DataTable();
                            con.Open();
                            SqlDataAdapter _adapter = new SqlDataAdapter(cmd);
                            _adapter.Fill(dt);
                            con.Close();
                            if (dt.Rows.Count > 0)
                            {

                                string[] daypdf = dt.Rows[0][0].ToString().Split('\\');
                                bool isexists = System.IO.Directory.Exists(Server.MapPath("~/assets/" + daypdf[1]));
                                if (!isexists)
                                    System.IO.Directory.CreateDirectory(Server.MapPath("~/assets/" + daypdf[1]));
                                string fullpath = Server.MapPath("~/assets/" + daypdf[1] + "/" + fileName);
                                fu.SaveAs(fullpath);
                                lblMessage.Text = "Successfully updated !";
                                lblMessage.ForeColor = Color.Green;


                            }
                        }
                    }
                    //Setting the EditIndex property to -1 to cancel the Edit mode in Gridview
                    grd_ImageListList.EditIndex = -1;
                    //Call ShowData method for displaying updated data
                }
                Bind_grd_ImageListList();

            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.ForeColor = Color.Red;
            }

        }
      
        protected void grd_ImageListList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {


                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //FileUpload flUpload = e.Row.FindControl("file_image_url") as FileUpload;
                    //ScriptManager.GetCurrent(this).RegisterPostBackControl(flUpload);
                    //if (e.Row.RowState.Equals(DataControlRowState.Edit))
                    //{
                    //    Button btnUpload = e.Row.FindControl("btnUpload") as Button;
                    //    ScriptManager.GetCurrent(this).RegisterPostBackControl(btnUpload);
                    //}
                }



            }

            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.ForeColor = Color.Red;
            }
        }
        protected void grd_ImageListList_RowCreate(object sender, GridViewRowEventArgs e)
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