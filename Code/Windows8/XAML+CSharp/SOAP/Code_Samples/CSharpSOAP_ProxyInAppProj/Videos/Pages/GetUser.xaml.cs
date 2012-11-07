using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Videos.Common;
using Videos.SapoServices;
using Videos.VideosServiceReference;
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
    public sealed partial class GetUser : Videos.Common.LayoutAwarePage
    {
        public GetUser()
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

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private async void BtGetUserClick(object sender, RoutedEventArgs e)
        {
            App app = Application.Current as App;

            if (app == null)
                return;

            var client = new VideosServiceClient(app.EsbUsername, app.EsbPassword, app.EsbAccessKey);

            User u = await client.GetUserAsync(this.tb_UserNameForGetUser.Text);

            this.tblock_Result.Text = String.Format("Username: {0}\nCreationDate: {1}\nLastLogin: {2}", 
                u.Username, u.CreationDate, u.LastLogin);
        }
    }
}
