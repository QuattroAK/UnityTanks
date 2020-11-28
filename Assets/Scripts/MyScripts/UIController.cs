using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider[] sliderHealth;
    [SerializeField] private Image[] fillImageHealth;
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color zeroHealthColor = Color.red;

    private PlayerManager playerManager;

    public void Init(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
        SetPlayers();
    }

    private void SetPlayers()
    {
        for (int i = 0; i < 2; i++)
        {
            PlayerController player = playerManager.GetPlayers();
            if (player != null)
            {
                sliderHealth[i] = player.gameObject.GetComponent<Slider>();
                sliderHealth[i].value = player.Currenthealth;
                fillImageHealth[i] = player.GetComponent<Image>();
                fillImageHealth[i].color = Color.Lerp(fullHealthColor, zeroHealthColor, player.Currenthealth / player.StartingHealth);
                player.OnDamage += ShowDamage;
            }
        }
    }

    //private void SetPlayers()
    //{
    //    for (int i = 0; i < playerManager.PlayerTargets.Length; i++)
    //    {
    //        players[i] = playerManager.GetPlayers();
    //    }
    //}

    private void ShowDamage()
    {
        //for (int i = 0; i < players.Length; i++)
        //{
        //    sliderHealth[i].value = players[i].Currenthealth / players[i].StartingHealth;
        //    fillImageHealth[i].color = Color.Lerp(fullHealthColor, zeroHealthColor, players[i].Currenthealth / players[i].StartingHealth);
        //}

        //playerController.Currenthealth / playerController.StartingHealth;
        //Color.Lerp(fullHealthColor, zeroHealthColor, playerController.Currenthealth / playerController.StartingHealth);
    }
}
