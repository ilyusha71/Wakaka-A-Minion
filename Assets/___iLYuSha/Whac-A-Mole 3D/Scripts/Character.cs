using UnityEngine;

public delegate void HitEventHandler<TEventArgs>(object sender, TEventArgs e) where TEventArgs : System.EventArgs;
public class HItEventArgs : System.EventArgs
{
    public readonly int index;
    public readonly CharacterIndex character;
    public HItEventArgs(int index, CharacterIndex character)
    {
        this.index = index;
        this.character = character;
    }
}

public class Character : MonoBehaviour
{
    public event HitEventHandler<HItEventArgs> HitEvent;
    private HItEventArgs hitArgs;
    public void Initialize(HitEventHandler<HItEventArgs> hitEventHandler, HItEventArgs hitEventArgs)
    {
        HitEvent = hitEventHandler;
        hitArgs = hitEventArgs;
    }
    void OnMouseDown()
    {
        Boom();
    }
    public void Boom()
    {
        OnHit(hitArgs);
    }
    protected virtual void OnHit(HItEventArgs e)
    {
        if (HitEvent != null)
        {
            HitEvent(this, e);
        }
    }
}
