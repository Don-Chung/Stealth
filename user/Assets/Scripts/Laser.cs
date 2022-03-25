using UnityEngine;

public class Laser : MonoBehaviour
{
    public bool isFlicker = false;

    public float onTime = 3;
    public float offTime = 3;
    private float timer = 0;

    void Update() {
        if (isFlicker)
        {
            timer += Time.deltaTime;
            if(GetComponent<Renderer>().enabled){
            if(timer >= onTime){
                GetComponent<Renderer>().enabled = false;
                GetComponent<Collider>().enabled = false;
                timer = 0;
            }
            }

            if(!GetComponent<Renderer>().enabled){
            if(timer >= offTime){
                GetComponent<Renderer>().enabled = true;
                GetComponent<Collider>().enabled = true;
                timer = 0;
            }
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        if(other.tag == Tags.player){
            GameController._instance.SeePlayer(other.transform);
        }
    }
}
