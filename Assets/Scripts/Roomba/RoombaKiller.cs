using UnityEngine;
using UnityEngine.InputSystem;

public class RoombaKiller : MonoBehaviour
{
    [SerializeField] Transform debugObject;

    void Start()
    {

    }

    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Debug.Log("[RoombaKiller] click derecho con el ratón");
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500f))
            {
                debugObject.position = hit.point;
                Roomba r = hit.collider.GetComponent<Roomba>();

                if (r != null)
                {
                    r.Hit();
                }
            }
        }
    }
}
