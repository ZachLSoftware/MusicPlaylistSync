using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistSync
{
    public class ListCompare
    {

        private List<(string, string)> youtubeList;
        private List<(string, string)> spotifyList;
        public ListCompare(List<(string, string)> youtube, List<(string,string)> spotify) { youtubeList = youtube; spotifyList = spotify; }

        public List<List<(string,string)>> getLists()
        {
            List<(string, string)> inBoth = new List<(string, string)>();
            List<(string, string)> artistIssue = new List<(string, string)>();
            List<(string, string)> notInYoutube = new List<(string, string)>();
            List<(string, string)> notInSpotify = new List<(string, string)>();
            List<List<(string, string)>> allLists = new List<List<(string, string)>> { inBoth, artistIssue, notInYoutube, notInSpotify };


            List<(string, string)> yt = new List<(string, string)>(youtubeList);
            List<(string, string)> sp = new List<(string, string)>(spotifyList);

            foreach (var track in yt.ToList())
            {
                for ( int i = sp.Count - 1; i>=0; i--) 
                {
                    //Console.WriteLine("YT:"+ track.Item1 + "SP:" + sp[i].Item1+"test");
                    if (track.Item1 == sp[i].Item1)
                    {

                        if ( track.Item2 != sp[i].Item2)
                        {
                            Console.WriteLine("YT:" + track.Item2 + "SP:" + sp[i].Item2 + "test");
                            artistIssue.Add(track);
                            yt.Remove(track);
                            sp.RemoveAt(i);
                            break;
                        }
                        else
                        {
                            inBoth.Add(track);
                            yt.Remove(track);
                            sp.RemoveAt(i);
                            break;
                        }

                    }else if (i == 0)
                    {
                        yt.Remove(track);
                        notInSpotify.Add(track);
                    }
                }
            }
            if (sp.Count > 0) 
            { 
                foreach (var track in sp) { notInYoutube.Add(track); }
            }

            printList("In both Lists", inBoth);
            printList("Artist Issue", artistIssue);
            printList("Not found in Youtube", notInYoutube);
            printList("Not found in Spotify", notInSpotify);

            return allLists;
        }

    public void printList(string name, List<(string,string)> list)
        {
            Console.WriteLine("LIST: " +  name);
            Console.WriteLine("==============================================\n");
            foreach (var track in list)
            {
                Console.WriteLine();
                Console.WriteLine("Title: " + track.Item1);
                Console.WriteLine("Artist: " + track.Item2 + "\n");
            }
        }

    }
}
