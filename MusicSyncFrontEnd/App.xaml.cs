using System.Configuration;
using System.Data;
using System.Windows;
using System.IO;
using System.Text.Json;
using MusicPlaylistSync;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web;


namespace MusicSyncFrontEnd
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static YoutubeApi youtubeApi;
        public static SpotifyApi spotifyApi;
        private Config _config;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _config = Configurations.Load();
            spotifyApi = new SpotifyApi(_config.spotifyClientID, _config.spotifySecret, _config.spotifyAccessToken);
            youtubeApi = new YoutubeApi(_config.youtubeClientID, _config.youtubeSecret);
            loginAPIs();
        }
        public async Task loginAPIs()
        {
            await spotifyApi.implicitGrant();
            var accessToken = spotifyApi.getToken();

            if (_config.spotifyAccessToken != accessToken)
            {
                _config.spotifyAccessToken = accessToken;
                Configurations.saveConfig(_config);
            }

        }
    }


    public static class Configurations
    {
        public static Config Load()
        {

            string jsonString = File.ReadAllText("..\\..\\..\\Resources\\config.json");

            Config config = JsonSerializer.Deserialize<Config> (jsonString);
            return config;

        }

        public static void saveConfig(Config config)
        {
            File.WriteAllText("..\\..\\..\\Resources\\config.json", JsonSerializer.Serialize(config));

        }
    }

}
