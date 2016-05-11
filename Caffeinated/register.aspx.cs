// I, Cameron Winters, student number 000299896, certify that this material is my
// original work. No other person's work has been used without due
// acknowledgement and I have not made my work available to anyone else.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class register : System.Web.UI.Page
{
    #region Class Variables

    string connString = WebConfigurationManager.ConnectionStrings["caffeineDB"].ConnectionString;
    string username = string.Empty;

    #endregion

    #region Page Initialization Methods

    /// <summary>
    /// Force logout on page initialization.  This is to prevent users from seeing this page if they're currently logged in.
    /// </summary>
    /// <param name="sender">Page Initialization</param>
    /// <param name="e">Page Initialization</param>
    protected void Page_Init(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
    }


    /// <summary>
    /// Populate initial values and settings when the page loads.
    /// </summary>
    /// <param name="sender">Page Load</param>
    /// <param name="e">Page Load</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        int currentYear = System.DateTime.Now.Year;

        // Set the focus and populate the "Year" dropdown list
        SetFocus(textBoxUsername);

        for (int i = currentYear; i >= 1900; i--)
        {
            dropDownListYear.Items.Add(i.ToString());
        }
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Custom validation for the length of the username.  It must be greater than 3 characters
    /// and 20 characters or less.
    /// </summary>
    /// <param name="source">CustomValidatorUsernameLength</param>
    /// <param name="args">Events</param>
    protected void UsernameLength(object sender, ServerValidateEventArgs e)
    {
        if (textBoxUsername.Text.Length < 3)
        {
            CustomValidatorUsernameLength.Text = "Username too short";
            e.IsValid = false;
        }
        else if (textBoxUsername.Text.Length > 20)
        {
            CustomValidatorUsernameLength.Text = "Username too long";
            e.IsValid = false;
        }
        else
        {
            e.IsValid = true;
        }
    }


    /// <summary>
    /// Check to make sure the username doesn't contain any special characters.  The username may only contain
    /// alphanumeric characters.
    /// </summary>
    /// <param name="sender">CustomValidatorUsernameNoSpecChars</param>
    /// <param name="e">Validation</param>
    protected void UsernameCharactersCheck(object sender, ServerValidateEventArgs e)
    {
        Regex exp = new Regex("^[A-Za-z0-9]*$");
        if (exp.IsMatch(textBoxUsername.Text))
        {
            e.IsValid = true;
        }
        else
        {
            CustomValidatorUsernameNoSpecChars.Text = "Contains special characters";
            e.IsValid = false;
        }
    }


    /// <summary>
    /// Validates to ensure the the First Name entered is betweeen 3 and 15 characters in length.
    /// </summary>
    /// <param name="source">CustomValidatorFirstNameLength</param>
    /// <param name="args">Validation</param>
    protected void FirstNameLength(object sender, ServerValidateEventArgs e)
    {
        if (textBoxFirstName.Text.Length < 3)
        {
            CustomValidatorFirstNameLength.Text = "Name is too short";
            e.IsValid = false;
        }
        else if (textBoxFirstName.Text.Length > 15)
        {
            CustomValidatorFirstNameLength.Text = "Name is too long";
            e.IsValid = false;
        }
        else
        {
            e.IsValid = true;
        }
    }


    /// <summary>
    /// Validates to ensure that the Last Name entered is between 3 and 15 characters in length.
    /// </summary>
    /// <param name="sender">CustomValidatorLastNameLength</param>
    /// <param name="e">Validation</param>
    protected void LastNameLength(object sender, ServerValidateEventArgs e)
    {
        if (textBoxLastName.Text.Length < 3)
        {
            CustomValidatorLastNameLength.Text = "Name is too short";
            e.IsValid = false;
        }
        else if (textBoxLastName.Text.Length > 15)
        {
            CustomValidatorLastNameLength.Text = "Name is too long";
            e.IsValid = false;
        }
        else
        {
            e.IsValid = true;
        }
    }


    /// <summary>
    /// Method calls the 'checkUsername' stored procedure in the database.  If the username
    /// already exists in the database the sp will return 1.  If that username is not already
    /// taken it will return 2.
    /// </summary>
    /// <param name="source">CustomValidatorUsername</param>
    /// <param name="args">Events</param>
    protected void CheckUsername(object sender, ServerValidateEventArgs e)
    {
        // Define variables, connection strings, and queries
        int exists;
        string userName = textBoxUsername.Text;
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("UsernameExists", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", userName);

        try
        {
            using (conn)
            {
                conn.Open();

                exists = Convert.ToInt32(cmd.ExecuteScalar());

                // 1 = username is taken.  0 = username is available.
                if (exists == 1)
                {
                    e.IsValid = false;
                }
                else
                {
                    e.IsValid = true;
                }
                conn.Close();
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
            e.IsValid = false;
        }
    }


    /// <summary>
    /// When the user clicks the "Submit" button this event fires.  It checks to see that the page is valid, and if it is
    /// calls the AddUser method, which will add the user to the database.
    /// </summary>
    /// <param name="sender">ButtonSubmit</param>
    /// <param name="e">Button Click Event</param>
    protected void ButtonSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            AddUser();
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Add the user to the database.
    /// </summary>
    protected void AddUser()
    {
        bool isSuccess = false;
        string userName = textBoxUsername.Text;
        string email = textBoxEmail.Text;
        string fName = textBoxFirstName.Text;
        string lName = textBoxLastName.Text;
        string password = textBoxPassword.Text;
        string question = textBoxSecQuestion.Text;
        string answer = textBoxSecAnswer.Text;
        string country = "";
        string birthYear = "1753"; // Minimum year allowed in SQL; used as default
        string birthMonth = "10"; // Default month is October
        string birthDay = "10"; // Default day is 10
        DateTime birthDate; // To hold the date-converted value of birthyear, month, and day
        // Determine values for optional field "Country"
        if (dropDownListCountry.SelectedIndex != 0)
        {
            country = dropDownListCountry.SelectedValue;
        }
        else
        {
            country = "None Selected";
        }

        // Determine values for optional field "Date of birth"
        if (dropDownListYear.SelectedIndex != 0 && dropDownListMonth.SelectedIndex != 0 && dropDownListDay.SelectedIndex != 0)
        {
            birthYear = dropDownListYear.SelectedValue;
            birthMonth = dropDownListMonth.SelectedValue;
            birthDay = dropDownListDay.SelectedValue;
        }

        birthDate = Convert.ToDateTime(birthMonth + " " + birthDay + ", " + birthYear);

        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("InsertUser", conn);
        SqlParameter insStatus = cmd.Parameters.Add("status", SqlDbType.Int);
        insStatus.Direction = ParameterDirection.ReturnValue;
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", userName);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@firstname", fName);
        cmd.Parameters.AddWithValue("@lastname", lName);
        cmd.Parameters.AddWithValue("@password", password);
        cmd.Parameters.AddWithValue("@sec_question", question);
        cmd.Parameters.AddWithValue("@sec_answer", answer);
        cmd.Parameters.AddWithValue("@country", country);
        cmd.Parameters.AddWithValue("@birth_date", birthDate);

        try
        {
            
            using (conn)
            {
                int status = 0;

                conn.Open();
                status = (int)cmd.ExecuteScalar();
                if (status == 1)
                {
                    isSuccess = true;
                    username = userName;
                }
                conn.Close();
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }

        if (isSuccess)
        {
            LogActivity("User account created.");
            Response.Redirect("registration_successful.aspx");
        }
        else
        {
            Response.Redirect("registration_unsuccessful.aspx");
        }
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