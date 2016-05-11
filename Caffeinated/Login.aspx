<%@ Page Title="Login" Language="C#" MasterPageFile="~/Caffeinated.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 211px;
        }
        .auto-style2 {
            width: 384px;
        }
        .auto-style3 {
            margin-left: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div id="loginForm">
        <asp:Login ID="caffeineLogin" runat="server" OnAuthenticate="LoginAuthenticate" OnLoggedIn="Login_LoggedIn" CssClass="caffeineLogin" UserNameLabelText="Username:" Width="355px">
            <LayoutTemplate>
                <table cellpadding="1" cellspacing="0" style="border-collapse:collapse;">
                    <tr>
                        <td>
                            <table cellpadding="0" class="auto-style2">
                                <tr>
                                    <td align="center" colspan="2" style="font-size:XX-Large;font-weight:normal;">Log In</td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Username:</asp:Label>
                                    </td>
                                    <td class="auto-style1">
                                        <asp:TextBox ID="UserName" runat="server" CssClass="auto-style3" Width="191px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="caffeineLogin">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                    </td>
                                    <td class="auto-style1">
                                        <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="191px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="caffeineLogin">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="color:Red;">
                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="caffeineLogin" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
            <TitleTextStyle Font-Bold="False" Font-Size="XX-Large" />
        </asp:Login>
        <p>Don't have an account?  <a href="register.aspx">Register now!</a></p>
    </div>
</asp:Content>


