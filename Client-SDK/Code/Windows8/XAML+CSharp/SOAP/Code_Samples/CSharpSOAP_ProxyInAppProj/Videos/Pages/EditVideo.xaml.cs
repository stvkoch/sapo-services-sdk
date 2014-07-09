﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Videos.SapoServices;
using Videos.VideosReference;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Videos.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class EditVideo : Videos.Common.LayoutAwarePage
    {
        public EditVideo()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        private void BtEditVideoClick(object sender, RoutedEventArgs e)
        {
            App app = Application.Current as App;

            if (app == null)
                return;

            var client = new VideosServiceClient(app.EsbUsername, app.EsbPassword, app.EsbAccessKey);


            //Title and randname are mandatory.
            VideoSubmition v = new VideoSubmition
            {
                Randname = this.tb_VideoRandnameForEditVideo.Text,
                Synopse = this.tb_VideoSynopseForEditVideo.Text,
                Title = this.tb_VideoTitleForEditVideo.Text
            };

            client.EditVideoAsync(v);

            this.tblock_Result.Text = "SUCCESS";
        }


        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }
    }
}
