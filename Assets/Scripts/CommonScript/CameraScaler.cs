using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScaler : MonoBehaviour
{
    // Reference dimensions for 16:9 (e.g., 1920 x 1080)
    public float targetWidth = 1920f;
    public float targetHeight = 1080f;
    
    // Reference orthographic size you want for the target dimensions.
    // Usually 5 is a common default for an orthographic camera in many 2D setups.
    public float targetOrthographicSize = 11f;
    
    private Camera _camera;

    void Awake()
    {
        _camera = GetComponent<Camera>();

        // Calculate the target aspect ratio
        float targetAspect = targetWidth / targetHeight;

        // Get the current screen aspect ratio
        float currentAspect = (float)Screen.width / (float)Screen.height;

        // Scale the orthographic size based on the ratio difference
        // If the current aspect is wider, you reduce the orthographic size
        // If narrower, you increase the size
        float ratioDifference = currentAspect / targetAspect;
        _camera.orthographicSize = targetOrthographicSize / ratioDifference;
    }
}