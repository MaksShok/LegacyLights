using UnityEngine;

public class CameraFollowX : MonoBehaviour
{
    [Header("Следование")]
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;

    [Header("Ограничения")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private float minX = float.NegativeInfinity;
    [SerializeField] private float maxX = float.PositiveInfinity;

    [Header("Границы фона")]
    [SerializeField] private SpriteRenderer backgroundSprite;
    [SerializeField] private float cameraHalfWidth;

    private Vector3 initialPosition;
    private float initialY;
    private float initialZ;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
            mainCamera = Camera.main;

        CalculateCameraHalfWidth();

        initialPosition = transform.position;
        initialY = transform.position.y;
        initialZ = transform.position.z;

        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }

        if (backgroundSprite != null && useBounds)
        {
            AutoCalculateBoundsFromBackground();
        }
    }

    private void CalculateCameraHalfWidth()
    {
        if (mainCamera != null)
        {
            cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        }
        else
        {
            cameraHalfWidth = 5f;
        }
    }

    private void AutoCalculateBoundsFromBackground()
    {
        if (backgroundSprite == null)
            return;

        float backgroundMinX = backgroundSprite.bounds.min.x;
        float backgroundMaxX = backgroundSprite.bounds.max.x;

        minX = backgroundMinX + cameraHalfWidth;
        maxX = backgroundMaxX - cameraHalfWidth;

        if (minX > maxX)
        {
            float centerX = (backgroundMinX + backgroundMaxX) / 2f;
            minX = centerX;
            maxX = centerX;
        }
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        float targetX = target.position.x;

        if (useBounds)
        {
            targetX = Mathf.Clamp(targetX, minX, maxX);
        }

        Vector3 targetPosition = new Vector3(targetX, initialY, initialZ);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (!useBounds)
            return;

        if (mainCamera == null)
            mainCamera = GetComponent<Camera>();

        if (mainCamera != null)
        {
            CalculateCameraHalfWidth();
        }

        Gizmos.color = Color.green;
        Vector3 minBound = new Vector3(minX, transform.position.y - 2f, transform.position.z);
        Vector3 maxBound = new Vector3(maxX, transform.position.y + 2f, transform.position.z);
        
        Gizmos.DrawLine(minBound, new Vector3(minBound.x, minBound.y + 4f, minBound.z));
        Gizmos.DrawLine(maxBound, new Vector3(maxBound.x, maxBound.y + 4f, maxBound.z));
        
        Gizmos.DrawWireCube(
            new Vector3((minX + maxX) / 2f, transform.position.y, transform.position.z),
            new Vector3(maxX - minX, 4f, 0.1f)
        );
    }
}