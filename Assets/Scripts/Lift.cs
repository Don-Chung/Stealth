using UnityEngine;
using UnityEngine.SceneManagement;

public class Lift : MonoBehaviour
{
    public Transform outer_left;
    public Transform inner_left;
    public Transform outer_right;
    public Transform inner_right;
    public float inner_door_moveSpeed = 3;
    public float liftUpTime = 3f;
    private float liftUpTimer = 0;
    private bool isIn;
    private float gameWinTimer  = 0;

    void Update()
    {
        float inner_left_x = Mathf.Lerp(inner_left.position.x, outer_left.position.x, Time.deltaTime*inner_door_moveSpeed);
        float inner_right_x = Mathf.Lerp(inner_right.position.x, outer_right.position.x, Time.deltaTime*inner_door_moveSpeed);
        inner_left.position = new Vector3(inner_left_x, inner_left.position.y, inner_left.position.z);
        inner_right.position = new Vector3(inner_right_x, inner_right.position.y, inner_right.position.z);

        if (isIn)
        {
            liftUpTimer += Time.deltaTime;
            if (liftUpTimer > liftUpTime)
            {
                transform.Translate(Vector3.up*Time.deltaTime);
                gameWinTimer += Time.deltaTime;
                if (gameWinTimer > 5f)
                {
                    SceneManager.LoadScene(0);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == Tags.player)
        {
           isIn = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == Tags.player)
        {
           isIn = false;
           liftUpTimer = 0;
        }
    }
}