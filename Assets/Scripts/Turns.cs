using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Turns : MonoBehaviour
{
    [SerializeField] bool playerTurn;
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;
    [SerializeField] Transform turnMarker;
    [SerializeField] Transform attackMarker;
    [SerializeField] Hero heroScript;
    [SerializeField] Enemy enemyScript;
    
    [SerializeField] Button attackButton;
    [SerializeField] Button specialButton;
    [SerializeField] Button nextTurnButton;

    [SerializeField] GameObject[] characters = new GameObject[4];
    [SerializeField] GameObject[] enemies = new GameObject[4];
    [SerializeField] Animator scoreAnim;
    [SerializeField] SpriteRenderer topImage;
    [SerializeField] GameObject bottomImage;

    public bool nextTurn = false;
    int turnNum = 0;

    void Start(){
        StartCoroutine("WaitToStart");
    }

    void Update(){
        if(nextTurn){
            //print("Next Turn");
            bottomImage.SetActive(false);
            heroScript.currentStats = null;
            if(turnNum == 4) {
                turnNum = 0;
                if(playerTurn){
                    playerTurn = false;
                    heroScript.enabled = false;
                    enemyScript.enabled = true;
                } else {
                    playerTurn = true;
                    heroScript.enabled = true;
                    enemyScript.enabled = false;
                }
            }
            nextTurnButton.interactable = false;
            attackMarker.parent = null;
            nextTurn = false;
            attackMarker.position = new Vector3(11, 0, 0);
            if(playerTurn && CheckForEnemies()){
                nextTurnButton.interactable = true;
                if(characters[turnNum] == null){
                    turnNum++;
                    nextTurn = true;
                } else {
                    heroScript.currentStats = characters[turnNum].GetComponent<StatHolder>();
                    heroScript.NextTurn();
                    topImage.sprite = characters[turnNum].GetComponent<StatHolder>().stats.body;
                    heroScript.hasAttacked = false;
                    turnMarker.parent = characters[turnNum].transform;
                    turnMarker.position = characters[turnNum].transform.position;
                    turnNum++;
                }
            } else if(CheckForHeros()){
                heroScript.CanSelect = false;
                if(enemies[turnNum] == null){
                    turnNum++;
                    nextTurn = true;
                } else {
                    topImage.sprite = enemies[turnNum].GetComponent<StatHolder>().stats.body;
                    heroScript.EnableButtons(false);
                    attackButton.interactable = false;
                    turnMarker.parent = enemies[turnNum].transform;
                    turnMarker.position = enemies[turnNum].transform.position;
                    enemyScript.NextTurn(turnNum);
                    turnNum++;
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene(0);
        }
    }

    public void OnClick(){
        nextTurn = true;
    }

    public bool CheckForHeros(){
        bool herosLeft = false;
        foreach(GameObject hero in characters){
            if(hero != null) herosLeft = true;
        }

        if(!herosLeft){
            scoreAnim.SetBool("Defeat", true);
            print("Defeat");
            enemyScript.enabled = false;
            heroScript.enabled = false;
            nextTurnButton.interactable = false;
            GameObject.Destroy(this);
        }

        return herosLeft;
    }

    public bool CheckForEnemies(){
        bool herosLeft = false;
        foreach(GameObject hero in enemies){
            if(hero != null && hero.tag != "Lol") herosLeft = true;
        }

        if(!herosLeft){
            scoreAnim.SetBool("Victory", true);
            print("Victory");
            enemyScript.enabled = false;
            heroScript.enabled = false;
            nextTurnButton.interactable = false;
            GameObject.Destroy(this);
        }

        return herosLeft;
    }

    public GameObject GetHero(){
        return characters[turnNum-1];
    }

    private IEnumerator WaitToStart(){
        yield return new WaitForSeconds(0.1f);
        nextTurn = true;
    }
}
