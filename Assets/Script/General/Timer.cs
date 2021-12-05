using UnityEngine;

public struct Timer
{
    public float cooldown;
    public float lasttime { get; set; }
    public float nexttimeAvailable { get => lasttime + cooldown; }

    public Timer(float cooldown, float lasttime)
    {
        this.cooldown = cooldown;
        this.lasttime = lasttime;
    }
    public bool Update()
    {
        if (Time.time > nexttimeAvailable)
        {
            lasttime = Time.time;
            return true;
        }
        else { return false; }
    }
}
