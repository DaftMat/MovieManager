namespace Movies
{
    public abstract class Fiction
    {
        protected string title;
        private string realisator;
        private Genre genre;
        private Language language;

        public Fiction(string title, string realisator, Genre genre, Language language)
        {
            this.title = title;
            this.realisator = realisator;
            this.genre = genre;
            this.language = language;
        }

        /* Getters & Setters */

        public string Title
        {
            get => title;
            set => title = value;
        }

        public string Realisator
        {
            get => realisator;
            set => realisator = value;
        }

        public Genre Genre
        {
            get => genre;
            set => genre = value;
        }

        public Language Language
        {
            get => language;
            set => language = value;
        }

        /* Overrode methods */
        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) return false;
            Fiction temp = (Fiction)obj;
            return title.Equals(temp.title);
        }

        public override int GetHashCode()
        {
            return title.GetHashCode();
        }

        public override string ToString()
        {
            return title;
        }
    }

    /* Enums */

    public enum Genre
    {
        Absurdist,
        Action,
        Adventure,
        Comedy,
    	Crime,
    	Drama,
    	Fantasy,
    	Historical,
    	Historical_fiction,
    	Horror,
    	Magical_realism,
    	Mystery,
    	Paranoid_fiction,
    	Philosophical,
    	Political,
    	Romance,
    	Satire,
    	Science_fiction,
    	Social,
    	Speculative,
    	Thriller,
    	Urban,
    	Western,
        Multi
    }

    public enum Language
    {
        English,
        French,
        Italian,
        Portuguese,
        Spanish,
        Multi
    }
}
