using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Logo : MonoBehaviour
{
    [SerializeField] private string nextScene;

    // Components
    private VideoPlayer _videoPlayer;

    private float _curTime = 0;

    private void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
    }

    private void Update()
    {
        _curTime += Time.deltaTime;
        if (_curTime >= 7.57f) SceneManager.LoadScene(nextScene);
    }
}
