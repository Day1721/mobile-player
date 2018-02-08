using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MobilePlayer.Models.Music;
using MobilePlayer.Services;
using Environment = Android.OS.Environment;

namespace MobilePlayer
{
    [Activity(Label = "MobilePlayer", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private readonly IMusicScanner _scanner;
        private readonly IMusicParser _parser;
        private MusicServiceAbstract _musicService;
        
        public MainActivity()
        {
            _scanner = App.Container.Resolve<IMusicScanner>();
            _parser = App.Container.Resolve<IMusicParser>();
            _musicService = App.Container.Resolve<MusicServiceAbstract>();
        }

        private List<Song> _songs;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var songDirs = _scanner.Scan(Environment.ExternalStorageDirectory);
            _songs = songDirs.Select(_parser.Parse).ToList();

            var listView = FindViewById<ListView>(Resource.Id.SongList);
            listView.Adapter = new ArrayAdapter<Song>(this, Android.Resource.Layout.SimpleListItem1, _songs);
            
            listView.ItemClick += ListViewOnItemClick;
            StartService(MusicServiceIntent);
            
            var serviceConnection = new MusicService.Connection();
            
            serviceConnection.ServiceConnected += (name, binder) =>
            {
                _musicService = ((MusicService.Binder) binder).Service;
            };
            
            BindService(MusicServiceIntent, serviceConnection, Bind.AutoCreate);
        }

        private async void ListViewOnItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (!_musicService.IsPlaying)
            {
                _musicService.Play(_songs[e.Position].Path);
            }
            else if (_songs[e.Position].Path.CompareTo(_musicService.WhatPlays) == 0)
            {
                _musicService.Pause();
            }
            else
            {
                _musicService.Play();
            }
        }
        
        
        private Intent MusicServiceIntent => new Intent(this, _musicService.GetType());
    }
}