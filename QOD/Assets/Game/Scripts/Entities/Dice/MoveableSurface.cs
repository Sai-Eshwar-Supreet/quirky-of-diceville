using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MoveableSurface : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent<MovableObject>(out var obj))
        {
            obj.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent<MovableObject>(out var obj) && (obj.CurrentParent == transform))
        {
            obj.ReturnToDefault();
        }
    }
}
