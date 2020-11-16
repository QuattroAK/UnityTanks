using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerManager playerManager;

    private void Start()
    {
        cameraController.Init(playerManager);
        playerManager.Init(cameraController);
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        cameraController.Refresh();
        playerManager.Refresh();
    }
}
