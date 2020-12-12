using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float smoothTime;
    [SerializeField] private float screenEdgeBuffer;
    [SerializeField] private float minSize;
    [Header("Camera")]
    [SerializeField] private Camera cam;

    private Vector3 desiredPosition;
    private Vector3 moveVelocity;
    private float zoomSpeed;
    private Transform[] targets;

    private PlayerManager playerManager;

    public void Init(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
        playerManager.Reset += SetStartPositionAndSize;
        SetTargets();
        SetStartPositionAndSize();
    }

    public void Refresh()
    {
        Move();
        Zoom();
    }

    private void SetTargets()
    {
        targets = playerManager.PlayerTargets;
    }

    private void Move()
    {
        FindAveragePosition();
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, smoothTime);
    }

    private void FindAveragePosition()
    {
        Vector3 averagePosition = new Vector3();
        int numTagets = 0;

        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].gameObject.activeSelf)
                continue;

            averagePosition += targets[i].position; // TO DO уточнить как складываются позиции
            numTagets++;
        }

        if (numTagets > 0)
        {
            averagePosition /= numTagets;
        }
        averagePosition.y = transform.position.y;
        desiredPosition = averagePosition;
    }

    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, requiredSize, ref zoomSpeed, smoothTime);
    }

    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(desiredPosition);
        float size = 0;

        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].gameObject.activeSelf)
            {
                continue;
            }

            Vector3 targetLocalPos = transform.InverseTransformPoint(targets[i].position);
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / cam.aspect);
        }
        size += screenEdgeBuffer;
        size = Mathf.Max(size, minSize);

        return size;
    }

    public void SetStartPositionAndSize()
    {
        FindAveragePosition();
        transform.position = desiredPosition;
        cam.orthographicSize = FindRequiredSize();
    }
}
