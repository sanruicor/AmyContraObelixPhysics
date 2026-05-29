using UnityEngine;

public class TransparencyHunter : MonoBehaviour
{
    [SerializeField] Transform cameraTarget;  // Sitio al que miramos
    private TransparentObject lastTransparentObject;


    void Update()
    {
        Vector3 targetPositionVector = cameraTarget.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, targetPositionVector, out hit, targetPositionVector.magnitude, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            TransparentObject to = hit.collider.GetComponent<TransparentObject>();
            if (to != null)
            {
                to.ChangeTransparency(true);
                lastTransparentObject = to;
            }
        }
        else if (lastTransparentObject != null)
        {
            lastTransparentObject.ChangeTransparency(false);
            lastTransparentObject = null;
        }
    }
}
