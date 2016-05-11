<%@ Page Title="Blog" Language="C#" MasterPageFile="~/Caffeinated.master" AutoEventWireup="true" CodeFile="blog.aspx.cs" Inherits="protected_blog" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="Server">
    <style>
        h2 {
        }

        .blogs_view {
            width: 75%;
            margin-left: auto;
            margin-right: auto;
        }

        .blogs_header {
            background-color: #363636;
            border: 1px solid #363636;
            color: white;
            width: 79%;
            margin-left: auto;
            margin-right: auto;
            text-align: center;
        }

        #blog_link {
            background-color: #ff9a33;
            box-shadow: 3px 3px 3px #803400;
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

        #blog_username {
            margin: 0;
        }

        #all_blogs_template {
            background-color: #FFEEE5;
            border: 1px solid #FF8041;
            padding: 10px 50px 10px 50px;
            margin: 20px auto 20px auto;
            width: 70%;
        }

        #new_blog_form {
            width: 70%;
            background-color: #f0f0f5;
            border: 1px solid #7575a3;
            margin: 10px auto 50px auto;
            padding: 25px 50px 60px 50px;
        }

        .full_blog_body {
            width: 53%;
            border: 1px solid black;
            margin: 0 auto 20px auto;
            padding: 0 50px 0 50px;
        }

        #comments_body {
            margin-bottom: 25px;
        }

        #blog_rating {
            text-align: center;
            margin-bottom: 50px;
        }

        #blog_rating_total {
            margin: 0;
        }

    </style>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div id="all_blogs_view" class="blogs_view" runat="server">
        <div class="blogs_header">
            <h2 style="margin: 0; padding: 0;">Blogs</h2>
        </div>
        <div id="all_blogs_body">
            <div id="new_blog_form">
                <p style="margin: 0;">
                    <b>
                        <asp:Label ID="LabelNewBlogTitle" runat="server" Text="Title"></asp:Label></b>
                </p>
                <p style="margin: 0 0 10px 0;">
                    <asp:TextBox ID="TextBoxNewBlogTitle" runat="server" Width="50%" BorderStyle="Groove" MaxLength="255"></asp:TextBox>
                </p>
                <p style="margin: 0;">
                    <b>
                        <asp:Label ID="LabelNewBlogContent" runat="server" Text="Content"></asp:Label></b>
                </p>
                <p style="margin: 0;">
                    <asp:TextBox ID="TextBoxNewBlogContent" runat="server" TextMode="MultiLine" Width="100%" Height="300px" BorderStyle="Groove" MaxLength="2147483647" Style="resize: none; overflow: auto;"></asp:TextBox>
                </p>
                <p style="display: inline;">
                    <asp:Label ID="LabelSubmitMessage" runat="server" Text=""></asp:Label>
                </p>
                <p style="display: inline; float: right;">
                    <asp:Button ID="ButtonSubmitBlog" runat="server" Text="Add Blog Post" BackColor="#3366FF" BorderStyle="None" ForeColor="White" OnClick="ButtonSubmitBlog_Click" />
                </p>
            </div>
            <asp:ListView ID="ListViewAllBlogs" runat="server" DataSourceID="SqlDataSourceBlogs">
                <ItemTemplate>
                    <div id="all_blogs_template">
                        <p id="blog_title">
                            <asp:Label ID="LabelBlogTitle" runat="server"><%# Eval("title") %></asp:Label>
                        </p>
                        <p id="blog_username">
                            By user:
                        <asp:Label ID="LabelBlogUsername" runat="server" ForeColor="#ff471a"><%# Eval("username") %></asp:Label>
                        </p>
                        <p id="blog_date">
                            <asp:Label ID="LabelBlogDate" Font-Size="Small" runat="server"><%# Eval("blog_date", "{0:MMMM d, yyyy}") %> at <%# Eval("blog_date", "{0:hh:mm tt}") %></asp:Label>
                        </p>
                        <p id="blog_content">
                            <asp:TextBox ID="TextBoxBlogContent" runat="server" ReadOnly="True" TextMode="MultiLine" Rows="5" Width="100%" BorderStyle="None" Text='<%# Eval("content") %>' Style="overflow: auto;"></asp:TextBox>
                        </p>
                        <p>
                            <asp:Button ID="ButtonReadMore" runat="server" Text="Read More" CommandArgument='<%# Eval("blog_id") %>' OnClick="ButtonReadMore_Click" ForeColor="White" BackColor="#3366FF" BorderStyle="None" />
                        </p>
                    </div>
                </ItemTemplate>
            </asp:ListView>

            <p style="text-align: center;">
                <asp:DataPager ID="DataPagerAllBlogs" runat="server" PagedControlID="ListViewAllBlogs" PageSize="10">
                    <Fields>
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField />
                    </Fields>
                </asp:DataPager>
            </p>

            <asp:SqlDataSource
                ID="SqlDataSourceBlogs"
                runat="server"
                ConnectionString="<%$ ConnectionStrings:caffeinated_dbConnectionString %>"
                SelectCommand="SELECT blog_id, blog_date, title, content, username FROM [Blogs]
JOIN [Customers] ON [Blogs].customer_id = [Customers].customer_id
ORDER BY blog_date DESC"></asp:SqlDataSource>
        </div>
    </div>
    <div id="full_blogs_view" class="blogs_view" runat="server" visible="false">
        <p style="text-align: center;">
            <asp:Button ID="ButtonReturnToBlogs" runat="server" Text="Back to Blogs" BackColor="Green" BorderStyle="None" ForeColor="White" OnClick="ButtonReturnToBlogs_Click" />
        </p>
        <div class="blogs_header" style="width: 53%; padding: 0 50px 0 50px; font-weight: bold;">
            <h2 style="margin: 0; padding: 0;">
                <asp:Label ID="LabelBlogTitle" runat="server"></asp:Label>
            </h2>
        </div>
        <div class="full_blog_body">
            <p id="blog_username">
                By user:
                        <asp:Label ID="LabelBlogUsername" runat="server" ForeColor="#ff471a"></asp:Label>
            </p>
            <p id="blog_date">
                <asp:Label ID="LabelBlogDate" runat="server"></asp:Label>
            </p>
            <p id="blog_rating_total">
                <b>Rating:</b> <asp:Label ID="LabelBlogRating" runat="server" Text=""></asp:Label>
                <asp:Label ID="LabelRatingStatus" runat="server" style="margin-left: 5px;"></asp:Label>
            </p>
            <p id="blog_content" style="border: none;">
                <asp:TextBox ID="TextBoxBlogContent" runat="server" Font-Size="Large" ReadOnly="True" TextMode="MultiLine" Rows="15" Width="100%" BorderStyle="None" Style="overflow: auto;"></asp:TextBox>
            </p>
        </div>
        <div id="blog_rating">
            <p>Did you enjoy this post?</p>
            <asp:Button ID="ButtonThumbsUp" runat="server" BackColor="Green" BorderStyle="None" ForeColor="White" Text="Thumbs up" OnClick="ButtonThumbsUp_Click" style="margin: 0 5px 0 5px;" />
            <asp:Button ID="ButtonThumbsDown" runat="server" Text="Thumbs down" BorderStyle="None" BackColor="Red" ForeColor="White" OnClick="ButtonThumbsDown_Click" style="margin: 0 5px 0 5px;" />
        </div>
        <div id="full_blog_comments" class="blogs_header" style="width: 53%; padding: 0 50px 0 50px; font-weight: bold;">
            <h2 style="margin: 0; padding: 0;">Comments</h2>
        </div>
        <div id="full_blog_comments_body" class="full_blog_body">
            <p style="margin: 10px 0 5px 0;"><b>Add a new comment:</b></p>
            <asp:TextBox ID="TextBoxNewComment" runat="server" MaxLength="250" TextMode="MultiLine" Style="resize: none; overflow: auto;" Width="100%" Rows="5"></asp:TextBox>
            <asp:Button ID="ButtonSubmitComment" runat="server" Text="Add Comment" Style="float: right; clear: both; margin-top: 10px; height: 21px;" BackColor="#3366FF" BorderStyle="None" ForeColor="White" OnClick="ButtonSubmitComment_Click" />
            <br style="clear: both;" />
            <hr />
            <h3 style="text-align: center;">User Comments</h3>
            <br />
            <asp:ListView ID="ListViewComments" runat="server" DataSourceID="SqlDataSourceComments">
                <ItemTemplate>
                    <div id="comments_body">
                        <asp:Label ID="LabelCommenterUsername" runat="server" Text='<%# Eval("username") %>' Font-Size="Medium" ForeColor="#ff471a"></asp:Label>&nbsp;says:
                        <br />
                        <asp:Label ID="LabelCommentDate" runat="server" Font-Size="Small"><%# Eval("comment_date", "{0:MMMM d, yyyy}") %> at <%# Eval("comment_date", "{0:hh:mm tt}") %></asp:Label>
                        <asp:TextBox ID="TextBoxUserComment" runat="server" ReadOnly="True" Font-Size="Large" TextMode="MultiLine" Rows="5" Width="100%" BorderStyle="None" Text='<%# Eval("comment") %>' Style="overflow: auto;"></asp:TextBox>
                    </div>
                </ItemTemplate>
            </asp:ListView>
            <p style="text-align: center;">
                <asp:DataPager ID="DataPagerComments" runat="server" PagedControlID="ListViewComments" PageSize="10">
                    <Fields>
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField />
                    </Fields>
                </asp:DataPager>
            </p>
            <asp:SqlDataSource ID="SqlDataSourceComments" runat="server" ConnectionString="<%$ ConnectionStrings:caffeinated_dbConnectionString %>"></asp:SqlDataSource>
        </div>
    </div>
</asp:Content>

