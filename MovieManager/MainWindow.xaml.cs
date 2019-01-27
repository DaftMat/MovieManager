using Movies;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MovieManager
{
    /// <summary>
    /// Interaction MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string status = "Home";
        private Saga currentSaga = null;
        private Series currentSeries = null;
        private Season currentSeason = null;
        private bool isPlaying = false;
        private bool isFullScreen = false;
        private int hideBar = 0;
        private static string docFolder = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents");
        public static FictionCollection collection = new FictionCollection();
        //Slider management
        bool userIsDraggingSlider = false;

        /* Initialization */
        public MainWindow()
        {
            loadJson();
            //look for image buttons
            InitializeComponent();
            loadLogos();
        }

        private void loadLogos()
        {
            BitmapImage play = new BitmapImage();
            BitmapImage pause = new BitmapImage();
            BitmapImage full = new BitmapImage();
            BitmapImage reduce = new BitmapImage();

            play.BeginInit();
            play.UriSource = new Uri("./imgs/play.png", UriKind.Relative);
            play.EndInit();
            Play.Stretch = System.Windows.Media.Stretch.Fill;
            Play.Source = play;

            pause.BeginInit();
            pause.UriSource = new Uri("pause.png", UriKind.Relative);
            pause.EndInit();
            Pause.Source = pause;

            full.BeginInit();
            full.UriSource = new Uri("full_screen.png", UriKind.Relative);
            full.EndInit();
            FullScreen.Source = full;

            reduce.BeginInit();
            reduce.UriSource = new Uri("leave_screen.png", UriKind.Relative);
            reduce.EndInit();
            ReduceScreen.Source = reduce;
        }

        /** Json methods :
         *      loadJson() -> Find the saved file that registered all users fictions sorted as it was before;
         *      saveJson() -> Write over (or create) json file with all users fiction sorted as it currently is.
         */
        public static void loadJson()
        {
            Directory.CreateDirectory(docFolder + "\\Movie Manager");
            if (!File.Exists(docFolder + "\\Movie Manager\\collection.txt")) return;
            FileStream file = new FileStream(docFolder + "\\Movie Manager\\collection.txt", FileMode.Open, FileAccess.Read);
            using (StreamReader reader = new StreamReader(file))
            {
                string json = reader.ReadToEnd();
                if (!json.Equals("")) collection = JsonConvert.DeserializeObject<FictionCollection>(json);
            }
        }

        public static void saveJson()
        {
            string json = JsonConvert.SerializeObject(collection, Formatting.Indented);
            FileStream file = new FileStream(docFolder + "\\Movie Manager\\collection.txt", FileMode.OpenOrCreate, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.Write(json);
            }
        }

        /* Actions */

        /* Left panel buttons */
        private void prepareHeaderCollection(string button)
        {
            datas.Visibility = Visibility.Collapsed;
            seasonComboGrid.Visibility = Visibility.Collapsed;
            HeaderRow.Height = new GridLength(150);
            Add.Visibility = Visibility.Visible;
            smallPlayerContainer.Visibility = Visibility.Collapsed;
            smallPlayer.Source = null;
            CurrentSelectionGrid.Visibility = Visibility.Visible;
            if (button.Equals("Home"))
            {
                SubTitle.Visibility = Visibility.Collapsed;
                HeaderButtons.Visibility = Visibility.Collapsed;
            }
            else
            {
                SubTitle.Content = "All " + button;
                SubTitle.Visibility = Visibility.Visible;
                HeaderButtons.Visibility = Visibility.Visible;
            }
            GeneralTitle.Content = button;
            status = button;
        }
        private void GoHome(object sender, RoutedEventArgs e)
        {
            /* TODO: Load lists and print them => 1.1 */
            /* Modify window */
            prepareHeaderCollection("Home");
            CurrentSelectionGrid.Visibility = Visibility.Collapsed;
        }

        private void GoMovies(object sender, RoutedEventArgs e)
        {
            CurrentSelectionGrid.ItemsSource = collection.Movies;
            /* Modify window */
            prepareHeaderCollection("Movies");
        }

        private void GoSagas(object sender, RoutedEventArgs e)
        {
            CurrentSelectionGrid.ItemsSource = collection.Sagas;
            /* Modify window */
            prepareHeaderCollection("Sagas");
        }

        private void GoSeries(object sender, RoutedEventArgs e)
        {
            CurrentSelectionGrid.ItemsSource = collection.Series;
            /* Modify window */
            prepareHeaderCollection("Series");
        }

        /* Header buttons */
        private void AddFic(object sender, RoutedEventArgs e)
        {
            AddFiction addPanel;
            if (status.Equals("Home"))
            {
                addPanel = new AddFiction();
            }
            else if (status.Equals("Saga"))
            {
                addPanel = new AddFiction(currentSaga);
            }
            else if (status.Equals("Season"))
            {
                addPanel = new AddFiction(currentSeries, currentSeason);
            }
            else
            {
                addPanel = new AddFiction(status);
            }
            addPanel.Show();
        }

        private void AddSeason(object sender, RoutedEventArgs e)
        {
            collection.Add(currentSeries, true);
            seasonCombo.ItemsSource = new List<Season>(currentSeries.GetSeasons);
            saveJson();
        }

        private void selectItem(object sender, SelectedCellsChangedEventArgs e)
        {
            /* General case */
            var selected = CurrentSelectionGrid.SelectedItem as Fiction;
            if (selected == null)
            {
                Add.Visibility = Visibility.Visible;
                smallPlayerContainer.Visibility = Visibility.Collapsed;
                return;
            }
            if (status.Equals("Series") || status.Equals("Sagas"))  CurrentSelectionGrid.UnselectAll();
            SubTitle.Visibility = Visibility.Visible;
            HeaderButtons.Visibility = Visibility.Visible;
            datas.Visibility = Visibility.Visible;
            seasonComboGrid.Visibility = Visibility.Collapsed;
            GeneralTitle.Content = "Fiction";
            SubTitle.Content = selected.Title;
            dataRealisator.Text = "Realised by " + selected.Realisator;
            dataGenre.Text = "Genre : " + selected.Genre;
            dataLanguage.Text = "Language : " + selected.Language;
            HeaderRow.Height = new GridLength(400);
            smallPlayer.Source = null;
            if (status.Equals("Series"))
            {
                var series = selected as Series;
                currentSeries = series;
                currentSeason = series.GetSeasons.First();
                seasonComboGrid.Visibility = Visibility.Visible;
                seasonCombo.ItemsSource = series.GetSeasons;
                seasonCombo.SelectedIndex = 0;
                CurrentSelectionGrid.ItemsSource = currentSeason.GetEpisodes;
                Add.Visibility = Visibility.Visible;
                smallPlayerContainer.Visibility = Visibility.Collapsed;
                status = "Season";
            }
            else if (status.Equals("Sagas"))
            {
                var saga = selected as Saga;
                currentSaga = saga;
                CurrentSelectionGrid.ItemsSource = saga.GetEpisodes;
                Add.Visibility = Visibility.Visible;
                smallPlayerContainer.Visibility = Visibility.Collapsed;
                status = "Saga";
            }
            else
            {
                if (status.Equals("Season"))
                {
                    seasonComboGrid.Visibility = Visibility.Visible;
                }
                var movieOrEp = selected as Movie;
                Add.Visibility = Visibility.Collapsed;
                smallPlayerContainer.Visibility = Visibility.Visible;
                smallPlayer.Source = new Uri(movieOrEp.FilePath);
                smallPlayer.Pause();
                Pause.Visibility = Visibility.Collapsed;
                Play.Visibility = Visibility.Visible;
                //Initialize timer
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;
                timer.Start();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((smallPlayer.Source != null) && (smallPlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                TimeEnd.Text = smallPlayer.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss");
                TimeSlider.Minimum = 0;
                TimeSlider.Maximum = smallPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                TimeSlider.Value = smallPlayer.Position.TotalSeconds;
                hideBar++;
                if (isFullScreen && hideBar >= 4)
                {
                    MediaBar.Visibility = Visibility.Collapsed;
                    hideBar = 0;
                }
            }
        }

        private void ChangeSeason(object sender, RoutedEventArgs e)
        {
            if (seasonCombo.SelectedIndex == -1) return;
            var season = seasonCombo.SelectedItem as Season;
            currentSeason = season;
            CurrentSelectionGrid.ItemsSource = season.GetEpisodes;
        }

        private void PlayMedia(object sender, RoutedEventArgs e)
        {
            smallPlayer.Play();
            Play.Visibility = Visibility.Collapsed;
            Pause.Visibility = Visibility.Visible;
            isPlaying = true;
        }
        private void PauseMedia(object sender, RoutedEventArgs e)
        {
            smallPlayer.Pause();
            Play.Visibility = Visibility.Visible;
            Pause.Visibility = Visibility.Collapsed;
            isPlaying = false;
        }

        private void EnterFullScreen(object sender, RoutedEventArgs e)
        {
            double position = smallPlayer.Position.TotalSeconds;
            Header.Children.Remove(smallPlayerContainer);
            this.Content = smallPlayerContainer;
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
            smallPlayer.Position = TimeSpan.FromSeconds(position);
            ReduceScreen.Visibility = Visibility.Visible;
            FullScreen.Visibility = Visibility.Collapsed;
            isFullScreen = true;
        }

        private void LeaveFullScreen(object sender, RoutedEventArgs e)
        {
            double position = smallPlayer.Position.TotalSeconds;
            this.Content = LayoutRoot;
            Header.Children.Add(smallPlayerContainer);
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.WindowState = WindowState.Maximized;
            smallPlayer.Position = TimeSpan.FromSeconds(position);
            ReduceScreen.Visibility = Visibility.Collapsed;
            FullScreen.Visibility = Visibility.Visible;
            isFullScreen = false;
        }

        private void ClickPlayer(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (isFullScreen)
                {
                    LeaveFullScreen(sender, e);
                }
                else
                {
                    EnterFullScreen(sender, e);
                }
            }
            //Switch playing/onpause for each clicks
            //(so it keep paused/playing on double click)
            if (isPlaying)
            {
                PauseMedia(sender, e);
            }
            else
            {
                PlayMedia(sender, e);
            }
        }

        private void TimeSliderStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void TimeSliderCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            smallPlayer.Position = TimeSpan.FromSeconds(TimeSlider.Value);
        }

        private void SetTime(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimePlayer.Text = TimeSpan.FromSeconds(TimeSlider.Value).ToString(@"hh\:mm\:ss");
        }

        private void ShowMenuBar(object sender, MouseEventArgs e)
        {
            MediaBar.Visibility = Visibility.Visible;
        }
    }
}
