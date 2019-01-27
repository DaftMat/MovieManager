using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movies
{
    public class FictionCollection
    {
        private List<Series> series = new List<Series>();
        private List<Movie> movies = new List<Movie>();
        private List<Saga> sagas = new List<Saga>();

        /**
         * Adds a Movie to the movies list
         *  @param movie -> Movie to add
         */
        public void Add(Movie movie)
        {
            if (!movies.Contains(movie))
                movies.Add(movie);
        }

        /**
         * Adds a Saga to the sagas list
         *  @param saga -> Saga to add
         */
        public void Add(Saga saga)
        {
            if (!sagas.Contains(saga))
                sagas.Add(saga);
        }

        /**
         * Adds a Series to the series list
         *  @param series -> Series to add
         *  @param season -> if true, adds a season to the created series
         */
        public void Add(Series series, bool season)
        {
            if (!this.series.Contains(series))
                this.series.Add(series);
            if (season)
            {
                foreach (Series s in this.series)
                {
                    if (s.Equals(series))
                    {
                        s.Add();
                        return;
                    }
                }
            }
        }

        /**
         * Removes a Movie from the movies list
         *  @param movie -> Movie to remove
         */
        public void Remove(Movie movie)
        {
            movies.Remove(movie);
        }

        /**
         * Removes a Saga from the sagas list
         *  @param saga -> Saga to remove
         */
        public void Remove(Saga saga)
        {
            sagas.Remove(saga);
        }

        /**
         * Removes a Series from the series list
         *  @param series -> Series to remove
         */
        public void Remove(Series series)
        {
            this.series.Remove(series);
        }

        /* Getters */

        public List<Movie> Movies => movies;

        public List<Saga> Sagas => sagas;

        public List<Series> Series => series;

        /* By Genre */

        public List<Movie> GetMovies(Genre genre)
        {
            List<Movie> ret = new List<Movie>();
            foreach(Movie movie in movies)
            {
                if (movie.Genre == genre)
                {
                    ret.Add(movie);
                }
            }
            return ret;
        }

        public List<Saga> GetSagas(Genre genre)
        {
            List<Saga> ret = new List<Saga>();
            foreach (Saga saga in sagas)
            {
                if (saga.Genre == genre)
                {
                    ret.Add(saga);
                }
            }
            return ret;
        }

        public List<Series> GetSeries(Genre genre)
        {
            List<Series> ret = new List<Series>();
            foreach (Series s in series)
            {
                if (s.Genre == genre)
                {
                    ret.Add(s);
                }
            }
            return ret;
        }
    }
}
