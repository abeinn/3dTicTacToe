using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void playGame()
    {
        if(!Options.flat){
            SceneManager.LoadScene("3DGame");
        } else {
            if(Options.horizontal){
                SceneManager.LoadScene("HorizontalGame");
            }
            else {
                SceneManager.LoadScene("VerticalGame");
            }
         }    
    }

    public void goToOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
