// I, Cameron Winters, student number 000299896, certify that this material is my
// original work. No other person's work has been used without due
// acknowledgement and I have not made my work available to anyone else.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Web.Configuration;
using System.Web.UI.WebControls;

public partial class protected_profile : System.Web.UI.Page
{
    #region Class Variables

    string connString = WebConfigurationManager.ConnectionStrings["caffeineDB"].ConnectionString;
    string username = System.Web.HttpContext.Current.User.Identity.Name;

    #endregion

    #region Page Initialization Methods

    /// <summary>
    /// On Page Initialization, set SelectCommand for the SqlDataSouceBlogs.
    /// </summary>
    /// <param name="sender">Page Initialization</param>
    /// <param name="e">Page Initialization</param>
    protected void Page_Init(object sender, EventArgs e)
    {
        SqlDataSourceBlogs.SelectCommand = "SELECT blog_id, Blogs.customer_id, blog_date, title, content FROM Blogs JOIN Customers ON Customers.customer_id = Blogs.customer_id WHERE username = '" + username + "';";
    }


    /// <summary>
    /// Events that occur before the page is rendered, such as getting the user's images from the directory
    /// </summary>
    protected void Page_PreRender()
    {
        string folder = MapPath("~/protected/Images/") + username;
        DirectoryInfo directory = new DirectoryInfo(folder);
        if (directory.Exists)
        {
            UserImages.DataSource = directory.GetFiles();
            UserImages.DataBind();
        }
    }


    /// <summary>
    /// Events that occur when the page loads, like populating the profile info and profile pic
    /// </summary>
    /// <param name="sender">Page_Load</param>
    /// <param name="e">Page_Load</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ShowActivities();
        LabelTypeMessage.Text = string.Empty;
        LabelPasswordUpdateResponse.Text = string.Empty;

        if (!IsPostBack)
        {
            // Personal info and profile picture
            DisplayProfileInfo();
            DisplayProfilePic();

            // Populate the "Year" dropdown list w/ values ranging from 1900 to the current year
            int currentYear = System.DateTime.Now.Year;
            for (int i = currentYear; i >= 1900; i--)
            {
                dropDownListYear.Items.Add(i.ToString());
            }
        }
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Event for when the user clicks the "Upload Image" button.  Checks filetype and calls the UploadImage
    /// method if filetype is correct
    /// </summary>
    /// <param name="sender">Add Image button</param>
    /// <param name="e">Button Click Event</param>
    protected void AddImageButton_Click(object sender, EventArgs e)
    {
        if (FileUploadImage.HasFile)
        {
            bool hasType = CheckFileType();

            // Upload image to server if correct type chosen and is smaller than 5MB; otherwise return error message
            if (hasType && FileUploadImage.FileBytes.Length < 5242880)
            {
                UploadImage();
            }
            else if (!hasType)
            {
                LabelTypeMessage.ForeColor = System.Drawing.Color.Red;
                LabelTypeMessage.Text = "File type must be JPEG, GIF, or PNG";
            }
            else
            {
                LabelTypeMessage.ForeColor = System.Drawing.Color.Red;
                LabelTypeMessage.Text = "Image must be 5MB or less in size";
            }
        }
    }


    /// <summary>
    /// Event for when the user clicks the "Update" button for the "About Me" section.  Enables/disables the About Me textbox to allow
    /// the user to update their "About Me" section.
    /// </summary>
    /// <param name="sender">About Me</param>
    /// <param name="e">Button Click Event</param>
    protected void ButtonAboutMe_Click(object sender, EventArgs e)
    {
        if (ButtonAboutMe.Text == "Edit")
        {
            TextBoxAboutMe.ReadOnly = false;
            TextBoxAboutMe.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            TextBoxAboutMe.BorderColor = System.Drawing.Color.Green;
            ButtonAboutMe.Text = "Submit";
            ButtonAboutMeCancel.Visible = true;
        }
        else
        {
            UpdateAboutMe();
            TextBoxAboutMe.ReadOnly = true;
            TextBoxAboutMe.Text = GetAboutMe();
            TextBoxAboutMe.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
            ButtonAboutMe.Text = "Edit";
            ButtonAboutMeCancel.Visible = false;
        }
    }


    /// <summary>
    /// Event for cancelling the About Me update.  Sets the text of the About Me box to the what's available in the db and
    /// sets it to read only.
    /// </summary>
    /// <param name="sender">About Me</param>
    /// <param name="e">Button Click Event</param>
    protected void ButtonAboutMeCancel_Click(object sender, EventArgs e)
    {
        TextBoxAboutMe.ReadOnly = true;
        TextBoxAboutMe.Text = GetAboutMe();
        TextBoxAboutMe.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
        ButtonAboutMe.Text = "Edit";
        ButtonAboutMeCancel.Visible = false;
    }


    /// <summary>
    /// Event for when user clicks the Change Profile Photo button.  The button has two states: 1. "Change Profile Photo" and
    /// 2. "Update Profile Photo".  If it's in state 1 when the user clicks it they are able to change their profile picture.  If
    /// it's in state 2 when they click it the profile picture is updated to the newly chosen pic.
    /// </summary>
    /// <param name="sender">ButtonProfilePicture button</param>
    /// <param name="e">Button Click Event</param>
    protected void ButtonProfilePicture_Click(object sender, EventArgs e)
    {
        // User clicks the button when it's in the "Change Profile Photo" state
        if (ButtonProfilePicture.Text == "Change Profile Photo")
        {
            // Populate CheckBoxListProfilePicture w/ a list of images
            PopulateProfileCheckBoxList();
            CheckBoxListProfilePicture.Visible = true;

            // Update buttons
            ButtonProfilePicture.Text = "Update Profile Photo";
            ButtonProfilePicture.BackColor = System.Drawing.Color.Green;
            ButtonProfilePictureCancel.Visible = true;
        }
        // User clicks the button when it's in the "Update Profile Photo" state
        else
        {
            bool picSelected = false;

            // Determine if a picture has been selected
            foreach (ListItem item in CheckBoxListProfilePicture.Items)
            {
                if (item.Selected)
                {
                    picSelected = true;
                    break;
                }
            }

            // If a picture has been selected, update the customer profile in the db w/ the selected image and display it
            if (picSelected)
            {
                UpdateUserProfilePicture();
                DisplayProfilePic();
            }

            // Update buttons
            ButtonProfilePicture.Text = "Change Profile Photo";
            ButtonProfilePicture.BackColor = System.Drawing.Color.Blue;
            ButtonProfilePictureCancel.Visible = false;

            // Hide Checkbox List and clear its contents
            ArrayList itemList = new ArrayList();
            CheckBoxListProfilePicture.Visible = false;
            foreach (ListItem item in CheckBoxListProfilePicture.Items)
            {
                itemList.Add(item);
            }
            foreach (ListItem item in itemList)
            {
                CheckBoxListProfilePicture.Items.Remove(item);
            }
        }
    }


    /// <summary>
    /// Event for cancelling the profile picture update.
    /// </summary>
    /// <param name="sender">ButtonProfilePictureCancel button</param>
    /// <param name="e">Button Click Event</param>
    protected void ButtonProfilePictureCancel_Click(object sender, EventArgs e)
    {
        // Update buttons
        ButtonProfilePicture.Text = "Change Profile Photo";
        ButtonProfilePicture.BackColor = System.Drawing.Color.Blue;
        ButtonProfilePictureCancel.Visible = false;

        // Hide Checkbox List and clear its contents
        ArrayList itemList = new ArrayList();
        CheckBoxListProfilePicture.Visible = false;
        foreach (ListItem item in CheckBoxListProfilePicture.Items)
        {
            itemList.Add(item);
        }
        foreach (ListItem item in itemList)
        {
            CheckBoxListProfilePicture.Items.Remove(item);
        }
    }


    /// <summary>
    /// When the user clicks an item in the profile picture checkbox list to select a profile picture,
    /// this method ensures they can only select 1 item.  If an item is selected, all other items are
    /// disabled.  If they deselect an item all items are enabled.
    /// </summary>
    /// <param name="sender">CheckBoxListProfilePicture</param>
    /// <param name="e">Index Changed</param>
    protected void CheckBoxList_Changed(object sender, EventArgs e)
    {
        // This code is only necessary if the user has more than 1 image uploaded
        if (CheckBoxListProfilePicture.Items.Count > 1)
        {
            int count = 0;

            // Determine how many items are selected (should only be 1)
            for (int i = 0; i < CheckBoxListProfilePicture.Items.Count; i++)
            {
                if (CheckBoxListProfilePicture.Items[i].Selected)
                {
                    count++;
                }
            }

            // If the user selects a photo, disable the rest of the selection items
            if (count >= 1)
            {
                foreach (ListItem item in CheckBoxListProfilePicture.Items)
                {
                    if (!item.Selected)
                    {
                        item.Enabled = false;
                    }
                }
            }
            // If there are no selections (e.g. the user deselects an item), make them all enabled
            else
            {
                foreach (ListItem item in CheckBoxListProfilePicture.Items)
                {
                    if (!item.Selected)
                    {
                        item.Enabled = true;
                    }
                }
            }
        }
    }


    /// <summary>
    /// Event for when user clicks the Update Information button.
    /// </summary>
    /// <param name="sender">ButtonUpdateInfo button</param>
    /// <param name="e">Button Click Event</param>
    protected void ButtonUpdateInfo_Click(object sender, EventArgs e)
    {
        if (ButtonUpdateInfo.Text == "Update Information")
        {
            PersonalInfoButtonStates("change");
        }
        else // user clicked "Submit"
        {
            if (Page.IsValid)
            {
                // Update the info in the db
                UpdatePersonalInfo();

                // Update the button states
                PersonalInfoButtonStates("submit");

                // Update the information displayed on the page
                DisplayProfileInfo();
            }
        }
    }


    /// <summary>
    /// User clicks the cancel button for updating the profile information
    /// </summary>
    /// <param name="sender">ButtonUpdateProfileCancel button</param>
    /// <param name="e">Button Click Event</param>
    protected void ButtonUpdateProfileCancel_Click(object sender, EventArgs e)
    {
        PersonalInfoButtonStates("cancel");
        DisplayProfileInfo();
    }


    /// <summary>
    /// Event for updating the user's public preferences when they select/deselect the preferences checkbox.
    /// </summary>
    /// <param name="sender">Personal Information Checkbox</param>
    /// <param name="e">Button Click Event</param>
    protected void UpdatePreferences(object sender, EventArgs e)
    {
        string logMessage = string.Empty;
        CheckBox checkbox = (CheckBox)(sender);
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("UpdatePersonalInfo", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", username);

        if (checkbox.Text.Contains("first name"))
        {
            if (checkbox.Checked)
            {
                cmd.Parameters.AddWithValue("@fname_privacy", 1);
                logMessage = "First name set to public.";
            }
            else
            {
                cmd.Parameters.AddWithValue("@fname_privacy", 0);
                logMessage = "First name set to private.";
            }
        }
        else if (checkbox.Text.Contains("last name"))
        {
            if (checkbox.Checked)
            {
                cmd.Parameters.AddWithValue("@lname_privacy", 1);
                logMessage = "Last name set to public.";
            }
            else
            {
                cmd.Parameters.AddWithValue("@lname_privacy", 0);
                logMessage = "Last name set to private.";
            }
        }
        else if (checkbox.Text.Contains("date of birth"))
        {
            if (checkbox.Checked)
            {
                cmd.Parameters.AddWithValue("@birth_privacy", 1);
                logMessage = "Birth date set to public.";
            }
            else
            {
                cmd.Parameters.AddWithValue("@birth_privacy", 0);
                logMessage = "Birth date set to private.";
            }
        }
        else if (checkbox.Text.Contains("location"))
        {
            if (checkbox.Checked)
            {
                cmd.Parameters.AddWithValue("@location_privacy", 1);
                logMessage = "Location set to public.";
            }
            else
            {
                cmd.Parameters.AddWithValue("@location_privacy", 0);
                logMessage = "Location set to private.";
            }
        }
        else if (checkbox.Text.Contains("email"))
        {
            if (checkbox.Checked)
            {
                cmd.Parameters.AddWithValue("@email_privacy", 1);
                logMessage = "Email set to public.";
            }
            else
            {
                cmd.Parameters.AddWithValue("@email_privacy", 0);
                logMessage = "Email set to private.";
            }
        }

        try
        {
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            LogActivity(logMessage);
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }
    }


    /// <summary>
    /// Event for when user clicks the Change Password button.  Makes visible the "Change Password" panel and hides the button.
    /// </summary>
    /// <param name="sender">ButtonChangePass button</param>
    /// <param name="e">Button Click Event</param>
    protected void ButtonChangePass_Click(object sender, EventArgs e)
    {
        PanelNewPassword.Visible = true;
        ButtonChangePass.Visible = false;
    }


    /// <summary>
    /// Event for when user clicks the "Cancel" button when changing their password.  Clears the textboxes and the session data
    /// and reloads the page.
    /// </summary>
    /// <param name="sender">ButtonCancelNewPassword button</param>
    /// <param name="e">Button Click Event</param>
    protected void ButtonCancelNewPassword_Click(object sender, EventArgs e)
    {
        TextBoxCurrentPassword.Text = string.Empty;
        TextBoxNewPassword.Text = string.Empty;
        TextBoxConfirmPassword.Text = string.Empty;
        Session.Clear();
        Response.Redirect("profile.aspx");
    }


    /// <summary>
    /// Event for when user clicks the "Submit" button when changing their password.  Performs additional server side validation and
    /// if updates the user's password in the database.
    /// </summary>
    /// <param name="sender">ButtonSubmitNewPassword button</param>
    /// <param name="e">Button Click Event</param>
    protected void ButtonSubmitNewPassword_Click(object sender, EventArgs e)
    {
        string currentPassword = TextBoxCurrentPassword.Text;
        string newPassword = TextBoxNewPassword.Text;
        string confirmPassword = TextBoxConfirmPassword.Text;

        // If the user entered their current password correctly and entered appropriate values for the new password,
        // we'll update their password
        if (AuthenticateUser(currentPassword))
        {
            if (newPassword.Length >= 8 && newPassword.Length <= 50)
            {
                if (newPassword == confirmPassword)
                {
                    // Update the user's password
                    if (UpdatePassword(newPassword))
                    {
                        // Confirm message
                        LabelPasswordUpdateResponse.ForeColor = System.Drawing.Color.Green;
                        LabelPasswordUpdateResponse.Text = "Password has been updated!";
                    }
                    else
                    {
                        // Fail message
                        LabelPasswordUpdateResponse.ForeColor = System.Drawing.Color.Red;
                        LabelPasswordUpdateResponse.Text = "Password update failed.";
                    }
                }
            }
        }
        PanelNewPassword.Visible = false;
    }


    /// <summary>
    /// When the user clicks the "Delete" button for a blog entry, this event is fired.  Deletes
    /// the user's blog entry.
    /// </summary>
    /// <param name="sender">ButtonDeleteBlogEntry button</param>
    /// <param name="e">Button Click Event</param>
    protected void ButtonDeleteBlogEntry_Click(object sender, EventArgs e)
    {
        Button btn = (Button)(sender);
        int blog_id = Convert.ToInt32(btn.CommandArgument);
        Dictionary<string, string> blogInfo = GetBlogInfo(blog_id);

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("RemoveBlogEntry", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@blog_id", blog_id);

        try
        {
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            LogActivity("Deleted blog entry '" + blogInfo["title"] + "'");
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }
        Response.Redirect("profile.aspx");
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// The GetBlogInfo method obtains the blog information from the database.
    /// </summary>
    /// <param name="blog_id">The id of the blog whose information will be obtained.</param>
    /// <returns></returns>
    private Dictionary<string, string> GetBlogInfo(int blog_id)
    {
        Dictionary<string, string> blog = new Dictionary<string, string>();
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("GetBlog", conn);
        SqlDataReader rdr;
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@blog_id", blog_id);

        try
        {
            using (conn)
            {
                conn.Open();
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    blog["customer_id"] = Convert.ToString(rdr["customer_id"]);
                    blog["username"] = Convert.ToString(rdr["username"]);
                    blog["blog_date"] = Convert.ToString(rdr["blog_date"]);
                    blog["title"] = Convert.ToString(rdr["title"]);
                    blog["content"] = Convert.ToString(rdr["content"]);
                }
                conn.Close();
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }

        return blog;
    }


    /// <summary>
    /// Gets the profile picture and updates the profile pic ImageMap.
    /// </summary>
    protected void DisplayProfilePic()
    {
        string profile_pic = string.Empty;
        SqlConnection conn = new SqlConnection(connString);
        string getPic = "SELECT profile_picture FROM Customers WHERE username = '" + username + "';";
        SqlCommand cmd = new SqlCommand(getPic, conn);
        SqlDataReader rdr;

        try
        {
            using (conn)
            {
                conn.Open();
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    profile_pic = rdr["profile_picture"].ToString();
                }
                conn.Close();
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }

        // Update profile picture imagemap w/ pic info
        if (profile_pic != string.Empty)
        {
            ImageMapProfilePic.ImageUrl = "~/protected/Images/" + username + "/" + profile_pic;
        }
    }


    /// <summary>
    /// Gets personal info from the DB and sets the profile info labels.
    /// </summary>
    protected void DisplayProfileInfo()
    {
        Dictionary<string, string> personalInfo = GetPersonalInfo();
        Dictionary<string, bool> privacySettings = GetPrivacySettings();
        string birthDate = Convert.ToDateTime(personalInfo["dob"]).ToLongDateString();
        string birthMonth = Convert.ToDateTime(personalInfo["dob"]).Month.ToString();
        string birthDay = Convert.ToDateTime(personalInfo["dob"]).Day.ToString();
        string birthYear = Convert.ToDateTime(personalInfo["dob"]).Year.ToString();

        // Name
        TextBoxAboutMe.Text = GetAboutMe();
        LabelFirstName.Text = personalInfo["firstname"];
        TextBoxFirstNameUpdate.Text = personalInfo["firstname"];
        LabelLastName.Text = personalInfo["lastname"];
        TextBoxLastNameUpdate.Text = personalInfo["lastname"];

        // Date of Birth
        if (Convert.ToInt32(birthYear) < 1900) // "User didn't select a birthdate"
        {
            LabelDateOfBirth.Text = "None selected";
            dropDownListMonth.SelectedIndex = 1;
            dropDownListDay.SelectedIndex = 1;
            dropDownListYear.SelectedValue = "1900";
        }
        else
        {
            LabelDateOfBirth.Text = birthDate;
            dropDownListMonth.SelectedIndex = Convert.ToInt32(birthMonth);
            dropDownListDay.SelectedIndex = Convert.ToInt32(birthDay);
            dropDownListYear.SelectedValue = birthYear;
        }

        // Location, email
        LabelLocation.Text = personalInfo["location"];
        dropDownListCountry.SelectedItem.Text = personalInfo["location"];
        LabelEmail.Text = personalInfo["email"];
        TextBoxEmailUpdate.Text = personalInfo["email"];

        // Privacy settings
        if (privacySettings["fname"])
        {
            CheckBoxFirstName.Checked = true;
        }
        else
        {
            CheckBoxFirstName.Checked = false;
        }

        if (privacySettings["lname"])
        {
            CheckBoxLastName.Checked = true;
        }
        else
        {
            CheckBoxLastName.Checked = false;
        }

        if (privacySettings["birth"])
        {
            CheckBoxDOB.Checked = true;
        }
        else
        {
            CheckBoxDOB.Checked = false;
        }

        if (privacySettings["location"])
        {
            CheckBoxLocation.Checked = true;
        }
        else
        {
            CheckBoxLocation.Checked = false;
        }

        if (privacySettings["email"])
        {
            CheckBoxEmail.Checked = true;
        }
        else
        {
            CheckBoxEmail.Checked = false;
        }
    }


    /// <summary>
    /// Checks the filetype of the user's chosen file.  Must be .png, .gif, or .jpg.
    /// </summary>
    /// <returns>True if filetype is .gif, .png, .jpg, or .jpeg.  False otherwise.</returns>
    protected bool CheckFileType()
    {
        string[] allowedTypes = new string[4] { ".jpg", ".jpeg", ".png", ".gif" };

        foreach (string type in allowedTypes)
        {
            if (Path.GetExtension(FileUploadImage.FileName).ToLower() == type)
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// Uploads the image to the server.
    /// </summary>
    protected void UploadImage()
    {
        // Variables
        string filename = FileUploadImage.FileName;
        // Server: string directory = Server.MapPath("~/protected/Images/") + username;
        string directory = MapPath("~/protected/Images/") + username;
        DateTime date = DateTime.Now;

        // Database connection string and parameters
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("InsertImage", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@filename", filename);
        cmd.Parameters.AddWithValue("@date", date);
        cmd.Parameters.AddWithValue("@directory", directory);

        // Check if user directory exists; if not, create it; directory is named after user
        if (!System.IO.Directory.Exists(directory))
        {
            System.IO.Directory.CreateDirectory(directory);
        }

        // Upload image to directory
        FileUploadImage.SaveAs(directory + "/" + filename);
        LabelTypeMessage.ForeColor = System.Drawing.Color.Green;
        LabelTypeMessage.Text = "Image has been uploaded";

        try
        {
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            LogActivity("Uploaded image '" + filename + "'.");
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
            LabelTypeMessage.ForeColor = System.Drawing.Color.Red;
            LabelTypeMessage.Text = "An error has occured.  Please try again later.";
        }
    }


    /// <summary>
    /// Updates the user's "about me" column in the database.
    /// </summary>
    protected void UpdateAboutMe()
    {
        // Database connection strings
        string textboxEntry = TextBoxAboutMe.Text;
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("UpdateAboutMe", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@aboutme", textboxEntry);

        try
        {
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            LogActivity("Updated 'About Me' section.");
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }
    }


    /// <summary>
    /// Gets the user's "About Me" column from the database.
    /// </summary>
    /// <returns>About Me text.</returns>
    protected string GetAboutMe()
    {
        string aboutMe = string.Empty;

        // Database connection strings
        SqlConnection conn = new SqlConnection(connString);
        string getCmd = "SELECT * FROM Customers WHERE username = '" + username + "';";
        SqlCommand cmd = new SqlCommand(getCmd, conn);
        SqlDataReader rdr;

        try
        {
            using (conn)
            {
                conn.Open();
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    aboutMe = rdr["about_me"].ToString();
                }
                conn.Close();
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }
        return aboutMe;
    }


    /// <summary>
    /// Gets the user's personal info from the database.
    /// </summary>
    /// <returns>First name, last name, date of birth, location, email.</returns>
    private Dictionary<string, string> GetPersonalInfo()
    {
        Dictionary<string, string> personalInfo = new Dictionary<string, string>();
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("GetUserInfo", conn);
        SqlDataReader rdr;
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", username);

        try
        {
            using (conn)
            {
                conn.Open();
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    personalInfo["firstname"] = rdr["first_name"].ToString();
                    personalInfo["lastname"] = rdr["last_name"].ToString();
                    personalInfo["dob"] = rdr["birth_date"].ToString();
                    personalInfo["email"] = rdr["email"].ToString();
                    personalInfo["location"] = rdr["name"].ToString();
                }
                conn.Close();
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }

        return personalInfo;
    }


    /// <summary>
    /// Gets the list of images from the database and populates the CheckBoxList.
    /// </summary>
    protected void PopulateProfileCheckBoxList()
    {
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("GetUserImages", conn);
        SqlDataReader rdr;
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", username);

        try
        {
            using (conn)
            {
                conn.Open();
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    CheckBoxListProfilePicture.Items.Add(rdr["name"].ToString());
                }
                conn.Close();
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }
    }


    /// <summary>
    /// Update the user's profile picture in the database.
    /// </summary>
    protected void UpdateUserProfilePicture()
    {
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("UpdateProfilePicture", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@picture", CheckBoxListProfilePicture.SelectedItem.ToString());

        try
        {
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            LogActivity("Profile picture updated.");
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }
    }


    /// <summary>
    /// Updates the user's personal information in the database.
    /// </summary>
    protected void UpdatePersonalInfo()
    {
        DateTime birthDate = Convert.ToDateTime(dropDownListMonth.SelectedValue + " " + dropDownListDay.SelectedValue + ", " + dropDownListYear.SelectedValue);
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("UpdatePersonalInfo", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@firstname", TextBoxFirstNameUpdate.Text);
        cmd.Parameters.AddWithValue("@lastname", TextBoxLastNameUpdate.Text);
        cmd.Parameters.AddWithValue("@dob", birthDate);
        cmd.Parameters.AddWithValue("@location", dropDownListCountry.SelectedItem.Text);
        cmd.Parameters.AddWithValue("@email", TextBoxEmailUpdate.Text);

        try
        {
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            LogActivity("Personal information updated.");
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }
    }


    /// <summary>
    /// Updates the states of the personal information buttons.  There are two states: 1. change, and 2. submit.  In state 1
    /// the user sees the button as "Update Information".  Clicking "Update Information" make their personal information
    /// editable.  In state 2 they can choose to submit their newly edited info or they may cancel the update.
    /// </summary>
    /// <param name="state"></param>
    protected void PersonalInfoButtonStates(string state)
    {
        if (state == "change")
        {
            // Get birthday info 
            Dictionary<string, string> personalInfo = GetPersonalInfo();
            string birthMonth = Convert.ToDateTime(personalInfo["dob"]).Month.ToString();
            string birthDay = Convert.ToDateTime(personalInfo["dob"]).Day.ToString();
            string birthYear = Convert.ToDateTime(personalInfo["dob"]).Year.ToString();

            // Update button
            ButtonUpdateInfo.Text = "Submit";
            ButtonUpdateInfo.CausesValidation = true;
            ButtonUpdateInfo.BackColor = System.Drawing.Color.Green;
            ButtonUpdateProfileInfoCancel.Visible = true;

            // Hide labels for personal info
            LabelFirstName.Visible = false;
            LabelLastName.Visible = false;
            LabelDateOfBirth.Visible = false;
            LabelLocation.Visible = false;
            LabelEmail.Visible = false;

            // Replace labels w/ textboxes for user to input new info
            TextBoxFirstNameUpdate.Visible = true;
            TextBoxLastNameUpdate.Visible = true;
            dropDownListMonth.Visible = true;
            dropDownListDay.Visible = true;
            dropDownListYear.Visible = true;
            TextBoxEmailUpdate.Visible = true;
            dropDownListCountry.Visible = true;

            // Name, email, location
            TextBoxFirstNameUpdate.Text = LabelFirstName.Text;
            TextBoxLastNameUpdate.Text = LabelLastName.Text;
            TextBoxEmailUpdate.Text = LabelEmail.Text;
            dropDownListCountry.SelectedItem.Text = LabelLocation.Text;

            // Birth date
            if (Convert.ToInt32(birthYear) < 1900) // "User didn't select a birth date"
            {
                dropDownListMonth.SelectedIndex = 1;
                dropDownListDay.SelectedIndex = 1;
                dropDownListYear.SelectedValue = "1900";
            }
            else
            {
                dropDownListMonth.SelectedIndex = Convert.ToInt32(birthMonth);
                dropDownListDay.SelectedIndex = Convert.ToInt32(birthDay);
                dropDownListYear.SelectedValue = birthYear;
            }

        }
        else if (state == "submit")
        {
            // Update button
            ButtonUpdateInfo.Text = "Update Information";
            ButtonUpdateInfo.BackColor = System.Drawing.Color.Blue;
            ButtonUpdateProfileInfoCancel.Visible = false;
            ButtonUpdateInfo.CausesValidation = false;

            // Restore labels and hide textboxes
            LabelFirstName.Visible = true;
            LabelLastName.Visible = true;
            LabelDateOfBirth.Visible = true;
            LabelLocation.Visible = true;
            LabelEmail.Visible = true;
            TextBoxFirstNameUpdate.Visible = false;
            TextBoxLastNameUpdate.Visible = false;
            TextBoxEmailUpdate.Visible = false;
            dropDownListCountry.Visible = false;
            dropDownListMonth.Visible = false;
            dropDownListDay.Visible = false;
            dropDownListYear.Visible = false;
        }
        else // Cancellation state
        {
            // Hide textboxes
            TextBoxFirstNameUpdate.Visible = false;
            TextBoxLastNameUpdate.Visible = false;
            TextBoxEmailUpdate.Visible = false;
            dropDownListCountry.Visible = false;
            dropDownListMonth.Visible = false;
            dropDownListDay.Visible = false;
            dropDownListYear.Visible = false;

            // Unhide labels
            LabelFirstName.Visible = true;
            LabelLastName.Visible = true;
            LabelDateOfBirth.Visible = true;
            LabelLocation.Visible = true;
            LabelEmail.Visible = true;

            // Update buttons
            ButtonUpdateInfo.Text = "Update Information";
            ButtonUpdateInfo.BackColor = System.Drawing.Color.Blue;
            ButtonUpdateProfileInfoCancel.Visible = false;
            ButtonUpdateInfo.CausesValidation = false;
        }
    }


    /// <summary>
    /// Obtains the user's privacy settings from the database.
    /// </summary>
    /// <returns>Privacy settings.</returns>
    private Dictionary<string, bool> GetPrivacySettings()
    {
        Dictionary<string, bool> userPrivacy = new Dictionary<string, bool>();
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("GetUserInfo", conn);
        SqlDataReader rdr;
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", username);

        try
        {
            using (conn)
            {
                conn.Open();
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    userPrivacy["fname"] = Convert.ToBoolean(rdr["fname_private"]);
                    userPrivacy["lname"] = Convert.ToBoolean(rdr["lname_private"]);
                    userPrivacy["birth"] = Convert.ToBoolean(rdr["birth_private"]);
                    userPrivacy["location"] = Convert.ToBoolean(rdr["location_private"]);
                    userPrivacy["email"] = Convert.ToBoolean(rdr["email_private"]);
                }
                conn.Close();
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }
        return userPrivacy;
    }


    /// <summary>
    /// Updates the "Activities" textbox.
    /// </summary>
    private void ShowActivities()
    {
        bool hasActivity = false;
        List<DateTime> logDate = new List<DateTime>();
        List<string> activityLog = new List<string>();
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("GetActivities", conn);
        SqlDataReader rdr;
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", username);

        try
        {
            using (conn)
            {
                conn.Open();
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    hasActivity = true;
                    logDate.Add(Convert.ToDateTime(rdr["log_date"]));
                    activityLog.Add(Convert.ToString(rdr["activity"]));
                }
                conn.Close();
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }

        // Clear the activity textbox to avoid concatentating the data on every page load
        TextBoxRecentActivity.Text = string.Empty;

        // Update the text in the recent activity textbox
        if (hasActivity)
        {
            for (int i = 0; i < logDate.Count; i++)
            {
                TextBoxRecentActivity.Text += logDate[i] + ": " + activityLog[i] + "\n";
            }
        }
        else
        {
            TextBoxRecentActivity.Text = "No recent activity";
        }
    }


    /// <summary>
    /// Updates the user's password.
    /// </summary>
    /// <param name="password">The user's new password.</param>
    /// <returns>True if the password was successfully updated.  False otherwise.</returns>
    private bool UpdatePassword(string password)
    {
        bool success = false;
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("UpdatePassword", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@password", password);

        try
        {
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            success = true;
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }

        return success;
    }


    /// <summary>
    /// Authenticates the user.
    /// </summary>
    /// <param name="password">The user's password.</param>
    /// <returns></returns>
    private bool AuthenticateUser(string password)
    {
        bool auth = false;
        int result = 0;

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("AuthenticateUser", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@password", password);

        try
        {
            using (conn)
            {
                conn.Open();
                result = Convert.ToInt16(cmd.ExecuteScalar());
                conn.Close();
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }

        if (result == 1)
        {
            auth = true;
            LogActivity("Password updated.");
        }

        return auth;
    }


    /// <summary>
    /// Logs the activity for a database event.
    /// </summary>
    /// <param name="message">Information about the database event.</param>
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