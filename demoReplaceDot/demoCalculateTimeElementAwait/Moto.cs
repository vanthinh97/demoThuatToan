using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoCalculateTimeElementAwait
{
    public class Moto
    {
        public int Id { get; set; }

        public async Task<int> AccessTheWebAsync()
        {
            HttpClient client = new HttpClient();

            // GetStringAsync returns a Task<string>. That means that when you await the
            Task<string> getStringTask = client.GetStringAsync("http://msdn.microsoft.com");

            // You can do work here that doesn't rely on the string from GetStringAsync.
            //DoIndependentWork();

            //  - The await operator then retrieves the string result from getStringTask.
            string urlContents = await getStringTask;

            // The return statement specifies an integer result.
            // Any methods that are awaiting AccessTheWebAsync retrieve the length value.
            return urlContents.Length;
        }

        public async Task<int> AccessTheWebAsync1()
        {
            HttpClient client = new HttpClient();

            // GetStringAsync returns a Task<string>. That means that when you await the
            Task<string> getStringTask = client.GetStringAsync("http://msdn.microsoft.com");

            // You can do work here that doesn't rely on the string from GetStringAsync.
            //DoIndependentWork();

            //  - The await operator then retrieves the string result from getStringTask.
            string urlContents = await getStringTask;

            // The return statement specifies an integer result.
            // Any methods that are awaiting AccessTheWebAsync retrieve the length value.
            return urlContents.Length;
        }

        public async Task<int> AccessTheWebAsync2()
        {
            HttpClient client = new HttpClient();

            // GetStringAsync returns a Task<string>. That means that when you await the
            Task<string> getStringTask = client.GetStringAsync("http://msdn.microsoft.com");

            // You can do work here that doesn't rely on the string from GetStringAsync.
            //DoIndependentWork();

            //  - The await operator then retrieves the string result from getStringTask.
            string urlContents = await getStringTask;

            // The return statement specifies an integer result.
            // Any methods that are awaiting AccessTheWebAsync retrieve the length value.
            return urlContents.Length;
        }

        public async Task<int> AccessTheWebAsync3()
        {
            HttpClient client = new HttpClient();

            // GetStringAsync returns a Task<string>. That means that when you await the
            Task<string> getStringTask = client.GetStringAsync("http://msdn.microsoft.com");

            // You can do work here that doesn't rely on the string from GetStringAsync.
            //DoIndependentWork();

            //  - The await operator then retrieves the string result from getStringTask.
            string urlContents = await getStringTask;

            // The return statement specifies an integer result.
            // Any methods that are awaiting AccessTheWebAsync retrieve the length value.
            return urlContents.Length;
        }
    }
}
