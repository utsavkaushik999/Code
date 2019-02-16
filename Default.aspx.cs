using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if ((FileUpload1.HasFile))
        {
            if (!Convert.IsDBNull(FileUpload1.PostedFile) & FileUpload1.PostedFile.ContentLength > 0)
            {
                //  path = System.IO.Path.GetFullPath(FileUpload1.FileName);
                if (!(System.IO.Path.GetExtension(FileUpload1.FileName).ToLower() == ".xml"))
                {
                    AlertMsg("Only File of type XML extension are allowed");
                    return;
                }
                //FIRST, SAVE THE SELECTED FILE IN THE ROOT DIRECTORY.
                FileUpload1.SaveAs(Server.MapPath(".") + "\\input.xml");
            }
            else
            {
                AlertMsg("XML file has no content");
                return;
            }
        }
        int distance = 1000;
        int horse_count = 4;

        string[] nam ;
        int[] lane, basespeed, loop1, loop2, final_speed,time_taken, sorted;
        lane = new int[horse_count];
        nam = new string[horse_count];
        basespeed = new int[horse_count];
        loop1 = new int[horse_count];
        loop2 = new int[horse_count];
        final_speed = new int[horse_count];
        time_taken = new int[horse_count];
        sorted = new int[horse_count];

        XmlDocument doc = new XmlDocument();
        doc.Load(Server.MapPath(".") + "\\input.xml");
        XmlNode nodeResult = doc.SelectSingleNode("/harryKart");
        if (nodeResult != null)
        {
            XmlNodeList nodeList = doc.SelectNodes("/harryKart/startList");
            foreach (XmlNode node in nodeList)
            {
                XmlNodeList childNodes = node.ChildNodes;
                int i = 0;
                foreach (XmlNode childNode in childNodes)
                {
                    lane[i] = Convert.ToInt16(childNode["lane"].InnerText.ToString());
                    nam[i] = childNode["name"].InnerText.ToString();
                    basespeed[i] = Convert.ToInt16(childNode["baseSpeed"].InnerText.ToString());
                    i = i + 1;
                }
            }

            XmlNodeList nodeList1 = doc.SelectNodes("/harryKart/powerUps/loop");
            foreach (XmlNode node1 in nodeList1)
            {
                XmlNodeList childNodes = node1.ChildNodes;
                int i=1;
                if (node1.Attributes["number"].Value == "1")
                {
                    foreach (XmlNode childNode in childNodes)
                    {
                        loop1[i - 1] = Convert.ToInt16(childNode.InnerText.ToString());
                        i = i + 1;
                    }
                }
                else
                {
                    foreach (XmlNode childNode in childNodes)
                    {
                        loop2[i - 1] = Convert.ToInt16(childNode.InnerText.ToString());
                        i = i + 1;
                    }
                }
            }
        }

        //COMPLETING 1ST ROUND
        for (int i = 0; i < horse_count; i++)
        {
            final_speed[i] = basespeed[i];
            time_taken[i] = time_taken[i] + distance / final_speed[i];
        }

        // COMPLETING 2ND ROUND
        for (int i = 0; i < horse_count; i++)
        {
            final_speed[i] = final_speed[i] + loop1[i];
            time_taken[i] = time_taken[i] + distance / final_speed[i];
        }

        //COMPLETING 3RD ROUND
        for (int i = 0; i < horse_count; i++)
        {
            final_speed[i] = final_speed[i] + loop2[i];
            time_taken[i] = time_taken[i] + distance / final_speed[i];
            sorted[i] = time_taken[i];
        }

        //WRITING TO JSON FILE
        Array.Sort(sorted);
        List <ranking> lst = new List<ranking>();
        string path = Server.MapPath(".") + "\\output.json";
        System.IO.File.Delete(path);
        string JSONresult="";
        for (int i = 0; i < 3; i++)
        {
            lst.Add(new ranking(i + 1, nam[Array.IndexOf(time_taken, sorted[0])].ToString()));
        }
        JSONresult = JsonConvert.SerializeObject(lst, Newtonsoft.Json.Formatting.Indented );
        System.IO.File.AppendAllText(path, JSONresult);
        Label1.Text = "JSON File generated successfully at: " + path;
    }
 
    public class ranking
    {
        public ranking(int p, string h)
        {
            this.position = p;
            this.horse = h;
        }
        public int position{ set; get; }
        public string horse{ set; get; }
    }
    protected void AlertMsg(string msg)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<script type = 'text/javascript'>");
        sb.Append("window.onload=function(){");
        sb.Append("alert('");
        sb.Append(msg);
        sb.Append("')};");
        sb.Append("</script>");
        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
    }
}