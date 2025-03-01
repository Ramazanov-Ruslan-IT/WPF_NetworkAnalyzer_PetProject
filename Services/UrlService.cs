using NetworkAnalyzer.Models;
using System;

namespace NetworkAnalyzer.Services
{
    public class UrlService
    {
        public UrlInfo ParseUrl(string url)
        {
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "http://" + url;
            }
            Uri uri = new Uri(url);
            return new UrlInfo
            {
                Scheme = uri.Scheme,
                Host = uri.Host,
                Port = uri.Port,
                Path = uri.AbsolutePath,
                Query = uri.Query,
                Fragment = uri.Fragment
            };
        }
    }
}
