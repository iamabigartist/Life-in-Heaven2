using UnityEngine;

public class CitizenInfo : MonoBehaviour
{
    static public CitizenInfo instance;
    public Material Man1, Woman1, Child1;
    public GameObject LovingHeart;
    public GameObject Citizen;
    public float move_cooldown, breed_cooldown;
    public Transform l_breed_point, r_breed_point;
    public AudioClip a_breed, a_blame;
    public GameObject ctizen_list;
    private void Awake()
    {
        instance = this;
    }

}
