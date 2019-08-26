using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.EventSystems;

public class Tree : MonoBehaviour
{
    public class treeStatus
    {
        public const string GREEN = "GREEN";
        public const string IGNITION = "IGNITION";
        public const string BURNING = "BURNING";
        public const string BURNED = "BURNED";
    }

    public string status;
    private IEnumerator coroutine;
    private IEnumerator subCoroutine;
    private IEnumerator subCoroutine2;
    // public GameObject radiousSphere;
    public GameObject windDirection;
    public Slider sliderDirection;
    public Slider sliderSpeed;
    public Color32 color;
    public float ignitionProgress = 0;
    public float ignitionRate = 0;
    public float burningProgress = 0;
    public float ignitionTime;
    public float burningTime;
    private GameManager gameManagerScript;
    private Color32 colorOrange = new Color32(241, 90, 34, 255);
    public bool calculateWindSpread = false;
    public bool calculateRadialSpread = false;
    //private Transform winddirection;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    void Green()
    {
        status = treeStatus.GREEN;
        this.gameObject.GetComponent<Renderer>().material.color = Color.green;
    }

    IEnumerator Ignite()
    {
        status = treeStatus.IGNITION;
        this.gameObject.GetComponent<Renderer>().material.color = colorOrange;
        yield return new WaitForSeconds(ignitionTime);
            calculateRadialSpread = true;
            calculateWindSpread = true;
            status = treeStatus.BURNING;
            coroutine = Burning();
            StartCoroutine(coroutine);
        yield return null;
    }

    IEnumerator Burning()
    {
        status = treeStatus.BURNING;
        this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        subCoroutine = spreadFireForward();
        StartCoroutine(subCoroutine);
        subCoroutine2 = spreadFireRadious();
        StartCoroutine(subCoroutine2);
        yield return new WaitForSeconds(burningTime);
        Burned();
        yield return null;
    }

    void Burned() {
        status = treeStatus.BURNED;
        this.gameObject.GetComponent<Renderer>().material.color = Color.black;
    }

    IEnumerator spreadFireForward()
    {
        float minDistance = 0;
        RaycastHit[] hits = new RaycastHit[0];
        windDirection.transform.localRotation = Quaternion.Euler(0, sliderDirection.value, 0);
        var dicHits = new Dictionary<RaycastHit, float>();
            hits = Physics.RaycastAll(windDirection.transform.position, windDirection.transform.forward, 100.0f);
        if (hits.Length > 1)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.name.Contains("Tree")) { 
                    dicHits.Add(hits[i], Vector3.Distance(windDirection.transform.position, hits[i].transform.position));
                }

            }
            minDistance = dicHits.Values.Min();
            RaycastHit min = dicHits.FirstOrDefault(x => x.Value == minDistance).Key;
            if (minDistance < 15 )
            {
                Tree treeScript = min.transform.GetComponent<Tree>();
                if (treeScript.status == Tree.treeStatus.GREEN)
                {
                    treeScript.status = Tree.treeStatus.IGNITION;
                    StartCoroutine(treeScript.Ignite());
                    
                }
            }
        }

        dicHits = null;
        calculateWindSpread = false;
        yield return null;
    }

    IEnumerator spreadFireRadious()
    {
        Collider[] hitColliders = new Collider[10];
       int numColiders = Physics.OverlapSphereNonAlloc(this.gameObject.transform.position, 1, hitColliders);
        for (int i = 0; i < numColiders; i++) {
            if (hitColliders[i].name.Contains("Tree")) {
                Tree treeScript = hitColliders[i].transform.GetComponent<Tree>();
                if (treeScript.status == Tree.treeStatus.GREEN)
                {
                    treeScript.status = Tree.treeStatus.IGNITION;
                    StartCoroutine(treeScript.Ignite());
                }
            }
        }
        calculateRadialSpread = false;
        hitColliders = null;
        yield return null;
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

            RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.point.x + "," + hit.point.y + "," + hit.point.z);
            float x = hit.point.x;
            float z = hit.point.z;
            Vector3 pos = new Vector3(x, 0, z);

            switch(gameManagerScript.clickMode)
            {
                case GameManager.clickModeClass.ADD:
                    Debug.Log("Add error message Tree already in place");
                    break;
                case GameManager.clickModeClass.REMOVE:
                    Destroy(this.gameObject);
                    break;
                case GameManager.clickModeClass.FIRE:
                    if (this.status == treeStatus.BURNING || this.status == treeStatus.IGNITION) {
                        Green();
                        StopCoroutine(coroutine);
                    }
                    else {
                        ignite();
                    }
                    break;
            }
        }
   
    }

    public void ignite() {
        this.status = treeStatus.IGNITION;
        coroutine = Ignite();
        StartCoroutine(coroutine);
    }

}
