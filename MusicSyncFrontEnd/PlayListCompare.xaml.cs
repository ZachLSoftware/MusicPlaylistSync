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
using MusicPlaylistSync;

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
            ListCompare listCompare = new MusicPlaylistSync.ListCompare(youtubeTracks, spotifyTracks);
            List<List<(string,string)>> allLists = listCompare.getLists();

            for (int i = 0; i < allLists.Count; i++)
            {
                foreach (var track in allLists[i])
                {
                    var item = new ListBoxItem { Content = track };
                    if (i == 0)
                    {
                        item.Style = (Style)FindResource("InBoth");
                        spotifyListBox.Items.Add(new ListBoxItem { Content = track, Style = item.Style });
                        youtubeListBox.Items.Add(new ListBoxItem { Content = track, Style = item.Style });
                    }
                    // Add separate instances of ListBoxItem to each ListBox

                    else if ( i == 1)
                    {
                        item.Style = (Style)FindResource("SameName");
                        spotifyListBox.Items.Add(new ListBoxItem { Content = track, Style = item.Style });
                        youtubeListBox.Items.Add(new ListBoxItem { Content = track, Style = item.Style });
                    }
                    else if ( i == 2)
                    {
                        item.Style = (Style)FindResource("NotIn");
                        youtubeListBox.Items.Add(new ListBoxItem { Content = track, Style = item.Style });
                    }
                    else
                    {
                        item.Style = (Style)FindResource("NotIn");
                        spotifyListBox.Items.Add(new ListBoxItem { Content = track, Style = item.Style });

                    }
                }
            }

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
