using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image boardWinner;
    [SerializeField] private Text roundWinner;
    [SerializeField] private Text scorePlayers;
    [SerializeField] private Text gameWinner;
    [SerializeField] private Text gameRound;

    private PlayerManager playerManager;

    public void Init(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
        playerManager.OnPlayerDie += ShowWinnerRound;
        playerManager.OnPlayerDie += ShowPlayerScore;
        playerManager.Reset += RemoveBoard;
        playerManager.PlayerWinnerGame += ShowWinnerGame;
    }

    private void ShowWinnerRound()
    {
        PlayerController playerwin = playerManager.ReturnPlayerWinner();

        if (playerwin != null)
        {
            roundWinner.text = $"{playerwin.ColorPlayerText} WINS THE ROUND!";
            playerwin.WinsUp();
        }
        else
        {
            roundWinner.text = $"DRAW!";
        }
        boardWinner.gameObject.SetActive(true);
    }

    private void ShowPlayerScore()
    {
        for (int i = 0; i < playerManager.Players.Count; i++)
        {
            scorePlayers.text += $"{playerManager.Players[i].ColorPlayerText} {playerManager.Players[i].Wins} WINS\n";
        }
    }

    private void RemoveBoard()
    {
        boardWinner.gameObject.SetActive(false);
        roundWinner.text = string.Empty;
        scorePlayers.text = string.Empty;
    }

    private void ShowWinnerGame(PlayerController player)
    {
        RemoveBoard();
        gameWinner.text = $"{player.ColorPlayerText} WINS THE GAME!";
    }

    public void ShowRound(int round)
    {
        boardWinner.gameObject.SetActive(true);
        gameRound.text = $"ROUND {round}";
        DOVirtual.DelayedCall(2f, DeactiveShowRound);
    }

    private void DeactiveShowRound()
    {
        gameRound.text = string.Empty;
        boardWinner.gameObject.SetActive(false);
    }
}