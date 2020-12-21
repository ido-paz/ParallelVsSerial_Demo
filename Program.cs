using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace ParallelVsSerialDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> addresses = new List<string>()//create a websites address list
            {"http://msn.co.il","https://yahoo.com","https://cnn.com","https://bbc.com","https://ynet.co.il",
             "http://youtube.com","https://facebook.com","https://jango.com","https://soundcloud.com","https://rotter.net"};
            //
            Console.WriteLine("----------");
            GetWebsitesData_Serial(addresses);
            Console.WriteLine("----------");
            GetWebsitesData_Parralel(addresses);
        }
        
        ///get the websites data in the list in serial manner
        static void GetWebsitesData_Serial(List<string> addresses)
        {
            Console.WriteLine($"{DateTime.Now}:GetWebsitesData_Serial started");
            //
            Stopwatch stopwatch =new Stopwatch();
            List<WebSiteData> webSiteDataList = new List<WebSiteData>(); 
            stopwatch.Start();
            //
            foreach (string address in addresses)//get webSiteData and add it to a list
                webSiteDataList.Add(GetWebsiteData(address));
            //
            foreach (WebSiteData webSiteData in webSiteDataList)//display webSiteData
                Console.WriteLine($"Address:{webSiteData.Address},Size:{webSiteData.DataSize},DownloadDuration:{webSiteData.DownloadDuration}");
            //
            stopwatch.Stop();          
            //
            Console.WriteLine($"{DateTime.Now}:GetWebsitesData_Serial ended and took {stopwatch.Elapsed}");
        }
        
        ///get the websites data in the list in Parralel manner
        static void GetWebsitesData_Parralel(List<string> addresses)
        {
            Console.WriteLine($"{DateTime.Now}:GetWebsitesData_Parralel started");
            //
            Stopwatch stopwatch =new Stopwatch();
            List<Task<WebSiteData>> webSiteDataTaskList = new List<Task<WebSiteData>>(); 
            stopwatch.Start();
            //
            foreach (string address in addresses)//run tasks that fetchs webSiteData and add it to a list
                webSiteDataTaskList.Add(Task.Run(()=>GetWebsiteData(address)));
            //
            Task.WaitAll(webSiteDataTaskList.ToArray());
            //
            foreach (Task<WebSiteData> webSiteDataTask in webSiteDataTaskList)//display webSiteData
                Console.WriteLine($"Address:{webSiteDataTask.Result.Address},Size:{webSiteDataTask.Result.DataSize},DownloadDuration:{webSiteDataTask.Result.DownloadDuration}");
            //
            stopwatch.Stop();             
            //
            Console.WriteLine($"{DateTime.Now}:DownloadWebsites_Parralel ended and took {stopwatch.Elapsed}");
        }

        ///get web site data function
        static WebSiteData GetWebsiteData(string address)
        {
            WebClient webClient = new WebClient();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            byte[] data = webClient.DownloadData(address);
            stopwatch.Stop();
            return new WebSiteData(){Address=address,DownloadDuration = stopwatch.Elapsed,DataSize = data.Length};
        }
    }
}
