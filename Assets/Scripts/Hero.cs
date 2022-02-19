using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hero : MonoBehaviour
{
    [SerializeField] Button attackButton;
    [SerializeField] Button specialButton;
    [SerializeField] Transform EnemyMarker;
    [SerializeField] HeroSpecials specialMove;
    [SerializeField] Turns turns;
    [SerializeField] TMP_Text specialText;

    [SerializeField] GameObject MissText;
    [SerializeField] GameObject StunText;
    [SerializeField] GameObject bottomImage;
    [SerializeField] GameObject topImage;

    int damage;
    int health;
    float energy;
    float accuracy;

    GameObject choosenEnemy;
    
    public StatHolder currentStats;
    public bool CanSelect;
    bool canSpecial;
    public bool hasAttacked;

    void Start(){
        damage = Random.Range(2, 4);
        health = Random.Range(15, 20);
        energy = Random.Range(30, 40);
        accuracy = Random.Range(40, 100);
    }

    void Update(){
        if(currentStats != null) {
            specialText.text = currentStats.stats.specialName;
            canSpecial = currentStats.charge >= 100;
        }
        if(Input.GetMouseButtonDown(0) && CanSelect){
            CastRay();
        }
        specialButton.interactable = canSpecial;

        if(hasAttacked && !canSpecial){
            StartCoroutine("SetNextTurn");
        }
    }

    void CastRay(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100)){
            if(hit.collider.gameObject.transform.childCount > 0 && hit.collider.gameObject.transform.GetChild(0).name == "Turn Marker"){
                //Don't Let move
            } else if(choosenEnemy == hit.collider.gameObject){
                choosenEnemy = null;
                EnemyMarker.parent = null;
                EnemyMarker.position = new Vector3(-11, 0, 0);
                bottomImage.SetActive(false);
                
            } else {
                choosenEnemy = hit.collider.gameObject;
                EnemyMarker.parent = choosenEnemy.transform;
                EnemyMarker.position = choosenEnemy.transform.position;
                bottomImage.GetComponent<SpriteRenderer>().sprite = choosenEnemy.GetComponent<StatHolder>().stats.body;
                bottomImage.SetActive(true);
            }
        }
    }

    public void OnAttack(){
        if(choosenEnemy != null && !choosenEnemy.CompareTag("Hero")){
            if(Random.Range(0, 100) <= currentStats.accuracy){
                float attack = currentStats.attack;
                if(CheckForCrit(currentStats.stats.specials.ToString(), choosenEnemy.GetComponent<StatHolder>().stats.specials.ToString())){
                    attack += Random.Range(5, 15);
                }
                choosenEnemy.GetComponent<StatHolder>().Damaged(attack);
                turns.GetHero().GetComponent<StatHolder>().charge += 20;
                Instantiate(currentStats.stats.attackPart, bottomImage.transform.position, Quaternion.identity);
                Instantiate(currentStats.stats.magicPart, topImage.transform.position, Quaternion.identity);
            } else{
                turns.GetHero().GetComponent<StatHolder>().charge += 10;
                var temp = Instantiate(MissText, choosenEnemy.transform.position, Quaternion.identity);
                temp.transform.SetParent(choosenEnemy.transform);
            }
            attackButton.interactable = false;
            hasAttacked = true;
            StartCoroutine("CheckForDead");
        }
    }

    public void NextTurn(){
        if(currentStats.stunned){
            StartCoroutine("Stunned");
        } else{
            CanSelect = true;
            EnableButtons(true);
        }
    }

    public void OnSpecial(){
        string moves = currentStats.stats.specials.ToString();
        if(moves  == "Wave"){
            specialMove.Wave(currentStats.attack);
        } else if(choosenEnemy != null && choosenEnemy.CompareTag("Enemy")) {
            switch (moves){
                case "Concentrated": specialMove.Concentrated(choosenEnemy, currentStats.attack); break;
                case "Smoke": specialMove.LowerAccuracy(choosenEnemy); break;
                case "Stun": specialMove.StunEffect(choosenEnemy); break;
            }
        } else{
            if(choosenEnemy == null) {
                choosenEnemy = turns.GetHero(); 
                specialMove.isCurrentHero = true;
            } else{
            }
            switch (moves){
                case "Highten": specialMove.RaiseAccuracy(choosenEnemy); break;
                case "Heal": specialMove.Heal(choosenEnemy); break;
            }
        }
        //choosenEnemy = null;
        //EnemyMarker.position = new Vector3(11, 0, 0);
        //EnemyMarker.parent = null;
        canSpecial = false;
    }

    public void ResetSpecial(){
        attackButton.interactable = true;
    }

    public void EnableButtons(bool enabled){
        attackButton.interactable = enabled;
        specialButton.interactable = enabled;
        //canSpecial = enabled;
    }

    private IEnumerator CheckForDead(){
        yield return new WaitForSeconds(0.4f);
        turns.CheckForEnemies();
    }

    private IEnumerator Stunned(){
        Instantiate(StunText, currentStats.transform.position, Quaternion.identity);
        currentStats.stunned = false;
        yield return new WaitForSeconds(1f);
        turns.nextTurn = true;
        StopCoroutine("Stunned");
    }

    private IEnumerator SetNextTurn(){
        yield return new WaitForSeconds(1f);
        turns.nextTurn = true;
        StopCoroutine("SetNextTurn");
    }

    bool CheckForCrit(string hero, string enemy){
        print(hero + " | " + enemy);
        if(hero == "Concentrated" && enemy == "Wave"){
            print("Crit");
            return true;
        } else if(hero == "Wave" && enemy == "Concentrated"){
            print("Crit");
            return true;
        } else if(hero == "Smoke" && enemy == "Highten"){
            print("Crit");
            return true;
        } else if(hero == "Highten" && enemy == "Smoke"){
            print("Crit");
            return true;
        } else if(hero == "Stun" && enemy == "Heal"){
            print("Crit");
            return true;
        } else if(hero == "Heal" && enemy == "Stun"){
            print("Crit");
            return true;
        }
        return false;
    }
}
