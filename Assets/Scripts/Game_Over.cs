using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Over: MonoBehaviour
{
    public void PlayAgain () {
        if (GlobalVars.level == 1){
            SceneManager.LoadScene("ForestLevel");
        } else if (GlobalVars.level == 2){
            SceneManager.LoadScene("CityLevel");
        }
    }
}
