using UnityEngine;

public class CameraFollowX : MonoBehaviour
{
    [Header("Настройки следования")]
    [SerializeField] private Transform target; 
    [SerializeField] private float smoothSpeed = 5f; // Скорость сглаживания движения
    
    private Vector3 initialPosition; 
    private float initialY; 
    private float initialZ; 

    void Start()
    {
        // Запоминаем начальную позицию камеры
        initialPosition = transform.position;
        initialY = transform.position.y;
        initialZ = transform.position.z;
        
        // Если цель не назначена в инспекторе, пробуем найти её автоматически
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
            else
                Debug.LogError("CameraFollowX: Не найден объект с тегом 'Player'!");
        }
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // Вычисляем новую позицию: X как у цели, Y и Z как при запуске
        Vector3 targetPosition = new Vector3(target.position.x, initialY, initialZ);
        
        // Плавно перемещаем камеру (сглаживание)
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}