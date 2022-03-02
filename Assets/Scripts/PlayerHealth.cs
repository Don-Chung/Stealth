using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float HP = 100;

    private Animator anim;

    private void Awake() {
        anim = this.GetComponent<Animator>();
    }

    public void TakeDamage(float damage){
        HP -= damage;
        if (HP <= 0)
        {
            anim.SetBool("Dead", true);
            StartCoroutine(ReloadScene());
        }
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(0);
    }
}
