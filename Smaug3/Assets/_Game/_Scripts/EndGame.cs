using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private string creditsScene;
    [SerializeField] private float waitTime;

    // References
    private FadeVFX _fadeIn;

    private void Start()
    {
        _fadeIn = GameObject.FindGameObjectWithTag("Fade In").GetComponent<FadeVFX>();
    }

    public void Activate()
    {
        StartCoroutine(LoadInterval(waitTime));
    }

    private IEnumerator LoadInterval(float t)
    {
        yield return new WaitForSeconds(t);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().speed = 0f;
        _fadeIn.enabled = true;
        SceneManager.LoadScene(creditsScene);
    }
}
