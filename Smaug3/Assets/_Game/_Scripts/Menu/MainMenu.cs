using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string firstScene;
    [SerializeField] private string musicMenu;
    private AudioManager _audioManager;
    
    public void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        _audioManager.PlayMusic(musicMenu);
    }

    public void StartGame ()      
   {    

        // Outro jeito de fazer � com este c�digo, que vai pegar o index do
        // menu (o primeiro) e ir para o pr�ximo (qnd o jogo come�a)
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PauseMenu.menuName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(firstScene);
   }

    public void QuitGame()
    {
        Application.Quit();
    }
}
