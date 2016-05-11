// I, Cameron Winters, student number 000299896, certify that this material is my
// original work. No other person's work has been used without due
// acknowledgement and I have not made my work available to anyone else.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class login : System.Web.UI.Page
{
    #region Page Initialization Methods

    /// <summary>
    /// The focus is set to the 'UserName' field on page load.
    /// </summary>
    /// <param name="sender">Page Load</param>
    /// <param name="e">Page Load</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        SetFocus(caffeineLogin.FindControl("UserName"));
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Authenticate the user when they attempt to log in.
    /// </summary>
    /// <param name="sender">caffeineLogin</param>
    /// <param name="e">Login Authentication Event</param>
    protected void LoginAuthenticate(object sender, AuthenticateEventArgs e)
    {
        bool loginAuthenticated = Authentication(caffeineLogin.UserName, caffeineLogin.Password);

        if (loginAuthenticated)
        {
            e.Authenticated = true;
        }
        else
        {
            e.Authenticated = false;
        }
    }


    /// <summary>
    /// Redirects the user to the home page if they are successfully logged in.
    /// </summary>
    /// <param name="sender">caffeineLogin</param>
    /// <param name="e">Logged In Event</param>
    protected void Login_LoggedIn(object sender, EventArgs e)
    {
        Response.Redirect("protected/home.aspx");
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Authentication method checks the database to determine if the user's credentials are valid.
    /// </summary>
    /// <param name="username">The user's username</param>
    /// <param name="password">The user's password</param>
    /// <returns></returns>
    protected bool Authentication(string username, string password)
    {
        bool auth = false;
        int result;
        string connStr = WebConfigurationManager.ConnectionStrings["caffeineDB"].ConnectionString;
        SqlConnection conn = new SqlConnection(connStr);
        SqlCommand cmd = new SqlCommand("AuthenticateUser", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", caffeineLogin.UserName.Trim());
        cmd.Parameters.AddWithValue("@password", caffeineLogin.Password.Trim());

        try
        {
            using (conn)
            {
                conn.Open();
                result = (int)cmd.ExecuteScalar();
                if (result == 1)
                {
                    auth = true;
                }
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }
        return auth;
    }

    #endregion
}