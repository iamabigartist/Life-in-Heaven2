using UnityEngine;

public class LovingHeart : MonoBehaviour
{
    public Rigidbody m_anchor;
    public float life_span;
    public Timer life_timer;
    public float speed;
    public Citizen belonger;
    void Start()
    {
        life_timer = new Timer(life_span, Time.time);
        Vector3 move_vec = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        m_anchor.AddForce(speed * move_vec);
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(m_anchor.position, Vector3.down, out RaycastHit raycastHit_terrain, 1000, 1 << 12);
        transform.position = new Vector3(m_anchor.position.x, raycastHit_terrain.point.y, m_anchor.position.z);
        if (m_anchor.velocity.magnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation(m_anchor.velocity, Vector3.up);
        }
        if (life_timer.Update())
        {
            Destroy(transform.parent.gameObject);
        }
    }
    private void OnDestroy()
    {
        //print(life_span - (Time.time - life_timer.lasttime));
    }
}
