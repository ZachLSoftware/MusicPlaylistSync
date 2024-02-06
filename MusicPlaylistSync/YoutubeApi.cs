using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;


namespace MusicPlaylistSync
{
    public class YoutubeApi
    {

        private YouTubeService youtubeService;
        private string playlistId;
        private string clientId;
        private string clientSecret;

        public YoutubeApi(string client, string secret)
        {
            this.clientId = client;
            this.clientSecret = secret;
        }
        public async Task<bool> login()
        {
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = clientId,
                        ClientSecret = clientSecret
                    },
                    new[] { YouTubeService.Scope.Youtube },
                    "user",
                    cts.Token,
                    new FileDataStore("Youtube.Auth.Store"));

                youtubeService = new YouTubeService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Music Playlist Sync"
                });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<(string, string)>> getTracks() { 
            var playlistRequest = youtubeService.Playlists.List("snippet,contentDetails");
            playlistRequest.Mine = true;
            playlistRequest.MaxResults = 10;
            List<(string, string)> tracks = new List<(string, string)>();

            try
            {
                var playlistsResponse = await playlistRequest.ExecuteAsync();

                foreach (var playlist in playlistsResponse.Items)
                {
                    if (playlist.Snippet.Title == "API_Test")
                    {
                        var playlistItemsRequest = youtubeService.PlaylistItems.List("snippet");
                        playlistId = playlist.Id;
                        playlistItemsRequest.PlaylistId = playlist.Id;
                        playlistItemsRequest.MaxResults = 10; // Limit the number of items for testing purposes

                        var playlistItemsResponse = await playlistItemsRequest.ExecuteAsync();
                      
                        // Print the titles of the videos in the playlist
                        foreach (PlaylistItem playlistItem in playlistItemsResponse.Items)
                        {
                            var artist = playlistItem.Snippet.VideoOwnerChannelTitle.Split(" - Topic")[0];
                            tracks.Add((playlistItem.Snippet.Title, artist));
                            Console.WriteLine(playlistItem.Snippet.ResourceId.VideoId);
                        }
                        Console.WriteLine();
                    }
                }
                return tracks;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return null;
            }
        }

        public async Task songSearch((string, string) song)
        {
            try
            {
                var channelSearch = youtubeService.Search.List("snippet");
                channelSearch.Q = song.Item2 + " - Topic";
                channelSearch.Type = "channel";
                //songSearch.VideoCategoryId = "10";
                channelSearch.MaxResults = 5;
                

                var results = await channelSearch.ExecuteAsync();
                string cId = "";

                foreach (var result in results.Items)
                { Console.WriteLine(result.Snippet.Title);
                    if (result.Snippet.Title == song.Item2 + " - Topic")
                    {
                        cId = result.Id.ChannelId;
                    }
                }
                //Console.WriteLine("Enter Number of song to Add");
                //int selection;
                //var parsed = int.TryParse(Console.ReadLine(), out selection);
                //var resource = results.Items[selection];
                //if (parsed)
                //{
                

                var songSearch = youtubeService.Search.List("snippet");
                songSearch.Q = song.Item1;
                songSearch.ChannelId = cId;
                songSearch.Type = "video";
                songSearch.VideoCategoryId = "10";
                songSearch.MaxResults = 1;

                
                var vidresults = await songSearch.ExecuteAsync();
                int count = 0;

                //Console.WriteLine("Enter Number of song to Add");
                //int selection;
                //var parsed = int.TryParse(Console.ReadLine(), out selection);
                //var resource = results.Items[selection];
                //if (parsed)
                //{
                    var playlistItem = new PlaylistItem();
                    playlistItem.Snippet = new PlaylistItemSnippet();
                    playlistItem.Snippet.ResourceId = vidresults.Items[0].Id;
                    playlistItem.Snippet.PlaylistId = playlistId;
                    playlistItem = await youtubeService.PlaylistItems.Insert(playlistItem, "snippet").ExecuteAsync();
                    Console.WriteLine("Playlist item id {0} was added to playlist id {1}.", playlistItem.Id, playlistId);


               // }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() );
            }

            
        }
    }
}
