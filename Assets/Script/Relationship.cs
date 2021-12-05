using UnityEngine;
using UnityEngine.SocialPlatforms;

public struct Relationship
{
    public enum Attitude
    {
        Like,
        Moderate,
        Refuse
    }

    public Relationship(Citizen other) : this()
    {
        this.other = other;
        otherlikability2me = other.m_charming;
        myattitude2other = Attitude.Moderate;
        lastattitude = Attitude.Moderate;
        like_timer = new Timer(10, 0);
    }

    Citizen other;

    [Range(0, 1)]
    public float otherlikability2me;
    public Attitude myattitude2other;
    public Attitude lastattitude;
    public Timer like_timer;
    public bool Rethink()
    {
        lastattitude = myattitude2other;
        float attitude_value = Random.Range(0f, 1f);
        if (1 >= attitude_value
            && attitude_value > 1 - otherlikability2me / 3)
        {
            myattitude2other = Attitude.Like;
            like_timer.lasttime = Time.time;
            return true;
        }
        else if (1 - otherlikability2me / 3 >= attitude_value
            && attitude_value > (1 - otherlikability2me) / 2)
        {
            myattitude2other = Attitude.Moderate;
        }
        else if ((1 - otherlikability2me) / 2 >= attitude_value
            && attitude_value >= 0)
        {
            myattitude2other = Attitude.Refuse;
        }
        return false;
    }

}
