using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpecial : MonoBehaviour
{
    [SerializeField] GameObject BuffText;
    [SerializeField] Transform AttackMarker;
    [SerializeField] GameObject bottomImage;
    StatHolder current;

    public IEnumerator Special(bool ally, StatHolder _current, string type){
        current = _current;
        StatHolder selected;
        if(ally){
            GameObject[] allies = GameObject.FindGameObjectsWithTag("Enemy");
            selected = allies[Random.Range(0, allies.Length)].GetComponent<StatHolder>();
        } else{
            GameObject[] allies = GameObject.FindGameObjectsWithTag("Hero");
            selected = allies[Random.Range(0, allies.Length)].GetComponent<StatHolder>();
        }
        AttackMarker.SetParent(selected.transform);
        AttackMarker.position = selected.transform.position;
        bottomImage.GetComponent<SpriteRenderer>().sprite = selected.GetComponent<StatHolder>().stats.body;
        bottomImage.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        
        
        switch(type){
            case "Concentrated": Concentrated(selected, current.attack); break;
            case "Wave": Wave(current.attack);  break;
            case "Smoke": LowerAccuracy(selected); break;
            case "Highten": RaiseAccuracy(selected); break;
            case "Stun": StunEffect(selected); break;
            case "Heal": Heal(selected); break;
        }

    }

    public void Concentrated(StatHolder selected, float baseDamage){
        float damage = baseDamage * Random.Range(1.5f, 2.5f);
        selected.Damaged((int)damage);
        current.charge -= 100;
    }

    public void Wave(float baseDamage){
        GameObject[] heros = GameObject.FindGameObjectsWithTag("Hero");

        foreach(GameObject hero in heros){
            float damage = baseDamage * Random.Range(1.5f, 2);
            hero.GetComponent<StatHolder>().Damaged((int)damage);
        }
        current.charge -= 100;
    }

    public void LowerAccuracy(StatHolder selected){
        int change = (int)Random.Range(5, 10);
        selected.accuracy = Mathf.Clamp(selected.accuracy - change, 0, 100);
        SetBuff(selected.gameObject, "-" + change.ToString());
        current.charge -= 100;
    }

    public void RaiseAccuracy(StatHolder selected){
        int change = (int)Random.Range(5, 10);
        selected.accuracy = Mathf.Clamp(selected.accuracy + change, 0, 100);
        SetBuff(selected.gameObject, "+" + change.ToString());
        current.charge -= 100;
    }

    public void StunEffect(StatHolder selected){
        selected.stunned = true;
        SetBuff(selected.gameObject, "Stun");
        current.charge -= 100;
    }

    public void Heal(StatHolder selected){
        int change = (int)Random.Range(10, 20);
        selected.health = Mathf.Clamp(selected.health + change, 0, 100);
        SetBuff(selected.gameObject, "+" + change.ToString());
        current.charge -= 100;
    }

    void SetBuff(GameObject target, string message){
        var temp = Instantiate(BuffText, target.transform.position, Quaternion.identity);
        temp.GetComponent<TMP_Text>().text = message;
        temp.transform.SetParent(target.transform); 
    }
}
