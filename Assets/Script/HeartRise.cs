using UnityEngine;

public class HeartRise : MonoBehaviour
{
    public float rotate_speed;
    public float shrink_speed;
    public float rise_speed;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotate_speed * Time.deltaTime);
        transform.localScale -= Vector3.one * shrink_speed * Time.deltaTime;
        transform.position += new Vector3(0, rise_speed * Time.deltaTime, 0);
        if (transform.localScale.x < 0.01) { Destroy(gameObject); }
    }
}
