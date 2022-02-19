using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject HowTo;
    [SerializeField] GameObject Settings;

    public void Play(){
        SceneManager.LoadScene(1);
    }

    public void ActivateHowTo(){
        MainMenu.SetActive(false);
        HowTo.SetActive(true);
    }

    public void ActivateMainMenu(){
        MainMenu.SetActive(true);
        HowTo.SetActive(false);
        Settings.SetActive(false);
    }

    public void ActivateSettings(){
        MainMenu.SetActive(false);
        Settings.SetActive(true);
    }
}
