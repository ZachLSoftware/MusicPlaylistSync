using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System.ComponentModel;
using SpotifyAPI.Web.Http;
using static SpotifyAPI.Web.PlaylistRemoveItemsRequest;

namespace MusicPlaylistSync
{
    public class SpotifyApi
    {
        private static EmbedIOAuthServer _server;
        private bool IsRunning;
        private SpotifyClient spotify;
        private string accessToken = "";
        private ImplictGrantResponse implicitGrantResponse;
        private string clientID;
        private string clientSecret;

        public SpotifyApi(string client, string secret, string accessToken)
        {
            this.clientID = client;
            this.clientSecret = secret;
            this.accessToken = accessToken;
        }
        public async Task getLogin()
        {
            var config = SpotifyClientConfig
            .CreateDefault()
            .WithAuthenticator(new ClientCredentialsAuthenticator(clientID, clientSecret));

            spotify = new SpotifyClient(config);
        }

        public async Task implicitGrant()
        {
            try
            {
                spotify = new SpotifyClient(accessToken);
                await spotify.UserProfile.Get(clientID);
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                _server = new EmbedIOAuthServer(new Uri("http://localhost:5543/callback"), 5543);
                _server.ImplictGrantReceived += OnImplicitGrantReceived;
                _server.ErrorReceived += OnErrorReceived;
                await _server.Start();
                IsRunning = true;

                var request = new LoginRequest(_server.BaseUri, clientID, LoginRequest.ResponseType.Token)
                {
                    Scope = new List<string> { Scopes.UserReadEmail, Scopes.PlaylistModifyPublic }
                };
                BrowserUtil.Open(request.ToUri());
                while (IsRunning)
                {
                    await Task.Delay(100);
                }
            }
        }

        private async Task OnImplicitGrantReceived(object sender, ImplictGrantResponse response)
        {
            await _server.Stop();

            IsRunning = false;
            spotify = new SpotifyClient(response.AccessToken);
            accessToken = response.AccessToken;
            implicitGrantResponse = response;
            Console.WriteLine(response.AccessToken);
        }

        private static async Task OnErrorReceived(object sender, string error, string state)
        {
            Console.WriteLine($"Aborting authorization, error received: {error}");
            await _server.Stop();
        }

        public async Task<List<(string,string)>> getTracks()
        {

            List<(string,string)> tracks = new List<(string,string)> ();
            try
            {
                // Get a list of the current user's playlists
                var playlist = await spotify.Playlists.Get("28nHKDC0aHumvx1R1SKZRE");
                foreach (var item in playlist.Tracks.Items)
                {
                    if (item.Track is FullTrack track)
                        tracks.Add((track.Name, track.Artists[0].Name));
                }
                return tracks;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public string getToken()
        {
            return accessToken;
        }
        public async Task<FullTrack> songSearch((string, string) song)
        {
            string query = $"track:\"{song.Item1}\" artist:\"{song.Item2}\"";
            SearchRequest search = new SearchRequest(SearchRequest.Types.Track, query);
            SearchResponse searchResponse = await spotify.Search.Item(search);

            List<FullTrack> tracks = searchResponse.Tracks.Items;
            var track = tracks[0];
            //Console.WriteLine($"Track Name: {track.Name}");
            //Console.WriteLine($"Artists: {string.Join(", ", track.Artists.Select(a => a.Name))}");
            //Console.WriteLine($"Album: {track.Album.Name}");
            //Console.WriteLine();
            return track;
            IList<string> uri = new List<string> { track.Uri };

            var response = await spotify.Playlists.AddItems("28nHKDC0aHumvx1R1SKZRE", new PlaylistAddItemsRequest(uri));

            Console.WriteLine(response.ToString());
            
        }
    }
}
