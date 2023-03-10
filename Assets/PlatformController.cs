using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public GameObject objectPrefab;
    public float objectSpawnInterval = 2.0f;
    public float objectMinVelocity = 10.0f;
    public float objectMaxVelocity = 20.0f;
    public float objectMinAngle = 0.0f;
    public float objectMaxAngle = 180.0f;
    public float shakeDuration = 0.5f;
    public float shakeIntensity = 0.1f;

    private Rigidbody rb;
    private bool isShaking = false;
    private Vector3 startPosition;
    private float shakeTimer = 0.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        InvokeRepeating("SpawnObject", 0.0f, objectSpawnInterval);
    }

    void FixedUpdate()
    {
        if (isShaking)
        {
            float shakeX = Random.Range(-1.0f, 1.0f) * shakeIntensity;
            float shakeZ = Random.Range(-1.0f, 1.0f) * shakeIntensity;
            rb.AddForce(new Vector3(shakeX, 0.0f, shakeZ), ForceMode.Impulse);
            shakeTimer += Time.fixedDeltaTime;
            if (shakeTimer >= shakeDuration)
            {
                isShaking = false;
                shakeTimer = 0.0f;
                transform.position = startPosition;
            }
        }
    }

    void SpawnObject()
    {
        // Spawn object at random position and angle
        Vector3 spawnPosition = new Vector3(Random.Range(-5.0f, 5.0f), 15.0f, Random.Range(-5.0f, 5.0f));
        Quaternion spawnRotation = Quaternion.Euler(new Vector3(Random.Range(objectMinAngle, objectMaxAngle), 0.0f, 0.0f));
        GameObject obj = Instantiate(objectPrefab, spawnPosition, spawnRotation);

        // Set random velocity for object
        float velocityMagnitude = Random.Range(objectMinVelocity, objectMaxVelocity);
        Vector3 velocityDirection = new Vector3(Random.Range(-1.0f, 1.0f), -1.0f, Random.Range(-1.0f, 1.0f)).normalized;
        obj.GetComponent<Rigidbody>().velocity = velocityDirection * velocityMagnitude;

        // Destroy object after it hits the platform
        Destroy(obj, 10.0f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isShaking)
        {
            isShaking = true;
        }
    }
}