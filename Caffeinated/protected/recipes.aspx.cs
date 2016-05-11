// I, Cameron Winters, student number 000299896, certify that this material is my
// original work. No other person's work has been used without due
// acknowledgement and I have not made my work available to anyone else.

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Linq;

public partial class protected_recipes : System.Web.UI.Page
{
    #region Class Variables

    string connString = WebConfigurationManager.ConnectionStrings["caffeineDB"].ConnectionString;
    string username = System.Web.HttpContext.Current.User.Identity.Name;
    int recipe_id;
    List<TextBox> ingredients = new List<TextBox>();
    List<TextBox> amounts = new List<TextBox>();
    List<DropDownList> measurements = new List<DropDownList>();
    List<Button> deletes = new List<Button>();
    List<RequiredFieldValidator> ingredientsValidators = new List<RequiredFieldValidator>();
    List<RequiredFieldValidator> amountsValidators = new List<RequiredFieldValidator>();

    #endregion

    #region Page Initialization Methods

    /// <summary>
    /// At page initializion we set the session data, if none exists, and load the class variables.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Init(object sender, EventArgs e)
    {
        SqlDataSourceCaffeinated_MyRecipes.SelectParameters["username"].DefaultValue = User.Identity.Name;
        SqlDataSourceCaffeinated.SelectParameters["category"].DefaultValue = DropDownListCategories.SelectedValue;

        if (Session["ingredients"] == null) { Session["ingredients"] = new List<TextBox>(); }
        if (Session["amounts"] == null) { Session["amounts"] = new List<TextBox>(); }
        if (Session["measurements"] == null) { Session["measurements"] = new List<DropDownList>(); }
        if (Session["deletes"] == null) { Session["deletes"] = new List<Button>(); }
        if (Session["ingredientsValidators"] == null) { Session["ingredientsValidators"] = new List<RequiredFieldValidator>(); }
        if (Session["amountsValidators"] == null) { Session["amountsValidators"] = new List<RequiredFieldValidator>(); }
        if (Session["recipe_id"] == null) { Session["recipe_id"] = 0; }
        if (Session["comment_sql"] == null)
        {
            Session["comment_sql"] = "";
        }
        else
        {
            SqlDataSourceComments.SelectCommand = Session["comment_sql"].ToString();
        }

        ingredients = (List<TextBox>)Session["ingredients"];
        amounts = (List<TextBox>)Session["amounts"];
        measurements = (List<DropDownList>)Session["measurements"];
        deletes = (List<Button>)Session["deletes"];
        ingredientsValidators = (List<RequiredFieldValidator>)Session["ingredientsValidators"];
        amountsValidators = (List<RequiredFieldValidator>)Session["amountsValidators"];
        recipe_id = Convert.ToInt32(Session["recipe_id"]);
    }


    /// <summary>
    /// At page load we check whether or not the user clicked one of the dynamically created buttons.  If they did,
    /// the event will be picked up here and the appropriate function will be called.  This is necessary because 
    /// the dynamically created buttons cannot be wired up.  We also load the dynamically created items, which are
    /// stored across page loads via session.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            string clicked = Request.Params["__EVENTTARGET"].ToString();

            if (clicked.Contains("ButtonDeleteIngredient"))
            {
                removeIngredient(clicked);
            }
            else if (clicked.Contains("ButtonAddIngredients"))
            {
                addIngredientsItems(true); // "true" because this is a button click event
            }
            else
            {
                addIngredientsItems(false); // "false" because this is not a button click event
            }

            // Show the table if the user has added an ingredients row.  Hide if there are no rows.
            if (ingredients.Count == 0)
            {
                TableIngredients.Visible = false;
            }
            else
            {
                TableIngredients.Visible = true;
            }
        }

        if (Session["recipe_comment_submit"] != null)
        {
            showForm("recipe_full");
            //Session.Clear();
        }
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Update the All Recipes datalist when the user selects a new category.
    /// </summary>
    /// <param name="sender">DropDownListCategories</param>
    /// <param name="e">User selected a dropdownlist item</param>
    protected void SelectCategory(object sender, EventArgs e)
    {
        SqlDataSourceCaffeinated.SelectParameters["category"].DefaultValue = DropDownListCategories.SelectedValue;
        ListViewRecipes.DataBind();
    }


    /// <summary>
    /// When the user clicks the "Add Recipe" button the "showForm" method is
    /// called and the user will be presented with the "New Recipe" form.
    /// </summary>
    /// <param name="sender">ButtonAddRecipe</param>
    /// <param name="e">User clicked the button</param>
    protected void ButtonAddRecipe_Click(object sender, EventArgs e)
    {
        ButtonAddRecipe.Enabled = false;
        showForm("new_recipe");
    }


    /// <summary>
    /// When the user clicks the "Cancel" button on the "New Recipe" page the showForm method 
    /// will be called and the user will be presented with the "All Recipes" page.
    /// </summary>
    /// <param name="sender">ButtonCancel</param>
    /// <param name="e">User clicked the button</param>
    protected void ButtonCancel_Click(object sender, EventArgs e)
    {
        showForm("all_recipes");
    }


    /// <summary>
    /// When the user clicks the "Read More" button they will be shown the detailed information form for 
    /// the recipe.  If the user is the owner of the recipe they will be given the option to delete the recipe.
    /// </summary>
    /// <param name="sender">ButtonReadMore</param>
    /// <param name="e">User clicked the button</param>
    protected void ButtonReadMore_Click(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        recipe_id = Convert.ToInt32(button.CommandArgument.ToString());
        Session["recipe_id"] = recipe_id;
        showForm("recipe_full", sender);
        showRecipeView();
    }


    /// <summary>
    /// When the user clicks the comment submit button this event is fired.  We get the recipe id value by parsing the
    /// CommandArgument of the button and call the SubmitComment method if the comment textbox is not null.
    /// </summary>
    /// <param name="sender">Submit Comment Button</param>
    /// <param name="e">Button Click Event</param>
    protected void ButtonSubmitComment_Click(object sender, EventArgs e)
    {
        //Button btn = (Button)(sender);
        //int recipe_id = Convert.ToInt32(btn.CommandArgument);

        if (TextBoxNewComment.Text != string.Empty)
        {
            SubmitComment(recipe_id);
            showForm("recipe_full");
            showRecipeView();
            Session["recipe_comment_submit"] = sender;
            Response.Redirect("recipes.aspx");
        }
    }


    /// <summary>
    /// When the user clicks the "Delete" recipe button this event is fired.  The "RemoveRecipe" stored procedure
    /// is called and the activity is logged.
    /// </summary>
    /// <param name="sender">Delete Recipe button</param>
    /// <param name="e">Click Event</param>
    protected void ButtonDeleteRecipe_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        recipe_id = Convert.ToInt32(btn.CommandArgument.ToString());
        Dictionary<string, string> recipeInfo = getRecipe();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("RemoveRecipe", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@recipe_id", recipe_id);
        cmd.Parameters.AddWithValue("@username", username);

        try
        {
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                LogActivity("Deleted recipe '" + recipeInfo["title"] + "'.");
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }

        Response.Redirect("~/protected/recipes.aspx");
    }


    /// <summary>
    /// Perform validation on the user's recipe input and submit to the database.
    /// </summary>
    /// <param name="sender">ButtonSubmitRecipe</param>
    /// <param name="e">User clicked the button</param>
    protected void ButtonSubmitRecipe_Click(object sender, EventArgs e)
    {
        List<string> ingredientList = new List<string>();
        List<short> amountList = new List<short>();
        List<short> measurementList = new List<short>();
        int count = 0;

        // Obtain the user's added ingredient information
        foreach (TableRow row in TableIngredients.Rows)
        {
            short amount;
            short measurement;
            TextBox ingredientsTextBox = (TextBox)row.FindControl("TextBoxIngredients" + count);
            TextBox amountsTextBox = (TextBox)row.FindControl("TextBoxAmount" + count);
            DropDownList measurementDropDown = (DropDownList)row.FindControl("DropDownListMeasurement" + count);

            if (ingredientsTextBox != null && amountsTextBox != null && measurementDropDown != null)
            {
                Int16.TryParse(amountsTextBox.Text, out amount);
                Int16.TryParse(measurementDropDown.SelectedValue, out measurement);
                ingredientList.Add(ingredientsTextBox.Text);
                amountList.Add(amount);
                measurementList.Add(measurement);
            }
            count++;
        }

        // Ensure the user has added at least one ingredient
        if (ingredientList.Count > 0)
        {
            CustomValidatorIngredientsTextboxes.IsValid = true;
            addRecipeToDatabase(ingredientList, amountList, measurementList);
        }
        else
        {
            CustomValidatorIngredientsTextboxes.IsValid = false;
        }
    }


    /// <summary>
    /// When the user clicks the "Back to Recipes" button after submitting a recipe.  Clears the session data
    /// for good measure and refreshes the page.
    /// </summary>
    /// <param name="sender">ButtonSubmissionConfirm</param>
    /// <param name="e">User clicked the button</param>
    protected void ButtonSubmissionConfirm_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect(Request.RawUrl);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Gets the recipe and ingredient information for the selected recipe and populate the labels on 
    /// the recipe view
    /// </summary>
    private void showRecipeView()
    {
        Dictionary<string, string> recipe = getRecipe();
        List<List<string>> ingredients = getIngredients();
        List<string> ingredientNames = new List<string>();
        List<string> ingredientMeasurements = new List<string>();
        List<string> ingredientAmounts = new List<string>();
        DateTime date = Convert.ToDateTime(recipe["date"]);
        string category = recipe["category"];
        string title = recipe["title"];
        string descripton = recipe["description"];
        string directions = recipe["directions"];
        bool privacy = Convert.ToBoolean(recipe["privacy"]);
        string recipeUsername = recipe["username"];
        List<int> ratingInfo = getRatingInfo();
        int rating = 0;
        int ratingTotal = ratingInfo.Count;
        double ratingStatus = 0;

        // If ingredients array has values, populate the names, measurements, and amounts arrays
        if (ingredients.Count > 0)
        {
            ingredientNames = ingredients[0];
            ingredientMeasurements = ingredients[1];
            ingredientAmounts = ingredients[2];
        }

        foreach (int currRating in ratingInfo)
        {
            if (currRating == 1)
            {
                rating++;
            }
        }

        // Update rating label and status
        if (ratingTotal > 0)
        {
            LabelRecipeRating.Text = rating.ToString() + "/" + ratingTotal.ToString() + " thumbs up!";
        }
        else
        {
            LabelRecipeRating.Text = "No ratings available yet.";
        }

        // If ratings are available we'll determine the % of thumbs up and display the status
        if (ratingTotal > 0)
        {
            ratingStatus = rating / ratingTotal * 100;

            if (ratingStatus >= 80)
            {
                LabelRecipeRatingStatus.ForeColor = System.Drawing.Color.Green;
                LabelRecipeRatingStatus.Text = "Great!";
            }
            else if (ratingStatus >= 70 && ratingStatus < 80)
            {
                LabelRecipeRatingStatus.ForeColor = System.Drawing.Color.Yellow;
                LabelRecipeRatingStatus.Text = "Decent";
            }
            else if (ratingStatus >= 50 && ratingStatus < 70)
            {
                LabelRecipeRatingStatus.ForeColor = System.Drawing.Color.Orange;
                LabelRecipeRatingStatus.Text = "Mediocre";
            }
            else
            {
                LabelRecipeRatingStatus.ForeColor = System.Drawing.Color.Red;
                LabelRecipeRatingStatus.Text = "Bad!";
            }
        }

        // Update recipe information
        LabelRecipeDate.Text = date.ToLongDateString().ToString();
        LabelRecipeDescription.Text = descripton;
        LabelRecipeDirections.Text = directions;
        LabelRecipeTitle.Text = title;
        LabelRecipeUsername.Text = recipeUsername;

        // Populate ingredients table
        for (int i = 0; i < ingredientNames.Count; i++)
        {
            TableRow ingRow = new TableRow();
            TableCell nameCell = new TableCell();
            TableCell amtCell = new TableCell();
            TableCell measCell = new TableCell();
            nameCell.Text = ingredientNames[i];
            nameCell.HorizontalAlign = HorizontalAlign.Left;
            amtCell.Text = ingredientAmounts[i];
            amtCell.HorizontalAlign = HorizontalAlign.Center;
            measCell.Text = ingredientMeasurements[i];
            measCell.HorizontalAlign = HorizontalAlign.Left;
            TableRecipeIngredients.Rows.Add(ingRow);
            ingRow.Cells.Add(nameCell);
            ingRow.Cells.Add(amtCell);
            ingRow.Cells.Add(measCell);
        }
    }


    /// <summary>
    /// Get the rating information about the recipe.  The total number of thumbs up given to the recipe
    /// will be calculated based on "number of thumbs up given" / "total number of ratings given".  Note
    /// that a "thumbs_up" value of 1 is considered a thumbs up, and a value of 0 is considered a thumbs down
    /// </summary>
    /// <returns>Rating Information</returns>
    private List<int> getRatingInfo()
    {
        List<int> ratingInfo = new List<int>();
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("GetAllRatings", conn);
        SqlDataReader rdr;
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@recipe_id", recipe_id);

        try
        {
            using (conn)
            {
                conn.Open();
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ratingInfo.Add(Convert.ToInt16(rdr["thumbs_up"]));
                }
                conn.Close();
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }

        return ratingInfo;
    }


    /// <summary>
    /// Gets the recipe information for the chosen recipe
    /// </summary>
    /// <returns>Dictionary containing recipe information</returns>
    private Dictionary<string, string> getRecipe()
    {
        Dictionary<string, string> recipeInfo = new Dictionary<string, string>();
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("GetRecipe", conn);
        SqlDataReader rdr;
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@recipe_id", recipe_id);

        try
        {
            using (conn)
            {
                conn.Open();
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    recipeInfo.Add("date", Convert.ToString(rdr["recipe_date"]));
                    recipeInfo.Add("category", Convert.ToString(rdr["category"]));
                    recipeInfo.Add("title", Convert.ToString(rdr["title"]));
                    recipeInfo.Add("description", Convert.ToString(rdr["recipe_description"]));
                    recipeInfo.Add("directions", Convert.ToString(rdr["directions"]));
                    recipeInfo.Add("privacy", Convert.ToString(rdr["recipe_privacy"]));
                    recipeInfo.Add("username", Convert.ToString(rdr["username"]));
                    break;
                }
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }
        return recipeInfo;
    }


    /// <summary>
    /// Get ingrediens information.  This is done separately from recipes because the ingredients
    /// are stored in their own table
    /// </summary>
    /// <returns>A list of string list objects containing the ingredient information for the recipe</returns>
    private List<List<string>> getIngredients()
    {
        // "ingredients" is a List to contain the three List arrays
        List<List<string>> ingredients = new List<List<string>>();
        List<string> ingredientName = new List<string>();
        List<string> ingredientMeasurement = new List<string>();
        List<string> ingredientAmount = new List<string>();

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("GetRecipe", conn);
        SqlDataReader rdr;
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@recipe_id", recipe_id);

        try
        {
            using (conn)
            {
                conn.Open();
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ingredientName.Add(Convert.ToString(rdr["name"]));
                    ingredientAmount.Add(Convert.ToString(rdr["amount"]));
                    ingredientMeasurement.Add(Convert.ToString(rdr["long_name"]));
                }
                conn.Close();
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }

        ingredients.Add(ingredientName);
        ingredients.Add(ingredientMeasurement);
        ingredients.Add(ingredientAmount);
        return ingredients;
    }


    /// <summary>
    /// Adds a new table row to the "TableIngredient" table and adds dynamically created
    /// controls.  Allows user to add a new ingredient when creating a recipe.
    /// </summary>
    /// <param name="isNewIngredient">True if user is inserting a new ingredient.  False if just
    /// need to populate the ingredients table, such as during page_load</param>
    private void addIngredientsItems(bool isNewIngredient)
    {
        // If nextAvailable returns -1 it means the user has reached the maximum number of ingredients, 99.
        if (nextAvailableElement() > -1)
        {
            ButtonAddIngredients.Enabled = true;
            if (isNewIngredient)
            {
                addIngredientsTextBox();
            }
        }
        else
        {
            ButtonAddIngredients.Enabled = false;
        }

        int index = 0;
        foreach (TextBox ingredient in ingredients)
        {
            TableRow ingRow = new TableRow();
            TableCell ingCell = new TableCell();
            TableCell amtCell = new TableCell();
            TableCell measCell = new TableCell();
            TableCell deleteCell = new TableCell();
            ingCell.Controls.Add(ingredientsValidators[index]);
            ingCell.Controls.Add(ingredient);
            ingCell.HorizontalAlign = HorizontalAlign.Center;
            amtCell.Controls.Add(amountsValidators[index]);
            amtCell.Controls.Add(amounts[index]);
            amtCell.HorizontalAlign = HorizontalAlign.Center;
            measCell.Controls.Add(measurements[index]);
            measCell.HorizontalAlign = HorizontalAlign.Center;
            deleteCell.Controls.Add(deletes[index]);
            deleteCell.HorizontalAlign = HorizontalAlign.Center;
            TableIngredients.Rows.Add(ingRow);
            ingRow.Cells.Add(ingCell);
            ingRow.Cells.Add(amtCell);
            ingRow.Cells.Add(measCell);
            ingRow.Cells.Add(deleteCell);
            index++;
        }
    }


    /// <summary>
    /// Creates new controls for adding an ingredient row to the "TableIngredient" table.
    /// </summary>
    private void addIngredientsTextBox()
    {
        // If the nextAvailable() method returns -1, that means the user has 99 ingredients and they've reached
        // the max number of ingredients
        // Variable declarations
        int ingredientsCount = nextAvailableElement();
        int amountsCount = nextAvailableElement();
        int measCount = nextAvailableElement();
        int deletesCount = nextAvailableElement();
        TextBox ingTextbox = new TextBox();
        TextBox amtTextbox = new TextBox();
        DropDownList measDropdown = new DropDownList();
        RequiredFieldValidator ingValidator = new RequiredFieldValidator();
        RequiredFieldValidator amtValidator = new RequiredFieldValidator();
        Button deleteButton = new Button();

        // Set form values
        ingTextbox.CssClass = "ingredientsText";
        ingTextbox.ID = "TextBoxIngredients" + ingredientsCount;
        ingTextbox.TextMode = TextBoxMode.SingleLine;
        ingTextbox.ValidationGroup = "validRecipe";

        amtTextbox.CssClass = "ingredientsText";
        amtTextbox.ID = "TextBoxAmount" + amountsCount;
        amtTextbox.Width = 30;
        amtTextbox.TextMode = TextBoxMode.Number;
        amtTextbox.MaxLength = 3;
        amtTextbox.ValidationGroup = "validRecipe";

        measDropdown.ID = "DropDownListMeasurement" + measCount;
        measDropdown.DataSourceID = "SqlDataSourceMeasurement";
        measDropdown.DataTextField = "long_name";
        measDropdown.DataValueField = "measurement_id";

        deleteButton.ID = "ButtonDeleteIngredient" + deletesCount;
        deleteButton.Text = "Remove";
        deleteButton.BackColor = System.Drawing.Color.Red;
        deleteButton.ForeColor = System.Drawing.Color.White;
        deleteButton.BorderStyle = BorderStyle.None;
        deleteButton.CausesValidation = false;
        deleteButton.UseSubmitBehavior = false;

        // Set validation values
        ingValidator.ID = "TextValidatorIngredients" + ingredientsCount;
        ingValidator.ForeColor = System.Drawing.Color.Red;
        ingValidator.ControlToValidate = "TextBoxIngredients" + ingredientsCount;
        ingValidator.ErrorMessage = "Please enter the name of the ingredient";
        ingValidator.Text = "*";
        ingValidator.SetFocusOnError = true;
        ingValidator.ValidationGroup = "validRecipe";
        ingValidator.Display = ValidatorDisplay.Dynamic;

        amtValidator.ID = "TextValidatorAmounts" + amountsCount;
        amtValidator.ForeColor = System.Drawing.Color.Red;
        amtValidator.ControlToValidate = "TextBoxAmount" + amountsCount;
        amtValidator.ErrorMessage = "Please enter the ingredient amount";
        amtValidator.Text = "*";
        amtValidator.SetFocusOnError = true;
        amtValidator.ValidationGroup = "validRecipe";
        amtValidator.Display = ValidatorDisplay.Dynamic;

        // Add the controls to the arrays
        ingredients.Add(ingTextbox);
        amounts.Add(amtTextbox);
        measurements.Add(measDropdown);
        deletes.Add(deleteButton);
        ingredientsValidators.Add(ingValidator);
        amountsValidators.Add(amtValidator);

        // Update session data
        Session["ingredients"] = ingredients;
        Session["amounts"] = amounts;
        Session["measurements"] = measurements;
        Session["deletes"] = deletes;
        Session["ingredientsValidators"] = ingredientsValidators;
        Session["amountsValidators"] = amountsValidators;

        ButtonAddIngredients.Enabled = true;
    }


    /// <summary>
    /// Inserts the user's new recipe into the database.
    /// </summary>
    /// <param name="ingredientList">List of the ingredient names the user has entered</param>
    /// <param name="amountList">List of the amounts the user has entered</param>
    /// <param name="measurementList">List of the measurement types the user has enetered</param>
    private void addRecipeToDatabase(List<string> ingredientList, List<short> amountList, List<short> measurementList)
    {
        int recipe_id = insertRecipe();

        if (recipe_id > -1)
        {
            if (insertIngredients(ingredientList, amountList, measurementList, recipe_id))
            {
                showForm("recipe_confirmation");
            }
            else
            {
                showForm("recipe_failed");
            }
        }
        else
        {
            showForm("recipe_failed");
        }
    }


    /// <summary>
    /// Inserts the recipe data into the Recipe table.
    /// </summary>
    /// <returns>One of two possible values: 1. recipe id; 2. -1 if the recipe id couldn't be parsed</returns>
    private int insertRecipe()
    {
        // Variables
        bool privacy = false;
        bool idIsParsed = false;
        int recipe_id = 0;
        string category = DropDownListNewCategories.SelectedValue;
        string title = TextBoxNewRecipeTitle.Text;
        string description = TextBoxNewRecipeDescription.Text;
        string directions = TextBoxNewRecipeDirections.Text;
        DateTime date = DateTime.Now;

        if (RadioButtonListPublicPrivateRecipe.SelectedValue == "Private")
        {
            privacy = true;
        }

        // Database connection string and parameters
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("InsertRecipe", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@date", date);
        cmd.Parameters.AddWithValue("@category", category);
        cmd.Parameters.AddWithValue("@title", title);
        cmd.Parameters.AddWithValue("@description", description);
        cmd.Parameters.AddWithValue("@directions", directions);
        cmd.Parameters.AddWithValue("privacy", privacy);

        // Insert into Recipes table
        try
        {
            using (conn)
            {
                object id;
                conn.Open();
                id = cmd.ExecuteScalar();
                idIsParsed = Int32.TryParse(id.ToString(), out recipe_id);
                conn.Close();
                LogActivity("Recipe '" + title + "' added.");
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }

        // Return the recipe_id if it was properly parsed to an int32, otherwise -1 to indicate the parse failed
        if (idIsParsed)
        {
            return recipe_id;
        }
        else
        {
            return -1;
        }
    }


    /// <summary>
    /// Inserts the list of ingredients the user has added to their recipe.
    /// </summary>
    /// <param name="ingredientList">List of the ingredient names the user has entered.</param>
    /// <param name="amountList">List of the amounts the user has entered.</param>
    /// <param name="measurementList">List of the measurement types the user has enetered.</param>
    /// <param name="recipe_id">The id of the recently inserted recipe.</param>
    /// <returns>True if the ingredients were successfully inserted into the DB.  False otherwise.</returns>
    private bool insertIngredients(List<string> ingredientList, List<short> amountList, List<short> measurementList, int recipe_id)
    {
        bool ingredientsIsInserted = false;

        // Insert the list of ingredients into Ingredients table
        for (int i = 0; i < ingredientList.Count; i++)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("InsertIngredient", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@recipe_id", recipe_id);
            cmd.Parameters.AddWithValue("@measurement_id", measurementList[i]);
            cmd.Parameters.AddWithValue("@name", ingredientList[i]);
            cmd.Parameters.AddWithValue("@amount", amountList[i]);

            try
            {
                using (conn)
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    ingredientsIsInserted = true;
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc);
                ingredientsIsInserted = false;
            }
        }
        return ingredientsIsInserted;
    }


    /// <summary>
    /// Changes the current form on the page.
    /// </summary>
    /// <param name="body">The name of the form the user would like to switch to.</param>
    private void showForm(string body, object sender = null)
    {
        all_recipes_body.Visible = false;
        new_recipe_body.Visible = false;
        recipe_view_body.Visible = false;
        recipe_confirmation_body.Visible = false;
        recipe_failed_body.Visible = false;

        switch (body)
        {
            case "all_recipes":
                LabelLeftSide.Text = "New and Trending Recipes";
                all_recipes_body.Visible = true;
                ButtonAddRecipe.Enabled = true;
                Session.Clear();
                break;
            case "new_recipe":
                DropDownListNewCategories.SelectedValue = "Beverage";
                TextBoxNewRecipeTitle.Text = string.Empty;
                TextBoxNewRecipeDescription.Text = string.Empty;
                TextBoxNewRecipeDirections.Text = string.Empty;
                RadioButtonListPublicPrivateRecipe.SelectedValue = "Public";
                LabelLeftSide.Text = "Add a New Recipe";
                new_recipe_body.Visible = true;
                break;
            case "recipe_full":
                Session["comment_sql"] = "SELECT [username], [comment_date], [recipe_id], [comment] FROM [Comments] JOIN [Customers] ON [Customers].customer_id = [Comments].customer_id WHERE [recipe_id] = " + recipe_id + " ORDER BY [comment_date] DESC";
                SqlDataSourceComments.SelectCommand = Session["comment_sql"].ToString();
                recipe_view_body.Visible = true;
                break;
            case "recipe_confirmation":
                LabelLeftSide.Text = "Recipe Confirmation";
                recipe_confirmation_body.Visible = true;
                break;
            case "recipe_failed":
                LabelLeftSide.Text = "Recipe Not Entered";
                recipe_failed_body.Visible = true;
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// Returns the next available element in the ingredients list.  This keeps the list clean as the user
    /// adds and deletes ingredient rows.
    /// </summary>
    /// <returns>One of two possible values: 1. the next available element; 2. -1 if there are already 100
    /// ingredients in the list.</returns>
    private int nextAvailableElement()
    {
        int nextAvailable = ingredients.Count;
        List<int> elements = new List<int>();
        int num;

        // There must be fewer than 100 ingredients
        if (nextAvailable < 100)
        {
            foreach (TextBox ingredient in ingredients)
            {
                bool isDoubleDigit = int.TryParse(ingredient.ID.Substring(ingredient.ID.Length - 2, 2), out num);

                if (isDoubleDigit)
                {
                    elements.Add(Convert.ToInt16(ingredient.ID.Substring(ingredient.ID.Length - 2, 2)));
                }
                else
                {
                    elements.Add(Convert.ToInt16(ingredient.ID.Substring(ingredient.ID.Length - 1, 1)));
                }
            }

            elements.Sort();

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i] != i)
                {
                    nextAvailable = i;
                    break;
                }
            }
        }
        else
        {
            nextAvailable = -1;
        }
        return nextAvailable;
    }


    /// <summary>
    /// The SubmitComment method submits the comment to the database.  It then logs the activity.
    /// </summary>
    /// <param name="recipe_id">The id of the recipe the user is submitting a comment to.</param>
    /// <returns>True if comment submitted.  False otherwise.</returns>
    private bool SubmitComment(int recipe_id)
    {
        Dictionary<string, string> recipeInfo = getRecipe();
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("InsertComment", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@date", DateTime.Now);
        cmd.Parameters.AddWithValue("@recipe_id", recipe_id);
        cmd.Parameters.AddWithValue("@comment", TextBoxNewComment.Text);

        try
        {
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            LogActivity("Comment added to " + recipeInfo["title"] + " recipe.");
            return true;
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
            return false;
        }
    }


    /// <summary>
    /// Removes a specified ingredient row
    /// </summary>
    /// <param name="clicked"></param>
    private void removeIngredient(string clicked)
    {
        int num;
        int length;
        bool isDoubleDigit = int.TryParse(clicked.Substring(clicked.Length - 2, 2), out num);
        string element;

        if (isDoubleDigit)
        {
            length = 2;
            element = clicked.Substring(clicked.Length - 2, 2);
        }
        else
        {
            length = 1;
            element = clicked.Substring(clicked.Length - 1, 1);
        }

        for (int i = 0; i < ingredients.Count; i++)
        {
            if (ingredients[i].ID.Substring(ingredients[i].ID.Length - length, length).Contains(element))
            {
                ingredients.RemoveAt(i);
                amounts.RemoveAt(i);
                measurements.RemoveAt(i);
                deletes.RemoveAt(i);
                ingredientsValidators.RemoveAt(i);
                amountsValidators.RemoveAt(i);
                break;
            }
        }
        addIngredientsItems(false);
    }


    /// <summary>
    /// Handles the button click event for both the "Delicious" and the "Yuck" buttons.  We can determine
    /// which was clicked by checking the value of the Command Argument of the sender button.
    /// </summary>
    /// <param name="sender">The "Delicious" or "Yuck" button</param>
    /// <param name="e">Button click</param>
    protected void ButtonThumbs_Click(object sender, EventArgs e)
    {
        Button btn = (Button)(sender);
        int thumb = Convert.ToInt32(btn.CommandArgument);
        Dictionary<string, string> recipeInfo = getRecipe();
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("InsertRating", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@recipe_id", recipe_id);
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@date", DateTime.Now);
        cmd.Parameters.AddWithValue("@thumbs", thumb);

        try
        {
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                if (thumb == 0)
                {
                    LogActivity("Gave recipe '" + recipeInfo["title"] + "' a thumbs down.");
                }
                else
                {
                    LogActivity("Gave recipe '" + recipeInfo["title"] + "' a thumbs up.");
                }
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }

        showRecipeView();
    }


    /// <summary>
    /// Logs the activity for a database event
    /// </summary>
    /// <param name="message">Information about the database event</param>
    protected void LogActivity(string message)
    {
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("InsertActivity", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@date", DateTime.Now);
        cmd.Parameters.AddWithValue("@activity", message);

        try
        {
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }
    }

    #endregion

}