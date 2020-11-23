using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerManager playerManager;

    private void Start()
    {
        playerManager.Init();
        cameraController.Init(playerManager);
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
