using System.Net.Mime;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

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

    private Canvas endCanvas;
    private Text ranks;
    //private TcpTest sendTcp;
    TcpClientHandler tcpClient;
    private long Timestart;
    private long Timeend;
    private string editString;
    private bool isEnd;

    private void Awake() {
        endCanvas = GameObject.FindGameObjectWithTag(Tags.endCanvas).GetComponent<Canvas>();
        ranks = GameObject.FindGameObjectWithTag(Tags.ranks).GetComponent<Text>();
        // sendTcp = gameObject.AddComponent<TcpTest>();
        // sendTcp.init();
        tcpClient = gameObject.AddComponent<TcpClientHandler>();
        tcpClient.InitSocket();
        Timestart = DateTime.Now.ToUniversalTime().Ticks;
    }

    void Update()
    {
        float inner_left_x = Mathf.Lerp(inner_left.position.x, outer_left.position.x, Time.deltaTime*inner_door_moveSpeed);
        float inner_right_x = Mathf.Lerp(inner_right.position.x, outer_right.position.x, Time.deltaTime*inner_door_moveSpeed);
        inner_left.position = new Vector3(inner_left_x, inner_left.position.y, inner_left.position.z);
        inner_right.position = new Vector3(inner_right_x, inner_right.position.y, inner_right.position.z);

        if (isIn)
        {
            Timeend = DateTime.Now.ToUniversalTime().Ticks;
            if(!isEnd){
                editString = Convert.ToString((Timeend - Timestart) / 10000);
                Debug.Log(editString);
                tcpClient.SocketSend(editString);
                Thread.Sleep(5);
                string res = tcpClient.GetRecvStr();
                Debug.Log(res);
                string[] t = res.Split('#');
                string textTmp = "通关排名\n\n";
                for(int i = 1; i < t.Length; i++){
                    textTmp += ("第" + Convert.ToString(i) + "名：" + t[i] + "ms\n");
                    Debug.Log(t[i]);
                }
                textTmp += "\n本次通关时长：" + editString +"ms";
                ranks.text = textTmp;
                endCanvas.enabled = true;
                tcpClient.SocketQuit();
                isEnd = true;
            }
            liftUpTimer += Time.deltaTime;
            if (liftUpTimer > liftUpTime)
            {
                transform.Translate(Vector3.up*Time.deltaTime);
                gameWinTimer += Time.deltaTime;
                if (gameWinTimer > 5f)
                {
                    endCanvas.enabled = false;
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
