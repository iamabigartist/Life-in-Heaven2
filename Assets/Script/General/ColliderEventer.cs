using System;

using UnityEngine;

public class ColliderEventer : MonoBehaviour
{
    public Action<Collision> AOnCollisionEnter;
    public Action<Collider> AOnTriggerEnter;
    public Action AOnMouseOver;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        AOnCollisionEnter?.Invoke(collision);
    }
    private void OnTriggerEnter(Collider other)
    {
        AOnTriggerEnter?.Invoke(other);
    }
    private void OnMouseOver()
    {
        AOnMouseOver?.Invoke();
    }
}
