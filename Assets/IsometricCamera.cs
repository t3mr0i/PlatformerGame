using UnityEngine;

public class IsometricCamera : MonoBehaviour
{
    // Define the speed of the camera movement
    public float moveSpeed = 10f;

    // Define the strength of the camera shake
    public float shakeStrength = 0.1f;

    // Define the duration of the camera shake
    public float shakeDuration = 0.2f;

    // Reference to the player game object
    public GameObject player;

    // Reference to the main camera game object
    public Camera mainCamera;

    // Update is called once per frame
    void Update()
    {
        // Get the player's position in world space
        Vector3 playerPosition = player.transform.position;

        // Set the camera's position in isometric space
        transform.position = new Vector3(playerPosition.x, playerPosition.y + 10f, playerPosition.z - 10f);

        // If the player is touching the ground
   
    }

    // Coroutine for shaking the camera
    void ShakeCamera()
    {
        // Get the initial position of the camera
        Vector3 initialPosition = mainCamera.transform.position;

        // Shake the camera for the defined duration
        float timeElapsed = 0f;
        while (timeElapsed < shakeDuration)
        {
            // Calculate a random offset for the camera position
            Vector3 shakeOffset = Random.insideUnitSphere * shakeStrength;

            // Apply the offset to the camera's position
            mainCamera.transform.position = initialPosition + shakeOffset;

            // Increment the elapsed time
            timeElapsed += Time.deltaTime;
            
        }

        // Reset the camera's position
        mainCamera.transform.position = initialPosition;
    }
}