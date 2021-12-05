using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.AI;

public enum Gender
{
    Male,
    Female
}

[RequireComponent(typeof(NavMeshAgent))]
public class Citizen : MonoBehaviour
{
    CitizenInfo citizenInfo;

    public enum State
    {
        Breed,
        Idle,
        Cheer,
        Blame
    }

    #region Components
    public NavMeshAgent m_navMeshAgent;
    public MeshRenderer m_renderer;
    public Animator m_animator;
    public AudioSource m_audiosuorce;
    ColliderEventer m_collider;
    #endregion

    #region Properties
    [Header("Properties")]
    public Gene m_gene;
    public Gender m_gender;
    [Range(10, 20)]
    public float m_move_speed;
    [Range(0.5f, 1.5f)]
    public float m_appealing;
    [Range(0.2f, 0.8f)]
    public float m_charming;
    [Range(0, 2)]
    public float m_seducing_speed_persecond;
    Timer bullet_timer;
    #endregion

    #region Relationships
    public Citizen m_Lover;
    float breed_timer;
    public Dictionary<Citizen, Relationship> m_Friends;
    public List<Citizen> Love_me_list;
    #endregion

    #region State
    public State m_state;
    public bool is_child;
    Timer child_timer;
    #endregion

    #region Moving
    public Rigidbody m_anchor;
    public Timer move_timer;
    #endregion

    void Start()
    {
        GeneSet();

        m_Friends = new Dictionary<Citizen, Relationship>();
        m_collider = transform.Find("collider").GetComponent<ColliderEventer>();
        m_collider.AOnTriggerEnter += MyOnTriggerEnter;
        citizenInfo = CitizenInfo.instance;

        m_renderer.material = citizenInfo.Child1;
        move_timer = new Timer(citizenInfo.move_cooldown, Time.time - citizenInfo.move_cooldown * (1 + Random.Range(-1f, 1f)));
        bullet_timer = new Timer(1 / m_seducing_speed_persecond, Time.time - (1 / m_seducing_speed_persecond) * (1 + Random.Range(-1f, 1f)));
        child_timer = new Timer(10, Time.time);

        m_state = State.Idle;
        m_Lover = null;
        Love_me_list = new List<Citizen>();

        m_navMeshAgent.speed = m_move_speed;
    }

    void GeneSet()
    {
        m_gender = m_gene.gender;

        m_charming = Gene.limit_dict["charming"].x + (m_gene.gene_dict["charming"] / m_gene.sum) * (Gene.limit_dict["charming"].y - Gene.limit_dict["charming"].x);

        m_appealing = Gene.limit_dict["appealing"].x + (m_gene.gene_dict["appealing"] / m_gene.sum) * (Gene.limit_dict["appealing"].y - Gene.limit_dict["appealing"].x);

        m_move_speed = Gene.limit_dict["move_speed"].x + (m_gene.gene_dict["move_speed"] / m_gene.sum) * (Gene.limit_dict["move_speed"].y - Gene.limit_dict["move_speed"].x);

        m_seducing_speed_persecond = Gene.limit_dict["seducing_speed_persecond"].x + (m_gene.gene_dict["seducing_speed_persecond"] / m_gene.sum) * (Gene.limit_dict["seducing_speed_persecond"].y - Gene.limit_dict["seducing_speed_persecond"].x);
    }
    void Update()
    {
        #region Moving
        Physics.Raycast(m_anchor.position, Vector3.down, out RaycastHit raycastHit_terrain, 1000, 1 << 12);
        Vector3 dst = new Vector3(m_anchor.position.x, raycastHit_terrain.point.y, m_anchor.position.z);
        m_navMeshAgent.SetDestination(dst);
        #endregion

        #region Grow
        if (is_child)
        {
            if (child_timer.Update())
            {
                is_child = false;
                #region Growup Setting
                switch (m_gender)
                {
                    case Gender.Female:
                        {
                            m_renderer.material = citizenInfo.Woman1;
                            break;
                        }
                    case Gender.Male:
                        {
                            m_renderer.material = citizenInfo.Man1;
                            break;
                        }
                }
                #endregion
            }
            else
            {
                #region RandomChildMove
                Vector3 rmove_vec = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1, 1f)) * 100f;
                m_anchor.AddForce(rmove_vec);
                #endregion
                return;
            }
        }
        #endregion

        #region SpeedLimit
        if (m_anchor.velocity.magnitude > 10) { m_anchor.velocity = m_anchor.velocity.normalized * 10; }
        #endregion

        #region UpdateState: Idle,Cheer,Blame,Breed
        switch (m_state)
        {
            case State.Idle:
                {
                    #region RandomMove
                    if (move_timer.Update())
                    {
                        Vector3 rmove_vec = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1, 1f)) * 0.5f;
                        m_anchor.AddForce(rmove_vec);
                    }
                    #endregion

                    #region LikeMove
                    for (int i = 0; i < m_Friends.Count; i++)
                    {
                        Citizen another = m_Friends.Keys.ElementAt(i);
                        Relationship relationship = m_Friends.Values.ElementAt(i);
                        Vector3 limove_vec = another.transform.position - transform.position;
                        switch (relationship.myattitude2other)
                        {
                            case Relationship.Attitude.Like:
                                {
                                    m_anchor.AddForce(1 * (limove_vec.magnitude < 1 ? limove_vec : limove_vec.normalized) * another.m_appealing);
                                    break;
                                }
                            case Relationship.Attitude.Moderate:
                                {

                                    break;
                                }
                            case Relationship.Attitude.Refuse:
                                {
                                    if (limove_vec.magnitude < 5)
                                    {
                                        m_anchor.AddForce(limove_vec.normalized * -1);
                                    }
                                    break;
                                }
                        }
                    }
                    #endregion

                    #region Leave forsets
                    Vector3 vec_l = citizenInfo.l_breed_point.position - transform.position;
                    Vector3 vec_r = citizenInfo.r_breed_point.position - transform.position;
                    float a = (citizenInfo.l_breed_point.position - citizenInfo.r_breed_point.position).magnitude + 5;
                    if (vec_l.magnitude + vec_r.magnitude < 1.5 * 2 * a)
                    {
                        m_anchor.AddForce(new Vector3(-1, 0, 1));
                    }
                    #endregion

                    break;
                }
            case State.Cheer:
                {
                    m_animator.SetTrigger("Cheer!");
                    breed_timer = citizenInfo.breed_cooldown;
                    m_state = State.Breed;
                    m_audiosuorce.PlayOneShot(citizenInfo.a_breed);
                    for (int i = 0; i < Love_me_list.Count; i++)
                    {
                        if (Love_me_list[i] == m_Lover) { continue; }
                        Love_me_list[i].m_state = State.Blame;
                    }//make others fail
                    break;
                }
            case State.Blame:
                {
                    m_state = State.Idle;
                    m_animator.SetTrigger("Blame self");
                    m_audiosuorce.PlayOneShot(citizenInfo.a_blame);
                    break;
                }

            case State.Breed:
                {
                    Vector3 lover_pos = m_Lover.transform.position;
                    Vector3 lomove_vec = lover_pos - transform.position;

                    #region Move2Love
                    m_anchor.AddForce(1 * (lomove_vec.magnitude < 1 ? lomove_vec : lomove_vec.normalized));
                    #endregion

                    #region Enter Forests
                    Vector3 vec_l = citizenInfo.l_breed_point.position - transform.position;
                    Vector3 vec_r = citizenInfo.r_breed_point.position - transform.position;
                    float a = (citizenInfo.l_breed_point.position - citizenInfo.r_breed_point.position).magnitude + 5;
                    if (vec_l.magnitude + vec_r.magnitude > 2 * a)
                    {
                        m_anchor.AddForce((vec_l + vec_r).normalized);
                    }
                    #endregion

                    #region Breeding
                    if ((lover_pos - transform.position).magnitude < 1.5)
                    {
                        if (breed_timer > 0) { breed_timer -= Time.deltaTime; }
                    }
                    if (breed_timer <= 0 || m_Lover.m_Lover == null)//end breeding
                    {
                        #region bornbaby
                        GameObject obj1 = Instantiate(citizenInfo.Citizen, new Vector3(transform.position.x, 5, transform.position.z), Quaternion.identity, citizenInfo.ctizen_list.transform);
                        var citizen = obj1.GetComponentInChildren<Citizen>();
                        citizen.m_gene = Gene.Genefromparent(m_gene, m_Lover.m_gene);
                        print("yes@@!!!");

                        #endregion
                        m_audiosuorce.Stop();
                        m_state = State.Idle;
                        m_Friends[m_Lover] = new Relationship(m_Lover);
                        m_Lover = null;//divorce


                    }
                    #endregion

                    break;
                }
        }
        #endregion

        #region ShotHeart
        if (bullet_timer.Update())
        {
            Vector3 bullet_position = new Vector3(transform.position.x, 0, transform.position.z);
            var bullet = Instantiate(citizenInfo.LovingHeart, bullet_position, transform.rotation);
            LovingHeart heart = bullet.GetComponentInChildren<LovingHeart>();
            heart.belonger = this;
        }
        #endregion


    }

    /// <summary>
    /// Only excute when the citizen is idling.
    /// </summary>
    /// <param name="collider"></param>
    private void MyOnTriggerEnter(Collider collider)
    {
        if (m_state != State.Idle || is_child) { return; }
        int other_layer = collider.gameObject.layer;
        Relationship.Attitude like = Relationship.Attitude.Like;
        Relationship.Attitude refuse = Relationship.Attitude.Refuse;
        switch (other_layer)
        {
            case 8://citizen
                {

                    Citizen other = collider.transform.parent.gameObject.GetComponent<Citizen>();
                    if (!m_Friends.ContainsKey(other) || !other.m_Friends.ContainsKey(this)) { break; }
                    Relationship.Attitude a_m2o = m_Friends[other].myattitude2other;
                    Relationship.Attitude a_o2m = other.m_Friends[this].myattitude2other;

                    if ((a_m2o == like || a_o2m == like) && (a_m2o != refuse && a_o2m != refuse))
                    {
                        m_Lover = other;
                        m_state = State.Cheer;
                        print("yeah!!!");
                    }//Marriage
                    break;

                }
            case 11://heart
                {
                    LovingHeart heart = collider.gameObject.GetComponent<LovingHeart>();
                    Citizen other = heart.belonger;
                    if (other == this) { break; }
                    if (!m_Friends.ContainsKey(other)) { m_Friends[other] = new Relationship(other); }
                    Relationship relationship = m_Friends[other];

                    if (relationship.otherlikability2me < 1) { relationship.otherlikability2me += 0.05f; }//ajust like ability.

                    if (relationship.myattitude2other != like || relationship.like_timer.Update())//when not like or time is up, rethink.
                    { relationship.Rethink(); }

                    #region Thrill
                    if (relationship.myattitude2other == like && relationship.lastattitude != like)
                    {
                        relationship.lastattitude = like;
                        m_animator.SetTrigger("Thrill!");
                    }
                    #endregion

                    #region registLovelist
                    if (relationship.myattitude2other == Relationship.Attitude.Like)
                    {
                        if (!other.Love_me_list.Contains(this)) { other.Love_me_list.Add(this); }
                    }
                    else
                    {
                        if (other.Love_me_list.Contains(this)) { other.Love_me_list.Remove(this); }
                    }
                    #endregion

                    m_Friends[other] = relationship;//assign back
                    Destroy(collider.transform.parent.gameObject);//eat the heart

                    break;
                }

        }
    }

    void Blame2Idle() { m_state = State.Idle; }

    void BoreBaby(Vector3 position)
    {

    }
}
