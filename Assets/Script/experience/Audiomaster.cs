using UnityEngine;
using UnityEngine.UI;

public class Audiomaster : MonoBehaviour
{
    public Button Play, Pause, Playaclip;
    public AudioSource player;
    public AudioClip blame;
    // Start is called before the first frame update
    void Start()
    {
        Play.onClick.AddListener(() =>
        {
            player.Play();
        });
        Pause.onClick.AddListener(() =>
        {
            player.Stop();
        });
        Playaclip.onClick.AddListener(() =>
        {
            player.PlayOneShot(blame);
        });
    }

    // Update is called once per frame
    void Update()
    {
        print(Input.mousePosition);
    }
}
