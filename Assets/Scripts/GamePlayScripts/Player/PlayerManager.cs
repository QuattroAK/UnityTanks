using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    [Header("Transform settings")]
    [SerializeField] private Transform parentBullet;
    [Header("Players settings")]
    [SerializeField] private List<PlayersInfo> playersInfo;

    private List<PlayerController> players;
    private Transform[] playerTargets;

    public event Action<PlayerController> PlayerWinnerGame;
    public event Action OnPlayerDie;
    public event Action ShowRound;
    public event Action EndGame;
    public event Action Reset;

    #region Properties

    public Transform[] PlayerTargets
    {
        get
        {
            return playerTargets;
        }
    }

    public List<PlayerController> Players
    {
        get
        {
            return players;
        }
    }

    #endregion

    public void Init()
    {
        CreatePlayersPool();
        SetCameraTargets();
        StartCoroutine(SpawnPlayers());
    }

    public void RefreshFixed()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].gameObject.activeSelf)
            {
                players[i].RefreshFixed();
            }
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].gameObject.activeSelf)
            {
                players[i].Refresh();
            }
        }
    }

    public PlayerController ReturnPlayerWinner()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].gameObject.activeSelf)
            {
                return players[i];
            }
        }
        return null;
    }

    public void SetCameraTargets()
    {
        playerTargets = new Transform[players.Count];

        for (int i = 0; i < playerTargets.Length; i++)
        {
            playerTargets[i] = players[i].transform;
        }
    }

    public void CreatePlayersPool()
    {
        players = new List<PlayerController>();

        for (int i = 0; i < playersInfo.Count; i++)
        {
            PlayerController playerController = Instantiate(playersInfo[i].playerPrefab, playersInfo[i].spawnPoint.position, playersInfo[i].spawnPoint.rotation);
            MeshRenderer[] renderers = playerController.GetComponentsInChildren<MeshRenderer>();

            for (int j = 0; j < renderers.Length; j++)
            {
                renderers[j].material.color = playersInfo[i].playerColor;
            }

            playerController.gameObject.SetActive(false);
            playerController.Init(playersInfo[i].playerNumber, parentBullet, OnPlayerDieHandler, ResetHandler, playersInfo[i].playerColor, OnEndGameHandler);
            players.Add(playerController);
        }
    }

    public IEnumerator SpawnPlayers()
    {
        ShowRound?.Invoke();
        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].gameObject.activeSelf)
            {
                if (players[i].gameObject != null)
                {
                    players[i].transform.position = playersInfo[i].spawnPoint.position;
                    players[i].transform.rotation = playersInfo[i].spawnPoint.rotation;
                    players[i].gameObject.SetActive(true);
                }
            }
        }
        yield return null;
    }

    private void ResetPlayersPosition()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].gameObject.SetActive(false);
            players[i].transform.position = playersInfo[i].spawnPoint.position;
        }
        StartCoroutine(SpawnPlayers());
    }

    public void OnPlayerDieHandler()
    {
        OnPlayerDie?.Invoke();
    }

    public void OnEndGameHandler(PlayerController player)
    {
        PlayerWinnerGame?.Invoke(player);
        EndGame?.Invoke();
    }

    public void ResetHandler()
    {
        Reset?.Invoke();
        ResetPlayersPosition();
    }
}

[Serializable]
public class PlayersInfo
{
    public Color playerColor;
    public Transform spawnPoint;
    public PlayerController playerPrefab;
    public int playerNumber;
}
