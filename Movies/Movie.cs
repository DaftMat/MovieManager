using System.IO;

namespace Movies
{
    public class Movie : Fiction
    {
        private string file_path;
        private bool isValid = true;

        public Movie(string title, string realisator, Genre genre, Language language, string file_path) : base(title, realisator, genre, language)
        {
            this.file_path = file_path;
            //TODO: Find duration of the file;
            if (!File.Exists(file_path))
            {
                isValid = false;
                return;
            }
        }

        /* Getters & Setters */

        public string FilePath
        {
            get => file_path;
            set => file_path = value;
        }

        public bool IsValid
        {
            get => isValid;
            set => isValid = value;
        }
    }

    public class Episode : Movie
    {
        private int num_ep;

        public Episode(string title, string realisator, Genre genre, Language language, string file_path) : base(title, realisator, genre, language, file_path) { }

        public int NumEp {
            get => num_ep;
            set => num_ep = value;
        }

        public override string ToString()
        {
            return NumEp + " - " + Title;
        }
    }
}