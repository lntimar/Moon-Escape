using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SFX", menuName = "Scriptable Objects/SFX")]
public class SFX : ScriptableObject
{
    public AudioClip Clip;
    [Range(0f, 1f)] public float Volume;
}
