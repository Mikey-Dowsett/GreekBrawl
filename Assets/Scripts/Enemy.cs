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

    float attack;

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
        attack = teamStats[num].attack;
        yield return new WaitForSeconds(1f);
        StatHolder weakest = GetEnemy(num, stats);
        //print(weakest);
        attackTarget.position = weakest.gameObject.transform.position;
        attackTarget.parent = weakest.gameObject.transform;
        bottomImage.GetComponent<SpriteRenderer>().sprite = weakest.stats.body;
        bottomImage.SetActive(true);
        yield return new WaitForSeconds(1f);
        if(Random.Range(0, 100) <= teamStats[num].accuracy){
            teamStats[num].charge += 20;
            weakest.Damaged(attack);
            //Instantiate(teamStats[num].stats.attackPart, bottomImage.transform.position, Quaternion.identity);
            //Instantiate(teamStats[num].stats.magicPart, topImage.transform.position, Quaternion.identity);
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
            print("Special");
            yield return new WaitForSeconds(2.5f);
            turns.nextTurn = true;
            StopCoroutine("FindTarget");
        }else{
            print("else");
            turns.nextTurn = true;
            StopCoroutine("FindTarget");
        }
        
    }

    private IEnumerator Stunned(int num){
        print("Enemy Stunned");
        Instantiate(StunText, teamStats[num].transform.position, Quaternion.identity);
        teamStats[num].stunned = false;
        yield return new WaitForSeconds(1f);
        turns.nextTurn = true;
        StopCoroutine("Stunned");
    }

    StatHolder GetEnemy(int num, List<StatHolder> stats){
        string special = teamStats[num].stats.specials.ToString();
        switch(special){
            case "Concentrated": return FindSpecial("Wave", stats);
            case "Wave": return FindSpecial("Concentrated", stats);
            case "Highten": return FindSpecial("Smoke", stats);
            case "Smoke": return FindSpecial("Highten", stats);
            case "Heal": return FindSpecial("Stun", stats);
            case "Stun": return FindSpecial("Heal", stats);
        }
        return null;
    }

    StatHolder FindSpecial(string target, List<StatHolder> stats){
        foreach(StatHolder stat in stats){
            if(stat.stats.specials.ToString() == target){
                attack += Random.Range(5, 15);
                return stat;
            }
        }
        return stats[Random.Range(0, stats.Count)];
    }
}
