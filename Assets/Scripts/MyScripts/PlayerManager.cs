using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Transform playersParent;
    [SerializeField] private List<PlayersInfo> playersInfo;
    [SerializeField] private float spawnDelay;

    private CameraController cameraController;
    private Transform[] playerTargets;
    private List<PlayerController> players;

    public Transform[] PlayerTargets
    {
        get
        {
            return playerTargets;
        }
    }

    public void Init(CameraController cameraController)
    {
        this.cameraController = cameraController;
        GreatPlayersPool();
        SetCameraTargets();
        StartCoroutine(SpawnPlayers());
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

    public void SetCameraTargets()
    {
        playerTargets = new Transform[players.Count];

        for (int i = 0; i < playerTargets.Length; i++)
        {
            playerTargets[i] = players[i].transform;
        }
    }

    public void GreatPlayersPool()
    {
        players = new List<PlayerController>();

        for (int i = 0; i < playersInfo.Count; i++)
        {
            PlayerController playerController = Instantiate(playersInfo[i].playerPrefab, playersInfo[i].spawnPoint.position, Quaternion.identity, playersParent); // уточнить по созданию объекта как распознает и понимает юнити
            MeshRenderer[] renderers = playerController.GetComponentsInChildren<MeshRenderer>();

            for (int j = 0; j < renderers.Length; j++)
            {
                renderers[j].material.color = playersInfo[i].playerColor;
            }

            playerController.gameObject.SetActive(false);
            playerController.Init(playersInfo[i].playerNumber);
            players.Add(playerController);
        }
    }

    public IEnumerator SpawnPlayers()
    {
        yield return new WaitForSeconds(spawnDelay);

        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].gameObject.activeSelf)
            {
                if (players[i].gameObject != null)
                {
                    players[i].transform.position = playersInfo[i].spawnPoint.position;
                    players[i].gameObject.SetActive(true);
                }
            }
        }
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
