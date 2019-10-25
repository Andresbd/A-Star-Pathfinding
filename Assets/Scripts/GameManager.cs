using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject Car;
    public InputField NS_Ammount;
    public InputField WE_Ammount;
    private int contNS, contWE, setN, setW;
    Semaphore s;
    // Start is called before the first frame update
    void Start()
    {
        s = FindObjectOfType<Semaphore>();
        contNS = 0;
        contWE = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(contNS >= setN) {
            CancelInvoke("InvokeCarNS");
            contNS = 0;
        }

        if(contWE >= setW) {
            CancelInvoke("InvokeCarWE");
            contWE = 0;
        }
    }

    public void SpawnCars() {

        setN = int.Parse(NS_Ammount.text);
        setW = int.Parse(WE_Ammount.text);

        InvokeRepeating("InvokeCarNS",0.0f,0.2f);

        InvokeRepeating("InvokeCarWE",0.0f,0.2f);
        
        s.go = true;
    }

    void InvokeCarNS() {
        contNS++;
        Car = Instantiate(Car,new Vector3(50,1,99),Quaternion.identity);
        Car.GetComponent<AStarPathfinding>().nSpawn = true;
        Car.GetComponent<AStarPathfinding>().GenerateCar();
    }

    void InvokeCarWE() {
        contWE++;
        Car = Instantiate(Car,new Vector3(0,1,50),Quaternion.identity);
        Car.GetComponent<AStarPathfinding>().nSpawn = false;
        Car.GetComponent<AStarPathfinding>().GenerateCar();
    }
}
