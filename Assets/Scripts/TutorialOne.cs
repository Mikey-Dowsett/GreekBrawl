using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOne : MonoBehaviour
{
    [SerializeField] GameObject DisplayOne;
    [SerializeField] GameObject DisplayTwo;
    [SerializeField] GameObject DisplayThree;

    [SerializeField] Menu menu;
    
    int num = 1;

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            if(num == 1){
                DisplayTwo.SetActive(true);
                DisplayOne.SetActive(false);
                num++;
            } else if(num == 2){
                DisplayThree.SetActive(true);
                DisplayTwo.SetActive(false);
                num++;
            } else {
                DisplayOne.SetActive(true);
                DisplayThree.SetActive(false);
                num = 1;
                menu.ActivateMainMenu();
            }
        }
    }
}
