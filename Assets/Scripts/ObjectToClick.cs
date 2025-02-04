using UnityEngine;
using UnityEngine.Events;

public class ObjectToClick : MonoBehaviour
{
    public UnityEvent onTap;

    private void OnTap() 
    {
        onTap.Invoke();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Update()
    {
        // Raycast for mouse or touch input
        if (Input.GetMouseButtonDown(0))  // Detect left-click or touch input
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Raycast to detect if the mouse is over the object
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);  // Check for 2D collision at touch position

            if (hit.collider != null && hit.collider.gameObject == gameObject)  // Check if the ray hit the current object
            {
                onTap.Invoke();  // Invoke the assigned methods in UnityEvent
            }
        }
    }
}
