using UnityEngine;

public class Door : MonoBehaviour
{
    public bool requireKey = false;
    public AudioSource musicDenied;
    private int count = 0;
    private Animator anim;

    void Awake()
    {
        anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetBool("Close", count <= 0);
        if(anim.IsInTransition(0)){
            if(!GetComponent<AudioSource>().isPlaying){
                GetComponent<AudioSource>().Play();
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(requireKey){
            if(other.tag == Tags.player){
                Player player = other.GetComponent<Player>();
                if(player.hasKey){
                    count++;
                }else{
                    musicDenied.Play();
                }
            }
        }else{
            if(other.tag==Tags.player){
                count++;
            }else if (other.tag==Tags.enemy && other.GetComponent<Collider>().isTrigger == false)
            {
                count++;
            }
        }

    }
    void OnTriggerExit(Collider other) {
        if(requireKey){
            if(other.tag == Tags.player){
                Player player = other.GetComponent<Player>();
                if(player.hasKey){
                    count--;
                }
            }
        }else{
           if(other.tag==Tags.player){
                count--;
            }else if (other.tag==Tags.enemy && other.GetComponent<Collider>().isTrigger == false)
            {
                count--;
            }
        }
    }
}
