//using System.Diagnostics;
using System.Net.Sockets;
using UnityEngine;
using System;
using System.Collections;
using System.Threading;

public class TcpTest : MonoBehaviour
{
    private long Timestart;
    private long Timeend;
    private int tmp;
    string editString;

    TcpClientHandler tcpClient;
    // Use this for initialization
    public void init()
    {
        tcpClient = gameObject.AddComponent<TcpClientHandler>();
        tcpClient.InitSocket();
        Timestart = DateTime.Now.ToUniversalTime().Ticks;
    }
    // void Start()
    // {
    //     //初始化网络连接
    //     //tcpClient=new TcpClientHandler(); //因为tcp的类继承了mono behaviour所以不能用new，或者去掉对mono behaviour继承就可以用new
    //     player = GameObject.FindGameObjectWithTag(Tags.player);
    // }

    // void OnGUI()
    // {
    //     editString = GUI.TextField(new Rect(10, 10, 100, 20), editString);
    //     GUI.Label(new Rect(10, 30, 300, 20), tcpClient.GetRecvStr());
    //     if (GUI.Button(new Rect(10, 50, 60, 20), "send"))
    //         tcpClient.SocketSend(editString);
    // }

    // Update is called once per frame
    // void FixedUpdate()
    // {
    //     if (tcpClient.GetRecvStr()=="start")
    //     {

        // string strCube_x = player.transform.position.x.ToString();
        // string strCube_y = player.transform.position.y.ToString();
        // string strCube_z = player.transform.position.z.ToString();
        // Debug.Log("strcube的差："+strCube_x + " ," + strCube_y + " ," + strCube_z + ", sum:" + sum);
        // Debug.Log("新的位置："+x_st + " ," + y_st + " ," + z_st + ", sum:" + sum);
        // Debug.Log(strCube_x);
        // tmp = 11;
        // Timeend = DateTime.Now.ToUniversalTime().Ticks;
        // editString = Convert.ToString(tmp); //+ sum;
        // tcpClient.SocketSend(editString);
        // sum++;
        // }
        // else if(tcpClient.GetRecvStr() == "stop")
        // {
        //     tcpClient.SocketQuit();
        // }
        // }

    // void OnApplicationQuit()
    public string quit()
    {
        Timeend = DateTime.Now.ToUniversalTime().Ticks;
        editString = Convert.ToString((Timeend - Timestart) / 10000); //+ sum;
        Debug.Log(editString);
        tcpClient.SocketSend(editString);
        Thread.Sleep(5);
        string rec = tcpClient.GetRecvStr();
        Debug.Log(rec);
        //退出时关闭连接
        tcpClient.SocketQuit();
        return rec;
    }
}
