using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManagerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject audioManager;
    private void Awake()
    {
        var audioManagerCount = GameObject.FindGameObjectsWithTag("AudioManager").Length;
        if (audioManagerCount == 0)
        {
            audioManager.SetActive(true);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
