using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class MultiPlayerAuth : MonoBehaviour
{
    [Title("Perfiles simulados (anónimos)")]
    [InfoBox("Simula varios jugadores en el mismo dispositivo. Cada perfil tiene su propio PlayerId.")]
    [TableList]
    public List<PlayerProfile> profiles = new List<PlayerProfile>();

    [Serializable]
    public class PlayerProfile
    {
        [ReadOnly]
        public string playerId;
        public string displayName;
    }

    [Button("Inicializar Unity Services")]
    public async void InitializeServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("[AUTH] Unity Services inicializado.");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    [Button("Crear nuevo perfil anónimo")]
    public async void CreateNewAnonymousProfile()
    {
        try
        {
            if (!UnityServices.State.Equals(ServicesInitializationState.Initialized))
            {
                await UnityServices.InitializeAsync();
            }

            AuthenticationService.Instance.SignOut();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            var profile = new PlayerProfile
            {
                playerId = AuthenticationService.Instance.PlayerId,
                displayName = AuthenticationService.Instance.PlayerName
            };
            profiles.Add(profile);

            Debug.Log($"[AUTH] Nuevo perfil creado. PlayerId: {profile.playerId} | Name: {profile.displayName}");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    [Button("Autenticar como primer perfil")]
    public async void SignInFirstProfile()
    {
        if (profiles.Count == 0)
        {
            Debug.LogWarning("No hay perfiles creados.");
            return;
        }

        await SignInWithProfileIndex(0);
    }

    [Button("Autenticar como perfil por índice")]
    public async void SignInWithIndex(int index)
    {
        await SignInWithProfileIndex(index);
    }

    private async Task SignInWithProfileIndex(int index)
    {
        try
        {
            if (index < 0 || index >= profiles.Count)
            {
                Debug.LogWarning("Índice de perfil fuera de rango.");
                return;
            }

            if (!UnityServices.State.Equals(ServicesInitializationState.Initialized))
            {
                await UnityServices.InitializeAsync();
            }

            // Nota: Authentication anónima no permite reasignar un PlayerId existente manualmente.
            // Esto sirve sobre todo como demostración para crear varios perfiles en una sesión.
            AuthenticationService.Instance.SignOut();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            Debug.Log($"[AUTH] Sesión anónima activa. PlayerId actual: {AuthenticationService.Instance.PlayerId}");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
