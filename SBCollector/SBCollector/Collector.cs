using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abot.Crawler;
using Abot.Poco;
using System.Net;

namespace SBCollector
{
    class Collector
    {
        private PoliteWebCrawler Crawler;

        struct Element
        {
            string ID;
            string Name;
            string Actor;
            string Uri;
            string CoverUri;
            ulong FlagMask0;
            ulong FlagMask1;
        };

        public Collector()
        {
            CrawlConfiguration crawlConfig = new CrawlConfiguration();
            crawlConfig.MaxCrawlDepth = 1;

            Crawler = new PoliteWebCrawler(crawlConfig, new CrawlDecisionMaker(), null, new Scheduler(), null, null, null, null, null);
            Crawler.PageCrawlStartingAsync += ProcessPageCrawlStarting;
            Crawler.PageCrawlCompletedAsync += ProcessPageCrawlCompleted;
            Crawler.PageCrawlDisallowedAsync += PageCrawlDisallowed;
            Crawler.PageLinksCrawlDisallowedAsync += PageLinksCrawlDisallowed;
        }

        public void Start()
        {
            //Crawler.Crawl();
        }

        private void ProcessPageCrawlStarting(object sender, PageCrawlStartingArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            Console.WriteLine("About to crawl link {0} which was found on page {1}", pageToCrawl.Uri.AbsoluteUri, pageToCrawl.ParentUri.AbsoluteUri);
        }

        private void ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;

            if (crawledPage.WebException != null || crawledPage.HttpWebResponse.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Crawl of page failed {0}", crawledPage.Uri.AbsoluteUri);
            else
                Console.WriteLine("Crawl of page succeeded {0}", crawledPage.Uri.AbsoluteUri);

            if (string.IsNullOrEmpty(crawledPage.Content.Text))
                Console.WriteLine("Page had no content {0}", crawledPage.Uri.AbsoluteUri);

            var htmlAgilityPackDocument = crawledPage.HtmlDocument; //Html Agility Pack parser
            var angleSharpHtmlDocument = crawledPage.AngleSharpHtmlDocument; //AngleSharp parser
        }

        private void PageLinksCrawlDisallowed(object sender, PageLinksCrawlDisallowedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;
            Console.WriteLine("Did not crawl the links on page {0} due to {1}", crawledPage.Uri.AbsoluteUri, e.DisallowedReason);
        }

        private void PageCrawlDisallowed(object sender, PageCrawlDisallowedArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            Console.WriteLine("Did not crawl page {0} due to {1}", pageToCrawl.Uri.AbsoluteUri, e.DisallowedReason);
        }
    }
}
