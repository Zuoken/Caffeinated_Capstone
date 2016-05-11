<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Caffeinated.master" AutoEventWireup="true" CodeFile="profile.aspx.cs" Inherits="protected_profile" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="Server">
    <style>
        #profile_link {
            background-color: #ff9a33;
            box-shadow: 3px 3px 3px #803400;
        }

        #recent_activity {
            width: 100%;
            padding: 1px 0 1px 0;
            clear: both;
        }

            #recent_activity #title {
                background-color: grey;
                color: white;
                font-weight: bold;
                font-size: 1.25em;
                width: 100%;
                padding-top: 5px;
                padding-bottom: 5px;
            }

                #recent_activity #title p {
                    padding-left: 50px;
                    margin: 0;
                }

            #recent_activity #background {
                background-color: lightgrey;
                padding-top: 1px;
                padding-bottom: 1px;
            }

                #recent_activity #background #recent_act_body {
                    background-color: white;
                    border: 1px solid black;
                    margin: 25px;
                }

        #left_side {
            border: 1px solid black;
            float: left;
            width: 35%;
            padding: 5px;
        }

        #right_side {
            border: 1px solid black;
            float: right;
            width: 63%;
        }

        #about_me {
            border: 1px solid black;
            margin: 5px;
            padding: 5px;
        }

        #all_images {
            border: 1px solid black;
            margin: 5px;
            padding: 5px;
        }

        #recent_blogs {
            border: 1px solid black;
            margin: 5px;
            padding: 5px;
        }

        #profile_picture {
            border: 1px solid black;
            padding: 5px;
            margin: 5px;
        }

        #personal_info {
            border: 1px solid black;
            padding: 5px;
            margin: 5px;
        }

        #all_blogs_template {
            background-color: #FFEEE5;
            border: 1px solid #FF8041;
            padding: 10px 50px 10px 50px;
            margin: 20px auto 20px auto;
            width: 70%;
        }

                #blog_title {
            font-weight: bold;
            font-size: 1.75em;
            margin: 0px;
        }

        #blog_content {
            margin-top: 10px;
            background-color: white;
            border: 1px solid #FF8041;
            padding: 5px;
        }

        #blog_date {
            margin: 0;
        }
    </style>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div id="left_side">
        <div id="profile_picture">
            <asp:ImageMap ID="ImageMapProfilePic" runat="server" Height="250px" Width="250px"></asp:ImageMap>
            <asp:CheckBoxList ID="CheckBoxListProfilePicture" runat="server" Visible="false" OnSelectedIndexChanged="CheckBoxList_Changed" AutoPostBack="True">
            </asp:CheckBoxList>
            <br />
            <asp:Button ID="ButtonProfilePicture" runat="server" Text="Change Profile Photo" BackColor="Blue" BorderStyle="None" Font-Bold="True" Font-Size="Medium" ForeColor="White" OnClick="ButtonProfilePicture_Click" />
            &nbsp;<asp:Button ID="ButtonProfilePictureCancel" runat="server" Text="Cancel" BackColor="Red" BorderStyle="None" Font-Bold="True" Font-Size="Medium" ForeColor="White" OnClick="ButtonProfilePictureCancel_Click" Visible="False" CausesValidation="False" />
        </div>
        <div id="personal_info">
            <h2>Personal Information</h2>
            <asp:Table ID="TablePersonalInfo" runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="LabelFN" runat="server" Text="First name:"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="LabelFirstName" runat="server"></asp:Label>
                        <asp:TextBox ID="TextBoxFirstNameUpdate" runat="server" Visible="false" MaxLength="15" TabIndex="1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorFirstName" runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="TextBoxFirstNameUpdate"></asp:RequiredFieldValidator>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:CheckBox ID="CheckBoxFirstName" runat="server" Text="Make first name public" OnCheckedChanged="UpdatePreferences" AutoPostBack="true" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="LabelLN" runat="server" Text="Last name:"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="LabelLastName" runat="server"></asp:Label>
                        <asp:TextBox ID="TextBoxLastNameUpdate" runat="server" Visible="false" MaxLength="15" TabIndex="2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorLastName" runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="TextBoxLastNameUpdate"></asp:RequiredFieldValidator>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:CheckBox ID="CheckBoxLastName" runat="server" Text="Make last name public" OnCheckedChanged="UpdatePreferences" AutoPostBack="true" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="LabelDOB" runat="server" Text="Date of birth:"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="LabelDateOfBirth" runat="server"></asp:Label>
                        <asp:DropDownList ID="dropDownListMonth" runat="server" Style="margin-right: auto; margin-top: 0; margin-bottom: 5px;" TabIndex="3" CssClass="auto-style2" Visible="false">
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
                        <asp:DropDownList ID="dropDownListDay" runat="server" Style="margin: 0 auto 5px auto;" TabIndex="4" Visible="false">
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
                        <asp:DropDownList ID="dropDownListYear" runat="server" Style="margin: 0 auto 5px auto;" TabIndex="5" Visible="false">
                            <asp:ListItem Selected="True">Year</asp:ListItem>
                        </asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:CheckBox ID="CheckBoxDOB" runat="server" Text="Make date of birth public" OnCheckedChanged="UpdatePreferences" AutoPostBack="true" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="LabelLoc" runat="server" Text="Location:"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="LabelLocation" runat="server"></asp:Label>
                        <asp:DropDownList ID="dropDownListCountry" runat="server" AppendDataBoundItems="true" DataSourceID="caffeineDB" DataTextField="name" DataValueField="name" Style="margin-bottom: 5px;" TabIndex="6" Width="220px" Visible="false">
                            <asp:ListItem Text="Select Country" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorLocation" runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="dropDownListCountry"></asp:RequiredFieldValidator>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:CheckBox ID="CheckBoxLocation" runat="server" Text="Make location public" OnCheckedChanged="UpdatePreferences" AutoPostBack="true" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="LabelEA" runat="server" Text="Email:"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="LabelEmail" runat="server"></asp:Label>
                        <asp:TextBox ID="TextBoxEmailUpdate" runat="server" Visible="false" TabIndex="7"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmail" runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="TextBoxEmailUpdate" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmailUpdate" runat="server" ErrorMessage="Must be an e-mail address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red" ControlToValidate="TextBoxEmailUpdate" Display="Dynamic"></asp:RegularExpressionValidator>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:CheckBox ID="CheckBoxEmail" runat="server" Text="Make email public" OnCheckedChanged="UpdatePreferences" AutoPostBack="true" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:SqlDataSource ID="caffeineDB" runat="server" ConnectionString="<%$ ConnectionStrings:caffeineDB %>" SelectCommand="SELECT [name] FROM [Countries]"></asp:SqlDataSource>
            <br />
            <asp:Button ID="ButtonUpdateInfo" runat="server" Text="Update Information" BackColor="Blue" BorderStyle="None" Font-Bold="True" Font-Size="Medium" ForeColor="White" OnClick="ButtonUpdateInfo_Click" Style="height: 25px" />
            &nbsp;<asp:Button ID="ButtonUpdateProfileInfoCancel" runat="server" Text="Cancel" BackColor="Red" BorderStyle="None" Font-Bold="True" Font-Size="Medium" ForeColor="White" OnClick="ButtonUpdateProfileCancel_Click" Visible="False" CausesValidation="False" />
            <br />
            <br />
            <asp:Label ID="LabelPasswordUpdateResponse" runat="server"></asp:Label>
            <asp:Panel ID="PanelNewPassword" runat="server" Visible="false">
                <asp:Table ID="TableChangePassword" runat="server">
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right">
                            <asp:Label ID="LabelCurrentPassword" runat="server" Font-Bold="true">Current Password:</asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="TextBoxCurrentPassword" runat="server" TextMode="Password" MaxLength="50"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorCurrentPassword" runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="TextBoxCurrentPassword" Display="Dynamic"></asp:RequiredFieldValidator>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right">
                            <asp:Label ID="LabelNewPassword" runat="server" Font-Bold="true">New Password:</asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="TextBoxNewPassword" runat="server" TextMode="Password" MaxLength="50"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorNewPassword" runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="TextBoxNewPassword" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorNewPasswordRange" runat="server" ErrorMessage="Must be between 8 and 50 characters" ValidationExpression="^.{8,50}$" ControlToValidate="TextBoxNewPassword" Display="Dynamic" ForeColor="Red"></asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorNewPasswordNumber" runat="server" ErrorMessage="Must contain at least 1 number" Display="Dynamic" ValidateRequestMode="Inherit" ForeColor="Red" ValidationExpression="^[a-zA-Z0-9\W]*\d[a-zA-Z0-9\W]*$" ControlToValidate="TextBoxNewPassword"></asp:RegularExpressionValidator>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Right">
                            <asp:Label ID="LabeConfirmPassword" runat="server" Font-Bold="true">Confirm Password:</asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="TextBoxConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorConfirmPassword" runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="TextBoxConfirmPassword" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidatorConfirmPassword" runat="server" ErrorMessage="Passwords must match" ControlToValidate="TextBoxConfirmPassword" ControlToCompare="TextBoxNewPassword" ForeColor="Red"></asp:CompareValidator>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Button ID="ButtonSubmitNewPassword" runat="server" Text="Submit" BackColor="Green" ForeColor="White" Style="margin: 0 5px 0 5px;" BorderStyle="None" OnClick="ButtonSubmitNewPassword_Click" />
                <asp:Button ID="ButtonCancelNewPassword" runat="server" Text="Cancel" BackColor="Red" ForeColor="White" Style="margin: 0 5px 0 5px;" BorderStyle="None" CausesValidation="False" OnClick="ButtonCancelNewPassword_Click" />
            </asp:Panel>
            <asp:Button ID="ButtonChangePass" runat="server" Text="Change Password" BackColor="Blue" BorderStyle="None" Font-Bold="True" Font-Size="Medium" ForeColor="White" OnClick="ButtonChangePass_Click" CausesValidation="False" />
        </div>
    </div>
    <div id="right_side">
        <div id="about_me">
            <h2>About me</h2>
            <asp:TextBox
                ID="TextBoxAboutMe"
                runat="server"
                Width="99%"
                EnableTheming="True"
                ReadOnly="True"
                TextMode="MultiLine"
                Style="resize: none;" BorderStyle="None" Height="140px"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="ButtonAboutMe" runat="server" Text="Edit" OnClick="ButtonAboutMe_Click" BackColor="Blue" BorderStyle="None" Font-Bold="True" Font-Size="Medium" ForeColor="White" />
            &nbsp;<asp:Button ID="ButtonAboutMeCancel" runat="server" Text="Cancel" BackColor="Red" BorderStyle="None" Font-Bold="True" Font-Size="Medium" ForeColor="White" OnClick="ButtonAboutMeCancel_Click" Visible="False" CausesValidation="False" />
        </div>
        <div id="all_images">
            <h2>Uploaded Images</h2>
            <div style="height: inherit; overflow: auto;">
                <asp:DataList ID="UserImages" runat="server" Width="100%" RepeatColumns="3" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" ForeColor="Black" GridLines="Both" CellSpacing="2">
                    <FooterStyle BackColor="#CCCCCC" />
                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                    <ItemStyle BackColor="White" />
                    <ItemTemplate>
                        <asp:Image ID="Image" runat="server" Height="250px" ImageAlign="Middle" ImageUrl='
                        <%# Eval("Name", "~/protected/Images/" + System.Web.HttpContext.Current.User.Identity.Name + "/{0}") %>'
                            Width="250px" />
                    </ItemTemplate>
                    <SelectedItemStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                </asp:DataList>
            </div>
            <br />
            <asp:Label ID="LabelUploadImage" runat="server" Text="Upload an image:"></asp:Label>
            <br />
            <asp:FileUpload ID="FileUploadImage" runat="server" />
            <br />
            <asp:Button ID="ButtonUpload" runat="server" OnClick="AddImageButton_Click" Text="Add Image" BackColor="Blue" BorderStyle="None" Font-Bold="True" Font-Size="Medium" ForeColor="White" />
            <asp:Label ID="LabelTypeMessage" runat="server" ForeColor="Red"></asp:Label>
        </div>
        <div id="recent_blogs">
            <h2>Recent Blog Entries</h2>
            <asp:ListView ID="ListViewBlogs" runat="server" DataSourceID="SqlDataSourceBlogs">
                <ItemTemplate>
                    <div id="all_blogs_template">
                        <p id="blog_title">
                            <asp:Label ID="LabelBlogTitle" runat="server"><%# Eval("title") %></asp:Label>
                        </p>
                        <p id="blog_date">
                            <asp:Label ID="LabelBlogDate" Font-Size="Small" runat="server"><%# Eval("blog_date", "{0:MMMM d, yyyy}") %> at <%# Eval("blog_date", "{0:hh:mm tt}") %></asp:Label>
                        </p>
                        <p id="blog_content">
                            <asp:TextBox ID="TextBoxBlogContent" runat="server" ReadOnly="True" TextMode="MultiLine" Rows="5" Width="100%" BorderStyle="None" Text='<%# Eval("content") %>' Style="overflow: auto;"></asp:TextBox>
                        </p>
                        <asp:Button ID="ButtonDeleteBlogEntry" runat="server" Text="Delete" ForeColor="White" BackColor="Red" BorderStyle="None" OnClick="ButtonDeleteBlogEntry_Click" CommandArgument='<%# Eval("blog_id") %>' />
                    </div>
                </ItemTemplate>
            </asp:ListView>
            <asp:SqlDataSource ID="SqlDataSourceBlogs" runat="server" ConnectionString="<%$ ConnectionStrings:caffeinated_dbConnectionString %>" SelectCommand="SELECT * FROM [Blogs]"></asp:SqlDataSource>
            <p style="text-align: center;">
                <asp:DataPager ID="DataPagerBlogs" runat="server" PagedControlID="ListViewBlogs" PageSize="5">
                    <Fields>
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField />
                    </Fields>
                </asp:DataPager>
            </p>
        </div>
    </div>
    <div id="recent_activity">
        <div id="title">
            <p>Recent Activity</p>
        </div>
        <div id="background">
            <div id="recent_act_body">
                <asp:TextBox ID="TextBoxRecentActivity" runat="server" TextMode="MultiLine" Rows="10" Height="100px" Width="99%"></asp:TextBox>
            </div>
        </div>
    </div>
</asp:Content>

