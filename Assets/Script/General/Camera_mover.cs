using UnityEngine;

public class Camera_mover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 h_vec = Input.GetAxis("Horizontal") * Vector3.right;
        Vector3 v_vec = Input.GetAxis("Vertical") * Vector3.forward;
        transform.position += 10*Time.deltaTime * (h_vec + v_vec);
    }
}
