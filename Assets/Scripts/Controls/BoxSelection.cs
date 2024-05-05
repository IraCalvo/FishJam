using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSelection : MonoBehaviour
{
    public static BoxSelection Instance { get; private set; }
    LineRenderer lineRenderer;
    [SerializeField] private Vector2 initialMousePosition;
    [SerializeField] private Vector2 currentMousePosition;
    private BoxCollider2D boxCollider;

    [SerializeField] public List<Fish> selectedFish;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Start
        if (Input.GetMouseButtonDown(0))
        {
            lineRenderer.positionCount = 4;
            initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.SetPosition(0, new Vector2(initialMousePosition.x, initialMousePosition.y));
            lineRenderer.SetPosition(1, new Vector2(initialMousePosition.x, initialMousePosition.y));
            lineRenderer.SetPosition(2, new Vector2(initialMousePosition.x, initialMousePosition.y));
            lineRenderer.SetPosition(3, new Vector2(initialMousePosition.x, initialMousePosition.y));

            boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
            boxCollider.offset = new Vector3(transform.position.x, transform.position.y, 0);
        }

        // Drag
        if (Input.GetMouseButton(0))
        {
            currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.SetPosition(0, new Vector2(initialMousePosition.x, initialMousePosition.y));
            lineRenderer.SetPosition(1, new Vector2(initialMousePosition.x, currentMousePosition.y));
            lineRenderer.SetPosition(2, new Vector2(currentMousePosition.x, currentMousePosition.y));
            lineRenderer.SetPosition(3, new Vector2(currentMousePosition.x, initialMousePosition.y));

            transform.position = (currentMousePosition + initialMousePosition) / 2;

            boxCollider.size = new Vector2(
                    Mathf.Abs(initialMousePosition.x - currentMousePosition.x),
                    Mathf.Abs(initialMousePosition.y - currentMousePosition.y)
                );
        }

        // End
        if (Input.GetMouseButtonUp(0))
        {

            CheckCollisions();

            lineRenderer.positionCount = 0;
            Destroy(boxCollider);
            transform.position = Vector3.zero;
        }
    }

    void CheckCollisions()
    {
        selectedFish = new List<Fish>();
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCollider.bounds.center, boxCollider.bounds.size, 0);

        foreach (Collider2D collider in colliders)
        {
            // Check if the collider is not part of the same GameObject as the boxCollider
            if (collider != boxCollider && collider.gameObject != gameObject && collider.gameObject.TryGetComponent<Fish>(out Fish fish))
            {
                if (fish.fishState.GetCurrentState() != FishState.State.Dead)
                {
                    selectedFish.Add(fish);
                }
            }
        }
    }

    public bool ContainsFish(Fish fish)
    {
        return selectedFish.Contains(fish);
    }

}
