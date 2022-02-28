using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayClassStats : MonoBehaviour
{
    [SerializeField] string title;
    [SerializeField] string description;
    [SerializeField] Sprite self;
    [SerializeField] Sprite strong;
    [SerializeField] Sprite[] heros;
    [SerializeField] Sprite [] monsters;

    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] Image selfImage;
    [SerializeField] Image strongImage;
    [SerializeField] Image[] heroImages;
    [SerializeField] Image[] monsterImages;

    [SerializeField] Transform selected;
    [SerializeField] bool showOnStart;

    void Start(){
        if(showOnStart){
            titleText.text = title;
            descriptionText.text = description;
            heroImages[0].sprite = heros[0];
            heroImages[1].sprite = heros[1]; 
            monsterImages[0].sprite = monsters[0];
            monsterImages[1].sprite = monsters[1];
            strongImage.sprite = strong;
            selfImage.sprite = self;
            selected.position = transform.position;
        }
    }

    public void OnClick(){
        titleText.text = title;
        descriptionText.text = description;
        heroImages[0].sprite = heros[0];
        heroImages[1].sprite = heros[1]; 
        monsterImages[0].sprite = monsters[0];
        monsterImages[1].sprite = monsters[1];
        strongImage.sprite = strong;
        selfImage.sprite = self;
        selected.position = transform.position;
    }
}
