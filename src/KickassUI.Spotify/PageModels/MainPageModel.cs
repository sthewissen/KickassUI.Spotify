using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using FreshMvvm;
using KickassUI.Spotify.Models;
using Xamarin.Forms;

namespace KickassUI.Spotify.PageModels
{
    public class MainPageModel : FreshBasePageModel
    {
        bool openingPage;

        public List<Album> Albums { get; set; }
        public List<Playlist> Playlists { get; set; }
        public List<Song> Songs { get; set; }

        ICommand openPlayerCommand;
        public ICommand OpenPlayerCommand
        {
            get
            {
                return openPlayerCommand ?? (openPlayerCommand = new Command<Song>(async (item) => await OpenPlayer(item), (arg) => !openingPage));
            }
        }

        public MainPageModel()
        {
            Albums = new List<Album> {
                new Album(){ Artist="Royal Blood", Name="Royal Blood", ImageUrl="https://upload.wikimedia.org/wikipedia/en/b/b0/Royal_Blood_-_Royal_Blood_%28Artwork%29.jpg"},
                new Album(){ Artist="Fall Out Boy", Name="Infinity On High", ImageUrl="https://upload.wikimedia.org/wikipedia/en/6/69/Infinityonhigh.jpg"},
                new Album(){ Artist="Muse", Name="Drones", ImageUrl="https://upload.wikimedia.org/wikipedia/en/4/44/MuseDronesCover.jpg"},
                new Album(){ Artist="System Of A Down", Name="Mesmerize", ImageUrl="https://upload.wikimedia.org/wikipedia/en/0/02/Mezmerize-LP.jpg"},
                new Album(){ Artist="Muse", Name="Black Holes And Revelations", ImageUrl="https://upload.wikimedia.org/wikipedia/en/c/c5/BlackHolesCover.jpg"},
            };

            Playlists = new List<Playlist>()
            {
                    new Playlist(){ Name="Funk Rock", NrOfFollowers=49205, ImageUrl="https://i.scdn.co/image/a9cdead5cf5d85a33e7bc12b49d1006821650ca4"},
                    new Playlist(){ Name="Rock Solid", NrOfFollowers=5025, ImageUrl="https://i.scdn.co/image/993ea43e0521938d5bf7a4fbe4f349acf6500975"},
                    new Playlist(){ Name="This Is: Muse", NrOfFollowers=2140415, ImageUrl="https://i.scdn.co/image/3770c6d556b864e60684d0706013ff08dac76918"},
                    new Playlist(){ Name="100% Scooter", NrOfFollowers=7447, ImageUrl="https://i.scdn.co/image/98cc42f3671e7c14b2593cce05cf2abb87247ab2"},
                    new Playlist(){ Name="Feeling Good, Feeling Great", NrOfFollowers=250211, ImageUrl="https://i.scdn.co/image/9c003edf2bcc3386c400d087b3bb4adb75ee1f5a"},
            };

            Songs = new List<Song>()
            {
                    new Song(){ Artist="Muse", Title="Dig Down", LengthInSeconds=228, AlbumImageUrl="https://i.scdn.co/image/08d56eac0c7d48bb8bf7752b2202c3314db79394"},
                    new Song(){ Artist="Gorillaz", Title="Clint Eastwood", LengthInSeconds=341, AlbumImageUrl="https://i.scdn.co/image/6c6086f6922b9a44920310b34ef98161bd7adf78"},
                    new Song(){ Artist="Jamiroquai", Title="Virtual Insanity", LengthInSeconds=229, AlbumImageUrl="https://i.scdn.co/image/bb3810cd18de42b93c54536d7e9ab7f8c10a8229"},
                    new Song(){ Artist="Biffy Clyro", Title="The Captain", LengthInSeconds=223, AlbumImageUrl="https://i.scdn.co/image/f8d0b0bdf4a541fb2d13cb63e958aa760e3547e5"},
                    new Song(){ Artist="System of a Down", Title="Hypnotize", LengthInSeconds=189, AlbumImageUrl="https://i.scdn.co/image/66eb75e0f3a8a91822ba7154e4b41066e63e51f2"},
                    new Song(){ Artist="Paramore", Title="Hard Times", LengthInSeconds=182, AlbumImageUrl="https://i.scdn.co/image/d8296568ae1b856050976111fa892d8db693efd5"}
            };
        }

        async Task OpenPlayer(Song item)
        {
            await CoreMethods.PushPageModel<PlayerPageModel>(item, true, true);
        }
    }
}
