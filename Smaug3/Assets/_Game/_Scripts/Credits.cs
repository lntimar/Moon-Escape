using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private Animator finalText;

    public void ShowFinalText()
    {
        finalText.enabled = true;
    }
}
