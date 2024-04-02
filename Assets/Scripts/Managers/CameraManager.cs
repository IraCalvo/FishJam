using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private CinemachineConfiner confiner;
    [SerializeField] private Collider2D boundingBox;

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
        Vector3 targetPosition = (Vector3)moveDir * moveSpeed * Time.deltaTime + cinemachineVirtualCamera.transform.position;
        targetPosition.x = Mathf.Clamp(targetPosition.x, boundingBox.bounds.min.x, boundingBox.bounds.max.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, boundingBox.bounds.min.y, boundingBox.bounds.max.y);
        float distance = Vector2.Distance(targetPosition, camera.transform.position);
        if (distance > 1)
        {
            return;
        }
        cinemachineVirtualCamera.transform.position = targetPosition;
    }

    private void HandleZoom()
    {
        targetOrthographicSize += -Input.mouseScrollDelta.y * zoomAmount;

        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize);

        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);

        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;

        float distance = Vector2.Distance(cinemachineVirtualCamera.transform.position, camera.transform.position);
        if (distance > 1)
        {
            cinemachineVirtualCamera.transform.position = camera.transform.position;
        }
    }

    public void PlayerInput(InputAction.CallbackContext context)
    {
        inputs = context.ReadValue<Vector2>();
    }
}
