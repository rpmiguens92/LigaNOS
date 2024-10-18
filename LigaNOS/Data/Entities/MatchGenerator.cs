using LigaNOS.Data.Repositories;
using LigaNOS.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LigaNOS.Data.Entities
{
    public class MatchGenerator : IMatchGenerator
    {
        private readonly IClubRepository _clubRepository;
        private readonly IMatchRepository _matchRepository;
        private List<Match> Matches { get; set; }
        private int contJourneys;

        public MatchGenerator(IClubRepository clubRepository, IMatchRepository matchRepository)
        {
            _clubRepository = clubRepository;
            _matchRepository = matchRepository;
            Matches = new List<Match>();
            contJourneys = 0;
        }

        public async Task<MatchViewModel> GenerateMatch()
        {

            var clubs = _clubRepository.GetAll().ToList();
            if (clubs.Any(c => c.Id == 0))
            {
                throw new InvalidOperationException("Club Id invalid.");
            }

            if (clubs.Count % 2 != 0)
            {
                throw new InvalidOperationException("Insufficient clubs");
            }


            if (Matches.Count == clubs.Count * (clubs.Count - 1))
            {
                throw new InvalidOperationException("Full season!");
            }


            int gamesPerJourney = clubs.Count / 2;
            if (Matches.Count % gamesPerJourney == 0)
            {
                contJourneys++;
            }


            HashSet<Club> clubsPlayedThisJourney = new HashSet<Club>();
            int currentJourney = Matches.Count - (Matches.Count % gamesPerJourney);
            for (int i = currentJourney; i < Matches.Count; i++)
            {
                clubsPlayedThisJourney.Add(Matches[i].HomeClub);
                clubsPlayedThisJourney.Add(Matches[i].AwayClub);
            }

            Club randomHomeGame;
            Club randomAwayGame;


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

                    var randClubID = new Random();
                    randomHomeGame = clubs[randClubID.Next(clubs.Count)];
                    randomAwayGame = clubs[randClubID.Next(clubs.Count)];


                    if (randomHomeGame == randomAwayGame)
                    {
                        validMatch = false;
                    }
                    else
                    {

                        if (clubsPlayedThisJourney.Contains(randomHomeGame) || clubsPlayedThisJourney.Contains(randomAwayGame))
                        {
                            validMatch = false;
                        }
                        else
                        {

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


                clubsPlayedThisJourney.Add(randomHomeGame);
                clubsPlayedThisJourney.Add(randomAwayGame);
            }


            var newMatch = new MatchViewModel
            {
                HomeClub = randomHomeGame.Name,
                AwayClub = randomAwayGame.Name,
                Stadium = randomHomeGame.Stadium,
                HomeClubId = randomHomeGame.Id,  
                AwayClubId = randomAwayGame.Id

            };


            var randomMatch = new Match
            {
                HomeClubId = randomHomeGame.Id,
                AwayClubId = randomAwayGame.Id,
                Stadium = newMatch.Stadium,

            };
           
            return newMatch;
        }
    }
}
