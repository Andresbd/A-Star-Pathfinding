﻿using UnityEngine;
using UnityEngine.UI;

public class Semaphore : MonoBehaviour
{
    public int NS, WE, cars;
    public string currTraffNS, currTraffWE, currTraffStatus, currTimerType;
    TrafficBoard b;
    public Text dispTime;
    Node[] NS_Semaphore; //[39,20] - [39,40]
    Node[] WE_Semaphore;
    private double time, currentTimer;
    private bool  redLight;
    public bool go;
    public float[] set;
    
    // Start is called before the first frame update
    void Start()
    {
        b = FindObjectOfType<TrafficBoard>();
        WE_Semaphore = new Node[20];
        NS_Semaphore = new Node[20];
        SetSemaphores();
        time = 300;
        currentTimer = time;
        currTraffStatus = "low";
        currTimerType = "slow";
        set = new float[3];
        cars = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(go) {
            NS = b.nCars;
            WE = b.wCars;
            cars = NS + WE /2;

            currTimerType = TimerType(currTimerType, time);
            currTraffStatus = TrafficStatus(currTraffStatus);
            NextAction(currTimerType, currTraffStatus);

            dispTime.text = time.ToString();

            if(currentTimer <= 0) {
                SwitchLights();
                currentTimer = time;
            }else {
                currentTimer--;
            }
        }
    }

    void SetSemaphores() {
        for(int i = 0; i < 20; i++) {
            WE_Semaphore[i] = b.grid[39,i+40];
            NS_Semaphore[i] = b.grid[40+i,60];
        }
        for(int i = 0; i < 20; i++) {
                b.WE[i].gameObject.GetComponent<Renderer>().material.color = Color.green;
                b.NS[i].gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
    }

    void SwitchLights()
    {
        if(redLight == false) {
            for(int i = 0; i < 20; i++) {
                b.WE[i].gameObject.GetComponent<Renderer>().material.color = Color.red;
                b.NS[i].gameObject.GetComponent<Renderer>().material.color = Color.green;
                b.grid[WE_Semaphore[i].posX,WE_Semaphore[i].posY].isTrafficLight = true;
                b.grid[NS_Semaphore[i].posX,NS_Semaphore[i].posY].isTrafficLight = false;
            }
            redLight = true;
        }else {
            for(int i = 0; i < 20; i++) {
                b.WE[i].gameObject.GetComponent<Renderer>().material.color = Color.green;
                b.NS[i].gameObject.GetComponent<Renderer>().material.color = Color.red;
                b.grid[WE_Semaphore[i].posX,WE_Semaphore[i].posY].isTrafficLight = false;
                b.grid[NS_Semaphore[i].posX,NS_Semaphore[i].posY].isTrafficLight = true;
            }
            redLight = false;
        }
    }

    //fuzz and defuzz

    void Reduce()
    {
        float aux = (set[0] + set[1] + set[2]) / 2;
        aux = (float)(aux + 0.5);
        double timeaux = aux * 600;
        if (timeaux < time) time = timeaux;
        else time -= 30;
    }

    void Increase()
    {
        float aux = (set[0] + set[1] + set[2]) / 2;
        aux = (float)(aux + 0.5);
        double timeaux = aux * 600;
        if (timeaux > time) time = timeaux;
        else time += 15;
    }

    void noChange(){
        float aux;
        if(currTimerType.Equals("fast")){
            if(set[0] > 1 ){
                aux = 0.5f + ((set[0] + set[1]) / 2);
                time = aux*600;
            }
            else{
                time = (set[0] +0.5f) * 600;
            }
        }else if(currTimerType.Equals("medium")){
            if(set[1] <= 1.2f) {
                aux = 0.5f + ((set[0] + set[1]) / 2);
                time = aux*600;
            }else if(set[1] > 1.2f && set[1] <= 2.2f) {
                aux = 0.5f + ((set[1] + set[2]) / 2);
                time = aux*600;
            }else {
                time = (set[1] +0.5f) * 900;
            }

        }else if(currTimerType.Equals("slow")) {
            if(set[2] > 2.2f){
                aux = 0.5f + ((set[1] + set[2]) / 2);
                time = aux*600;
            }
            else{
                time = (set[2] +0.5f) * 600;
            }
        }
    }

    void NextAction(string TimrType, string TraffType)
    {
        double auxTime = time;

        if (TimrType.Equals("fast") && TraffType.Equals("low"))
        {
            set[0] = FuncionLow(cars);
            set[1] = FuncionMed(cars);
            set[2] = 0;
            noChange();
            //time = auxTime; //nochange
        }
        else if (TimrType.Equals("fast") && TraffType.Equals("med"))
        {
            set[0] = FuncionLow(cars);
            set[1] = FuncionMed(cars);
            set[2] = 0;
            Increase();
            
        }
        else if (TimrType.Equals("fast") && TraffType.Equals("heavy"))
        {
            set[0] = 0;
            set[1] = FuncionMed(cars);
            set[2] = FuncionHigh(cars);
            Increase();
        }
        else if (TimrType.Equals("medium") && TraffType.Equals("low"))
        {
            set[0] = FuncionLow(cars);
            set[1] = FuncionMed(cars);
            set[2] = 0;
            Reduce(); 
        }
        else if (TimrType.Equals("medium") && TraffType.Equals("med"))
        {
            set[0] = FuncionLow(cars);
            set[1] = FuncionMed(cars);
            set[2] = FuncionHigh(cars);
            noChange();
        }
        else if (TimrType.Equals("medium") && TraffType.Equals("heavy"))
        {
            set[0] = 0;
            set[1] = FuncionMed(cars);
            set[2] = FuncionHigh(cars);
            Increase();
        }
        else if (TimrType.Equals("slow") && TraffType.Equals("low"))
        {
            set[0] = FuncionLow(cars);
            set[1] = FuncionMed(cars);
            set[2] = 0;
            Reduce();
        }
        else if (TimrType.Equals("slow") && TraffType.Equals("med"))
        {
            set[0] = 0;
            set[1] = FuncionMed(cars);
            set[2] = FuncionHigh(cars);
            Reduce(); 
        }
        else if (TimrType.Equals("slow") && TraffType.Equals("heavy"))
        {
            time = auxTime; //nochange
        }
    }


    //membership functions cars and gives time

    public float FuncionLow(double x)
    {
        float result = 0;
        if (x < 402) {
            x /= 553;
            result = Mathf.Pow((float)x, 2);
        }
        if (result > 1.2) result = 0.01f;
        return result;
    }

    public float FuncionMed(double x)
    {
        float result = 0;
        x /= 533;

        result = (float)(0.5 + Mathf.Pow((float)x, 2));
        if (result > 1.4) result = (float)(1 + Mathf.Pow((float)x, 2));

        if (result > 2.2) result = 2.2f;
        return result;
    }

    public float FuncionHigh(double x)
    {        
        float result = 0;
        if (x > 490)
        {
            x /= 800;
            result = (float)(1.5 + Mathf.Pow((float)x, 2));
        }
        if (result > 2.5) result = 2.5f;
        return result;
    }

    //RULES

    public string TimerType(string prevTimer, double currTime)
    {
        string type = prevTimer;

        if (currTime >= 300 && currTime <= 600)
        {
            type = "fast";
        }
        else if (currTime > 600 && currTime <= 900)
        {
            type = "medium";
        }
        else if (currTime > 900 && currTime <= 1200)
        {
            type = "slow";
        }
        return type;
    }

    public string TrafficStatus(string prevStatus)
    {

        string status = prevStatus;

        currTraffNS = TrafficIntensity(NS);
        currTraffWE = TrafficIntensity(WE);

        if (currTraffNS.Equals("low") && currTraffWE.Equals("low"))
        {
            status = "low";
        }
        else if (currTraffNS.Equals("low") && currTraffWE.Equals("med"))
        {
            status = "med";
        }
        else if (currTraffNS.Equals("med") && currTraffWE.Equals("low"))
        {
            status = "med";
        }
        else if (currTraffNS.Equals("med") && currTraffWE.Equals("med"))
        {
            status = "med";
        }
        else if (currTraffNS.Equals("low") && currTraffWE.Equals("heavy"))
        {
            status = "heavy";
        }
        else if (currTraffNS.Equals("heavy") && currTraffWE.Equals("low"))
        {
            status = "heavy";
        }
        else if (currTraffNS.Equals("med") && currTraffWE.Equals("heavy"))
        {
            status = "heavy";
        }
        else if (currTraffNS.Equals("heavy") && currTraffWE.Equals("med"))
        {
            status = "heavy";
        }
        else if (currTraffNS.Equals("heavy") && currTraffWE.Equals("heavy"))
        {
            status = "heavy";
        }

        return status;
    }

    public string TrafficIntensity(int cars)
    {
        string type = "";
        if (cars >= 0 && cars <= 267)
        {
            type = "low";
        }
        else if (cars > 267 && cars <= 532)
        {
            type = "med";
        }
        else {
            type = "heavy";
        }
        return type;
    }

}
