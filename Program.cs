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
            PrintWebsitesData_Serial(addresses);
            Console.WriteLine("----------");
            PrintWebsitesData_Parralel(addresses);
        }
        
        ///print the websites data in the list in serial manner
        static void PrintWebsitesData_Serial(List<string> addresses)
        {
            Console.WriteLine($"{DateTime.Now}:PrintWebsitesData_Serial started");
            //
            Stopwatch stopwatch =new Stopwatch();
            stopwatch.Start();
            //
            foreach (string address in addresses)//print webSiteData
                PrintWebsiteData(address);
            //
            stopwatch.Stop();          
            //
            Console.WriteLine($"{DateTime.Now}:PrintWebsitesData_Serial ended and took {stopwatch.Elapsed}");
        }
        
        ///print the websites data in the list in Parralel manner
        static void PrintWebsitesData_Parralel(List<string> addresses)
        {
            Console.WriteLine($"{DateTime.Now}:PrintWebsitesData_Parralel started");
            //
            Stopwatch stopwatch =new Stopwatch();
            List<Task> pwsdTaskList = new List<Task>(); 
            stopwatch.Start();
            //
            foreach (string address in addresses)//run tasks that prints webSiteData 
                pwsdTaskList.Add(Task.Run(()=>PrintWebsiteData(address)));
            //
            Task.WaitAll(pwsdTaskList.ToArray());
            //
            stopwatch.Stop();             
            //
            Console.WriteLine($"{DateTime.Now}:PrintWebsitesData_Parralel ended and took {stopwatch.Elapsed}");
        }

        ///print web site data function
        static void PrintWebsiteData(string address)
        {
            WebClient webClient = new WebClient();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            byte[] data = webClient.DownloadData(address);
            stopwatch.Stop();
            Console.WriteLine($"Address:{address},Size:{data.Length},DownloadDuration:{ stopwatch.Elapsed}");
        }
    }
}
