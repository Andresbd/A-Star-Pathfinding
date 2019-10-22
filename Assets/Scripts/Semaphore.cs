using UnityEngine;

public class Semaphore : MonoBehaviour
{
    public int NS, WE, CARS;
    private double time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(CARS < 200 && time > 30) {
            //Reduce(CARS, time);
        }
        
    }
}
