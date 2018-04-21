using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MobilePlayer.Services;

namespace MobilePlayer.Activities
{
    [Activity(Label = "MobilePlayer", MainLauncher = false)]
    public class CurrentSongActivity : Activity
    {
        private MusicServiceAbstract _musicService;
        
        private readonly Type _musicServiceType;

        private ImageView _cover;
        private TextView _title;
        private TextView _artist;
        
        public CurrentSongActivity()
        {
            _musicServiceType = App.Container.Resolve<MusicServiceAbstract>().GetType();
        }
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.CurrentSong);

            FindViewById<ImageView>(Resource.Id.CurrentClose).Click += CloseOnClick;
            
            InitMusicService(() =>
            {
                _cover = FindViewById<ImageView>(Resource.Id.CurrentCover);
                _title = FindViewById<TextView>(Resource.Id.CurrentTitle);
                _artist = FindViewById<TextView>(Resource.Id.CurrentArtist);
            
                var current = _musicService.CurrentSong;
                if (current.Cover == null)
                {
                    _cover.SetImageResource(Resource.Drawable.NoCover);
                }
                else
                {
                    _cover.SetImageBitmap(current.Cover);
                }
                _cover.SetAdjustViewBounds(true);
                _title.Text = current.Title;
                _artist.Text = current.Artist;
            });
        }

        private void CloseOnClick(object obj, EventArgs e) => Finish();

        private void InitMusicService(Action afterInit)
        {
            StartService(MusicServiceIntent);
            var serviceConnection = new MusicService.Connection();
            serviceConnection.ServiceConnected += (name, binder) =>
            {
                _musicService = ((MusicService.Binder) binder).Service;
                afterInit();
            };
            BindService(MusicServiceIntent, serviceConnection, Bind.AutoCreate);
        }
        
        private Intent MusicServiceIntent => new Intent(this, _musicServiceType);
    }
}