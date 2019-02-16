<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            height: 40px;
        }
        .auto-style3 {
            height: 32px;
        }
    </style>
</head>
<body style="font-weight: 700; font-size: medium; font-family: Arial, Helvetica, sans-serif">
    <form id="form1" runat="server">
    <div>
    
        <table class="auto-style1">
            <tr>
                <td class="auto-style2">Upload XML File</td>
                <td class="auto-style2">
        <asp:FileUpload ID="FileUpload1" runat="server" Width="224px" />
        <asp:Button ID="Button1" runat="server" Text="Upload" OnClick="Button1_Click" />
                </td>
            </tr>
            <tr>
                <td class="auto-style3" colspan="2">
                    <asp:Label ID="Label1" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    
    </div>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </form>
</body>
</html>
