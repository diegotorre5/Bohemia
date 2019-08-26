using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int layerMask = 1 << 8;
    public string State = gameState.PLAY;
    public Tree.treeStatus treeStatus;
    private GameObject bush;
    public Slider sliderDirection;
    public Slider sliderSpeed;
    public GameObject treePrefab;
    private int numberOfObjects = 100;
    public float radius = 5f;
    private Ray ray;
    private RaycastHit hit;
    private int TreeNumber;
    public string clickMode = "ADD";
    public bool directionChange = false;
    public int windSpeedTime = 1;

    public class clickModeClass
    {
        public const string ADD = "ADD";
        public const string REMOVE = "REMOVE";
        public const string FIRE = "FIRE";
    }

    public class gameState
    {
        public const string PLAY = "PLAY";
        public const string PAUSE = "PAUSE";
    }


    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreen = !Screen.fullScreen;
        Screen.SetResolution(1024, 768, true);
    }

    public void Generate()
    {
        Clear();
        bush = new GameObject();
        bush.name = "bush";
        int numberOfObjects = Random.Range(6000, 8000);
        int numberOfObject = 0;
        GameObject clone;
        Tree Treescript; 
        changeWindSpeed((int)sliderSpeed.value);
        for (int i = 0; i < numberOfObjects; i++)
        {
            float x = Random.Range(0, 150);
            float z = Random.Range(0, 100);
            Vector3 pos = new Vector3(x, Terrain.activeTerrain.SampleHeight(new Vector3(x,0,z)), z);

            if (Physics.CheckSphere(pos, 0.01f, layerMask))
            {
                numberOfObjects--;
            }
            else
            {
                numberOfObject++;
                clone = Instantiate(treePrefab, pos, Quaternion.identity);
                clone.name = "Tree" + numberOfObject.ToString();
                clone.transform.SetParent(bush.transform);

                Treescript = clone.GetComponent<Tree>();
                Treescript.ignitionTime = Random.Range(2, 3);
                Treescript.burningTime = Random.Range(5, 7);
                Treescript.status = Tree.treeStatus.GREEN;
                Treescript.sliderDirection = sliderDirection;
                Treescript.sliderSpeed = sliderSpeed;
            }

        }
        TreeNumber = numberOfObjects - 1;
    }

    public void addTree(Vector3 position)
    {
        if (GameObject.Find("bush") == null)
        {
            bush = new GameObject();
            bush.name = "bush";
        }
        GameObject clone;
        Tree Treescript; 
        TreeNumber++;
        clone = Instantiate(treePrefab, position, Quaternion.identity);
        clone.name = "Tree" + TreeNumber;
        clone.transform.SetParent(bush.transform);
        Treescript = clone.GetComponent<Tree>();
        Treescript.ignitionTime = Random.Range(5, 7);
        Treescript.burningTime = Random.Range(5, 7);
        Treescript.status = Tree.treeStatus.GREEN;
        Treescript.sliderDirection = sliderDirection;
        Treescript.sliderSpeed = sliderSpeed;
    }

    public void Clear()
    {
        TreeNumber = 0;
        Destroy(bush);
    }

    public void changeGameState() {
        if (State == GameManager.gameState.PLAY)
        {
            State = GameManager.gameState.PAUSE;
            Time.timeScale = 0;
        }
        else {
            State = GameManager.gameState.PLAY;
            Time.timeScale = windSpeedTime;
        }
    }

    public void changeWindSpeed(int Value)
    {
        if (Value < 10)
         Value = 10; 
        windSpeedTime = Value / 10;
        Debug.Log(windSpeedTime);
        Time.timeScale = windSpeedTime;

    }

    public void randomFire()
    {
        if (TreeNumber == 0)
            return;
        int numberSparx;
        int[] Sparx = new int[10];
        numberSparx = Random.Range(8, 10);
        for (int i = 0; i < numberSparx; i++) {
            GameObject Tree = null;
            while (Tree == null) {
                Tree = GameObject.Find("Tree" + Random.Range(1, TreeNumber));
            }
            Tree treeScript = Tree.gameObject.GetComponent<Tree>();
            treeScript.ignite();
        }
    }

    // Update is called once per frame

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                 Application.Quit();
        #endif
    }


}
