using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    [SerializeField] private Material opaqueMaterial;
    [SerializeField] private Material transparentMaterial;
    private MeshRenderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<MeshRenderer>();
    }

    public void ChangeTransparency(bool transparent)
    {
        if (transparent)
        {
            objectRenderer.material = transparentMaterial;
        }
        else
        {
            objectRenderer.material = opaqueMaterial;
        }
    }
}
