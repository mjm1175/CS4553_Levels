using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Over: MonoBehaviour
{
    public void PlayAgain () {
        PlayerScript.level = 1;
        SceneManager.LoadScene("ForestLevel");
    }
}
