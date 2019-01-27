using Movies;
using System;
using System.Windows;
using System.Windows.Forms;

namespace MovieManager
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class AddFiction : Window
    {
        private bool isMovie = false;
        private bool isSagaEpisode = false;
        private bool isSeriesEpisode = false;
        private bool isSaga = false;
        private bool isSeries = false;

        public AddFiction()
        {
            InitializeComponent();
        }

        /**
         * Constructor
         *  @param: status -> sets the FictionChoser
         */
        public AddFiction(string status)
        {
            InitializeComponent();
            int indexFiction = -1;
            if (isMovie = status.Equals("Movies"))
            {
                indexFiction = 0;
            }
            else if (isSaga = status.Equals("Sagas"))
            {
                indexFiction = 2;
            }
            else if (isSeries = status.Equals("Series"))
            {
                indexFiction = 3;
            }
            FictionChoser.SelectedIndex = indexFiction;
        }

        /**
         * Constructor
         *  @param: saga -> sets the selection on the saga the user wants to add the episode to
         */
        public AddFiction(Saga saga)
        {
            InitializeComponent();
            FictionChoser.SelectedIndex = 1;
            EpisodeChoser.SelectedIndex = 0;
            sagaChoser.SelectedItem = saga;
            RealisatorBox.Text = saga.Realisator;
            GenreBox.SelectedItem = saga.Genre;
            LanguageBox.SelectedItem = saga.Language;
        }

        /**
         * Constructor
         *  @param: series, season -> sets the selection on the series and season the user wants to add the episode to
         */
        public AddFiction(Series series, Season season)
        {
            InitializeComponent();
            FictionChoser.SelectedIndex = 1;
            EpisodeChoser.SelectedIndex = 1;
            seriesChoser.SelectedItem = series;
            seasonChoser.SelectedItem = season;
            RealisatorBox.Text = series.Realisator;
            GenreBox.SelectedItem = series.Genre;
            LanguageBox.SelectedItem = series.Language;

        }

        /* Actions */
        void activateForm()
        {
            DataForm.IsEnabled = true;
            TitleBox.Text = "";
            RealisatorBox.Text = "";
            LanguageBox.SelectedIndex = -1;
            GenreBox.SelectedIndex = -1;
            FileBox.Text = "";
            EpisodeForm.Visibility = Visibility.Collapsed;
        }
        void movieSelected(object sender, RoutedEventArgs e)
        {
            activateForm();
            fileGrid.IsEnabled = true;
            isMovie = true;
            isSaga = isSeries = isSagaEpisode = isSeriesEpisode = false;
        }
        void sagaSelected(object sender, RoutedEventArgs e)
        {
            activateForm();
            fileGrid.IsEnabled = false;
            isSaga = true;
            isMovie = isSeries = isSagaEpisode = isSeriesEpisode = false;
        }
        void seriesSelected(object sender, RoutedEventArgs e)
        {
            activateForm();
            fileGrid.IsEnabled = false;
            isSeries = true;
            isMovie = isSaga = isSagaEpisode = isSeriesEpisode = false;
        }

        void episodeSelected(object sender, RoutedEventArgs e)
        {
            DataForm.IsEnabled = false;
            fileGrid.IsEnabled = true;
            isMovie = isSaga = isSagaEpisode = isSeries = isSeriesEpisode = false;
            EpisodeForm.Visibility = Visibility.Visible;
            EpisodeChoserTitle.Visibility = Visibility.Visible;
            EpisodeChoser.Visibility = Visibility.Visible;
            ContainerForm.Visibility = Visibility.Collapsed;
        }

        void sagaEpisode(object sender, RoutedEventArgs e)
        {
            containerTitle.Text = "Saga";
            isSagaEpisode = true;
            isSeriesEpisode = true;
            ContainerForm.Visibility = Visibility.Visible;
            DataForm.IsEnabled = false;
            SeriesGridForm.Visibility = Visibility.Collapsed;
            sagaChoser.Visibility = Visibility.Visible;
        }

        void seriesEpisode(object sender, RoutedEventArgs e)
        {
            containerTitle.Text = "Series";
            isSagaEpisode = false;
            isSeriesEpisode = true;
            ContainerForm.Visibility = Visibility.Visible;
            DataForm.IsEnabled = false;
            seasonChoser.IsEnabled = false;
            SeriesGridForm.Visibility = Visibility.Visible;
            sagaChoser.Visibility = Visibility.Collapsed;
        }

        void SeriesSelected(object sender, RoutedEventArgs e)
        {
            if (seriesChoser.SelectedIndex == -1) return;
            seasonChoser.IsEnabled = true;
            DataForm.IsEnabled = false;
            loadSeasonBox();

        }

        void SeasonSelected(object sender, RoutedEventArgs e)
        {
            if (seasonChoser.SelectedIndex == -1) return;
            DataForm.IsEnabled = true;
        }

        void SagaSelected(object sender, RoutedEventArgs e)
        {
            if (sagaChoser.SelectedIndex == -1) return;
            DataForm.IsEnabled = true;
        }

        void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void Submit(object sender, RoutedEventArgs e)
        {
            string title = TitleBox.Text;
            string realisator = RealisatorBox.Text;
            if (GenreBox.SelectedIndex == -1 || LanguageBox.SelectedIndex == -1) return;
            Genre genre = (Genre)GenreBox.SelectedItem;
            Language language = (Language)LanguageBox.SelectedItem;
            String file_path = FileBox.Text;
            if (title.Equals("") || realisator.Equals("")) return;
            if (file_path.Equals("") && (isMovie || isSagaEpisode || isSeriesEpisode)) return;
            if (isMovie)
            {
                Movie newElem = new Movie(title, realisator, genre, language, file_path);
                MainWindow.collection.Add(newElem);
            }
            else if (isSagaEpisode)
            {
                if (sagaChoser.SelectedIndex == -1) return;
                var saga = sagaChoser.SelectedItem as Saga;
                Episode newElem = new Episode(title, realisator, genre, language, file_path);
                saga.Add(newElem);
            }
            else if (isSeriesEpisode)
            {
                if (seriesChoser.SelectedIndex == -1 || seasonChoser.SelectedIndex == -1) return;
                var series = seriesChoser.SelectedItem as Series;
                var season = seasonChoser.SelectedItem as Season;
                Episode newElem = new Episode(title, realisator, genre, language, file_path);
                series.Add(newElem, season);
            }
            else if (isSaga)
            {
                Saga newElem = new Saga(title, realisator, genre, language);
                MainWindow.collection.Add(newElem);
            }
            else if (isSeries)
            {
                Series newElem = new Series(title, realisator, genre, language);
                MainWindow.collection.Add(newElem, true);
            }
            MainWindow.saveJson();
            resetPanel();
        }

        private void resetPanel()
        {
            FictionChoser.SelectedIndex = -1;
            EpisodeChoser.SelectedIndex = -1;
            sagaChoser.SelectedIndex = -1;
            seriesChoser.SelectedIndex = -1;
            seasonChoser.SelectedIndex = -1;
            seasonChoser.IsEnabled = false;
            EpisodeForm.Visibility = Visibility.Collapsed;
            ContainerForm.Visibility = Visibility.Collapsed;
            TitleBox.Text = "";
            RealisatorBox.Text = "";
            LanguageBox.SelectedIndex = -1;
            GenreBox.SelectedIndex = -1;
            FileBox.Text = "";
            DataForm.IsEnabled = false;
        }

        void loadSeriesBox(object sender, RoutedEventArgs e)
        {
            seriesChoser.ItemsSource = MainWindow.collection.Series;
        }
        void loadSeasonBox()
        {
            var chosenSeries = seriesChoser.SelectedItem as Series;
            seasonChoser.ItemsSource = chosenSeries.GetSeasons;
        }
        void loadSagaBox(object sender, RoutedEventArgs e)
        {
            sagaChoser.ItemsSource = MainWindow.collection.Sagas;
        }

        void FilePick(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.Filter = "Video Files (*.avi;*.m4v;*.mkv;*.mp4;*.wmv)|*.mp4;*.mkv;*.m4v;*.wmv;*.avi";

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FileBox.Text = openFileDialog.FileName;
                }
            }
        }
    }
}
