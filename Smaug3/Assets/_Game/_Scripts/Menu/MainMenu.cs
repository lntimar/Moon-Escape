using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void StartGame ()
   {    
        // Outro jeito de fazer � com este c�digo, que vai pegar o index do
        // menu (o primeiro) e ir para o pr�ximo (qnd o jogo come�a)
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("Armazem");
   }

    public void QuitGame()
    {
        Application.Quit();
    }
}
