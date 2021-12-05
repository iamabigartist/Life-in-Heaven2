using UnityEngine;

public class acoupledo : MonoBehaviour
{
    public Transform other;
    public Transform breeding_point;
    Rigidbody rigidbody;
    static public float scale = 10;

    float move_cooldown = 2;
    float last_move = 0;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 couple_vector = other.position - transform.position;
        rigidbody.AddForce(scale * (couple_vector.magnitude < 1 ? couple_vector : couple_vector.normalized));
        //Vector3 breeding_vector = breeding_point.position - transform.position;
        //if (breeding_vector.magnitude>10)
        //{
        //    rigidbody.AddForce(breeding_vector.normalized);
        //}

        if (Time.time > last_move + move_cooldown)
        {
            float x = Random.Range(-1.0f, 1.0f);
            float z = Random.Range(-1.0f, 1.0f);
            rigidbody.AddForce(new Vector3(x, 0, z) * 100);
            last_move = Time.time;
          
        }

    }
}
