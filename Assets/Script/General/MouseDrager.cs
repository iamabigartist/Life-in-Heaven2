using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrager : MonoBehaviour
{
    public Rigidbody anchor;
    private void OnMouseDrag()
    {
        
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            anchor.AddForce(1000*(mouse_pos - transform.position).normalized);
        }
    }
}
