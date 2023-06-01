using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    [SerializeField] private NPC npc;

    public void AnimationEvent()
    {
        npc.ReceiveAnimationEvent();
    }


}
