using System.Collections.Generic;

namespace Movies
{
    public class Saga : Fiction
    {
        private List<Episode> episodes = new List<Episode>();

        public Saga(string title, string realisator, Genre genre, Language language) : base (title, realisator, genre, language) { }

        /**
         * Adds an Episode to the end of this Saga
         *  @param episode -> Episode to add
         */
        public void Add(Episode episode)
        {
            episode.NumEp = Count + 1;
            if (!episodes.Contains(episode))
                episodes.Add(episode);
        }

        /**
         * Removes an Episode from this Saga
         */
        public void Remove(Episode episode)
        {
            episodes.Remove(episode);
        }

        public List<Episode> GetEpisodes => episodes;

        public int Count => episodes.Count;
    }
}