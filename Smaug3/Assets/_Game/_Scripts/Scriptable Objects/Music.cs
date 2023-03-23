using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Music", menuName = "Scriptable Objects/Music")]
public class Music : ScriptableObject
{
    public AudioClip Clip;
    [Range(0f, 1f)] public float Volume;
}
