using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private Animator _anim;
    private static readonly int Revive1 = Animator.StringToHash("Revive");

    public const string PUSH_BUTTON_ANIMATION = "ButtonPush";

    void Awake()
    {
        BakeReferences();
    }

    public void SubscribeToEvents(PlayerModel model)
    {
        model.LifeController.OnDead += Death;
        model.OnMove += SetWalkAnimation;
        model.LifeController.onRevive += (float n)=>Revive();
    }
    public void BakeReferences ()
    { 
        _anim = GetComponent<Animator>(); 
    }

    private void SetWalkAnimation(float velocity)
    {
        _anim.SetFloat(PlayerAnimationConstants.PLAYER_VELOCITY_ANIMATION_HASH, velocity);
    }

    private void Revive()
    {
        _anim.SetTrigger(Revive1);
    }
    public void RingBell()
    {
        _anim.Play(PlayerAnimationConstants.PLAYER_PUNCH_BELL_ANIMATION_HASH);
    }
    public void CallHerd()
    {
        _anim.Play(PlayerAnimationConstants.PLAYER_ATTRACT_HERD_ANIMATION_HASH);
    }

    private void Death()
    {
        _anim.Play(PlayerAnimationConstants.PLAYER_DEATH_ANIMATION_HASH);
    }
    /*
    public void PushButton()
    {
        _anim.Play(PlayerAnimationConstants.PLAYER_BUTTON_PUSH_ANIMATION_HASH);
    }*/
}
