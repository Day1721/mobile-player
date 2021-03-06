﻿using System;
using System.IO;
using Android.App;
using Android.Runtime;
using MobilePlayer.Models;
using MobilePlayer.Models.Music;
using MobilePlayer.Services;
using TinyIoC;

namespace MobilePlayer
{
#if DEBUG
    [Application(Debuggable=true)]
#else
    [Application(Debuggable=false)]
#endif
    public class App : Application
    {
        public App(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        { }

        private static readonly string LocalStoragePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public static string MakePathTo(string filename) => Path.Combine(LocalStoragePath, filename);
        
        internal static TinyIoCContainer Container { get; private set; }
        
        public override void OnCreate()
        {
            base.OnCreate();

            InitTinyIoc();
        }

        private void InitTinyIoc()
        {
            Container = TinyIoCContainer.Current;

            Container.Register<MusicServiceAbstract, MusicService>();
            Container.Register<IMusicScanner, MusicScanner>();
            Container.Register<IMusicParser, MusicParser>();
        }
    }
}