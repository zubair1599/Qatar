using HtmlAgilityPack;
using QatarRacing.EF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QatarRacing.UI.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            DownloadData("http://www.qrec.gov.qa/Website/EquisoftPage.aspx?page=RaceResults&raceFile=7050&lang=en", 7050);
            return View();
        }

        public async Task<HtmlDocument> GetData(string url)
        {
            try
            {
                HttpClient http = new HttpClient();
                var response = await http.GetStringAsync(url).ConfigureAwait(false);

                //String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
                string source = response;
                source = WebUtility.HtmlDecode(source);
                HtmlDocument resultat = new HtmlDocument();
                resultat.LoadHtml(source);
                return resultat;
            }
            catch (Exception ex)
            {


                //throw;
            }

            return null;

        }

        public void DownloadData(string url , int ID)
        {
            string MAIN_URL = url;

            HtmlDocument doc;
            Task<HtmlDocument> tsk = GetData(MAIN_URL);
            doc = tsk.Result;
            Link lnk = new Link();
            lnk.URL = (MAIN_URL);


            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//p[@class='margin_top margin_bottom']"))
            {
                string value = node.InnerText;
                // etc...
            }
            //var node = doc.DocumentNode.SelectNodes("//p[@class=margin_top margin_bottom]");
            //if (node!=null)
            {
                //int locationanddate= node.InnerText.Split(',').Count();
                //DateTime date = DateTime.Parse( node.InnerText.Split(',').ElementAt(locationanddate - 1));
                //Race race = new Race();
                //race.Location = node.InnerText.Replace(node.InnerText.Split(',').ElementAt(locationanddate - 1),string.Empty);



            }
        }

        // GET: Default/Details/5
      
    }
}
