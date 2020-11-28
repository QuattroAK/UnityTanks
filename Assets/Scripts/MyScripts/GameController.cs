using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private UIController uIController;
    [SerializeField] private Transform parentBulletObject;

    private void Start()
    {
        playerManager.Init(parentBulletObject);
        cameraController.Init(playerManager);
        uIController.Init(playerManager);
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
}
