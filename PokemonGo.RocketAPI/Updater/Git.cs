#region

using System;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace PokemonGo.RocketAPI.Updater
{
    public static class Git
    {
        public static async Task CheckVersion()
        {
            try
            {
                var match =
                    new Regex(
                        @"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]")
                        .Match(DownloadServerVersion());

                if (!match.Success) return;
                var gitVersion =
                    new Version(
                        string.Format(
                            "{0}.{1}.{2}.{3}",
                            match.Groups[1],
                            match.Groups[2],
                            match.Groups[3],
                            match.Groups[4]));

                if (gitVersion <= Assembly.GetExecutingAssembly().GetName().Version) return;


                Logger.Write("There is a new Version available: " + gitVersion);
                Logger.Write("Link: https://github.com/FeroxRev/Pokemon-Go-Rocket-API");
                Thread.Sleep(1000);
            }
            catch (Exception)
            {
                Logger.Write("Unable to check for updates...");
            }
        }

        private static string DownloadServerVersion()
        {
            using (var wC = new WebClient())
                return
                    wC.DownloadString(
                        "https://raw.githubusercontent.com/FeroxRev/Pokemon-Go-Rocket-API/master/PokemonGo.RocketAPI.Console/Properties/AssemblyInfo.cs");
        }
    }
}