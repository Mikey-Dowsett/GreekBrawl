using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private bool canMoveOn;

    public void Done(){
        canMoveOn = true;
    }

    private void Update(){
        if(canMoveOn && Input.GetMouseButtonDown(0)){
            SceneManager.LoadScene(0);
        }
    }
}
