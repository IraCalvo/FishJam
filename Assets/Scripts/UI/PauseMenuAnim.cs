using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuAnim : MonoBehaviour
{
    private Animator anim;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger("PlayAnim");
        Debug.Log("Called");
    }
}
