<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PDFView.aspx.cs" Inherits="IUML.PDFView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
         <div>
        <table id="Table1" width="100%">
            <tr>
                <td valign="middle">
                    <table cellpadding="0" cellspacing="0" width="100%" style="border: 2px Solid #80C6FF;">
                        <tr>
                            <td valign="middle" style="border-bottom: 2px Solid #80C6FF; padding-bottom: 10px;">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <table cellpadding="0" cellspacing="0" width="100%" style="border: 2px Solid #80C6FF;">
                                                <tr>
                                                    <td>
                                                        <asp:PlaceHolder ID="iframeDiv" runat="server" />
                                                        <iframe id="pdfFrame" runat="server" width="100%" height="800px"></iframe>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </td>
                          
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
