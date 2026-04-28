<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="upload.aspx.cs" Inherits="IUML.admin.upload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="shortcut icon" href="../assets/ico/favicon.png">
    <title>IUML-Gallery.</title>
    <!-- Bootstrap core CSS -->

    <link href='../css/fonts.css' rel='stylesheet' type='text/css'>
    <link rel="stylesheet" href="../css/font-awesome.min.css">
    <link href="../dist/css/jasny-bootstrap.min.css" rel="stylesheet">
    <link href="../css/bootstrap.min.css" rel="stylesheet">
    <!-- Custom styles for this template -->
    <link href="../css/navmenu-reveal.css" rel="stylesheet">
    <link href="../css/style2.css" rel="stylesheet">
    <link href="../css/full-slider.css" rel="stylesheet">
    <link href="../css/Icomoon/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/animated-masonry-gallery.css" rel="stylesheet" type="text/css" />
    <link href="../css/lightbox.css" rel="stylesheet" type="text/css" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jquery-footable/0.1.0/css/footable.min.css"
        rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-footable/0.1.0/js/footable.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <script src="https://iumltn.in/frontend\application\views/themes/theme-1/assets/js/jquery.min.js"></script>
    <script src="https://iumltn.in/frontend\application\views/themes/theme-1/assets/js/bootstrap.min.js"></script>
    <script src="https://iumltn.in/frontend\application\views/themes/theme-1/assets/js/plugins.js"></script>
    <script src="https://iumltn.in/frontend\application\views/themes/theme-1/assets/js/custom-scripts.js"></script>
    <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/jquery-footable/0.1.0/css/footable.min.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-footable/0.1.0/js/footable.min.js"></script>

    <style>
        .canvas {
            background-color: white !important;
        }

        .container {
            width: 97% !important;
            margin-left: 0px;
        }

        #gallery-content-center img {
            /*margin-left: 25px !important;*/
        }

        .gallery-content-center-full {
            /*width: 980px !important;*/
        }

        @media (max-width: 1090px) {
            .carousel, .item, .active {
                margin-top: 0px !important;
            }
        }

        @media only screen and (max-width: 768px) {
            #gallery-content-center img {
                width: 250px;
            }
        }


        .pagination-ys {
            /*display: inline-block;*/
            padding-left: 0;
            margin: 20px 0;
            border-radius: 4px;
        }

            .pagination-ys table > tbody > tr > td {
                display: inline;
            }

                .pagination-ys table > tbody > tr > td > a,
                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    color: #dd4814;
                    background-color: #ffffff;
                    border: 1px solid #dddddd;
                    margin-left: -1px;
                }

                .pagination-ys table > tbody > tr > td > span {
                    position: relative;
                    float: left;
                    padding: 8px 12px;
                    line-height: 1.42857143;
                    text-decoration: none;
                    margin-left: -1px;
                    z-index: 2;
                    color: #aea79f;
                    background-color: #f5f5f5;
                    border-color: #dddddd;
                    cursor: default;
                }

                .pagination-ys table > tbody > tr > td:first-child > a,
                .pagination-ys table > tbody > tr > td:first-child > span {
                    margin-left: 0;
                    border-bottom-left-radius: 4px;
                    border-top-left-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td:last-child > a,
                .pagination-ys table > tbody > tr > td:last-child > span {
                    border-bottom-right-radius: 4px;
                    border-top-right-radius: 4px;
                }

                .pagination-ys table > tbody > tr > td > a:hover,
                .pagination-ys table > tbody > tr > td > span:hover,
                .pagination-ys table > tbody > tr > td > a:focus,
                .pagination-ys table > tbody > tr > td > span:focus {
                    color: #97310e;
                    background-color: #eeeeee;
                    border-color: #dddddd;
                }
    </style>
    <script>

        $("#txtDate").datepicker({
            maxDate: 0
        }).on("change", function () {
            var selected = new Date($(this).val());
            var today = new Date();

            // If selected date is in the future, clear or reset it
            if (selected > today) {
                $(this).val("");
            }
        });
    </script>
    <style>
        .topnav {
            background-color: #333;
            overflow: hidden;
        }

            /* Style the links inside the navigation bar */
            .topnav a {
                float: left;
                color: #f2f2f2;
                text-align: center;
                padding: 14px 16px;
                text-decoration: none;
                font-size: 17px;
            }

                /* Change the color of links on hover */
                .topnav a:hover {
                    background-color: #ddd;
                    color: black;
                }

                /* Add a color to the active/current link */
                .topnav a.active {
                    background-color: #04AA6D;
                    color: white;
                }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div class="row">
            <div class="col-md-12" align="right">

                <div class="topnav">
                    <%--<a class="active" href="#home">Home</a>--%>
                    <a href="upload.aspx">PDF Upload</a>
                    <a href="ImgUpload.aspx">Image Upload</a>

                    <right>
                    <button type="button" class="btn" data-toggle="tooltip" data-placement="top" title="Home">
                        <a href="../../index.html"><i class="fa fa-home" style="font-size: 30px; color: green"></i></a>
                    </button>

                </right>
                </div>
            </div>
        </div>
        <div>
            <table cellpadding="0" cellspacing="0" width="100%" style="padding: 15px; border-radius: 15px; border: 2px Solid #80C6FF;">

                <tr>
                    <td valign="middle" style="padding: 0 15px 0 15px;">
                        <div class="row" style="padding: 20px 20px 20px 20px;">

                            <div class="col-lg-12 col-centered">
                                <h2>Upload PDF file</h2>
                                <table width="70%" style="padding: 15px; border-radius: 15px; border: 2px Solid #80C6FF; align-items: center;">
                                    <tr>
                                        <td>
                                            <label>
                                                Select Date</label>
                                            <input type="date" id="txtDate" cssclass="form-control" class="form-control mt-2" runat="server" />
                                        </td>
                                        <td>

                                            <label>
                                                Select File</label>
                                            <asp:FileUpload ID="fileUpload" runat="server" multiple accept=".pdf" />


                                        </td>
                                        <td>

                                            <asp:Button ID="btnUpload" Text="Upload" class="btn btn-primary" runat="server" OnClick="btnUpload_Click" />


                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>




                            </div>



                        </div>
                    </td>
                </tr>

                <tr>
                    <td valign="middle" style="padding: 0 15px 0 15px;">


                        <div class="row" style="padding: 20px 20px 20px 20px;">
                            <asp:UpdatePanel ID="updatepnl" runat="server">
                                <ContentTemplate>
                                    <div class="col-md-12 col-sm-12 col-lg-12">

                                        <table cellpadding="0" cellspacing="0" width="90%" style="padding: 15px; border-radius: 15px; border: 2px Solid #80C6FF;">
                                            <tr>
                                                <td>
                                                    <div class="row">
                                                        <div class="col-md-6 ">
                                                            <label>
                                                                <strong>Select Year  </strong>
                                                            </label>
                                                            <div class="form-group">
                                                                <asp:DropDownList class="form-control " ID="ddl_year" runat="server" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ddl_year_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <input type="hidden" id="hdn_value" runat="server" name="hdn_value" value="" />
                                                        </div>
                                                        <div class="col-md-6 ">
                                                            <label>
                                                                <strong>Select Month</strong>

                                                            </label>
                                                            <div class="form-group">
                                                                <asp:DropDownList class="form-control " ID="ddl_month" runat="server" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ddl_month_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <asp:GridView ID="grd_PDFList" CssClass="footable" runat="server" AutoGenerateColumns="false"
                                        PagerSettings-Position="Bottom" PagerSettings-Mode="NumericFirstLast"  DataKeyNames="AsofDate" OnRowCommand="grd_PDFList_RowCommand" ShowFooter="true" ShowHeader="true"
                                        Width="100%" OnRowCreated="grd_PDFList_RowCreate" OnRowDataBound="grd_PDFList_RowDataBound"
                                       
                                        
                                        EmptyDataText="No Records Found...!" ShowHeaderWhenEmpty="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="FileName" HeaderText="FileName" />
                                            <asp:BoundField DataField="Created_Year" HeaderText="Year" />
                                            <asp:BoundField DataField="Created_Month" HeaderText="Month" />
                                            <asp:BoundField DataField="AsofDate" HeaderText="Date" />


                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" class="btn btn-mini" CommandName="Select"
                                                        CommandArgument="<%# Container.DataItemIndex %>">
     <i class="fa fa-trash" style="font-size: 20px; color: red"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
