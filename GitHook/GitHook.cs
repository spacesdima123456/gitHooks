using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using GitHook.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GitHook
{
    public class GitHook
    {
        private const string Url = "http://35.237.131.8:7999";
        private const string Token = "c2FsaWtpbg==.NDgtMA==.mYK7lgtomcKc2KVcNVPE63EZovZBU5";

        public static void RunPreCommitGitHook(IList<string> args)
        {
            var merge = args.FirstOrDefault(f => f.ToLower().Contains("merge"));
            if (merge != null)
            {
                var branch = File.ReadAllText(args[0]).Replace("Merge branch", "").Replace("\'", "").Trim();
                var tags = ExecuteRequest<List<Tags>>($"/api/issues/{branch}/tags?fields=name");
                var issue = ExecuteRequest<Issues>($"/api/issues/{branch}?fields=summary");
                if (tags != null && issue != null)
                {
                    var message = DateTime.Now.ToString("dd-MM-yyyy'\'HH:mm:ss") + $" {tags[0].Name}. {issue.Summary} {branch}";
                    File.WriteAllText(args[0], message, Encoding.UTF8);
                }
            }
        }

        private static T ExecuteRequest<T>(string url)
        {
            try
            {
                T deserialize;
                var request = WebRequest.Create(Url + url);
                request.Headers.Add("access", Token);
                var response = request.GetResponse();

                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                    {
                        var json = reader.ReadToEnd();
                        deserialize = JsonConvert.DeserializeObject<T>(json);
                    }
                }

                response.Close();
                return deserialize;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return default;
            }
        }
    }
}
