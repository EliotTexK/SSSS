using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public void startGame() {
        SceneManager.LoadScene("Level");
    }
    
    public void exit() {
        Application.Quit();
    }
}
