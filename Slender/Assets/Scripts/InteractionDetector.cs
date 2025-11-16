using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Debug.Log("InteractionDetector: OnInteract performed. interactableInRange = " + (interactableInRange != null ? interactableInRange.ToString() : "null")); //AI ADDED
            interactableInRange?.Interact(); 
        }
    }
    
    // Fallback: also accept keyboard 'E' so interaction works even if the InputSystem event isn't wired.
    void Update()
    {
        if (interactableInRange == null) return;

        // New Input System check
        if (Keyboard.current != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                Debug.Log("InteractionDetector: E pressed (Keyboard.current) - invoking Interact");
                interactableInRange.Interact();
                return;
            }
        }

        // Old Input Manager fallback (works if both input systems are enabled)
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("InteractionDetector: E pressed (Input.GetKeyDown) - invoking Interact");
            interactableInRange.Interact();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("InteractionDetector: OnTriggerEnter2D with " + collision.gameObject.name); 
        if(collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            interactableInRange = interactable; 
            Debug.Log("InteractionDetector: interactable in range set to " + collision.gameObject.name); 
        }

    }

     private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("InteractionDetector: OnTriggerExit2D with " + collision.gameObject.name); 
        if(collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
        {
            interactableInRange = null; 
            Debug.Log("InteractionDetector: interactable in range cleared (" + collision.gameObject.name + ")"); 
        }
        
    }

}
