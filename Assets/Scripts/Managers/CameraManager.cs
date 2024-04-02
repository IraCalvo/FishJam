using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private float orthographicSize;
    private float targetOrthographicSize;
    public float minOrthographicSize = 5;
    public float maxOrthographicSize = 10;
    public float zoomSpeed = 5f;
    public float zoomAmount = 1f;
    public float moveSpeed = 10f;
    public Vector2 inputs;
    // Start is called before the first frame update
    private void Start()
    {
        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        
        float x = inputs.x;
        float y = inputs.y;
        Vector2 moveDir = new Vector2(x, y).normalized;
        if (CheckCameraOutOfBounds(moveDir))
        {
            return;
        }

        cinemachineVirtualCamera.transform.position += (Vector3)moveDir * moveSpeed * Time.deltaTime;
    }

    private bool CheckCameraOutOfBounds(Vector2 moveDir)
    {
        Debug.Log("MoveDirection: " + moveDir);
        // TopLeft
        Vector3 viewportPos = new Vector3(0f, 1f, camera.nearClipPlane);
        Debug.Log("Viewport" + viewportPos);
        Vector3 topLeft = camera.ViewportToWorldPoint(viewportPos);
        if (moveDir.x < 0 && cinemachineVirtualCamera.transform.position.x < -16)
        {
            return true;
        }
        if (moveDir.x > 0 && cinemachineVirtualCamera.transform.position.x > 16)
        {
            return true;
        }
        if (moveDir.y < 0 && cinemachineVirtualCamera.transform.position.y < -16)
        {
            return true;
        }
        if (moveDir.y > 0 && cinemachineVirtualCamera.transform.position.y > 16)
        {
            return true;
        }
        return false;
    }

    private void HandleZoom()
    {
        targetOrthographicSize += -Input.mouseScrollDelta.y * zoomAmount;

        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize);

        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);

        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }

    public void PlayerInput(InputAction.CallbackContext context)
    {
        inputs = context.ReadValue<Vector2>();
    }
}
