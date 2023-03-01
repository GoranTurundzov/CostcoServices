using Newtonsoft.Json.Linq;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mail;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace ContosoServicesIntegration
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var clientId = "f5104fd2-6af1-4a52-8e18-97ac5102c9fc";
            var clientSecret = "4Mq8Q~SYLjYYV6P1WulHy.SKtATqULXZWx8MHccT";
            var resource = "00000015-0000-0000-c000-000000000000";
            var oauthUrl = "https://login.windows.net/be-terna.com/oauth2/token";

            // Get access token
            var httpClient = new HttpClient();
            var oauthService = new OAuthService(httpClient, clientId, clientSecret, resource);
            var accessToken = await oauthService.GetAccessTokenAsync();

            // Create HTTP client with default headers
           
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            TSTimesheetSubmissionServiceClient client = new TSTimesheetSubmissionServiceClient();

            HttpRequestMessageProperty requestProperty = new HttpRequestMessageProperty();
            requestProperty.Headers["Authorization"] = "Bearer " + accessToken;

            OperationContextScope contextScope = new OperationContextScope(client.InnerChannel);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestProperty;



            var arr = new List<TsTimesheetEntry>()
            {

                   new TsTimesheetEntry() {

                        parmResource =  5637145078,
                        parmProjectDataAreaId = "ussi",
                        parmProjId = "00000101",
                        parmProjActivityNumber = "W00002480",
                        parmEntryDate = new DateTime(2019,9,4,10,10,12),
                        parmHrsPerDay = 4
                    },
                   new TsTimesheetEntry() {

                        parmResource =  5637145078,
                        parmProjectDataAreaId = "ussi",
                        parmProjId = "00000101",
                        parmProjActivityNumber = "W00002480",
                        parmEntryDate = new DateTime(2019,9,4,10,10,12),
                        parmHrsPerDay = 4
                    }

            }.ToArray();


            //Test Entry
            //new TsTimesheetEntry()
            //{

            //    parmResource = 5637145201,
            //    parmTimesheetNumber = "00000055",
            //    parmProjectDataAreaId = "ussi",
            //    parmProjId = "00000092",
            //    parmProjActivityNumber = "W00002388",
            //    parmEntryDate = new DateTime(2019, 9, 9, 12, 00, 00),
            //    parmHrsPerDay = 7,

            //}




            TSTimesheetEntryList tSTimesheetEntryList = new TSTimesheetEntryList();
            tSTimesheetEntryList.__k_parmEntryList = arr;

            var res = await client.createOrUpdateTimesheetLineAsync(new CallContext(), tSTimesheetEntryList);


           
            client.Close();

           
         
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
