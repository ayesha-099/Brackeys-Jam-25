using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperMarioCameraController : MonoBehaviour
{

    // The target the camera will follow (e.g., your Player).
    public Transform playerTransform;

    // Which view is active?
    public enum CameraMode
    {
        Isometric,
        SideScrolling,
        FrontView
    }
    [Header("Current Camera Mode")]
    public CameraMode currentMode = CameraMode.Isometric;

    [Header("Camera Offsets")]

    // 1) Isometric view (think ~45° angle)
    public Vector3 isoOffset = new Vector3(-10f, 10f, -10f);
    public Vector3 isoRotation = new Vector3(35f, 45f, 0f);

    // 2) Side-scrolling 2D platformer offset
    //    Typically looking along the X-axis, so Y is up, Z offset for distance.
    public Vector3 sideOffset = new Vector3(0f, 2f, -10f);
    // Might want the camera to look straight on the X-axis, so:
    public Vector3 sideRotation = new Vector3(0f, 0f, 0f);

    // 3) Front/presentation offset
    public Vector3 frontOffset = new Vector3(0f, 2f, 10f);
    public Vector3 frontRotation = new Vector3(0f, 180f, 0f);

    // Smooth time for camera movement when switching modes
    [Header("Transition Settings")]
    public float transitionSmoothTime = 1.0f;

    private Vector3 _velocity = Vector3.zero; // For smoothing camera movement
    private Quaternion _rotVelocity;          // For smoothing camera rotation
    private bool _isSwitching = false;

    private void Update()
    {
        // Check keyboard inputs to switch camera modes
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetCameraMode(CameraMode.Isometric);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetCameraMode(CameraMode.SideScrolling);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetCameraMode(CameraMode.FrontView);
        }

        // Continuously update position and rotation based on current mode
        UpdateCameraPositionAndRotation();
    }

    /// <summary>
    /// Set the camera mode externally (e.g., triggered by UI or keystroke).
    /// </summary>
    public void SetCameraMode(CameraMode mode)
    {
        if (mode != currentMode)
        {
            currentMode = mode;
            _isSwitching = true;
        }
    }

    /// <summary>
    /// Smoothly updates camera position and rotation based on the active mode.
    /// </summary>
    private void UpdateCameraPositionAndRotation()
    {
        if (!playerTransform)
        {
            Debug.LogWarning("PaperMarioCameraController: No player transform assigned!");
            return;
        }

        // Determine target offsets
        Vector3 targetOffset;
        Vector3 targetRotationEulers;

        switch (currentMode)
        {
            case CameraMode.Isometric:
                targetOffset = isoOffset;
                targetRotationEulers = isoRotation;
                break;

            case CameraMode.SideScrolling:
                targetOffset = sideOffset;
                targetRotationEulers = sideRotation;
                break;

            case CameraMode.FrontView:
                targetOffset = frontOffset;
                targetRotationEulers = frontRotation;
                break;

            default:
                targetOffset = isoOffset;
                targetRotationEulers = isoRotation;
                break;
        }

        // The desired position of the camera
        Vector3 desiredPosition = playerTransform.position + targetOffset;

        // If we want to smoothly transition when switching
        if (_isSwitching)
        {
            // Smooth position
            transform.position = Vector3.SmoothDamp(
                transform.position,
                desiredPosition,
                ref _velocity,
                transitionSmoothTime
            );

            // Smooth rotation
            Quaternion desiredRotation = Quaternion.Euler(targetRotationEulers);
            transform.rotation = SmoothDampRotation(transform.rotation, desiredRotation, ref _rotVelocity, transitionSmoothTime);

            // Check if nearly arrived
            if (Vector3.Distance(transform.position, desiredPosition) < 0.05f &&
                Quaternion.Angle(transform.rotation, Quaternion.Euler(targetRotationEulers)) < 1f)
            {
                _isSwitching = false;
            }
        }
        else
        {
            // If not switching, just lock onto position/rotation with minimal smoothing
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotationEulers), Time.deltaTime * 5f);
        }
    }

    /// <summary>
    /// Helper function to smoothly damp rotation (since Vector3.SmoothDamp handles positions only).
    /// </summary>
    private Quaternion SmoothDampRotation(Quaternion current, Quaternion target, ref Quaternion velocity, float smoothTime)
    {
        // This approach is somewhat naive but works decently for moderate smoothing.
        // You can replace with advanced methods if needed.
        float deltaTime = Time.deltaTime;
        if (deltaTime < Mathf.Epsilon) return current;

        // Convert to Euler angles
        Vector3 currentEulers = current.eulerAngles;
        Vector3 targetEulers = target.eulerAngles;

        // Smooth each axis independently
        float x = Mathf.SmoothDampAngle(currentEulers.x, targetEulers.x, ref velocity.x, smoothTime, Mathf.Infinity, deltaTime);
        float y = Mathf.SmoothDampAngle(currentEulers.y, targetEulers.y, ref velocity.y, smoothTime, Mathf.Infinity, deltaTime);
        float z = Mathf.SmoothDampAngle(currentEulers.z, targetEulers.z, ref velocity.z, smoothTime, Mathf.Infinity, deltaTime);

        return Quaternion.Euler(x, y, z);
    }
}