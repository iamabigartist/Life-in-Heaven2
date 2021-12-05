using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    CitizenInfo citizenInfo;
    public Button generate_citizen;
    Camera camera;
    public Citizen SelectedCitizen;
    public Text m, a, c, s;
    public Texture red, yellow, gray, white, line;
    private void Start()
    {
        camera = Camera.main;
        citizenInfo = CitizenInfo.instance;
        generate_citizen.onClick.AddListener(() =>
        {
            GameObject obj1 = Instantiate(citizenInfo.Citizen, new Vector3(Random.Range(-25f, 25f), 5, Random.Range(-25f, 25f)), Quaternion.identity, citizenInfo.ctizen_list.transform);
            var citizen = obj1.GetComponentInChildren<Citizen>();
            citizen.m_gene = Gene.Genefromrandom();
        });
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit_obj, 1000f, 1 << 8 | 1 << 12))
            {
                GameObject gameObject = raycastHit_obj.transform.parent.gameObject;//It's the collider
                if (gameObject.layer == 8)
                {
                    if (SelectedCitizen)
                    {
                        SelectedCitizen.GetComponent<AudioListener>().enabled = false;
                    }
                    else { camera.GetComponent<AudioListener>().enabled = false; }
                    SelectedCitizen = gameObject.GetComponent<Citizen>();
                    SelectedCitizen.GetComponent<AudioListener>().enabled = true;
                }
                else
                {
                    if (SelectedCitizen)
                    {
                        SelectedCitizen.GetComponent<AudioListener>().enabled = false;
                    }
                    camera.GetComponent<AudioListener>().enabled = true;
                    SelectedCitizen = null;
                }
            }

        }
        #region text
        if (SelectedCitizen)
        {
            m.text = "move_speed: " + SelectedCitizen.m_move_speed;
            a.text = "appealing: " + SelectedCitizen.m_appealing;
            c.text = "charming: " + SelectedCitizen.m_charming;
            s.text = "seducing_speed/second: " + SelectedCitizen.m_seducing_speed_persecond;
        }
        else
        {
            m.text = "move_speed: " + "?";
            a.text = "appealing: " + "?";
            c.text = "charming: " + "?";
            s.text = "seducing_speed/second: " + "?";
        }
        #endregion
    }
    //private void OnDrawGizmos()
    //{
    //    if (SelectedCitizen)
    //    {
    //        Vector3 citizen_pos = SelectedCitizen.transform.position;
    //        #region Draw Relasionship
    //        for (int i = 0; i < SelectedCitizen.m_Friends.Count; i++)
    //        {
    //            Citizen another = SelectedCitizen.m_Friends.Keys.ElementAt(i);
    //            Relationship relationship = SelectedCitizen.m_Friends.Values.ElementAt(i);
    //            Vector3 another_pos = another.transform.position;
    //            //Vector2 rect_pos = camera.WorldToScreenPoint((another_pos + citizen_pos) / 2);

    //            #region Relationship line and cube
    //            switch (relationship.myattitude2other)
    //            {
    //                case Relationship.Attitude.Like:
    //                    {
    //                        Gizmos.color = Color.red;
    //                        break;
    //                    }
    //                case Relationship.Attitude.Moderate:
    //                    {
    //                        Gizmos.color = Color.white;
    //                        break;
    //                    }
    //                case Relationship.Attitude.Refuse:
    //                    {
    //                        Gizmos.color = Color.black;
    //                        break;
    //                    }
    //            }
    //            Gizmos.DrawLine(citizen_pos, another_pos);
    //            Vector3 cube_pos = another_pos + Vector3.up;
    //            Gizmos.DrawSphere(cube_pos + Vector3.down, 2f * relationship.otherlikability2me);
    //            #endregion
    //        }
    //        #endregion

    //        #region Draw Position
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawSphere(citizen_pos, 1f);

    //        #endregion

    //        #region Draw Lover
    //        if (SelectedCitizen.m_Lover)
    //        {
    //            Gizmos.color = Color.yellow;
    //            Vector3 sphere_pos = SelectedCitizen.m_Lover.transform.position;
    //            Gizmos.DrawSphere(sphere_pos, 1f);
    //        }
    //        #endregion

    //        #region Draw Anchor
    //        Gizmos.color = Color.yellow;
    //        Vector3 anchor_vec = SelectedCitizen.m_navMeshAgent.destination - citizen_pos;
    //        Gizmos.DrawRay(citizen_pos, anchor_vec);
    //        #endregion

    //    }
    //}
    private void OnGUI()
    {
        if (SelectedCitizen)
        {
            Vector3 citizen_pos = SelectedCitizen.transform.position;
            #region Draw Relasionship
            for (int i = 0; i < SelectedCitizen.m_Friends.Count; i++)
            {
                Citizen another = SelectedCitizen.m_Friends.Keys.ElementAt(i);
                Relationship relationship = SelectedCitizen.m_Friends.Values.ElementAt(i);
                Vector3 another_pos = another.transform.position;
                //Vector2 rect_pos = camera.WorldToScreenPoint((another_pos + citizen_pos) / 2);

                #region Relationship line and cube
                Texture ralation_texture = white;
                switch (relationship.myattitude2other)
                {
                    case Relationship.Attitude.Like:
                        {
                            ralation_texture = red;
                            break;
                        }
                    case Relationship.Attitude.Moderate:
                        {

                            break;
                        }
                    case Relationship.Attitude.Refuse:
                        {
                            ralation_texture = gray;
                            break;
                        }
                }

                Vector2 other_tpos = camera.WorldToScreenPoint(another_pos);
                GUI.DrawTexture(new Rect(new Vector2(other_tpos.x, Screen.height - other_tpos.y), Vector2.one * 75 * relationship.otherlikability2me), ralation_texture);

                #endregion
            }
            #endregion

            #region Draw Position
            Vector2 tpos = camera.WorldToScreenPoint(citizen_pos);
            GUI.DrawTexture(new Rect(new Vector2(tpos.x, Screen.height - tpos.y), Vector2.one * 50), yellow);
            #endregion

            #region Draw Lover
            if (SelectedCitizen.m_Lover)
            {
                Vector2 other_tpos = camera.WorldToScreenPoint(SelectedCitizen.m_Lover.transform.position);
                GUI.DrawTexture(new Rect(new Vector2(other_tpos.x, Screen.height - other_tpos.y), Vector2.one * 50), yellow);
            }
            #endregion

            //#region Draw Anchor
            //Gizmos.color = Color.yellow;
            //Vector3 anchor_vec = SelectedCitizen.m_navMeshAgent.destination - citizen_pos;
            //Gizmos.DrawRay(citizen_pos, anchor_vec);
            //#endregion

        }
    }

}
