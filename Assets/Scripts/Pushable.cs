using UnityEngine;

public class Pushable : MonoBehaviour
{
    public void Push(Vector3 displacement)
    {
        transform.position += displacement;
    }
}
