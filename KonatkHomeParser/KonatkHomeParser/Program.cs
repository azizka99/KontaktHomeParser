using HtmlAgilityPack;
using KonatkHomeParser.Classes;
using KonatkHomeParser.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace KonatkHomeParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string sitemap = "https://kontakt.az/sitemap.xml";

            

            GoAndGet(FilterLinks(SitemapGetLinks(sitemap)));

            

            
        }

        //Этот метод соединяется с сервером.
        private static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(fullUrl);
            return response;
        }

        //этот метод с обзора берет инфу, как бы с поиска.(ПОКА ЧТО ЛИШНИЙ МЕТОД, НЕ НУЖЕН)
        private static List<Phone> ParsePhones(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var phoneList = new List<Phone>();
            var nodelist = new List<HtmlNodeCollection>();
            HtmlNodeCollection phonecard = htmlDoc.DocumentNode.SelectNodes("//div[@class=\"cart-body-top\"]");

            if (phonecard != null)
            {
                foreach (var product in phonecard)
                {
                    var nameNode = product.SelectSingleNode(".//div[@class=\"name\"]/a").InnerText;
                    Console.WriteLine(nameNode);
                    var priceNode = product.SelectSingleNode(".//div[@class=\"cart-footer\"]/button").Attributes["data-price"].Value;
                    Console.WriteLine(priceNode);

                }
            }
            return null;
        }

        //Этот метод Парсит Ситмеп, берет все ноды со ссылкой, если что посмотри на принцип  работы метода.
        private static List<string> SitemapGetLinks(string link)
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(link);


            XmlElement? xRoot = xmlDoc.DocumentElement;
            XmlNodeList nodes = xRoot?.SelectNodes("*");
            List<string> mylinks = new List<string>();
            if (nodes != null)
            {
                foreach (XmlNode item in nodes)
                {
                    //Console.WriteLine(item.FirstChild.InnerText);
                    mylinks.Add(item.FirstChild.InnerText);
                }
            }
            // Console.WriteLine($"MyLinks Coung: {mylinks.Count}");

            return mylinks;
        }

        //Этот метод фильтриует ссылки по реджексу и находит нам необходимые.
        private static List<string> FilterLinks(List<string> links)
        {

            Regex myRegex = new Regex(@"^https:\/\/kontakt\.az\/sitemap-pt-post-p\d{1,2}-20\d\d-[01]\d\.xml$");
            List<string> resultString = links.Where(f => myRegex.IsMatch(f)).ToList();

            //foreach (var item in resultString)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine(resultString.Count);
            return resultString;
        }


        //это я пытался написать один большой метод который по всем ссылкам находил ссылки на товары и парсил, но так как
        //это будет занимать ДОХуя времени решил на одном линке затестить работу а потом уже один большой метод написать.
        private static void GoAndGet(List<string> links)
        {
            List<string> mylinks = new List<string>();
            foreach (var item in links)
            {
                mylinks = GetRidOfRULinks(SitemapGetLinks(item));
                foreach (var xuy in mylinks)
                {
                    ParseSinglePage(xuy);
                }
            }
        }

        //Это практически копия вышестоящего метода, только всего лишь один определенный линк проверяет.
        //(Работает, но как бы уже не нужен, поскольку GoAndGet Всю работу делает.)
        private static void SingleGoAndGet(string link)
        {
            List<string> links = SitemapGetLinks(link);
            List<string> myLinks = GetRidOfRULinks(links);

            foreach (var item in myLinks)
            {
                ParseSinglePage(item);

            }

        }

        //Этот метод парсит страницу товара, берет из него Имя товара, цену и код. Все данные пихает в класс Item,
        //И пока что только выводит на экран.
        private static void ParseSinglePage(string html)
        {
            var result = CallUrl(html).Result;
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(result);
            MyItem item = new MyItem();

            var name = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class,\"feature-container\")]/h1[@class=\"title\"]");
            var price = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class,\"cont-price\")]/div[@class=\"calculator-responsive-select-mother\"]/div[@class=\"price\"]/p/span[@class=\"nprice\"]");
            var code = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class,\"product-status\")]/p/span");

            if (name != null && price != null && code != null)
            {
                item.Name = name.InnerText;
                item.Price = Convert.ToDouble(price.InnerText);
                item.Code = code.InnerText;

                using (AppDbContext db = new AppDbContext())
                {
                    Item item1 = new Item
                    {
                        Name = name.InnerText,
                        Price = Convert.ToDouble(price.InnerText),
                        Code = code.InnerText,
                        LinkToItem = html
                    };
                    db.Items.Add(item1);
                    db.SaveChanges();
                }



                Console.WriteLine($"Name: {item.Name}\nPrice: {item.Price}\nCode: {item.Code}");
                Console.WriteLine(html);
                Console.WriteLine("---------------------------ParseSinglePage----------------------------");
            }
            else
            {
                Console.WriteLine($"Some of the variables are null \n ---{html}");
            }



        }

        //Этот метод избавляется от ссылок которые введут на товары на русском языке.
        private static List<string> GetRidOfRULinks(List<string> links)
        {
            Regex regex = new Regex(@"^https:\/\/kontakt\.az\/(?!ru\/)");
            List<string> result = links.Where(f => regex.IsMatch(f)).ToList();


            return result;
        }

        //И так, щас тебе надо Парсинг сингл пейдж дописать, это как бы метод который парсит страничку одного определенного товара,
        //но прежде всего тебе надо реджекс внедрить чтобы убрал ссылки которые ведут на страницы на русском языке.
        //Сначала Реджексом подфильтруй ссылки допиши ParseSinglePage, про FakeGoAndGet не забудь, чтобы сам заходил на ссылки и парсил
        //и дальше сам разберешься.

    }
}
