using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSpecial : MonoBehaviour
{
    [SerializeField] StatHolder stat;
    [SerializeField] GameObject ReadyText;
    [SerializeField] GameObject Dead;
    Image image;
    bool isReady;

    void Start(){
        image = GetComponent<Image>();
    }

    void Update(){
        if(stat != null){
            if(stat.charge >= 100){
                image.color = new Color(1, 1, 1, 1);
                if(isReady){
                    var temp = Instantiate(ReadyText, transform.position, Quaternion.identity);
                    temp.transform.SetParent(transform);
                    isReady = false;
                }
            } else {
                image.color = new Color(1, 1, 1, 0.5f);
                isReady = true;
            }
        } else{
            Dead.SetActive(true);
            image.color = new Color(1, 1, 1, 0.5f);
        }
    }
}
