using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "Stats Object", order = 1)]
public class Stats : ScriptableObject
{
    public string title;
    public float attack;
    public float health;
    public float energy;
    public float accuracy;
    public Specials specials;
    public string specialName;
    public Sprite icon;
    public Sprite body;
    public Sprite classIcon;
    public string description;
    public ParticleSystem attackPart;
    public ParticleSystem specialPart;
    public ParticleSystem magicPart;

    public enum Specials{
        Concentrated,
        Wave,
        Smoke,
        Highten,
        Stun,
        Heal
    };

    
}
