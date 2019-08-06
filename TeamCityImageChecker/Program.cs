using System;
using System.Linq;
using System.Net;
using CommandLine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using TeamCityImageChecker.RegistryJson;

namespace TeamCityImageChecker
{
    class Program
    {
        private static RestClient _Client;

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(o => ProccessRegistry(o.Image, o.Registry));
        }

        private static void ProccessRegistry(string image, string registry)
        {
            _Client = new RestClient(registry);

            // Get list of all images
            var tagListResponse = GetTagList(image);

            // Get info about latest tag
            var historyDate = GetManifest(image, "latest");

            var allTagInfo = tagListResponse.Tags.Where(i => i != "latest")
                                            .Select(i => GetManifest(image, i))
                                            .OrderBy(i => i.Created).ToList();
            if (allTagInfo.Count == 0)
            {
                return;
            }

            if (allTagInfo[allTagInfo.Count - 2].Container == historyDate.Container)
            {
                Console.WriteLine("##teamcity[buildStop comment='canceling comment' readdToQueue='true']");
            }
            else
            {
                Console.WriteLine("Not equal!");
            }
        }

        private static HistoryCompartibility GetManifest(string image, string tagName)
        {
            var request = new RestRequest("v2/{image}/manifests/{tagName}", Method.GET);
            request.AddUrlSegment("image", image);
            request.AddUrlSegment("tagName", tagName);
            var response = _Client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Invalid response!");
            }

            var latestInfo = JObject.Parse(response.Content);
            var historyDate = JsonConvert.DeserializeObject<HistoryCompartibility>(
                    latestInfo.SelectToken("history[0].v1Compatibility").Value<string>());
            historyDate.Tag = latestInfo.SelectToken("tag").Value<string>();
            return historyDate;
        }

        private static TagsList GetTagList(string image)
        {
            var request = new RestRequest("v2/{image}/tags/list", Method.GET);
            request.AddUrlSegment("image", image);

            var tagListResponse = _Client.Execute<TagsList>(request);
            if (tagListResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Invalid response!");
            }

            return tagListResponse.Data;
        }
    }
}
