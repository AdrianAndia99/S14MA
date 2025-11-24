using UnityEngine;
using Unity.Services.Leaderboards;
using Sirenix.OdinInspector;
using System;
using Unity.Services.Leaderboards.Models;
using System.Collections.Generic;

public class Leaderboards : MonoBehaviour
{
    [Title("Leaderboard local de ejemplo")]
    public List<LocalEntry> localEntries = new List<LocalEntry>
    {
        new LocalEntry("Ana", 1500),
        new LocalEntry("Bruno", 1200),
        new LocalEntry("Carla", 900),
        new LocalEntry("Diego", 700)
    };

    [Serializable]
    public class LocalEntry
    {
        public string name;
        public int score;
        [HideInInspector]
        public int rank;

        public LocalEntry(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }

    [Title("Visual mínima (consola)")]
    [Button("Imprimir Leaderboard Local")]
    public void PrintLocalLeaderboard()
    {
        if (localEntries == null || localEntries.Count == 0)
        {
            Debug.Log("[LOCAL] No hay entradas en la leaderboard.");
            return;
        }

        // Ordenar de mayor a menor puntaje
        localEntries.Sort((a, b) => b.score.CompareTo(a.score));

        Debug.Log("===== LEADERBOARD LOCAL =====");
        Debug.Log("Posición | Nombre | Puntaje");

        for (int i = 0; i < localEntries.Count; i++)
        {
            localEntries[i].rank = i + 1;
            Debug.Log($"{localEntries[i].rank,7} | {localEntries[i].name,-10} | {localEntries[i].score}");
        }
        Debug.Log("==============================");
    }

    [Title("Acciones Locales")]
    [Button("Enviar nuevo puntaje local")]
    public void AddLocalScore(string playerName, int score)
    {
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("El nombre del jugador no puede estar vacío.");
            return;
        }

        localEntries.Add(new LocalEntry(playerName, score));
        PrintLocalLeaderboard();
    }

    [Button("Refrescar leaderboard local")] 
    public void RefreshLocalLeaderboard()
    {
        PrintLocalLeaderboard();
    }

    [Title("Unity Leaderboards (remoto)")]
    [Button("Enviar puntaje remoto")]
    public async void SendScore(string leaderboardID , double score)
    {
        try
        {
            AddPlayerScoreOptions options = new AddPlayerScoreOptions();
            options.Metadata = "Img_20";

            LeaderboardEntry entry = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardID, score);

            Debug.Log($"[REMOTE] Enviado puntaje. Jugador: {entry.PlayerName} | Puntaje: {entry.Score} | Rank: {entry.Rank}");
        }
        catch(Exception E)
        {
            Debug.LogException(E);
        }

    }

    [Button("Leer puntaje del jugador actual")]
    public async void GetScore(string leaderboardID)
    {
        try
        {
            GetPlayerScoreOptions options = new GetPlayerScoreOptions();
            options.IncludeMetadata = true;

            LeaderboardEntry entry = await LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardID, options);

            Debug.Log($"[REMOTE] Score actual -> Rank: {entry.Rank} | Jugador: {entry.PlayerName} | Puntaje: {entry.Score} | Metadata: {entry.Metadata}");
        }
        catch (Exception E)
        {
            Debug.LogException(E);
        }
    }

    [Button("Leer Top N (ej. Top 5)")]
    public async void GetTopScores(string leaderboardID)
    {
        GetScoresOptions options = new GetScoresOptions();
        options.Limit = 5; // Top 5

        try
        {
            LeaderboardScoresPage page = await LeaderboardsService.Instance.GetScoresAsync(leaderboardID, options);

            Debug.Log("===== TOP REMOTO =====");
            Debug.Log("Posición | Nombre | Puntaje");

            foreach (var entry in page.Results)
            {
                Debug.Log($"{entry.Rank,7} | {entry.PlayerName,-10} | {entry.Score} (id: {entry.PlayerId})");
            }
            Debug.Log("======================");
        }
        catch (Exception E)
        {
            Debug.LogException(E);
        }

    }
    [Button("Leer rango alrededor del jugador")]
    public async void GetRangeScores(string leaderboardID)
    {
        GetPlayerRangeOptions options = new GetPlayerRangeOptions();
        options.RangeLimit = 5; // 5 entradas alrededor del jugador

        try
        {
            LeaderboardScores page = await LeaderboardsService.Instance.GetPlayerRangeAsync(leaderboardID, options);

            Debug.Log("===== RANGO DEL JUGADOR (REMOTO) =====");
            Debug.Log("Posición | Nombre | Puntaje");

            foreach (LeaderboardEntry entry in page.Results)
            {
                Debug.Log($"{entry.Rank,7} | {entry.PlayerName,-10} | {entry.Score} (id: {entry.PlayerId})");
            }
            Debug.Log("======================================");
        }
        catch (Exception E)
        {
            Debug.LogException(E);
        }
    }

}
