using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public void Death(){
        GameObject.Destroy(gameObject);
    }
}
