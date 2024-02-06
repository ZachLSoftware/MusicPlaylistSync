using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicSyncFrontEnd
{
    /// <summary>
    /// Interaction logic for PlayListCompare.xaml
    /// </summary>
    public partial class PlayListCompare : Page
    {
        public PlayListCompare()
        {
            InitializeComponent();
            populateLists();

        }

        private async Task populateLists()
        {

            await App.youtubeApi.login();
            await App.spotifyApi.getLogin();
            var spotifyTracks = await App.spotifyApi.getTracks();
            var youtubeTracks = await App.youtubeApi.getTracks();

            spotifyTracks.Sort();
            youtubeTracks.Sort();
            spotifyListBox.ItemsSource = spotifyTracks;
            youtubeListBox.ItemsSource = youtubeTracks;

            //foreach (var track in spotifyTracks)
            //{
            //    spotifyListBox.Items.Add(track);
            //}
            //foreach (var track in youtubeTracks)
            //{
            //    youtubeListBox.Items.Add(track);
            //}
        }
    
    }
}
