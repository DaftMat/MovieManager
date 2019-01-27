
using System;
using System.Collections.Generic;

namespace Movies
{
    public class Series : Fiction
    {
        private List<Season> seasons = new List<Season>();

        public Series(string title, string realisator, Genre genre, Language language) : base (title, realisator, genre, language) { }

        /**
         * Adds a Season to the end of this Series
         */
        public Season Add()
        {
            Season toAdd = new Season(Count);
            seasons.Add(toAdd);
            return toAdd;
        }

        /**
         * Removes the last Season from this Series
         */
        public void Remove()
        {
            seasons.RemoveAt(seasons.Count-1);
        }

        /**
         * Adds an Episode to the end of the specified Season of this Series
         *  @param episode -> Episode to add
         *  @param season -> Season to add the episode at the end of
         */
        public void Add(Episode episode, Season season)
        {
            foreach (Season s in seasons)
            {
                if (s.Equals(season))
                {
                    s.Add(episode);
                    return;
                }
            }
        }

        /**
         * Removes an Episode from the specified Season of this Series
         *  @param episode -> Episode to remove
         *  @param season -> Season where the episode is
         */
        public void Remove(Episode episode, Season season)
        {
            foreach (Season s in seasons)
            {
                if (s.Equals(season))
                {
                    s.Remove(episode);
                    return;
                }
            }
        }

        /* Getters */

        public List<Season> GetSeasons => seasons;

        public int Count => seasons.Count;
    }

    public class Season : Saga
    {
        public Season(int num_season) : base("Season " + (num_season+1), "None", Genre.Multi, Language.Multi) { }
    }
}