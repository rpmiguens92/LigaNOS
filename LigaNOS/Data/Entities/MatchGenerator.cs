using LigaNOS.Data.Repositories;
using LigaNOS.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace LigaNOS.Data.Entities
{
    public class MatchGenerator
    {

        private readonly IClubRepository _clubRepository;
        private List<Match> Matches { get; set; }
        private int contJourneys;

        public MatchGenerator(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
            Matches = new List<Match>(); // Assume you have this stored elsewhere in reality
            contJourneys = 0;
        }

        public MatchViewModel GenerateMatch()
        {
            // Fetch all clubs
            var clubs = _clubRepository.GetAllWithClubs().ToList();

            // Club parity check
            if (clubs.Count % 2 != 0)
            {
                throw new InvalidOperationException("O Número total de equipas tem de ser par!");
            }

            // Season completion check
            if (Matches.Count == clubs.Count * (clubs.Count - 1))
            {
                throw new InvalidOperationException("Época completa!");
            }

            // Limit games per journey
            int gamesPerJourney = clubs.Count / 2;
            if (Matches.Count % gamesPerJourney == 0)
            {
                contJourneys++;
            }

            // Track clubs that have already played in the current journey
            HashSet<Club> clubsPlayedThisJourney = new HashSet<Club>();
            int currentJourney = Matches.Count - (Matches.Count % gamesPerJourney);
            for (int i = currentJourney; i < Matches.Count; i++)
            {
                clubsPlayedThisJourney.Add(Matches[i].HomeClub);
                clubsPlayedThisJourney.Add(Matches[i].AwayClub);
            }

            Club randomHomeGame;
            Club randomAwayGame;

            // Handle home/away alternation after the first round
            var totalGamesFirstRound = (clubs.Count * (clubs.Count - 1) / 2);
            if (Matches.Count >= totalGamesFirstRound)
            {
                var firstRoundMatch = Matches[Matches.Count - totalGamesFirstRound];
                randomHomeGame = firstRoundMatch.AwayClub;
                randomAwayGame = firstRoundMatch.HomeClub;
            }
            else
            {
                bool validMatch;
                do
                {
                    validMatch = true;

                    // Randomly select home and away clubs
                    var randClubID = new Random();
                    randomHomeGame = clubs[randClubID.Next(clubs.Count)];
                    randomAwayGame = clubs[randClubID.Next(clubs.Count)];

                    // Ensure the clubs are different
                    if (randomHomeGame == randomAwayGame)
                    {
                        validMatch = false;
                    }
                    else
                    {
                        // Check if the club has already played in the current journey
                        if (clubsPlayedThisJourney.Contains(randomHomeGame) || clubsPlayedThisJourney.Contains(randomAwayGame))
                        {
                            validMatch = false;
                        }
                        else
                        {
                            // Check if this match pairing already exists in the season
                            foreach (var match in Matches)
                            {
                                if (match.HomeClub == randomHomeGame && match.AwayClub == randomAwayGame)
                                {
                                    validMatch = false;
                                    break;
                                }
                            }
                        }
                    }
                } while (!validMatch);

                // Add clubs to the list of those who played this journey
                clubsPlayedThisJourney.Add(randomHomeGame);
                clubsPlayedThisJourney.Add(randomAwayGame);
            }

            // Create the match
            var newMatch = new MatchViewModel
            {
                HomeClub = randomHomeGame.Name,
                AwayClub = randomAwayGame.Name
            };

            // Add the match to the list of matches
            Matches.Add(new Match
            {
                HomeClub = randomHomeGame,
                AwayClub = randomAwayGame,
                MatchDay = DateTime.Now.AddDays(contJourneys), // Example match day
              
            });

            return newMatch;
        }
    }
}
