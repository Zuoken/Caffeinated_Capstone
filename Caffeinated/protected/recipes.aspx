<%@ Page Title="Recipes" Language="C#" MasterPageFile="~/Caffeinated.master" AutoEventWireup="true" CodeFile="recipes.aspx.cs" Inherits="protected_recipes" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="Server">
    <style>
        #recipes_link {
            background-color: #ff9a33;
            box-shadow: 3px 3px 3px #803400;
        }

        #my_recipes {
            width: 20%;
            display: inline-block;
            vertical-align: top;
            margin-left: 20px;
            border: 1px solid #363636;
        }

            #my_recipes h2 {
                background-color: #363636;
                color: white;
                text-align: center;
                padding: 5px;
                margin: 0;
            }

        #left_side {
            display: inline-block;
            width: 78%;
            border: 1px solid #363636;
            vertical-align: top;
        }

            #left_side h2 {
                background-color: #363636;
                color: white;
                text-align: center;
                padding: 5px;
                margin: 0;
            }

        #left_body {
            padding: 5px;
        }

        #right_body {
            padding: 5px;
        }

        #all_recipes_template {
            background-color: #FFEEE5;
            border: 1px solid #FF8041;
            width: 500px;
            padding: 0 10px 0 10px;
            margin: 0 auto 10px auto;
        }

        .recipe_title, .user_label, .recipe_description, .recipe_date {
            margin: 0;
        }

        .read_more {
            float: right;
        }

        #my_recipes_template {
            background-color: #ccf2ff;
            border: 1px solid #00ace6;
            padding: 0 10px 0 10px;
            margin: 0 0 10px 0;
        }

        #add_recipe_template {
            margin-top: 5px;
        }

        #new_recipe_form {
            margin-left: auto;
            margin-right: auto;
        }

        .ingredientsText {
            margin: 5px;
        }

        .addIngredientsButton {
            display: block;
            width: 125px;
            margin: 10px auto 0 auto;
        }

        .backToRecipesButton {
            display: block;
            margin: 10px auto 0 auto;
        }

        #recipe_label_title {
            font-size: 2.75em;
            font-weight: bold;
            margin-bottom: 0;
            margin-top: 0;
        }


        #recipe_label_date {
            font-size: 0.75em;
            margin-top: 0;
            margin-bottom: 0;
        }

        #recipe_username {
            margin-top: 0;
        }

        #recipe_label_description {
        }

        #recipe_ingredients_title {
            font-size: 1.75em;
            font-weight: bold;
            margin-bottom: 0;
        }

        #recipe_table {
        }

        #recipe_label_directions_title {
            font-size: 1.75em;
            font-weight: bold;
            margin-bottom: 0;
        }

        #recipe_view {
            background-color: #f0f0f5;
            padding: 10px 20px 10px 20px;
        }

        #recipe_rating {
            margin-bottom: 0;
        }

        #recipe_rating_status {
            margin-top: 0;
        }

        #full_recipe_comments_body {
            width: 53%;
            border: 1px solid black;
            margin: 0 auto 20px auto;
            padding: 0 50px 0 50px;
        }

        .recipe_header {
            background-color: #363636;
            border: 1px solid #363636;
            color: white;
            width: 79%;
            margin-left: auto;
            margin-right: auto;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
    <div id="left_side">
        <h2>
            <asp:Label ID="LabelLeftSide" runat="server" Text="New and Trending Recipes"></asp:Label>
        </h2>
        <div id="left_body">
            <div id="all_recipes_body" runat="server">
                <div style="text-align: center; margin: 20px 0 20px 0;">
                    <p style="display: inline; font-weight: bold;">Choose a category: </p>
                    <asp:DropDownList ID="DropDownListCategories" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SelectCategory">
                        <asp:ListItem>Appetizer</asp:ListItem>
                        <asp:ListItem Selected="True">Beverage</asp:ListItem>
                        <asp:ListItem>Breakfast</asp:ListItem>
                        <asp:ListItem>Dessert</asp:ListItem>
                        <asp:ListItem>Dinner</asp:ListItem>
                        <asp:ListItem>Lunch</asp:ListItem>
                        <asp:ListItem>Snack</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <asp:ListView ID="ListViewRecipes" runat="server" DataSourceID="SqlDataSourceCaffeinated" Style="margin-left: auto; margin-right: auto;">
                    <ItemTemplate>
                        <div id="all_recipes_template">
                            <asp:Label ID="imageLabel" runat="server" Text='<%# Eval("recipe_image") %>' />
                            <h1 class="recipe_title">
                                <asp:Label ID="titleLabel" runat="server" Text='<%# Eval("title") %>' /></h1>
                            <p class="user_label">By user <b><span style="color: #ff471a;"><%# Eval("username") %></span></b></p>
                            <h5 class="recipe_date">
                                <asp:Label ID="dateLabel" runat="server" Text='<%# Eval("recipe_date", "{0:MMMM d, yyyy}") %>' /></h5>
                            <br />
                            <p class="recipe_description">
                                <asp:Label ID="descriptionLabel" runat="server" Text='<%# Eval("recipe_description") %>' />
                            </p>
                            <span class="read_more">
                                <asp:Button ID="ButtonReadMore" runat="server" Text="Read More" BackColor="#3366FF" ForeColor="White" BorderStyle="None" OnClick="ButtonReadMore_Click" CommandArgument='<%# Eval("recipe_id") %>' />
                            </span>
                            <p style="clear: both; margin: 0; padding-bottom: 5px;"></p>
                        </div>

                        <!--recipe_id:
                        <asp:Label ID="recipe_idLabel" runat="server" Text='<%# Eval("recipe_id") %>' />
                        <br />
                        customer_id:
                        <asp:Label ID="customer_idLabel" runat="server" Text='<%# Eval("customer_id") %>' />
                        <br />
                        category:
                        <asp:Label ID="categoryLabel" runat="server" Text='<%# Eval("category") %>' />
                        directions:
                        <asp:Label ID="directionsLabel" runat="server" Text='<%# Eval("directions") %>' />
                        <br />
                        private:
                        <asp:Label ID="privateLabel" runat="server" Text='<%# Eval("recipe_privacy") %>' />
                        <br />
                    -->
                    </ItemTemplate>
                </asp:ListView>
                <p style="text-align: center;">
                    <asp:DataPager ID="DataPagerRecipes" runat="server" PagedControlID="ListViewRecipes" PageSize="15">
                        <Fields>
                            <asp:NumericPagerField />
                            <asp:NextPreviousPagerField />
                        </Fields>
                    </asp:DataPager>
                </p>
                <asp:SqlDataSource ID="SqlDataSourceCaffeinated" runat="server" ConnectionString="<%$ ConnectionStrings:caffeinated_dbConnectionString %>"
                    SelectCommand="SELECT c.username, r.recipe_id, r.customer_id, r.recipe_date, r.category, r.title, r.recipe_description, r.directions, r.recipe_image, r.recipe_privacy FROM Recipes r JOIN Customers c ON r.customer_id = c.customer_id WHERE r.category = @category AND r.recipe_privacy = 0 ORDER BY r.recipe_date DESC">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="" Name="category" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
            <div id="new_recipe_body" runat="server" visible="false">
                <table id="new_recipe_form">
                    <tr>
                        <td id="upload_recipe_image">Upload Image</td>
                    </tr>
                    <tr>
                        <td><b>Category:</b></td>
                    </tr>
                    <tr>
                        <td id="categories">
                            <asp:DropDownList ID="DropDownListNewCategories" runat="server" OnSelectedIndexChanged="SelectCategory">
                                <asp:ListItem>Appetizer</asp:ListItem>
                                <asp:ListItem Selected="True">Beverage</asp:ListItem>
                                <asp:ListItem>Breakfast</asp:ListItem>
                                <asp:ListItem>Dessert</asp:ListItem>
                                <asp:ListItem>Dinner</asp:ListItem>
                                <asp:ListItem>Lunch</asp:ListItem>
                                <asp:ListItem>Snack</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Title</b></td>
                    </tr>
                    <tr>
                        <td id="new_recipe_title">
                            <asp:TextBox ID="TextBoxNewRecipeTitle" runat="server" Width="500px"></asp:TextBox></td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorRecipeTitle" runat="server" ErrorMessage="Please enter a title" SetFocusOnError="True" ControlToValidate="TextBoxNewRecipeTitle" Text="Please enter a title" ForeColor="Red" ValidationGroup="validRecipe"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Description</b></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TextBoxNewRecipeDescription" runat="server" TextMode="MultiLine" Rows="10" Width="500px"></asp:TextBox></td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorRecipeDescription" runat="server" ErrorMessage="Please enter a description" ControlToValidate="TextBoxNewRecipeDescription" Text="Please enter a description" ForeColor="Red" ValidationGroup="validRecipe"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Table ID="TableIngredients" runat="server" Width="500px">
                                <asp:TableHeaderRow>
                                    <asp:TableHeaderCell>Ingredient</asp:TableHeaderCell>
                                    <asp:TableHeaderCell>Amount</asp:TableHeaderCell>
                                    <asp:TableHeaderCell>Measurement</asp:TableHeaderCell>
                                    <asp:TableHeaderCell></asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                            </asp:Table>
                            <asp:Button ID="ButtonAddIngredients" runat="server" Text="Add Ingredient" CausesValidation="False" UseSubmitBehavior="False" CssClass="addIngredientsButton" BackColor="#3399FF" BorderStyle="None" ForeColor="White" />
                            <asp:SqlDataSource ID="SqlDataSourceMeasurement" runat="server" ConnectionString="<%$ ConnectionStrings:caffeinated_dbConnectionString %>" SelectCommand="SELECT * FROM [Measurement] ORDER BY [long_name]"></asp:SqlDataSource>
                        </td>
                        <td>
                            <asp:CustomValidator ID="CustomValidatorIngredientsTextboxes" runat="server" ErrorMessage="You must add at least 1 ingredient" ValidationGroup="validRecipe" Visible="False"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Directions</b></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="TextBoxNewRecipeDirections" runat="server" TextMode="MultiLine" Rows="10" Width="500px"></asp:TextBox></td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorRecipeDirections" runat="server" ErrorMessage="Please enter the directions" ControlToValidate="TextBoxNewRecipeDirections" ForeColor="Red" Text="Please enter the directions" ValidationGroup="validRecipe"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Privacy:</b>
                            <asp:RadioButtonList ID="RadioButtonListPublicPrivateRecipe" runat="server">
                                <asp:ListItem Selected="True">Public</asp:ListItem>
                                <asp:ListItem>Private</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="ValidationSummaryRecipes" ValidationGroup="validRecipe" runat="server" DisplayMode="List" ForeColor="Red" />
                        </td>
                    </tr>
                </table>
                <div style='float: right;'>
                    <asp:Button ID="ButtonAdd" runat="server" Text="Add Recipe" BackColor="Green" BorderStyle="None" ForeColor="White" OnClick="ButtonSubmitRecipe_Click" Height="21px" ValidationGroup="validRecipe" />&nbsp;
                    <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" BackColor="Red" BorderStyle="None" ForeColor="White" OnClick="ButtonCancel_Click" PostBackUrl="~/protected/recipes.aspx" CausesValidation="False" />
                </div>
                <p style="clear: both; margin: 0; padding-bottom: 5px;"></p>
            </div>
            <div id="recipe_view_body" runat="server" visible="false">
                <asp:Button ID="ButtonReturnToRecipes" runat="server" Text="Back to Recipes" BackColor="Green" BorderStyle="None" CssClass="backToRecipesButton" ForeColor="White" OnClick="ButtonSubmissionConfirm_Click" />
                <br />
                <div id="recipe_view">
                    <p id="recipe_label_title">
                        <asp:Label ID="LabelRecipeTitle" runat="server" Text="Title"></asp:Label>
                    </p>
                    <p id="recipe_label_date">
                        <asp:Label ID="LabelRecipeDate" runat="server" Text="Date"></asp:Label>
                    </p>
                    <p id="recipe_username">
                        By user:
                        <asp:Label ID="LabelRecipeUsername" runat="server" Text="Username" ForeColor="#ff471a" Font-Bold="True"></asp:Label>
                    </p>
                    <p id="recipe_rating">
                        <b>Rating:</b>
                        <asp:Label ID="LabelRecipeRating" runat="server" Text=""></asp:Label>
                    </p>
                    <p id="recipe_rating_status">
                        <asp:Label ID="LabelRecipeRatingStatus" runat="server" Text=""></asp:Label>
                    </p>
                    <p id="recipe_label_description">
                        <asp:Label ID="LabelRecipeDescription" runat="server" Text="Description"></asp:Label>
                    </p>
                    <p id="recipe_ingredients_title">
                        <asp:Label ID="LabelRecipeIngredientsTitle" runat="server" Text="Ingredients"></asp:Label>
                    </p>
                    <p id="recipe_table">
                        <asp:Table ID="TableRecipeIngredients" runat="server" BorderStyle="Solid">
                            <asp:TableRow BorderStyle="Solid">
                                <asp:TableHeaderCell BackColor="#0099FF" ForeColor="White">Name</asp:TableHeaderCell>
                                <asp:TableHeaderCell BackColor="#0099FF" ForeColor="White">Amount</asp:TableHeaderCell>
                                <asp:TableHeaderCell BackColor="#0099FF" ForeColor="White">Measurement</asp:TableHeaderCell>
                            </asp:TableRow>
                        </asp:Table>
                    </p>
                    <p id="recipe_label_directions_title">
                        <asp:Label ID="LabelRecipeDirectionsTitle" runat="server" Text="Directions"></asp:Label>
                    </p>
                    <p id="recipe_label_directions">
                        <asp:Label ID="LabelRecipeDirections" runat="server" Text="Directions"></asp:Label>
                    </p>
                </div>
                <div id="rating_view">
                    <asp:Button ID="ButtonThumbsUp" runat="server" Text="Delicious" BackColor="Green" BorderStyle="None" ForeColor="White" OnClick="ButtonThumbs_Click" CommandArgument="1" />
                    <asp:Button ID="ButtonThumbsDown" runat="server" Text="Yuck" BackColor="Red" BorderStyle="None" ForeColor="White" OnClick="ButtonThumbs_Click" CommandArgument="0" style="height: 21px" />
                </div>
                <div id="comments_view">
                    <div id="full_recipe_comments" class="recipe_header" style="width: 53%; padding: 0 50px 0 50px; font-weight: bold;">
                        <h2 style="margin: 0; padding: 0;">Comments</h2>
                    </div>
                    <div id="full_recipe_comments_body">
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
            </div>
            <div id="recipe_confirmation_body" runat="server" visible="false">
                <h3 style="text-align: center;">Your recipe has been added!</h3>
                <asp:Button ID="ButtonSubmissionConfirm" runat="server" Text="Back to Recipes" BackColor="Green" BorderStyle="None" CssClass="backToRecipesButton" ForeColor="White" OnClick="ButtonSubmissionConfirm_Click" />
            </div>
            <div id="recipe_failed_body" runat="server" visible="false">
                <h3 style="text-align: center;">An error has occured</h3>
                <asp:Button ID="ButtonFailedConfirm" runat="server" Text="Back to Recipes" BackColor="Green" BorderStyle="None" CssClass="backToRecipesButton" ForeColor="White" OnClick="ButtonSubmissionConfirm_Click" />
            </div>
        </div>
    </div>
    <div id="my_recipes">
        <h2>My Recipes</h2>
        <div id="right_body">
            <asp:ListView ID="ListViewMyRecipes" runat="server" DataSourceID="SqlDataSourceCaffeinated_MyRecipes">
                <ItemTemplate>
                    <div id="my_recipes_template">
                        <asp:Label ID="imageLabel" runat="server" Text='<%# Eval("recipe_image") %>' />
                        <h3 class="recipe_title">
                            <asp:Label ID="titleLabel" runat="server" Text='<%# Eval("title") %>' />
                        </h3>
                        <h5 class="recipe_date">
                            <asp:Label ID="dateLabel" runat="server" Text='<%# Eval("recipe_date", "{0:MMMM d, yyyy}") %>' />
                        </h5>
                        <br />
                        <p class="recipe_description">
                            <asp:Label ID="descriptionLabel" runat="server" Text='<%# Eval("recipe_description") %>' />
                        </p>
                        <span class="read_more">
                            <asp:Button ID="ButtonReadMoreMine" runat="server" Text="Read More" BackColor="#3366FF" ForeColor="White" BorderStyle="None" OnClick="ButtonReadMore_Click" CommandArgument='<%# Eval("recipe_id") %>' />
                        </span>
                        <span class="delete_recipe">
                            <asp:Button ID="ButtonDeleteRecipe" runat="server" Text="Delete" BackColor="Red" ForeColor="White" BorderStyle="None" OnClick="ButtonDeleteRecipe_Click" CommandArgument='<%# Eval("recipe_id") %>' />
                        </span>
                        <p style="clear: both; margin: 0; padding-bottom: 5px;"></p>
                    </div>
                    <!--recipe_id:
                        <asp:Label ID="recipe_idLabel" runat="server" Text='<%# Eval("recipe_id") %>' />
                        <br />
                        customer_id:
                        <asp:Label ID="customer_idLabel" runat="server" Text='<%# Eval("customer_id") %>' />
                        <br />
                        <asp:Label ID="usernameLabel" runat="server" Text='<%# Eval("username") %>' />

                            category:
                        <asp:Label ID="categoryLabel" runat="server" Text='<%# Eval("category") %>' />
                        <br />
                            directions:
                        <asp:Label ID="directionsLabel" runat="server" Text='<%# Eval("directions") %>' />
                        <br />
                            private:
                        <asp:Label ID="privateLabel" runat="server" Text='<%# Eval("recipe_privacy") %>' />
                        <br />
                    -->
                </ItemTemplate>
            </asp:ListView>
            <p style="text-align: center;">
                <asp:DataPager ID="DataPagerMyRecipes" runat="server" PagedControlID="ListViewMyRecipes" PageSize="10">
                    <Fields>
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField />
                    </Fields>
                </asp:DataPager>
            </p>
            <div id="add_recipe_template">
                <p style="text-align: center;">
                    <asp:Button ID="ButtonAddRecipe" runat="server" Text="Add a New Recipe" BackColor="#FF6600" BorderStyle="None" ForeColor="White" OnClick="ButtonAddRecipe_Click" />
                </p>
                <p style="clear: both; margin: 0; padding-bottom: 5px;"></p>
            </div>
            <asp:SqlDataSource ID="SqlDataSourceCaffeinated_MyRecipes" runat="server" ConnectionString="<%$ ConnectionStrings:caffeinated_dbConnectionString %>" SelectCommand="SELECT r.recipe_id, r.customer_id, r.recipe_date, r.category, r.title, r.recipe_description, r.directions, r.recipe_image, r.recipe_privacy, c.username FROM [Recipes] r JOIN [Customers] c ON r.customer_id = c.customer_id WHERE c.username = @username;">
                <SelectParameters>
                    <asp:Parameter DefaultValue="" Name="username" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </div>
</asp:Content>

