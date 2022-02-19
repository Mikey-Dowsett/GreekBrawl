using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatHolder : MonoBehaviour
{
    [SerializeField] GameObject[] topTexts;
    //[SerializeField] GameObject[] bottomTexts;

    [SerializeField] Animator anim;
    [SerializeField] Transform start;
    [SerializeField] Transform end;
    [SerializeField] Image teamIcon;
    [SerializeField] Image classIcon;

    [SerializeField] GameObject DeadText;
    [SerializeField] GameObject HitText;

    public Stats stats;

    public float health;
    public float attack;
    public float accuracy;
    public float charge;

    public bool stunned;

    void Start(){
        health = stats.health;
        attack = stats.attack;
        accuracy = stats.accuracy;
        charge = stats.energy;

        if(stats.icon != null){
            GetComponent<SpriteRenderer>().sprite = stats.icon;
        }
        if(teamIcon != null){
            teamIcon.sprite = stats.icon;
        }
        anim.ResetTrigger("Hurt");

        topTexts[0].GetComponent<TMP_Text>().text = stats.title;
        topTexts[5].GetComponent<TMP_Text>().text = stats.specialName.ToString();
        classIcon.sprite = stats.classIcon;
    }

    void Update(){
        health = Mathf.Clamp(health, 0, 100);
        accuracy = Mathf.Clamp(accuracy, 0, 100);
        charge = Mathf.Clamp(charge, 0, 100);
        

        topTexts[1].GetComponent<Image>().fillAmount = health/100;
        topTexts[2].GetComponent<Image>().fillAmount = charge/100;
        topTexts[3].GetComponent<TMP_Text>().text = attack.ToString();
        topTexts[4].GetComponent<TMP_Text>().text = accuracy.ToString() + "%";

        if(stunned){
            GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f);
        } else{
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
        
    }

    public void Damaged(float damage){
        charge += 15;
        if(damage > 0) anim.SetTrigger("Hurt");
        health -= damage;
        if(health <= 0){
            transform.tag = "Lol";
            GameObject.Destroy(GameObject.Find("HitText"));
            Instantiate(DeadText, transform.position, Quaternion.identity);
            if(transform.childCount > 0){
                transform.GetChild(0).parent = null;
            }
            anim.SetTrigger("Dead");
        }else{ 
            var temp = Instantiate(HitText, transform.position, Quaternion.identity);
            temp.transform.SetParent(transform);
            temp.GetComponent<TMP_Text>().text = "-" + damage.ToString();
        }
        
    }

    private IEnumerator EndTime(){
        while(transform.position != end.position){
            transform.position = Vector2.MoveTowards(transform.position, end.position, 4f * Time.deltaTime);
            yield return new WaitForSeconds(0.0002f);
        }
        yield return null;
    }
    private IEnumerator StartTime(){
        while(transform.position != start.position){
            transform.position = Vector2.MoveTowards(transform.position, start.position, 4f * Time.deltaTime);
            yield return new WaitForSeconds(0.0002f);
        }
        yield return null;
    }

    public void FinalDeath(){
        GameObject.Destroy(gameObject);
    }
}
