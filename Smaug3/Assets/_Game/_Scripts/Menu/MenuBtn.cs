using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBtn : MonoBehaviour
{

    [SerializeField] public string menuName;

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuName);
    }

}
