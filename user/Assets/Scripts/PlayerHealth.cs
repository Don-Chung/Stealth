using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float HP = 100;

    //private TcpTest sendTcp;

    private Animator anim;

    private void Awake() {
        anim = this.GetComponent<Animator>();
        //sendTcp = gameObject.AddComponent<TcpTest>();
        //sendTcp.init();
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
        // Timeend = DateTime.Now.ToUniversalTime().Ticks;
        // editString = Convert.ToString((Timeend - Timestart) / 10000); //+ sum;
        // Debug.Log(editString);
        // tcpClient.SocketSend(editString);
        yield return new WaitForSeconds(2f);
        //sendTcp.quit();
        SceneManager.LoadScene(0);
    }
}
