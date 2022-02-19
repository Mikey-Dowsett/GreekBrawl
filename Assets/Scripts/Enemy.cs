using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] StatHolder[] teamStats;
    [SerializeField] Transform attackTarget;
    [SerializeField] Turns turns;
    [SerializeField] EnemySpecial special;

    [SerializeField] GameObject MissText;
    [SerializeField] GameObject HitText;
    [SerializeField] GameObject StunText;
    [SerializeField] GameObject bottomImage;
    [SerializeField] GameObject topImage;

    public void NextTurn(int num){
        List<StatHolder> stats = new List<StatHolder>();
        GameObject[] heros = GameObject.FindGameObjectsWithTag("Hero");
        foreach(GameObject hero in heros){
            stats.Add(hero.GetComponent<StatHolder>());
        }
        if(!teamStats[num].stunned) {
            StartCoroutine(FindTarget(num, stats));
        } else{
            StartCoroutine(Stunned(num));
        }
    }

    private IEnumerator FindTarget(int num, List<StatHolder> stats){
        yield return new WaitForSeconds(1f);
        StatHolder weakest = stats[Random.Range(0, stats.Count)].GetComponent<StatHolder>();
        attackTarget.position = weakest.gameObject.transform.position;
        attackTarget.parent = weakest.gameObject.transform;
        bottomImage.GetComponent<SpriteRenderer>().sprite = weakest.stats.body;
        bottomImage.SetActive(true);
        yield return new WaitForSeconds(1f);
        if(Random.Range(0, 100) <= teamStats[num].accuracy){
            teamStats[num].charge += 20;
            weakest.Damaged(teamStats[num].attack);
            Instantiate(teamStats[num].stats.attackPart, bottomImage.transform.position, Quaternion.identity);
            Instantiate(teamStats[num].stats.magicPart, topImage.transform.position, Quaternion.identity);
        } else {
            teamStats[num].charge += 10;
            var temp = Instantiate(MissText, weakest.transform.position, Quaternion.identity);
            temp.transform.SetParent(weakest.transform);
            temp.transform.position = weakest.transform.position;
        }
        attackTarget.parent = null;
        attackTarget.position = new Vector3(-11, 0, 0);
        yield return new WaitForSeconds(0.5f);
        bottomImage.SetActive(false);
        string moves = teamStats[num].stats.specials.ToString();
        yield return new WaitForSeconds(0.5f);
        if(teamStats[num].charge >= 100){
            switch(moves){
                case "Concentrated": special.StartCoroutine(special.Special(false, teamStats[num], "Concentrated")); break;
                case "Wave": special.StartCoroutine(special.Special(false, teamStats[num], "Wave")); break;
                case "Smoke": special.StartCoroutine(special.Special(false, teamStats[num], "Smoke")); break;
                case "Highten": special.StartCoroutine(special.Special(true, teamStats[num], "Highten")); break;
                case "Stun": special.StartCoroutine(special.Special(false, teamStats[num], "Stun")); break;
                case "Heal": special.StartCoroutine(special.Special(true, teamStats[num], "Heal")); break;
            }
            yield return new WaitForSeconds(2.5f);
            turns.nextTurn = true;
            StopCoroutine("FindTarget");
        }else{
            turns.nextTurn = true;
            StopCoroutine("FindTarget");
        }
        
    }

    private IEnumerator Stunned(int num){
        Instantiate(StunText, teamStats[num].transform.position, Quaternion.identity);
        teamStats[num].stunned = false;
        yield return new WaitForSeconds(1f);
        turns.nextTurn = true;
        StopCoroutine("Stunned");
    }
}
