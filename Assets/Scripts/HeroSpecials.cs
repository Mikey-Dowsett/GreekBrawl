using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeroSpecials : MonoBehaviour
{
    [SerializeField] GameObject BuffText;
    [SerializeField] Turns turns;

    public bool isCurrentHero = false;

    public void Concentrated(GameObject choosenEnemy, float baseAttack){
        float damage = baseAttack * Random.Range(1.5f, 2.5f);
        choosenEnemy.GetComponent<StatHolder>().Damaged((int)damage);
        turns.GetHero().GetComponent<StatHolder>().charge -= 100;
    }

    public void Wave(float baseAttack){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject baddy in enemies){
            float damage = baseAttack * Random.Range(1.5f, 2);
            baddy.GetComponent<StatHolder>().Damaged((int)damage);
        }
        turns.GetHero().GetComponent<StatHolder>().charge -= 100;
    }

    public void LowerAccuracy(GameObject choosenEnemy){
        int change = (int)Random.Range(5, 10);
        SetBuff(choosenEnemy, "-" + change.ToString());
        choosenEnemy.GetComponent<StatHolder>().accuracy = Mathf.Clamp(choosenEnemy.GetComponent<StatHolder>().accuracy - change, 0, 100);
        turns.GetHero().GetComponent<StatHolder>().charge -= 100;
    }

    public void RaiseAccuracy(GameObject choosenHero){
        int change = (int)Random.Range(5, 10);
        SetBuff(choosenHero, "+" + change.ToString());
        choosenHero.GetComponent<StatHolder>().accuracy = Mathf.Clamp(choosenHero.GetComponent<StatHolder>().accuracy + change, 0, 100);
        turns.GetHero().GetComponent<StatHolder>().charge -= 100;
        isCurrentHero = false;
    }

    public void StunEffect(GameObject choosenEnemy){
        SetBuff(choosenEnemy, "Stun");
        choosenEnemy.GetComponent<StatHolder>().stunned = true;
        turns.GetHero().GetComponent<StatHolder>().charge -= 100;
    }

    public void Heal(GameObject choosenHero){
        float change = (int)Random.Range(10, 20);
        SetBuff(choosenHero, "+" + change.ToString());
        choosenHero.GetComponent<StatHolder>().health = Mathf.Clamp(choosenHero.GetComponent<StatHolder>().health + change, 0, 100);
        turns.GetHero().GetComponent<StatHolder>().charge -= 100;
        isCurrentHero = false;
    }

    void SetBuff(GameObject target, string message){
        var temp = Instantiate(BuffText, target.transform.position, Quaternion.identity);
        temp.GetComponent<TMP_Text>().text = message;
        temp.transform.SetParent(target.transform); 
    }
}
