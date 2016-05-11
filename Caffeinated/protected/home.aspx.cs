// I, Cameron Winters, student number 000299896, certify that this material is my
// original work. No other person's work has been used without due
// acknowledgement and I have not made my work available to anyone else.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class protected_home : System.Web.UI.Page
{
    #region Class Variables

    string username = System.Web.HttpContext.Current.User.Identity.Name;

    #endregion

    #region Page Initialization Methods

    /// <summary>
    /// Page Initialization Method.  Sets the values for the datasources.
    /// </summary>
    /// <param name="sender">Page Init</param>
    /// <param name="e">Page Init</param>
    protected void Page_Init(object sender, EventArgs e)
    {
        SqlDataSourceLatestRecipeComments.SelectCommand = "SELECT * FROM Comments c JOIN Recipes r ON c.recipe_id = r.recipe_id JOIN Customers cus ON r.customer_id = cus.customer_id WHERE username = '" + username + "' ORDER BY c.comment_date DESC;";
        SqlDataSourceLatestBlogComments.SelectCommand = "SELECT * FROM Comments c JOIN Blogs b ON c.blog_id = b.blog_id JOIN Customers cus ON b.customer_id = cus.customer_id WHERE username = '" + username + "' ORDER BY c.comment_date DESC;";
    }


    /// <summary>
    /// Page Load method.  Nothing to load.
    /// </summary>
    /// <param name="sender">Page Load</param>
    /// <param name="e">Page Load</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Nothing to load
    }

    #endregion
}