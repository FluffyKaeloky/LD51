using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class TimelinePause : MonoBehaviour
{
    private PlayableDirector director = null;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    public void Pause()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(0.0f);
    }

    public void Play()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(1.0f);
    }
}
