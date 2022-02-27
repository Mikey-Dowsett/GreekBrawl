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
        selected = FindTarget(type);
        FoundSelected(selected);
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

    StatHolder FindTarget(string type){
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Hero");
        StatHolder allySelected = allies[0].GetComponent<StatHolder>();
        StatHolder enemySelected = enemies[0].GetComponent<StatHolder>();

        if(type == "Concentrated"){
            foreach(GameObject ally in enemies){
                if(ally.GetComponent<StatHolder>().health < enemySelected.health){
                    enemySelected = ally.GetComponent<StatHolder>();
                }
            }
            return enemySelected;
        } else if(type == "Smoke"){
            foreach(GameObject ally in enemies){
                if(ally.GetComponent<StatHolder>().accuracy > enemySelected.accuracy){
                    enemySelected = ally.GetComponent<StatHolder>();
                }
            }
            return enemySelected;
        } else if(type == "Highten"){
                foreach(GameObject ally in allies){
                if(ally.GetComponent<StatHolder>().accuracy < allySelected.accuracy){
                    allySelected = ally.GetComponent<StatHolder>();
                }
            }
            return allySelected;
        } else if(type == "Heal"){
            foreach(GameObject ally in allies){
                if(ally.GetComponent<StatHolder>().health < allySelected.health){
                    print(ally.GetComponent<StatHolder>().stats.name);
                    allySelected = ally.GetComponent<StatHolder>();
                }
            }
            return allySelected;
        } else {
            return enemies[Random.Range(0, enemies.Length)].GetComponent<StatHolder>();
        }
    }

    public void Concentrated(StatHolder selected, float baseDamage){
        float damage = baseDamage + Random.Range(5, 15);
        selected.Damaged((int)damage);
        current.charge -= 100;
    }

    public void Wave(float baseDamage){
        GameObject[] heros = GameObject.FindGameObjectsWithTag("Hero");

        foreach(GameObject hero in heros){
            float damage = baseDamage + Random.Range(0, 5);
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
        int change = (int)Random.Range(15, 30);
        selected.health = Mathf.Clamp(selected.health + change, 0, 100);
        SetBuff(selected.gameObject, "+" + change.ToString());
        current.charge -= 100;
    }

    void SetBuff(GameObject target, string message){
        var temp = Instantiate(BuffText, target.transform.position, Quaternion.identity);
        temp.GetComponent<TMP_Text>().text = message;
        temp.transform.SetParent(target.transform); 
    }

    void FoundSelected(StatHolder selected){
        AttackMarker.SetParent(selected.transform);
        AttackMarker.position = selected.transform.position;
        bottomImage.GetComponent<SpriteRenderer>().sprite = selected.GetComponent<StatHolder>().stats.body;
        bottomImage.SetActive(true);
    }
}
