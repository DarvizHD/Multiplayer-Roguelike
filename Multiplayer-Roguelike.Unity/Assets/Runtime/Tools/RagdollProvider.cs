using UnityEngine;
using System.Collections.Generic;

public class RagdollProvider : MonoBehaviour
{
    public bool Enable
    {
        get => _enable;

        set
        {
            foreach (var rb in _rigidbodies)
            {
                rb.isKinematic = !value;
            }

            foreach (var col in _colliders)
            {
                col.enabled = value;
            }
        }
    }

    private bool _enable;

    [SerializeField] private List<Rigidbody> _rigidbodies = new List<Rigidbody>();
    [SerializeField] private List<Collider> _colliders = new List<Collider>();

    private void Start()
    {
        Debug.Log($"{_rigidbodies.Count} Rigidbodies");
        Debug.Log($"{_colliders.Count} Rigidbodies");
        Enable = false;
    }

    [ContextMenu("Register Ragdoll Components")]
    public void RegisterRagdollComponents()
    {
        _rigidbodies.Clear();
        _colliders.Clear();

        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        var colliders = GetComponentsInChildren<Collider>();

        _rigidbodies.AddRange(rigidbodies);
        _colliders.AddRange(colliders);

        Debug.Log($"Ragdoll registered: {_rigidbodies.Count} Rigidbodies, {_colliders.Count} Colliders");
    }
}
