using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantenerPosicion : MonoBehaviour
{

    private Vector3 offsetFromParent;
    public Transform parent;
    private Quaternion initialRotation;

    void Start()
    {
        if (parent == null) parent = transform.parent; // Auto-assign if not set
        offsetFromParent = transform.position - parent.position; // Store initial offset
        initialRotation = transform.rotation; // Save initial world rotation
    }

    void LateUpdate()
    {
        // Apply only the parent's position changes, ignoring its rotation
        transform.position = parent.position + offsetFromParent;
        // Reset rotation to initial world rotation
        transform.rotation = initialRotation;
    }
}

