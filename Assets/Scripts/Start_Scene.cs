using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_Scene : MonoBehaviour
{
    public void PlayGame () {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //PlayerScript.level = 1;
        GlobalVars.level = 1;
        SceneManager.LoadScene("ForestLevel");
    }

    public void NextLevel(){
        GlobalVars.level = 2;
        SceneManager.LoadScene("CityLevel");
    }
}
