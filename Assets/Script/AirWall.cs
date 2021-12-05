using UnityEngine;

public class AirWall : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        int other_layer = collision.gameObject.layer;
        if (other_layer == 9)
        {
            Rigidbody rigidbody = collision.rigidbody;
            rigidbody.AddForce(-10 * collision.GetContact(0).normal);
        }
    }
}
