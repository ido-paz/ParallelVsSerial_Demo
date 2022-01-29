using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WebPageCrawler;

namespace WebPageCrawler_Task
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> urlList = new List<string>()//create a WebPage address list
                    {
                     "https://msn.co.il","https://yahoo.com","https://walla.co.il",
                     "https://ynet.co.il","https://youtube.com","https://facebook.com",
                     "https://jango.com","https://soundcloud.com","https://rotter.net"
                    };
            //
            Console.WriteLine("----------");
            PrintWebPageData_Parralel(urlList); 
            Console.WriteLine("----------");
            PrintWebPageData_Serial(urlList);
        }
        
        ///print the WebPage data in the list in serial manner
        static void PrintWebPageData_Serial(List<string> urlList)
        {
            Console.WriteLine($"{DateTime.Now}:PrintWebPageData_Serial started");
            //
            Stopwatch stopwatch =new Stopwatch();
            stopwatch.Start();
            //
            foreach (string url in urlList)//print webPageData
                PrintWebPageData(url);
            //
            stopwatch.Stop();          
            //
            Console.WriteLine($"{DateTime.Now}:PrintWebPageData_Serial ended and took {stopwatch.Elapsed}");
        }
        
        ///print the WebPage data in the list in Parralel manner
        static void PrintWebPageData_Parralel(List<string> urlList)
        {
            Console.WriteLine($"{DateTime.Now}:PrintWebPageData_Parralel started");
            //
            Stopwatch stopwatch =new Stopwatch();
            List<Task> pwsdTaskList = new List<Task>(); 
            stopwatch.Start();
            //
            //Parallel.ForEach(urlList, (url) => PrintWebPageData(url));
            //Parallel.For(0, urlList.Count, (i) => PrintWebPageData(urlList[i]));
            foreach (string url in urlList)//run tasks that prints webpagedata 
                pwsdTaskList.Add(Task.Run(() => PrintWebPageData(url)));
            Task.WaitAll(pwsdTaskList.ToArray());
            //
            stopwatch.Stop();             
            //
            Console.WriteLine($"{DateTime.Now}:PrintWebPageData_Parralel ended and took {stopwatch.Elapsed}");
        }
        ///print web site data function
        static void PrintWebPageData(string url)
        {
            try
            {
                WebPageData webPageData = WebPageData.Get(url);
                Console.WriteLine($"Address:{webPageData.URL},Size:{webPageData.Length},DownloadDuration:{ webPageData.DownloadElapsedMiliSeconds}");
            }
            catch (Exception e )
            {
                Console.WriteLine(e.Message);
            }            
        }

    }
}
