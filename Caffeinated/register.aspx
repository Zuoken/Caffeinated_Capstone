<%@ Page Title="Registration" Language="C#" MasterPageFile="~/caffeinated.master" AutoEventWireup="true" CodeFile="register.aspx.cs" Inherits="register" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 165px;
        }

        .auto-style2 {
            margin-left: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <asp:Panel ID="panelRegistration" runat="server" CssClass="registrationForm" DefaultButton="buttonSubmit" Width="514px">
        <div id="registrationHeader">
            <h2 style="width: 507px">Register Your Account</h2>
        </div>
        <div id="registrationBody">
            <table>
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="labelUserName" runat="server" Text="Username" AssociatedControlID="buttonSubmit"></asp:Label></td>
                    <td>
                        <asp:TextBox ID="textBoxUsername" runat="server" Style="margin-bottom: 5px;" MaxLength="20" TabIndex="1" Width="215px"></asp:TextBox></td>
                    <td>
                        <asp:CustomValidator ID="CustomValidatorUsernameLength" runat="server" ControlToValidate="textBoxUsername" ErrorMessage="Username must be between 3 and 20 characters in length" ForeColor="Red" OnServerValidate="UsernameLength" Display="Dynamic"></asp:CustomValidator>
                        <asp:CustomValidator ID="CustomValidatorUsername" runat="server" ControlToValidate="textBoxUsername" Display="Dynamic" EnableTheming="True" ErrorMessage="Username is taken" ForeColor="Red" OnServerValidate="CheckUsername">Username is taken</asp:CustomValidator>
                        <asp:CustomValidator ID="CustomValidatorUsernameNoSpecChars" runat="server" ErrorMessage="Username may only contain alphanumeric characters" OnServerValidate="UsernameCharactersCheck" Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorUsername" runat="server" ControlToValidate="textBoxUsername" ErrorMessage="Username is required" ForeColor="Red" SetFocusOnError="True" Display="Dynamic">Required</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="labelEmail" runat="server" AssociatedControlID="textBoxEmail" Text="Email:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textBoxEmail" runat="server" MaxLength="255" Style="margin-left: 0px; margin-bottom: 5px;" TabIndex="2" Width="215px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmail" runat="server" ControlToValidate="textBoxEmail" Display="Dynamic" ErrorMessage="E-mail must be of the format 'you@email.com'" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">Incorrect format</asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmail" runat="server" ControlToValidate="textBoxEmail" ErrorMessage="Email is required" ForeColor="Red" SetFocusOnError="True" Display="Dynamic">Required</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="labelEmailAgain" runat="server" AssociatedControlID="textBoxEmailAgain" Text="Re-Enter Email"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textBoxEmailAgain" runat="server" MaxLength="255" Style="margin-bottom: 5px;" TabIndex="3" Width="215px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmailAgain" runat="server" ControlToValidate="textBoxEmailAgain" Display="Dynamic" ErrorMessage="Please re-enter your e-mail address" ForeColor="Red" SetFocusOnError="True">Required</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidatorEmail" runat="server" ControlToCompare="textBoxEmail" ControlToValidate="textBoxEmailAgain" Display="Dynamic" ErrorMessage="E-mail addresses do not match" ForeColor="Red">E-mails must match</asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="labelFirstName" runat="server" AssociatedControlID="textBoxFirstName" Text="First Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textBoxFirstName" runat="server" MaxLength="15" Style="margin-bottom: 5px;" TabIndex="4" Width="215px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorFirstName" runat="server" ControlToValidate="textBoxFirstName" Display="Dynamic" ErrorMessage="First name is required" ForeColor="Red" SetFocusOnError="True">Required</asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidatorFirstNameLength" runat="server" ErrorMessage="Name must be between 3 and 15 characters in length" ForeColor="Red" OnServerValidate="FirstNameLength" Display="Dynamic"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="labelLastName" runat="server" AssociatedControlID="textBoxLastName" Text="Last Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textBoxLastName" runat="server" MaxLength="15" Style="margin-bottom: 5px;" TabIndex="5" Width="215px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorLastName" runat="server" ControlToValidate="textBoxLastName" Display="Dynamic" ErrorMessage="Last name is required" ForeColor="Red" SetFocusOnError="True">Required</asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="CustomValidatorLastNameLength" runat="server" ErrorMessage="Name must be between 3 and 15 characters in length" ForeColor="Red" OnServerValidate="LastNameLength" Display="Dynamic"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="labelCountry" runat="server" AssociatedControlID="dropDownListCountry" Text="Country (optional)"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="dropDownListCountry" runat="server" AppendDataBoundItems="true" DataSourceID="caffeineDB" DataTextField="name" DataValueField="name" Style="margin-bottom: 5px;" TabIndex="6" Width="220px">
                            <asp:ListItem Text="Select Country" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:SqlDataSource ID="caffeineDB" runat="server" ConnectionString="<%$ ConnectionStrings:caffeineDB %>" SelectCommand="SELECT [name] FROM [Countries]"></asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="labelDOB" runat="server" AssociatedControlID="dropDownListMonth" Text="Date of Birth (optional)"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="dropDownListMonth" runat="server" Style="margin-right: auto; margin-top: 0; margin-bottom: 5px;" TabIndex="7" CssClass="auto-style2">
                            <asp:ListItem Selected="True">Month</asp:ListItem>
                            <asp:ListItem>January</asp:ListItem>
                            <asp:ListItem>Februrary</asp:ListItem>
                            <asp:ListItem>March</asp:ListItem>
                            <asp:ListItem>April</asp:ListItem>
                            <asp:ListItem>May</asp:ListItem>
                            <asp:ListItem>June</asp:ListItem>
                            <asp:ListItem>July</asp:ListItem>
                            <asp:ListItem>August</asp:ListItem>
                            <asp:ListItem>September</asp:ListItem>
                            <asp:ListItem>October</asp:ListItem>
                            <asp:ListItem>November</asp:ListItem>
                            <asp:ListItem>December</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="dropDownListDay" runat="server" Style="margin: 0 auto 5px auto;" TabIndex="8">
                            <asp:ListItem Selected="True">Day</asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                            <asp:ListItem>8</asp:ListItem>
                            <asp:ListItem>9</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                            <asp:ListItem>13</asp:ListItem>
                            <asp:ListItem>14</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>16</asp:ListItem>
                            <asp:ListItem>17</asp:ListItem>
                            <asp:ListItem>18</asp:ListItem>
                            <asp:ListItem>19</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>21</asp:ListItem>
                            <asp:ListItem>22</asp:ListItem>
                            <asp:ListItem>23</asp:ListItem>
                            <asp:ListItem>24</asp:ListItem>
                            <asp:ListItem>25</asp:ListItem>
                            <asp:ListItem>26</asp:ListItem>
                            <asp:ListItem>27</asp:ListItem>
                            <asp:ListItem>28</asp:ListItem>
                            <asp:ListItem>29</asp:ListItem>
                            <asp:ListItem>30</asp:ListItem>
                            <asp:ListItem>31</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="dropDownListYear" runat="server" Style="margin: 0 auto 5px auto;" TabIndex="9">
                            <asp:ListItem Selected="True">Year</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="labelPassword" runat="server" AssociatedControlID="textBoxPassword" Text="Password"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textBoxPassword" runat="server" MaxLength="50" Style="margin-bottom: 5px;" TabIndex="10" TextMode="Password" Width="215px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPassword" runat="server" ControlToValidate="textBoxPassword" Display="Dynamic" ErrorMessage="Password is required" ForeColor="Red" SetFocusOnError="True">Required</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorNewPasswordRange" runat="server" ErrorMessage="Must be between 8 and 50 characters" ValidationExpression="^.{8,50}$" ControlToValidate="textBoxPassword" Display="Dynamic" ForeColor="Red"></asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorNewPasswordNumber" runat="server" ErrorMessage="Must contain at least 1 number" Display="Dynamic" ValidateRequestMode="Inherit" ForeColor="Red" ValidationExpression="^[a-zA-Z0-9\W]*\d[a-zA-Z0-9\W]*$" ControlToValidate="textBoxPassword"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="labelPasswordAgain" runat="server" AssociatedControlID="textBoxPasswordAgain" Text="Re-Enter Password"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textBoxPasswordAgain" runat="server" MaxLength="50" Style="margin-bottom: 5px;" TabIndex="11" TextMode="Password" Width="215px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPasswordAgain" runat="server" ControlToValidate="textBoxPasswordAgain" Display="Dynamic" ErrorMessage="Please re-enter your password" ForeColor="Red" SetFocusOnError="True">Required</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidatorPassword" runat="server" ControlToCompare="textBoxPassword" ControlToValidate="textBoxPasswordAgain" Display="Dynamic" ErrorMessage="Passwords do not match" ForeColor="Red">PWs must match</asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="labelSecQuestion" runat="server" AssociatedControlID="textBoxSecQuestion" Text="Secret Question"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textBoxSecQuestion" runat="server" MaxLength="5000" placeholder="e.g. 'What is your mother's maiden name?'" Style="margin-bottom: 5px;" TabIndex="12" Width="215px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorSecQuestion" runat="server" ControlToValidate="textBoxSecQuestion" Display="Dynamic" ErrorMessage="Secret question is required" ForeColor="Red" SetFocusOnError="True">Required</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="labelSecAnswer" runat="server" AssociatedControlID="textBoxSecAnswer" Text="Secret Answer"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="textBoxSecAnswer" runat="server" MaxLength="100" placeholder="'Robot Water Bottles'" TabIndex="13" Width="215px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorSecAnswer" runat="server" ControlToValidate="textBoxSecAnswer" Display="Dynamic" ErrorMessage="Secret answer is required" ForeColor="Red" SetFocusOnError="True">Required</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="2"><asp:Button ID="buttonSubmit" runat="server" Text="Submit" Style="text-align: center; margin-top: 5px;" OnClick="ButtonSubmit_Click" TabIndex="14" Width="100%" /></td>
                </tr>                
            </table>
        </div>
        <br />
        <asp:ValidationSummary ID="ValidationSummary" runat="server" DisplayMode="List" ForeColor="Red" style="margin: 0 0 20px 20px;" />
    </asp:Panel>
</asp:Content>

