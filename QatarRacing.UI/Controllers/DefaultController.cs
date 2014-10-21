using HtmlAgilityPack;
using QatarRacing.EF;
using QatarRacing.EF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
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
            for (int i = 7230; i < 7500; i++)
                {
            try
            {
               
                    DownloadData(i);
                    System.Diagnostics.Trace.WriteLine("Done for ID :" + i);
               
            }
            catch (Exception ex)
            {

                System.Diagnostics.Trace.WriteLine("EXCEPTION for ID :" + i);
            }
             }
            
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

        public void DownloadData(int ID)
        {

            string raceDetails = "http://www.qrec.gov.qa/CF/pageLoader.cfm?page=RaceResults&raceFile=" + ID.ToString();
            string raceInnerDetailes = "http://www.qrec.gov.qa/CF/pages/RaceResultDetails.cfm?raceID=" + ID.ToString();


            RaceContext cont = new RaceContext();


            HtmlDocument docOutDetails , docInDetails;
            Task<HtmlDocument> tskOuter = GetData(raceDetails);
            Task<HtmlDocument> tskInner = GetData(raceInnerDetailes);

            docOutDetails = tskOuter.Result;

            var mainParent = docOutDetails.DocumentNode.ChildNodes.Count>3 ? docOutDetails.DocumentNode.ChildNodes[3].ChildNodes[2].ChildNodes.Where(m => m.Attributes.Contains("style") && m.Attributes["style"].Value.Contains("width:600px;")).SingleOrDefault() : null;
            if (mainParent!=null)
            {
                string raceHeader = mainParent.ChildNodes.Where(m => m.Name == "p").SingleOrDefault().InnerText;


                Race race = new Race();
                DateTime date = DateTime.Parse("1/1/0001") ;
                if (raceHeader.Contains(','))
                {
                    
                    int locationanddate = raceHeader.Split(',').Count();
                    date = DateTime.Parse(raceHeader.Split(',').ElementAt(locationanddate - 1));
                    race.Date = date;
                }
                

                
                race.WinningPrices = new List<WinningPrice>();
                race.Runners = new List<Runner>();

                




                var tabRaces = mainParent.ChildNodes.Where(m => m.Attributes.Contains("id") && m.Attributes["id"].Value.Contains("tabsRace")).SingleOrDefault();
                if (tabRaces!=null)
                {
                    var allRacesOfLink = tabRaces.ChildNodes.Where(m => m.Name == "div").ToList();
                    var p = (from a in allRacesOfLink where (a.Attributes.Contains("id") && a.Attributes["id"].Value.Contains("tabRace_" + ID.ToString())) select a).SingleOrDefault();
                    var raceOuterDetail = p.ChildNodes.Where(m => m.Name == "div").ToList().ElementAt(0);
                    var raceDetail = raceOuterDetail.ChildNodes.Where(m => m.Name == "div").ToList().ElementAt(0);
                    
                    if (date.Year != 1)
                    {
                        var tmp1 = raceDetail.ChildNodes.Where(m => m.Name == "p").ToList();

                        

                        if (tmp1.Count()>2)
                        
                        {
                            race.Title = tmp1.ElementAt(0).InnerText;

                            var tmp2 = tmp1.ElementAt(1).ChildNodes.Where(m => m.Name == "span").ToList();
                            if (tmp2.Count==2)
                            {
                                date = date.AddHours(double.Parse(raceDetail.ChildNodes.Where(m => m.Name == "p").ToList().ElementAt(1).ChildNodes.Where(m => m.Name == "span").ToList().ElementAt(0).InnerText.Split(':')[0]));
                                date = date.AddMinutes(double.Parse(raceDetail.ChildNodes.Where(m => m.Name == "p").ToList().ElementAt(1).ChildNodes.Where(m => m.Name == "span").ToList().ElementAt(0).InnerText.Split(':')[1]));
                                race.Time = date;
                                race.TrackLength = raceDetail.ChildNodes.Where(m => m.Name == "p").ToList().ElementAt(1).ChildNodes.Where(m => m.Name == "span").ToList().ElementAt(1).InnerText;
                            }
                            else
                            {
                                race.TrackLength = raceDetail.ChildNodes.Where(m => m.Name == "p").ToList().ElementAt(1).ChildNodes.Where(m => m.Name == "span").ToList().ElementAt(0).InnerText;
                            }

                        }

                        
                    }

                    
                    race.TrackType = raceDetail.ChildNodes.Where(m => m.Name == "p").ToList().ElementAt(2).InnerText.Split(':')[1];
                    var winningPrize = raceOuterDetail.ChildNodes.Where(m => m.Name == "div").ToList().ElementAt(2).ChildNodes.Where(m => m.Name == "ul").SingleOrDefault().ChildNodes.Where(m => m.Name == "li").ToList();

                    for (int i = 0; i < winningPrize.Count; i++)
                    {
                        WinningPrice prize = new WinningPrice();
                        prize.Position = i + 1;
                        var tmpPrize = winningPrize[i].ChildNodes.ElementAt(2).InnerText.Trim().Split('.')[0];
                        prize.Price = long.Parse(tmpPrize);
                        prize.Race = race;
                        race.WinningPrices.Add(prize);

                        cont.WinningPrizes.Add(prize);

                    }
                    if (raceDetail.ChildNodes.Where(m => m.Name == "p").ToList().Count > 2)
                    {
                        if (raceDetail.ChildNodes.Where(m => m.Name == "p").ToList().Count > 3)
                        {
                            var text = raceDetail.ChildNodes.Where(m => m.Name == "p").ToList().ElementAt(3).InnerText;
                            if (text.Contains(':'))
                            {
                                race.RaceConditions = text.Split(':')[1];
                            }
                        }
                    }



                    docInDetails = tskInner.Result;
                    var ty = docInDetails.DocumentNode.ChildNodes.Where(m => m.Name == "table").SingleOrDefault();
                    if (ty != null)
                    {
                        var tmainParent = ty.ChildNodes.Where(m => m.Name == "tbody").SingleOrDefault().ChildNodes.Where(m => m.Name == "tr").ToList();
                        for (int i = 0; i < tmainParent.Count; i++)
                        {
                            Runner runn = new Runner();
                            var currentCols = tmainParent[i].ChildNodes.Where(t => t.Name == "td").ToList();
                            runn.Position = currentCols[0].InnerText.Trim();
                            runn.Margin = currentCols[1].InnerText.Trim();
                            runn.OR = currentCols[2].InnerText.Trim();
                            string nameRegex = currentCols[3].InnerHtml;


                            int start = nameRegex.IndexOf("<p >");
                            int end = nameRegex.IndexOf("</p>");
                            string paa = nameRegex.Substring(start + 4, end - start);
                            paa = paa.Trim();
                            start = paa.IndexOf(">");
                            end = paa.IndexOf("</a>");

                            paa = paa.Substring(start + 1, end - start).Trim();
                            runn.Name = paa;
                            runn.Horse = currentCols[3].InnerText;


                            Jockey joc = new Jockey();
                            joc.Weight = decimal.Parse(currentCols[4].InnerText);
                            runn.Equipment = currentCols[5].InnerText;
                            runn.Trainer = currentCols[6].InnerText;
                            joc.Name = currentCols[7].InnerText;
                            joc.Runners = new List<Runner>();
                            runn.Jockey = joc;
                            joc.Runners.Add(runn);

                            runn.Race = race;
                            race.Runners.Add(runn);



                            cont.Jockeys.Add(joc);
                            cont.Runners.Add(runn);


                        }

                        Link lnk = new Link();
                        lnk.URL = ID.ToString();
                        lnk.Races = new List<Race>();
                        lnk.Races.Add(race);

                        cont.Links.Add(lnk);
                        cont.Races.Add(race);
                        cont.SaveChanges();
                    }
                }
               

                
            }
           
        }

        // GET: Default/Details/5
      
    }
}
