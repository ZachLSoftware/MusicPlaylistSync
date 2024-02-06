using Google.Apis.YouTube.v3.Data;
using SpotifyAPI.Web;

namespace MusicPlaylistSync
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //Console.WriteLine("Test!");

            //YoutubeApi api = new YoutubeApi();
            //SpotifyApi spotifyApi = new SpotifyApi();

            //bool login = true;
            //while (login) 
            //{
            //    bool loggedin = await api.login();
            //    if (loggedin)
            //    {
            //        login = false;
            //    }
            //    else
            //    {
            //        Console.WriteLine("Log in failed, try again.");
            //    }
            //}
            

            //List<(string,string)> youtubeList = await api.getTracks();
            //await spotifyApi.implicitGrant();

            //List<(string,string)> tracks = await spotifyApi.getTracks();

            //listCompare lc = new listCompare(youtubeList, tracks);

            //List<List<(string,string)>> allLists = lc.getLists();
            ////if (allLists[2].Count > 0)
            ////{
            ////    foreach (var track in allLists[2])
            ////    {
            ////        await api.songSearch(track);
            ////    }
            ////}
            //if (allLists[3].Count > 0)
            //{
            //    foreach (var track in allLists[3])
            //    {
            //        await spotifyApi.songSearch(track);
            //    }
            //}
        }
    }
}
