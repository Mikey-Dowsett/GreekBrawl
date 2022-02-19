using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatSetter : MonoBehaviour
{
    [SerializeField] List<Stats> stats;
    [SerializeField] StatHolder[] statHolders;

    bool concentrated;
    bool wave;
    bool raise;
    bool lower;
    bool stun;
    bool heal;

    void Awake(){
        int i = 0;
        while(true){
            int rand = Random.Range(0, stats.Count);
            Stats stat = stats[rand];
            if(stat.specials.ToString() == "Concentrated" && !concentrated){
                statHolders[i].stats = stat;
                stats.Remove(stat);
                concentrated = true;
                i++;
            } else if(stat.specials.ToString() == "Wave" && !wave){
                statHolders[i].stats = stat;
                stats.Remove(stat);
                wave = true;
                i++;
            } else if(stat.specials.ToString() == "Smoke" && !lower){
                statHolders[i].stats = stat;
                stats.Remove(stat);
                lower = true;
                i++;
            } else if(stat.specials.ToString() == "Highten" && !raise){
                statHolders[i].stats = stat;
                stats.Remove(stat);
                raise = true;
                i++;
            } else if(stat.specials.ToString() == "Stun" && !stun){
                statHolders[i].stats = stat;
                stats.Remove(stat);
                stun = true;
                i++;
            } else if(stat.specials.ToString() == "Heal" && !heal){
                statHolders[i].stats = stat;
                stats.Remove(stat);
                heal = true;
                i++;
            } else{
                stats.Remove(stat);
            }
            if(i >= 4){
                break;
            }
            
        }
    }
}
