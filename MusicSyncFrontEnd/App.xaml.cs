using System.Configuration;
using System.Data;
using System.Windows;
using System.IO;
using System.Text.Json;
using MusicPlaylistSync;


namespace MusicSyncFrontEnd
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static YoutubeApi youtubeApi;
        public static SpotifyApi spotifyApi;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Config config = Configurations.Load();
            spotifyApi = new SpotifyApi(config.spotifyClientID, config.spotifySecret);
            youtubeApi = new YoutubeApi(config.youtubeClientID, config.youtubeSecret);

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
    }

}
