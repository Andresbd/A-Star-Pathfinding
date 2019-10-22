using UnityEngine;

public class TrafficBoard : MonoBehaviour
{
    public GameObject black, white;
    public Node[,] grid = new Node[100,100];
    public Node goalNode;
    // Start is called before the first frame update
    void Start()
    {
        GenerateBoard();
    }

    void GenerateBoard()  {
        for(int i = 0; i < 99;  i++){
            for(int j = 0; j < 100; j++) {
                if(i <= 40 && j > 39 && j < 60) {
                    white = Instantiate(white, new Vector3(i,0,j),Quaternion.identity);
                    grid[i,j] = new Node(false,i,j);
                }else if(i > 39 && i < 60){
                    white = Instantiate(white, new Vector3(i,0,j),Quaternion.identity);
                    grid[i,j] = new Node(false,i,j);
                }else if(i >= 60 && j > 39 && j  < 60) {
                    white = Instantiate(white, new Vector3(i,0,j),Quaternion.identity);
                    grid[i,j] = new Node(false,i,j);
                }else {
                    black = Instantiate(black, new Vector3(i,0,j),Quaternion.identity);
                    grid[i,j] = new Node(true,i,j);
                }
            }
        }
    }
}
