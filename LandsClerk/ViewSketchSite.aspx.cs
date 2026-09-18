using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ViewSite : System.Web.UI.Page
{
    string appconStr;
    string server, appdb, user, password, version, station;
    SqlConnection appconSQL2;

    private void readConf()
    {
        string filePath = Server.MapPath("../dbconn.ini");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Database configuration file not found: dbconn.ini");
        }

        using (System.IO.StreamReader sr = System.IO.File.OpenText(filePath))
        {
            string s = "";
            string[] rfInfo = new string[2];
            char SplitChar = '=';

            while ((s = sr.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(s) && !s.StartsWith("#"))
                {
                    rfInfo = s.Split(SplitChar);
                    if (rfInfo.Length >= 2)
                    {
                        switch (rfInfo[0].Trim().ToLower())
                        {
                            case "server":
                                server = rfInfo[1].Trim();
                                break;
                            case "user":
                                user = rfInfo[1].Trim();
                                break;
                            case "password":
                                password = rfInfo[1].Trim();
                                break;
                            case "appdb":
                                appdb = rfInfo[1].Trim();
                                break;
                            case "version":
                                version = rfInfo[1].Trim();
                                break;
                        }
                    }
                }
            }
        }
    }

    private void dbconnect()
    {
        appconStr = "Data Source=" + server + ";user id=" + user + ";password=" + password + ";max pool size= 65536;Initial Catalog=" + appdb + ";";
        appconSQL2 = new SqlConnection(appconStr);
        appconSQL2.Open();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            readConf();
            dbconnect();

            if (Session["USER"] != null && Session["USER"].ToString() != "")
            {
                lblSession.Text = Session["USER"].ToString();
            }
            else
            {
                Response.Redirect("../UserLogin.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                string id = Request.QueryString["id"];

                if (!string.IsNullOrEmpty(id))
                {
                    //lblPlotId.Text = id;
                    LoadUser();
                    LoadPlotDocument(id);
                }
                else
                {
                    lblError.Text = "No plot ID specified.";
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Application Error: " + ex.Message;
        }
    }

    public void LoadUser()
    {
        string sql = "SELECT Fullname, Location FROM Users WHERE Username = @username";

        using (SqlCommand cmd = new SqlCommand(sql, appconSQL2))
        {
            cmd.Parameters.AddWithValue("@username", lblSession.Text.Trim());

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    lblUser.Text = dr["Fullname"] != DBNull.Value ? dr["Fullname"].ToString() : "";
                    lblLocation.Text = dr["Location"] != DBNull.Value ? dr["Location"].ToString() : "";
                }
            }
        }
    }

    public void LoadPlotDocument(string Id)
    {
        string sketch = "";
        string sql = @"SELECT SiteMap FROM Sites WHERE ID = @Id";

        using (SqlCommand cmd = new SqlCommand(sql, appconSQL2))
        {
            cmd.Parameters.AddWithValue("@Id", Id);

            try
            {
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        sketch = dr["SiteMap"] != DBNull.Value ? dr["SiteMap"].ToString() : "";
                        lblPlotSketch.Text = sketch;
                    }
                }

                if (!string.IsNullOrEmpty(sketch))
                {
                    DisplayDocument(sketch);
                }
                else
                {
                    pnlDocumentViewer.Controls.Clear();
                    pnlDocumentViewer.Controls.Add(new LiteralControl(@"
                        <div class='document-placeholder'>
                            <i class='la la-file-excel-o'></i>
                            <h4>No Document Available</h4>
                            <p class='text-muted'>No sketch has been uploaded for this Site yet.</p>
                        </div>
                    "));
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error loading document: " + ex; //add .message here
            }
        }
    }

    private void DisplayDocument(string filename)
    {
        string filePath = Server.MapPath("~/assets/docs/" + filename);

        if (File.Exists(filePath))
        {
            string extension = Path.GetExtension(filename).ToLower();
            pnlDocumentViewer.Controls.Clear();

            if (extension == ".pdf")
            {
                // Display PDF using embed tag
                string pdfHtml = string.Format(@"
                    <embed src='../assets/docs/{0}' type='application/pdf' class='document-viewer' />
                    <div class='text-center mt-3'>
                        <small class='text-muted'>If PDF does not display, <a href='../assets/docs/{0}' target='_blank'>click here to open in new tab</a></small>
                    </div>", filename);
                pnlDocumentViewer.Controls.Add(new LiteralControl(pdfHtml));
            }
            else if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".bmp")
            {
                // Display image
                string imgHtml = string.Format(@"
                    <div class='text-center'>
                        <img src='../assets/docs/{0}' alt='Plot Sketch' style='max-width: 100%; max-height: 70vh; border-radius: 8px; box-shadow: 0 0.15rem 1.75rem 0 rgba(58, 59, 69, 0.1);' />
                    </div>", filename);
                pnlDocumentViewer.Controls.Add(new LiteralControl(imgHtml));
            }
            else if (extension == ".docx" || extension == ".doc")
            {
                // For Word documents, prompt download
                string docHtml = string.Format(@"
                    <div class='document-placeholder'>
                        <i class='la la-file-word-o'></i>
                        <h4>Word Document</h4>
                        <p class='text-muted'>This document cannot be previewed directly.</p>
                        <a href='../assets/docs/{0}' class='btn btn-primary mt-3' download>
                            <i class='la la-download'></i> Download Document
                        </a>
                    </div>", filename);
                pnlDocumentViewer.Controls.Add(new LiteralControl(docHtml));
            }
            else
            {
                // Generic file display with download option
                string genericHtml = string.Format(@"
                    <div class='document-placeholder'>
                        <i class='la la-file-text-o'></i>
                        <h4>Document Available</h4>
                        <p class='text-muted'>File: {0}</p>
                        <a href='../assets/docs/{0}' class='btn btn-primary mt-3' download>
                            <i class='la la-download'></i> Download File
                        </a>
                    </div>", filename);
                pnlDocumentViewer.Controls.Add(new LiteralControl(genericHtml));
            }
        }
        else
        {
            pnlDocumentViewer.Controls.Clear();
            pnlDocumentViewer.Controls.Add(new LiteralControl(@"
                <div class='document-placeholder'>
                    <i class='la la-file-excel-o'></i>
                    <h4>File Not Found</h4>
                    <p class='text-muted'>The document file could not be located on the server.</p>
                </div>
            "));
            lblError.Text = "Document file not found: " + filename;
        }
    }

    

    //protected override void Dispose(bool disposing)
    //{
    //    if (disposing && appconSQL2 != null && appconSQL2.State == ConnectionState.Open)
    //    {
    //        appconSQL2.Close();
    //        appconSQL2.Dispose();
    //    }
    //    base.Dispose(disposing);
    //}
}