using Pentibox.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Pentibox
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Game game;
        DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();

            this.game = new Game();
            this.timer = new DispatcherTimer();
            this.timer.Tick += this.OnTimerTick;
            this.timer.Interval = TimeSpan.FromMilliseconds(500);

            this.Loaded += this.OnLoaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.game.Start();
            this.timer.Start();
        }

        void OnTimerTick(object sender, object e)
        {
            this.game.OnTick();
        }

        void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    this.game.MoveLeft();
                    break;
                case VirtualKey.Right:
                    this.game.MoveRight();
                    break;
                case VirtualKey.Down:
                    this.game.MoveDown();
                    break;
                case VirtualKey.Up:
                    this.game.Rotate(true);
                    break;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;

            this.board.Attach(this.game);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
            this.timer.Stop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.game.OnTick();
        }

        private void Button_Rotate(object sender, RoutedEventArgs e)
        {
            this.game.Rotate(false);
        }
    }
}
