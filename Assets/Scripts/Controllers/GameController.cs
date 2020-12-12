using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private UIController uIController;
    [SerializeField] private SoundController soundController;

    private int round = 0;

    private void Start()
    {
        playerManager.Init();
        cameraController.Init(playerManager);
        uIController.Init(playerManager);
        soundController.Init();
        playerManager.EndGame += RestartGame;
        playerManager.ShowRound += ShowRound;
        ShowRound();
    }

    public void Update()
    {
        playerManager.Refresh();
    }

    private void FixedUpdate()
    {
        cameraController.Refresh();
        playerManager.RefreshFixed();
    }

    private void RestartGame()
    {
        DOVirtual.DelayedCall(2f, Restart);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ShowRound()
    {
        round++;
        uIController.ShowRound(round);
    }
}