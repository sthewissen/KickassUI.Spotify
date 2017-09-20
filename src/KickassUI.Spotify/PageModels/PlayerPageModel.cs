using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FreshMvvm;
using KickassUI.Spotify.Models;
using PropertyChanged;
using Xamarin.Forms;

namespace KickassUI.Spotify.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class PlayerPageModel : FreshBasePageModel
    {
        public Song Song { get; set; }
        public bool IsPlaying { get; set; }

        [AlsoNotifyFor(nameof(TicksLeft))]
        public int Ticks { get; set; }
        public int TicksLeft => Song.LengthInSeconds - Ticks;

        ICommand closePlayerCommand;
        public ICommand ClosePlayerCommand
        {
            get
            {
                return closePlayerCommand ?? (closePlayerCommand = new Command(async () => await ClosePlayer()));
            }
        }

        ICommand playCommand;
        public ICommand PlayCommand
        {
            get
            {
                return playCommand ?? (playCommand = new Command(() => StartStopPlaying()));
            }
        }

        private void StartStopPlaying()
        {
            if (!IsPlaying)
            {
                IsPlaying = true;

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    Ticks += 1;

                    // Stop playing when at the end.
                    if (Ticks == Song.LengthInSeconds)
                        IsPlaying = false;

                    // While the song is not over, return true for another tick.
                    return Ticks <= Song.LengthInSeconds && IsPlaying;
                });
            }
            else
            {
                // If it is currently playing, set it to false.
                IsPlaying = false;
            }
        }

        private async Task ClosePlayer()
        {
            await CoreMethods.PopPageModel(true, true);
        }

        public override void Init(object initData)
        {
            base.Init(initData);

            Song = initData as Song;
        }
    }
}
