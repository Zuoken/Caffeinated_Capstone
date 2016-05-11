// I, Cameron Winters, student number 000299896, certify that this material is my
// original work. No other person's work has been used without due
// acknowledgement and I have not made my work available to anyone else.

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class protected_blog : System.Web.UI.Page
{

    #region Class Variables

    string connString = WebConfigurationManager.ConnectionStrings["caffeineDB"].ConnectionString;
    string username = System.Web.HttpContext.Current.User.Identity.Name;

    #endregion

    #region Page Initialization Methods

    /// <summary>
    /// On page initialization we set the session variables to default values.  We also set SqlDataSourceComments to the value of the
    /// comments_sql session variable if it has a value.  This is to ensure that paging works for the comments as the data source needs
    /// to have been populated by the time paging kicks in.
    /// </summary>
    /// <param name="sender">Page initialization</param>
    /// <param name="e">Page initialization</param>
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["submit"] == null) { Session["submit"] = 0; }
        if (Session["blog_comment_submit"] == null) { object o = null; Session["blog_comment_submit"] = o; }
        if (Session["rating_submit"] == null) { object o = null; Session["rating_submit"] = o; }
        if (Session["comment_sql"] == null)
        {
            Session["comment_sql"] = "";
        }
        else
        {
            SqlDataSourceComments.SelectCommand = Session["comment_sql"].ToString();
        }
    }


    /// <summary>
    /// When the user submits their blog we want to display a success message on page reload.  The session variable "submit" is given the value
    /// 1 when the user submits a blog.  After displaying the success message its value is reset to 0.
    /// </summary>
    /// <param name="sender">Page load</param>
    /// <param name="e">Page load</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Convert.ToInt16(Session["submit"]) >= 1)
        {
            LabelSubmitMessage.ForeColor = System.Drawing.Color.Green;
            LabelSubmitMessage.Text = "Blog post submitted!";
            TextBoxNewBlogTitle.Text = string.Empty;
            TextBoxNewBlogContent.Text = string.Empty;
            Session["submit"] = 0;
        }

        if (Session["blog_comment_submit"] != null)
        {
            ChangeView("full_blog", (object)(Session["blog_comment_submit"]));
            Session.Clear();
        }

        if (Session["rating_submit"] != null)
        {
            ChangeView("full_blog", (object)(Session["rating_submit"]));
            Session.Clear();
        }
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// When the user clicks the Submit Blog button this event is fired.  We ensure that the title and
    /// content textboxes contain values and if so we submit the new blog post to the database.
    /// </summary>
    /// <param name="sender">Submit Blog Button</param>
    /// <param name="e">Button Click Event</param>
    protected void ButtonSubmitBlog_Click(object sender, EventArgs e)
    {
        LabelSubmitMessage.Text = string.Empty;

        // Ensure that the blog has a title
        if (TextBoxNewBlogTitle.Text == string.Empty)
        {
            TextBoxNewBlogTitle.BorderStyle = BorderStyle.Solid;
            TextBoxNewBlogTitle.BorderColor = System.Drawing.Color.Red;
            TextBoxNewBlogTitle.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffe5e5");
        }
        else
        {
            TextBoxNewBlogTitle.BorderStyle = BorderStyle.Groove;
            TextBoxNewBlogTitle.BorderColor = System.Drawing.Color.Empty;
            TextBoxNewBlogTitle.BackColor = System.Drawing.Color.White;
        }

        // Ensure that the blog has content
        if (TextBoxNewBlogContent.Text == string.Empty)
        {
            TextBoxNewBlogContent.BorderStyle = BorderStyle.Solid;
            TextBoxNewBlogContent.BorderColor = System.Drawing.Color.Red;
            TextBoxNewBlogContent.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffe5e5");
        }
        else
        {
            TextBoxNewBlogContent.BorderStyle = BorderStyle.Groove;
            TextBoxNewBlogContent.BorderColor = System.Drawing.Color.Empty;
            TextBoxNewBlogContent.BackColor = System.Drawing.Color.White;
        }

        // If the blog has both a title of min length 3 and content we can submit it to the database
        if (TextBoxNewBlogTitle.Text.Length > 3 && TextBoxNewBlogContent.Text != string.Empty)
        {
            // SubmitBlog will return false if the insertion failed
            if (SubmitBlog())
            {
                int submission = Convert.ToInt16(Session["submit"]) + 1;
                Session["submit"] = submission;
                Response.Redirect("blog.aspx");
            }
            else
            {
                LabelSubmitMessage.ForeColor = System.Drawing.Color.Red;
                LabelSubmitMessage.Text = "An error has occured.";
                TextBoxNewBlogTitle.Text = string.Empty;
                TextBoxNewBlogContent.Text = string.Empty;
            }
        }
    }


    /// <summary>
    /// When the user clicks the "Read More" button this event is fired and we call the "ChangeView" method.
    /// </summary>
    /// <param name="sender">Read More Button</param>
    /// <param name="e">Button Click Event</param>
    protected void ButtonReadMore_Click(object sender, EventArgs e)
    {
        ChangeView("full_blog", sender);
    }


    /// <summary>
    /// When the user clisk the "Return to Blogs" button this event is fired and we call the "ChangeView" method.
    /// </summary>
    /// <param name="sender">Return to Blogs Button</param>
    /// <param name="e">Button Click Event</param>
    protected void ButtonReturnToBlogs_Click(object sender, EventArgs e)
    {
        ChangeView("all_blogs");
    }


    /// <summary>
    /// When the user clicks the comment submit button this event is fired.  We get the blog id value by parsing the
    /// CommandArgument of the button and call the SubmitComment method if the comment textbox is not null.
    /// </summary>
    /// <param name="sender">Submit Comment Button</param>
    /// <param name="e">Button Click Event</param>
    protected void ButtonSubmitComment_Click(object sender, EventArgs e)
    {
        Button btn = (Button)(sender);
        int blog_id = Convert.ToInt32(btn.CommandArgument);

        if (TextBoxNewComment.Text != string.Empty)
        {
            SubmitComment(blog_id);
            ChangeView("full_blog", sender);
            Session["blog_comment_submit"] = sender;
            Response.Redirect("blog.aspx");
        }
    }


    /// <summary>
    /// When the user clicks the thumbs up button this event is fired.  We get the blog id by parsing the CommandArgument
    /// of the button and call the AddRating method, sending a value of "1".  Finally, session variable is set and we 
    /// refresh the page.
    /// </summary>
    /// <param name="sender">Thumbs Up Button</param>
    /// <param name="e">Button Click Event</param>
    protected void ButtonThumbsUp_Click(object sender, EventArgs e)
    {
        Button btn = (Button)(sender);
        int blog_id = Convert.ToInt32(btn.CommandArgument);
        AddRating(blog_id, 1);
        ChangeView("full_blog", sender);
        Session["rating_submit"] = sender;
        Response.Redirect("blog.aspx");
    }


    /// <summary>
    /// When the user clicks the thumbs down button this event is fired.  We get the blog id by parsing the CommandArgument
    /// of the button and call the AddRating method, sending a value of "0".  Finally, session variable is set and we 
    /// refresh the page.
    /// </summary>
    /// <param name="sender">Thumbs Down Button</param>
    /// <param name="e">Button Click Event</param>
    protected void ButtonThumbsDown_Click(object sender, EventArgs e)
    {
        Button btn = (Button)(sender);
        int blog_id = Convert.ToInt32(btn.CommandArgument);
        AddRating(blog_id, 0);
        ChangeView("full_blog", sender);
        Session["rating_submit"] = sender;
        Response.Redirect("blog.aspx");
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// The ChangeView method takes a "view" parameter and an optional "sender" parameter.  Depending on the value of the "view" parameter,
    /// we hide or show different sections of the page for the user (hide the "all_blogs" view and show the "full_blog" view if the user 
    /// clicks the "Read More" button of a blog post, for instance).  
    /// </summary>
    /// <param name="view">The requested view to display</param>
    /// <param name="sender">The value of the sender.  Optional, but required for "full_blog"</param>
    private void ChangeView(string view, object sender = null)
    {
        switch (view)
        {
            case "all_blogs":
                all_blogs_view.Visible = true;
                full_blogs_view.Visible = false;
                Session.Clear();
                break;
            case "full_blog":
                all_blogs_view.Visible = false;
                full_blogs_view.Visible = true;
                Button btn = (Button)(sender);
                int blog_id = Convert.ToInt32(btn.CommandArgument);
                List<int> ratingInfo = GetRatingInfo(blog_id);
                int rating = 0;
                int ratingTotal = ratingInfo.Count;
                Dictionary<string, string> blog = GetBlogInfo(blog_id);
                DateTime date = Convert.ToDateTime(blog["blog_date"]);

                LabelBlogTitle.Text = blog["title"];
                LabelBlogUsername.Text = blog["username"];
                LabelBlogDate.Text = date.ToLongDateString() + " at " + date.ToShortTimeString();
                TextBoxBlogContent.Text = blog["content"];
                Session["comment_sql"] = "SELECT [username], [comment_date], [blog_id], [comment] FROM [Comments] JOIN [Customers] ON [Customers].customer_id = [Comments].customer_id WHERE [blog_id] = " + blog_id + " ORDER BY [comment_date] DESC";
                SqlDataSourceComments.SelectCommand = Session["comment_sql"].ToString();
                ButtonSubmitComment.CommandArgument = blog_id.ToString();
                ButtonThumbsUp.CommandArgument = blog_id.ToString();
                ButtonThumbsDown.CommandArgument = blog_id.ToString();

                // Get the number of thumbs up
                foreach (int currRating in ratingInfo)
                {
                    if (currRating == 1)
                    {
                        rating++;
                    }
                }

                // If ratings exist display them (thumbs up / total thumbs) and set rating status
                if (ratingTotal > 0)
                {
                    LabelBlogRating.Text = rating + "/" + ratingTotal;
                    RatingStatus(rating, ratingTotal);
                }
                else
                {
                    LabelBlogRating.Text = "No ratings available yet.";
                }
                break;
            default:
                all_blogs_view.Visible = true;
                full_blogs_view.Visible = false;
                Session.Clear();
                break;
        }
    }


    /// <summary>
    /// The RatingStatus method takes two parameters, thumbs_up and total_thumbs.  The purpose of this method is to calculate the difference 
    /// between the number of thumbs up given to a post and the total number of thumbs given.  It sets the value of the "LabelRatingStatus" label.
    /// </summary>
    /// <param name="thumbs_up">The number of thumbs up given</param>
    /// <param name="total_thumbs">The total number of thumbs given</param>
    private void RatingStatus(int thumbs_up, int total_thumbs)
    {
        double ratingStatus = thumbs_up / total_thumbs * 100;

        if (ratingStatus >= 80)
        {

            LabelRatingStatus.ForeColor = System.Drawing.Color.Green;
            LabelRatingStatus.Text = "Great!";
        }
        else if (ratingStatus >= 70 && ratingStatus < 80)
        {
            LabelRatingStatus.ForeColor = System.Drawing.Color.Yellow;
            LabelRatingStatus.Text = "Decent";
        }
        else if (ratingStatus >= 50 && ratingStatus < 70)
        {
            LabelRatingStatus.ForeColor = System.Drawing.Color.Orange;
            LabelRatingStatus.Text = "Mediocre";
        }
        else
        {
            LabelRatingStatus.ForeColor = System.Drawing.Color.Red;
            LabelRatingStatus.Text = "Bad!";
        }
    }


    /// <summary>
    /// The SubmitBlog method submits the content in TextBoxNewBlogContent to the database.  It then logs the activity.
    /// </summary>
    /// <returns>True if submission was successful.  False otherwise.</returns>
    private bool SubmitBlog()
    {
        string title = TextBoxNewBlogTitle.Text;
        string content = TextBoxNewBlogContent.Text;
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("InsertBlog", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@date", DateTime.Now);
        cmd.Parameters.AddWithValue("@title", title);
        cmd.Parameters.AddWithValue("@content", content);

        try
        {
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            LogActivity("Blog post " + title + " added.");
            return true;
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
            return false;
        }
    }


    /// <summary>
    /// The SubmitComment method submits the comment to the database.  It then logs the activity.
    /// </summary>
    /// <param name="blog_id"></param>
    /// <returns>True if comment submitted.  False otherwise.</returns>
    private bool SubmitComment(int blog_id)
    {
        Dictionary<string, string> blogInfo = GetBlogInfo(blog_id);
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("InsertComment", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@date", DateTime.Now);
        cmd.Parameters.AddWithValue("@blog_id", blog_id);
        cmd.Parameters.AddWithValue("@comment", TextBoxNewComment.Text);

        try
        {
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            LogActivity("Comment added to " + blogInfo["title"] + " blog.");
            return true;
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
            return false;
        }
    }


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
    /// The AddRating method submits the value of the blog rating to the database.
    /// </summary>
    /// <param name="blog_id">The id of the blog whose rating is being given to</param>
    /// <param name="rating">The rating being given to the blog</param>
    private void AddRating(int blog_id, int rating)
    {
        Dictionary<string, string> blogInfo = GetBlogInfo(blog_id);
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("InsertRating", conn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@blog_id", blog_id);
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@date", DateTime.Now);
        cmd.Parameters.AddWithValue("@thumbs", rating);

        try
        {
            using (conn)
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            if (rating == 0)
            {
                LogActivity("Gave blog '" + blogInfo["title"] + "' a thumbs down.");
            }
            else
            {
                LogActivity("Gave blog '" + blogInfo["title"] + "' a thumbs up.");
            }
        }
        catch (Exception exc)
        {
            System.Diagnostics.Debug.WriteLine(exc);
        }
    }


    /// <summary>
    /// Logs the activity for a database event.
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


    /// <summary>
    /// Get the rating information about the blog.  The total number of thumbs up given to the blog
    /// will be calculated based on "number of thumbs up given" / "total number of ratings given".  Note
    /// that a "thumbs_up" value of 1 is considered a thumbs up, and a value of 0 is considered a thumbs down
    /// </summary>
    /// <returns>Rating Information</returns>
    private List<int> GetRatingInfo(int blog_id)
    {
        List<int> ratingInfo = new List<int>();
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand("GetAllRatings", conn);
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

    #endregion
}