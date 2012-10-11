using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using PhotosSample.Common;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace PhotosSample.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class DummyEcho : PhotosSample.Common.LayoutAwarePage
    {
        public DummyEcho()
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

        private async void BtDoDummyEchoClick(object sender, RoutedEventArgs e)
        {
            App app = Application.Current as App;

            if (app != null)
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("ESB", "AccessKey=" + app.EsbAccessKey);

                UriBuilder uriBuilder = new UriBuilder("http://services.sapo.pt/" + "Photos/DummyEcho");
                uriBuilder.Query = "ESBUsername=" + app.EsbUsername + "&ESBPassword=" +
                    app.EsbPassword+"&EchoStr=" + this.tbEchoStr.Text+"&json=true";

                HttpResponseMessage httpResponseMessage = await client.GetAsync(uriBuilder.Uri);

                string result = await httpResponseMessage.Content.ReadAsStringAsync();

                JsonObject dummyEchoResult = JsonObject.Parse(result);
                //dummyEchoResult
            }
        }
    }
}
