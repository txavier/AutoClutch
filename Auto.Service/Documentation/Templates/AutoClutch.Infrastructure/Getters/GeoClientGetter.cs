using NerdLunch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Getters
{
    public class GeoClientGetter : IGeoClientGetter
    {
        public GeoClientGetter()
        { }

        public async Task<Core.Objects.GeoClient.RootObject> GetGeoClient(string address)
        {
            //var nc = System.Net.CredentialCache.DefaultNetworkCredentials;
            NetworkCredential nc = new NetworkCredential("dertaapplication", "password#1", "ds");

            WebProxy proxy = new WebProxy("http://dep-prxy2:8080");

            proxy.Credentials = nc;

            WebRequest.DefaultWebProxy = proxy;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.cityofnewyork.us/geoclient/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                HttpResponseMessage response = await client.GetAsync("v1/search.json?input=" + address 
                    + "&app_id=3650d41a&app_key=2212bb49d9274c60e0dc92e2edcd14ee");

                if (response.IsSuccessStatusCode)
                {
                    Core.Objects.GeoClient.RootObject rootObject = await response.Content.ReadAsAsync<Core.Objects.GeoClient.RootObject>();

                    return rootObject;
                }
            }

            return null;
        }
    }
}
