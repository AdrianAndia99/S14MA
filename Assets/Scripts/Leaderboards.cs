using UnityEngine;
using Unity.Services.Leaderboards;
using Sirenix.OdinInspector;
using System;
using Unity.Services.Leaderboards.Models;

public class Leaderboards : MonoBehaviour
{
    [Button]
    public async void SendScore(string leaderboardID, double score)
    {
        try
        {
            AddPlayerScoreOptions options = new AddPlayerScoreOptions();
            options.Metadata = "Insano";

            LeaderboardEntry entry = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardID, score);
            print("player id" + entry.PlayerId + " name " + entry.PlayerName + "score: " + entry.Score);
        }
        catch(Exception E)
        {
            Debug.LogException(E);
        }

    }

    [Button]
    public async void GetScore(string leaderboardID)
    {
        try
        {
            GetPlayerScoreOptions options = new GetPlayerScoreOptions();
            options.IncludeMetadata = true;

           LeaderboardEntry entry = await LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardID, options);
            print("player id" + entry.PlayerId + " name " + entry.PlayerName + "score: " + entry.Score + " metadata: " + entry.Metadata.ToString());
        }
        catch (Exception E)
        {
            Debug.LogException(E);
        }
    }

    [Button]
    public async void GetTopScores(string leaderboardID)
    {
        GetScoresOptions options = new GetScoresOptions();
        options.Limit = 2;
        try
        {
            LeaderboardScoresPage page = await LeaderboardsService.Instance.GetScoresAsync(leaderboardID, options);


            foreach (var entry in page.Results)
            {
                print("player id" + entry.PlayerId + " name " + entry.PlayerName + "score: " + entry.Score + " metadata: " + entry.Metadata.ToString());
            }
        }
        catch (Exception E)
        {
            Debug.LogException(E);
        }

    }

    [Button]
    public async void GetRangeScores(string leaderboardID)
    {
        GetPlayerRangeOptions options = new GetPlayerRangeOptions();
        options.RangeLimit = 2;
        try
        {
            LeaderboardScores page = await LeaderboardsService.Instance.GetPlayerRangeAsync(leaderboardID);
            foreach (var entry in page.Results)
            {
                print("player id" + entry.PlayerId + " name " + entry.PlayerName + "score: " + entry.Score + " metadata: " + entry.Metadata.ToString());
            }
        }
        catch (Exception E)
        {
            Debug.LogException(E);
        }
    }
}       