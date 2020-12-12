using UnityEngine;
using System;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerShooting))]
[RequireComponent(typeof(PlayerUIController))]
[RequireComponent(typeof(PlayerSoundController))]
public class PlayerController : MonoBehaviour
{
    [Header("Components lins")]
    [SerializeField] private PlayerSoundController playerSoundController;
    [SerializeField] private PlayerUIController playerUIController;
    [SerializeField] private PlayerShooting playerShooting;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerHealth playerHealth;

    private string colorPlayerText;
    private int playerNumber;
    private int wins;

    public event Action<PlayerController> playerWinnerGame;
    public event Action OnDamage;

    #region Properties

    public PlayerController playerController
    {
        get
        {
            return this;
        }
    }

    public PlayerUIController PlayerUIController
    {
        get
        {
            return playerUIController;
        }
    }

    public float Currenthealth
    {
        get
        {
            return playerHealth.CurrentHealth;
        }
    }

    public float StartingHealth
    {
        get
        {
            return playerHealth.StartingHealth;
        }
    }

    public PlayerSoundController PlayerSoundController
    {
        get
        {
            return playerSoundController;
        }
    }

    public int PlayerNumber
    {
        get
        {
            return playerNumber;
        }
    }

    public string ColorPlayerText
    {
        get
        {
            return colorPlayerText;
        }
    }

    public int Wins
    {
        get
        {
            return wins;
        }
    }

    #endregion

    public void Init(int playerNumber, Transform parentBullet, Action OnPlayerDie, Action Reset, Color playerColor, Action<PlayerController> playerWinnerGame)
    {
        this.playerNumber = playerNumber;
        this.playerWinnerGame += playerWinnerGame;
        playerMovement.Init(this, playerNumber);
        playerShooting.Init(this, playerNumber, parentBullet);
        playerHealth.Init(this, OnDamageHandler, OnPlayerDie, Reset, playerUIController);
        playerUIController.Init(this);
        playerSoundController.Init();
        colorPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(playerColor) + ">PLAYER " + playerNumber + "</color>";
    }

    public void WinsUp()
    {
        wins++;
        CheckPlayerScore();
    }

    public void RefreshFixed()
    {
        playerMovement.Refresh();
    }

    public void Refresh()
    {
        playerShooting.Refresh();
    }

    private void OnDamageHandler()
    {
        playerUIController.UpdateHealth();
        OnDamage?.Invoke();
    }

    private void CheckPlayerScore()
    {
        if (wins == 5)
        {
            playerWinnerGame?.Invoke(this);
        }
    }
}