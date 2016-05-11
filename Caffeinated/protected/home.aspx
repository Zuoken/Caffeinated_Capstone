<%@ Page Title="Home" Language="C#" MasterPageFile="~/Caffeinated.master" AutoEventWireup="true" CodeFile="home.aspx.cs" Inherits="protected_home" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="Server">
    <style>
        #home_link {
            background-color: #ff9a33;
            box-shadow: 3px 3px 3px #803400;
        }

        #latest_events_header {
            background-color: #363636;
            border: 1px solid #363636;
            color: white;
            margin: 0 auto 0 auto;
            width: 75%;
            text-align: center;
            padding: 0 10px 0 10px;
        }

        .latest_header {
            color: white;
            margin: 50px auto 0 auto;
            width: 75%;
            text-align: center;
            padding: 0 10px 0 10px;
        }

        #blogs_comments_template {
            background-color: #eee5ff;
            border: 1px solid #aa80ff;
            width: 75%;
            padding: 10px;
            margin: 0 auto 10px auto;
        }

        #recipe_comments_template {
            background-color: #ffe5e5;
            border: 1px solid #ff4d4d;
            width: 75%;
            padding: 10px;
            margin: 0 auto 10px auto;
        }

        #blogs_template {
            background-color: #FFEEE5;
            border: 1px solid #FF8041;
            width: 75%;
            padding: 10px;
            margin: 0 auto 10px auto;
        }

        #blog_content {
            margin-top: 10px;
            background-color: white;
            border: 1px solid #FF8041;
            padding: 5px;
        }

        #blog_title {
            font-weight: bold;
            font-size: 1.75em;
            margin: 0;
            padding: 0;
        }

        #recipes_template {
            background-color: #e5fff9;
            border: 1px solid #004d39;
            width: 75%;
            padding: 10px;
            margin: 0 auto 10px auto;
        }
    </style>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <h1 style="text-align: center;">Welcome,
        <asp:LoginName ID="LoginNameUser" runat="server" ForeColor="#ff471a" />
        !</h1>
    <div id="latest_events">
        <h2 id="latest_events_header">The Latest Events</h2>
        <div id="latest_events_body">
            <!-- Recipe Comments Section -->
            <h2 class="latest_header" style="background-color: #ff3333; border: 1px solid #ff3333;">Comments on my Recipes</h2>
            <asp:ListView ID="ListViewLatestRecipeComments" runat="server" DataSourceID="SqlDataSourceLatestRecipeComments">
                <ItemTemplate>
                    <div id="recipe_comments_template">
                        <p style="margin: 0;">
                            <b>Recipe: </b>
                            <asp:Label ID="LabelRecipeTitle" runat="server"><%# Eval("title") %></asp:Label>
                        </p>
                        <p style="margin: 0;">
                            <b>Comment date: </b>
                            <asp:Label ID="LabelRecipeCommentDate" runat="server"><%# Eval("comment_date", "{0:MMMM d, yyyy}") %> at <%# Eval("comment_date", "{0:hh:mm tt}") %></asp:Label>
                        </p>
                        <p style="margin: 10px 0 0 0;">
                            <b>Comment: </b>
                            <asp:Label ID="LabelCommentContent" runat="server"><%# Eval("comment") %></asp:Label>
                        </p>
                    </div>
                </ItemTemplate>
            </asp:ListView>
            <p style="text-align: center;">
                <asp:DataPager ID="DataPagerLatestRecipeComments" runat="server" PagedControlID="ListViewLatestRecipeComments" PageSize="5">
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" />
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" />
                    </Fields>
                </asp:DataPager>
            </p>
            <!-- Blog Comments Section -->
            <h2 class="latest_header" style="background-color: #7733ff; border: 1px solid #7733ff;">Comments on my Blogs</h2>
            <asp:ListView ID="ListViewLatestBlogCommments" runat="server" DataSourceID="SqlDataSourceLatestBlogComments">
                <ItemTemplate>
                    <div id="blogs_comments_template">
                        <p style="margin: 0;">
                            <b>Blog: </b>
                            <asp:Label ID="LabelRecipeTitle" runat="server"><%# Eval("title") %></asp:Label>
                        </p>
                        <p style="margin: 0;">
                            <b>Comment date: </b>
                            <asp:Label ID="LabelRecipeCommentDate" runat="server"><%# Eval("comment_date", "{0:MMMM d, yyyy}") %> at <%# Eval("comment_date", "{0:hh:mm tt}") %></asp:Label>
                        </p>
                        <p style="margin: 10px 0 0 0;">
                            <b>Comment: </b>
                            <asp:Label ID="LabelCommentContent" runat="server"><%# Eval("comment") %></asp:Label>
                        </p>
                    </div>
                </ItemTemplate>
            </asp:ListView>
            <p style="text-align: center;">
                <asp:DataPager ID="DataPagerLatestBlogComments" runat="server" PagedControlID="ListViewLatestBlogCommments" PageSize="5">
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" />
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" />
                    </Fields>
                </asp:DataPager>
            </p>
            <!-- Blogs Section -->
            <h2 class="latest_header" style="background-color: #ff6600; border: 1px solid #ff6600;">Blogs</h2>
            <asp:ListView ID="ListViewLatestBlogs" runat="server" DataSourceID="SqlDataSourceLatestBlogs" Style="margin-left: auto; margin-right: auto;">
                <ItemTemplate>
                    <div id="blogs_template">
                        <p id="blog_title">
                            <asp:Label ID="LabelBlogTitle" runat="server"><%# Eval("title") %></asp:Label>
                        </p>
                        <p style="margin: 0;">
                            By user:
                        <asp:Label ID="LabelBlogUsername" runat="server" ForeColor="#ff471a"><%# Eval("username") %></asp:Label>
                        </p>
                        <p style="margin: 0;">
                            <asp:Label ID="LabelBlogDate" Font-Size="Small" runat="server"><%# Eval("blog_date", "{0:MMMM d, yyyy}") %> at <%# Eval("blog_date", "{0:hh:mm tt}") %></asp:Label>
                        </p>
                        <p style="margin: 0;">
                            <asp:TextBox ID="TextBoxBlogContent" runat="server" ReadOnly="True" TextMode="MultiLine" Rows="5" Width="100%" BorderStyle="None" Text='<%# Eval("content") %>' Style="overflow: auto;"></asp:TextBox>
                        </p>
                    </div>
                </ItemTemplate>
            </asp:ListView>
            <p style="text-align: center;">
                <asp:DataPager ID="DataPagerLatestBlogs" runat="server" PagedControlID="ListViewLatestBlogs" PageSize="5">
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" />
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" />
                    </Fields>
                </asp:DataPager>
            </p>
            <!-- Recipes Section -->
            <h2 class="latest_header" style="background-color: #008060; border: 1px solid #008060;">Recipes</h2>
            <asp:ListView ID="ListViewLatestRecipes" runat="server" DataSourceID="SqlDataSourceLatestRecipes">
                <ItemTemplate>
                    <div id="recipes_template">
                        <h1 style="margin: 0;">
                            <asp:Label ID="titleLabel" runat="server" Text='<%# Eval("title") %>' /></h1>
                        <p style="margin: 0;">By user <span style="color: #ff471a;"><%# Eval("username") %></span></p>
                        <h5 style="margin: 0;">
                            <asp:Label ID="dateLabel" runat="server" Text='<%# Eval("recipe_date", "{0:MMMM d, yyyy}") %>' /></h5>
                        <br />
                        <p style="margin: 0;">
                            <asp:Label ID="descriptionLabel" runat="server" Text='<%# Eval("recipe_description") %>' />
                        </p>
                        <p style="clear: both; margin: 0; padding-bottom: 5px;"></p>
                    </div>
                </ItemTemplate>
            </asp:ListView>
            <p style="text-align: center;">
                <asp:DataPager ID="DataPagerLatestRecipes" runat="server" PagedControlID="ListViewLatestRecipes" PageSize="5">
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" />
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False" />
                    </Fields>
                </asp:DataPager>
            </p>
            <!-- Datasources -->
            <asp:SqlDataSource ID="SqlDataSourceLatestBlogs" runat="server" ConnectionString="<%$ ConnectionStrings:caffeinated_dbConnectionString %>" SelectCommand="SELECT * FROM [Blogs]
JOIN Customers ON Blogs.customer_id = Customers.customer_id ORDER BY blog_date DESC"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceLatestRecipeComments" runat="server" ConnectionString="<%$ ConnectionStrings:caffeinated_dbConnectionString %>"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceLatestBlogComments" runat="server" ConnectionString="<%$ ConnectionStrings:caffeinated_dbConnectionString %>"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceLatestRecipes" runat="server" ConnectionString="<%$ ConnectionStrings:caffeinated_dbConnectionString %>" SelectCommand="SELECT * FROM [Recipes] 
JOIN Customers ON Recipes.customer_id = Customers.customer_id
WHERE ([recipe_privacy] = '0') ORDER BY recipe_date DESC"></asp:SqlDataSource>
        </div>
    </div>
</asp:Content>

