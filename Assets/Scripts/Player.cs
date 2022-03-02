// using System;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool hasKey = false;
    public float moveSpeed = 4;
    public float rotateSpeed = 0;
    //public float stopSpeed = 20;

    private Animator anim;

    void Awake() {
        anim = this.GetComponent<Animator>();
    }

    void Update(){
        if(Input.GetKey(KeyCode.LeftShift)){
            anim.SetBool("Sneak", true);
        }else{
            anim.SetBool("Sneak", false);
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if(Mathf.Abs(h) >  0.1 || Mathf.Abs(v) > 0.1){
            float newSpeed = Mathf.Lerp(anim.GetFloat("Speed"), 5.667774f, moveSpeed*Time.deltaTime);
            //Debug.Log(newSpeed);
            anim.SetFloat("Speed", newSpeed);

            Vector3 targetDir = new Vector3(h,0,v);

            Quaternion newRotation = Quaternion.LookRotation(targetDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotateSpeed*Time.deltaTime);
            // Vector3 nowDir = transform.forward;
            // float angle = Vector3.Angle(nowDir, targetDir);
            // if(angle > 180){
            //     angle = 360 - angle;
            // }
            // transform.Rotate(Vector3.up*angle*Time.deltaTime*rotateSpeed);
        }else{
            anim.SetFloat("Speed", 0);
        }

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")){
            PlayFootMusic();
        }else{
            StopFootMusic();
        }
    }

    private void PlayFootMusic(){
        if(!GetComponent<AudioSource>().isPlaying){
            GetComponent<AudioSource>().Play();
        }
    }
    private void StopFootMusic(){
        if(GetComponent<AudioSource>().isPlaying){
            GetComponent<AudioSource>().Stop();
        }
    }
}
