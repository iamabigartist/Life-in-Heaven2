using UnityEngine;
using UnityEngine.UI;
public class PauseGame : MonoBehaviour
{
    public Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        toggle.onValueChanged.AddListener(Pause);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Pause(bool isOn)
    {
        if (isOn) { Time.timeScale = 0; }
        else { Time.timeScale = 1; }
    }
}
