using UnityEngine;

public class PlayerBaseClass : IState
{
    protected PlayerController player;

    public PlayerBaseClass(PlayerController player)
    {
        this.player = player;
    }
    public virtual void OnEnter()
    {
        
    }
    public virtual void OnExit()
    {
        
    }
    public virtual void OnFixedUpdate()
    {
        
    }
    public virtual void OnUpdate()
    {
        
    }
}
