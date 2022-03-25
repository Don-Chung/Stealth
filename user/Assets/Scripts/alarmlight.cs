// using System.ComponentModel;
// using System.Linq.Expressions.Interpreter;
// using System.Xml.Schema;
// using System;
using UnityEngine;

public class alarmlight : MonoBehaviour
{
    public static alarmlight _instance;
    public bool alarmOn = false;
    public float animationSpeed = 3.0f;
    private float lowIntensity=0;
    private float highIntensity=1.0f;

    private float targetIntensity;

    // Start is called before the first frame update
    void Awake()
    {
      targetIntensity=highIntensity;
      alarmOn=false;
      _instance = this;
    }

    // Update is called once per frame
    //public float t=0;
    void Update()
    {
        // float res = Mathf.Lerp(99, 100, t);
        // print(res);
        if(alarmOn){
          GetComponent<Light>().intensity=Mathf.Lerp(GetComponent<Light>().intensity, targetIntensity, Time.deltaTime*animationSpeed);
          if(Mathf.Abs(GetComponent<Light>().intensity - targetIntensity) < 0.08f){
            if(targetIntensity == highIntensity){
              targetIntensity = lowIntensity;
            }else if(targetIntensity == lowIntensity){
              targetIntensity = highIntensity;
            }
          }
        }else{
          GetComponent<Light>().intensity=Mathf.Lerp(GetComponent<Light>().intensity, 0, Time.deltaTime*animationSpeed);
        }
    }
}
