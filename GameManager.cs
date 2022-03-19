using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    // the reference camera prefab that car uses
    public Camera carCamera;
    // camera that is used to navigate the city
    public Camera navigationCamera;
    // ctr and settings : the gameObjcts that holds the photon's UI,
    public GameObject ctr, settings;
    // speed of rotation with mouse during city navigation
    private const int rotationSpeed = 10;
    // mapping from node name to node transform in the map
    public Dictionary<string, Transform> xNodes = new Dictionary<string, Transform>();
    // start and end points are the current two points between them the car located
    public Text startPoint;  public Text endPoint;
    //pos between the startPoint and endPoint
    public Text xPos;
    // set of nodes names that define the path to the car destination
    public Text xRoute;
    // reference to the button that starts the trip
    public Button startBtn;
    // reference to the button that stops the trip
    public Button stopBtn;
    // input field where the user insert name of destination node
    public InputField destPoint;
    public delegate void TripEvent();
    // an event to be triggered when the trip starts
    public event TripEvent onStartTrip;
    // an event to be triggered when the trip stops
    public event TripEvent onStopTrip;
    // reference to trip panel that holds the controls and data about the trip
    public GameObject TripPanel;
    
    void Start()
    {
        GameManager.instance.TripPanel.SetActive(false);
        // add all the nodes on the map to the dictionary that map from their name to their transforms
        addNodesTransforms();
    }
    
    void Update()
    {
        // toggle for photon UI controls
        if (Input.GetKeyDown(KeyCode.C) && !settings.activeSelf)
        {
            ctr.SetActive(!ctr.activeSelf);
        }
        // toggle for photon UI settings
        if (Input.GetKeyDown(KeyCode.V) && !ctr.activeSelf)
        {
            settings.SetActive(!settings.activeSelf);
        }
        // switch from car view to navigation view and vice versa
        switchViews();
    }
    // press G to activate navigation mode , H for car View
    private void switchViews()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            turnNavView();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            turnCarView();
        }
    }

    public void turnCarView()
    {
        carCamera.enabled = true;
        navigationCamera.enabled = false;
    }
    public void turnNavView()
    {
        carCamera.enabled = false;
        navigationCamera.enabled = true;
    }
    // rotate the a gameObject [to be used in rotating gameobjects that holds a camera]
    public void rotateUsingAxis(ref Vector3 rotationVector, MonoBehaviour xGameObject)
    {
        rotationVector += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * rotationSpeed;
        rotationVector.x = Mathf.Clamp(rotationVector.x, -30, 90);
        xGameObject.transform.rotation = Quaternion.Euler(rotationVector);
    }

    // to be invoked whenever adding or removing nodes from the map so that changes get reflected on server side
    public void sendMapToServerOnchanges()
    {
        // mapping from node name to its edges to other nodes
        Dictionary<string, List<edge>> adjacencyList = new Dictionary<string, List<edge>>();
        // fill adjacency list
        foreach (string k in xNodes.Keys)
        {
            if (!string.IsNullOrEmpty(k) && k.Length < 3) // node
            {
                adjacencyList.Add(k, new List<edge>());
            }
            else // edge
            {
                string node = k.Substring(0, k.IndexOf('-'));
                string neighbour = k.Substring(k.IndexOf('-') + 1);
                // calculate manhaten distance between the node and its neighbour
                float distance =  Mathf.Abs(xNodes[node].position.x - xNodes[neighbour].position.x)
                                 +Mathf.Abs(xNodes[node].position.z - xNodes[neighbour].position.z);
                adjacencyList[node].Add(new edge(neighbour, distance));
            }
        }
        // fill a list of node's data to be converted into json
        List<NodeData> tmp = new List<NodeData>();
        foreach (KeyValuePair<string, List<edge>> kvp in adjacencyList)
        {
            tmp.Add(new NodeData(kvp.Key, kvp.Value));
        }
        string mapJson = JsonUtility.ToJson(new AdjacencyModel(tmp));
        // send mapJson to server using the key [mapJson]
        StartCoroutine(
        APIsManager.instance.getRequest(
            new paramListBuilder("mapJson", mapJson).ToString()
           , APIsManager.instance.postMap_URL
           , (jsonResponse) => { Debug.Log(jsonResponse);}
        ));
    }
    
    public void onClickStartTrip()
    {
        if(onStartTrip != null)
        {
            onStartTrip();
        }
    }

    public void onClickStopTrip()
    {
        if(onStopTrip != null)
        {
            onStopTrip();
        }
    }
    
    private void addNodesTransforms()
    {
        xNodes.Add("A", GameObject.Find("A").transform);
        xNodes.Add("B", GameObject.Find("B").transform);
        xNodes.Add("C", GameObject.Find("C").transform);
        xNodes.Add("D", GameObject.Find("D").transform);
        xNodes.Add("E", GameObject.Find("E").transform);
        xNodes.Add("F", GameObject.Find("F").transform);
        xNodes.Add("G", GameObject.Find("G").transform);
        xNodes.Add("H", GameObject.Find("H").transform);
        xNodes.Add("I", GameObject.Find("I").transform);
        xNodes.Add("J", GameObject.Find("J").transform);
        xNodes.Add("K", GameObject.Find("K").transform);
        xNodes.Add("L", GameObject.Find("L").transform);
        xNodes.Add("M", GameObject.Find("M").transform);
        xNodes.Add("N", GameObject.Find("N").transform);
        xNodes.Add("O", GameObject.Find("O").transform);
        xNodes.Add("P", GameObject.Find("P").transform);
        xNodes.Add("Q", GameObject.Find("Q").transform);
        xNodes.Add("R", GameObject.Find("R").transform);
        xNodes.Add("S", GameObject.Find("S").transform);
        xNodes.Add("T", GameObject.Find("T").transform);
        xNodes.Add("U", GameObject.Find("U").transform);
        xNodes.Add("V", GameObject.Find("V").transform);
        xNodes.Add("W", GameObject.Find("W").transform);
        xNodes.Add("X", GameObject.Find("X").transform);
        xNodes.Add("Y", GameObject.Find("Y").transform);
        xNodes.Add("Z", GameObject.Find("Z").transform);


        xNodes.Add("A2", GameObject.Find("A2").transform);
        xNodes.Add("B2", GameObject.Find("B2").transform);
        xNodes.Add("C2", GameObject.Find("C2").transform);
        xNodes.Add("D2", GameObject.Find("D2").transform);
        xNodes.Add("E2", GameObject.Find("E2").transform);
        xNodes.Add("F2", GameObject.Find("F2").transform);
        xNodes.Add("G2", GameObject.Find("G2").transform);
        xNodes.Add("H2", GameObject.Find("H2").transform);
        xNodes.Add("I2", GameObject.Find("I2").transform);
        xNodes.Add("J2", GameObject.Find("J2").transform);
        xNodes.Add("L2", GameObject.Find("L2").transform);
        xNodes.Add("M2", GameObject.Find("M2").transform);
        xNodes.Add("N2", GameObject.Find("N2").transform);
        xNodes.Add("O2", GameObject.Find("O2").transform);
        xNodes.Add("P2", GameObject.Find("P2").transform);
        xNodes.Add("Q2", GameObject.Find("Q2").transform);
        xNodes.Add("R2", GameObject.Find("R2").transform);
        xNodes.Add("S2", GameObject.Find("S2").transform);
        xNodes.Add("T2", GameObject.Find("T2").transform);
        xNodes.Add("U2", GameObject.Find("U2").transform);
        xNodes.Add("V2", GameObject.Find("V2").transform);
        xNodes.Add("W2", GameObject.Find("W2").transform);
        xNodes.Add("X2", GameObject.Find("X2").transform);
        xNodes.Add("Y2", GameObject.Find("Y2").transform);
        xNodes.Add("A3", GameObject.Find("A3").transform);
        xNodes.Add("C3", GameObject.Find("C3").transform);
        xNodes.Add("D3", GameObject.Find("D3").transform);
        xNodes.Add("E3", GameObject.Find("E3").transform);
        xNodes.Add("G3", GameObject.Find("G3").transform);
        xNodes.Add("H3", GameObject.Find("H3").transform);
        xNodes.Add("L3", GameObject.Find("L3").transform);
        xNodes.Add("M3", GameObject.Find("M3").transform);
        xNodes.Add("N3", GameObject.Find("N3").transform);
        xNodes.Add("O3", GameObject.Find("O3").transform);
        xNodes.Add("Q3", GameObject.Find("Q3").transform);


        xNodes.Add("A-B", GameObject.Find("A-B").transform);
        xNodes.Add("B-A", GameObject.Find("B-A").transform);
        xNodes.Add("B-C", GameObject.Find("B-C").transform);
        xNodes.Add("C-B", GameObject.Find("C-B").transform);
        xNodes.Add("C-Y2", GameObject.Find("C-Y2").transform);
        xNodes.Add("B-E", GameObject.Find("B-E").transform);
        xNodes.Add("E-B", GameObject.Find("E-B").transform);
        xNodes.Add("Y2-D", GameObject.Find("Y2-D").transform);
        xNodes.Add("D-C", GameObject.Find("D-C").transform);
        xNodes.Add("A3-B", GameObject.Find("A3-B").transform);
        xNodes.Add("B-A3", GameObject.Find("B-A3").transform);
        xNodes.Add("D-G", GameObject.Find("D-G").transform);
        xNodes.Add("G-D", GameObject.Find("G-D").transform);
        xNodes.Add("X2-H3", GameObject.Find("X2-H3").transform);
        xNodes.Add("H3-X2", GameObject.Find("H3-X2").transform);
        xNodes.Add("W2-X2", GameObject.Find("W2-X2").transform);
        xNodes.Add("X2-V2", GameObject.Find("X2-V2").transform);
        xNodes.Add("V2-W2", GameObject.Find("V2-W2").transform);
        xNodes.Add("F-E", GameObject.Find("F-E").transform);
        xNodes.Add("D-F", GameObject.Find("D-F").transform);
        xNodes.Add("A2-G", GameObject.Find("A2-G").transform);
        xNodes.Add("G-B2", GameObject.Find("G-B2").transform);
        xNodes.Add("D-X2", GameObject.Find("D-X2").transform);
        xNodes.Add("X2-D", GameObject.Find("X2-D").transform);
        xNodes.Add("G-H3", GameObject.Find("G-H3").transform);
        xNodes.Add("H3-G", GameObject.Find("H3-G").transform);
        xNodes.Add("B2-A2", GameObject.Find("B2-A2").transform);
        xNodes.Add("E-D", GameObject.Find("E-D").transform);
        xNodes.Add("Y-X", GameObject.Find("Y-X").transform);
        xNodes.Add("X-Y", GameObject.Find("X-Y").transform);
        xNodes.Add("X-Z", GameObject.Find("X-Z").transform);
        xNodes.Add("Z-X", GameObject.Find("Z-X").transform);
        xNodes.Add("X-G3", GameObject.Find("X-G3").transform);
        xNodes.Add("G3-X", GameObject.Find("G3-X").transform);
        xNodes.Add("G-H", GameObject.Find("G-H").transform);
        xNodes.Add("H-G", GameObject.Find("H-G").transform);
        xNodes.Add("B2-X", GameObject.Find("B2-X").transform);
        xNodes.Add("X-B2", GameObject.Find("X-B2").transform);
        xNodes.Add("H-I", GameObject.Find("H-I").transform);
        xNodes.Add("I-H", GameObject.Find("I-H").transform);
        xNodes.Add("V-S", GameObject.Find("V-S").transform);
        xNodes.Add("S-V", GameObject.Find("S-V").transform);
        xNodes.Add("Q3-V", GameObject.Find("Q3-V").transform);
        xNodes.Add("V-H3", GameObject.Find("V-H3").transform);
        xNodes.Add("H3-Q3", GameObject.Find("H3-Q3").transform);
        xNodes.Add("C2-G3", GameObject.Find("C2-G3").transform);
        xNodes.Add("H-C2", GameObject.Find("H-C2").transform);
        xNodes.Add("D2-I", GameObject.Find("D2-I").transform);
        xNodes.Add("G2-D2", GameObject.Find("G2-D2").transform);
        xNodes.Add("H-V", GameObject.Find("H-V").transform);
        xNodes.Add("V-H", GameObject.Find("V-H").transform);
        xNodes.Add("I-S", GameObject.Find("I-S").transform);
        xNodes.Add("S-I", GameObject.Find("S-I").transform);
        xNodes.Add("I-G2", GameObject.Find("I-G2").transform);
        xNodes.Add("G3-H", GameObject.Find("G3-H").transform);
        xNodes.Add("E2-H2", GameObject.Find("E2-H2").transform);
        xNodes.Add("H2-E2", GameObject.Find("H2-E2").transform);
        xNodes.Add("H2-J2", GameObject.Find("H2-J2").transform);
        xNodes.Add("J2-H2", GameObject.Find("J2-H2").transform);
        xNodes.Add("H2-I2", GameObject.Find("H2-I2").transform);
        xNodes.Add("I2-H2", GameObject.Find("I2-H2").transform);
        xNodes.Add("I-J", GameObject.Find("I-J").transform);
        xNodes.Add("J-I", GameObject.Find("J-I").transform);
        xNodes.Add("G2-H2", GameObject.Find("G2-H2").transform);
        xNodes.Add("H2-G2", GameObject.Find("H2-G2").transform);
        xNodes.Add("J-K", GameObject.Find("J-K").transform);
        xNodes.Add("K-J", GameObject.Find("K-J").transform);
        xNodes.Add("M-L", GameObject.Find("M-L").transform);
        xNodes.Add("L-M", GameObject.Find("L-M").transform);
        xNodes.Add("L2-M", GameObject.Find("L2-M").transform);
        xNodes.Add("S-L2", GameObject.Find("S-L2").transform);
        xNodes.Add("M-S", GameObject.Find("M-S").transform);
        xNodes.Add("I2-J", GameObject.Find("I2-J").transform);
        xNodes.Add("J-F2", GameObject.Find("J-F2").transform);
        xNodes.Add("J-M", GameObject.Find("J-M").transform);
        xNodes.Add("M-J", GameObject.Find("M-J").transform);
        xNodes.Add("F2-I2", GameObject.Find("F2-I2").transform);
        xNodes.Add("W2-U2", GameObject.Find("W2-U2").transform);
        xNodes.Add("U2-W2", GameObject.Find("U2-W2").transform);
        xNodes.Add("U2-R2", GameObject.Find("U2-R2").transform);
        xNodes.Add("R2-U2", GameObject.Find("R2-U2").transform);
        xNodes.Add("U2-O3", GameObject.Find("U2-O3").transform);
        xNodes.Add("O3-U2", GameObject.Find("O3-U2").transform);
        xNodes.Add("Q2-R2", GameObject.Find("Q2-R2").transform);
        xNodes.Add("T2-U2", GameObject.Find("T2-U2").transform);
        xNodes.Add("U2-T2", GameObject.Find("U2-T2").transform);
        xNodes.Add("Q2-P2", GameObject.Find("Q2-P2").transform);
        xNodes.Add("P2-Q2", GameObject.Find("P2-Q2").transform);
        xNodes.Add("X2-O3", GameObject.Find("X2-O3").transform);
        xNodes.Add("Q2-X2", GameObject.Find("Q2-X2").transform);
        xNodes.Add("H3-P2", GameObject.Find("H3-P2").transform);
        xNodes.Add("P2-N3", GameObject.Find("P2-N3").transform);
        xNodes.Add("Q2-C3", GameObject.Find("Q2-C3").transform);
        xNodes.Add("C3-Q2", GameObject.Find("C3-Q2").transform);
        xNodes.Add("P2-D3", GameObject.Find("P2-D3").transform);
        xNodes.Add("D3-P2", GameObject.Find("D3-P2").transform);
        xNodes.Add("S2-Q2", GameObject.Find("S2-Q2").transform);
        xNodes.Add("R2-S2", GameObject.Find("R2-S2").transform);
        xNodes.Add("N3-H3", GameObject.Find("N3-H3").transform);
        xNodes.Add("O3-Q2", GameObject.Find("O3-Q2").transform);
        xNodes.Add("Q3-L3", GameObject.Find("Q3-L3").transform);
        xNodes.Add("L3-Q3", GameObject.Find("L3-Q3").transform);
        xNodes.Add("L3-O2", GameObject.Find("L3-O2").transform);
        xNodes.Add("O2-L3", GameObject.Find("O2-L3").transform);
        xNodes.Add("L3-M3", GameObject.Find("L3-M3").transform);
        xNodes.Add("M3-L3", GameObject.Find("M3-L3").transform);
        xNodes.Add("P2-W", GameObject.Find("P2-W").transform);
        xNodes.Add("W-O2", GameObject.Find("W-O2").transform);
        xNodes.Add("N3-L3", GameObject.Find("N3-L3").transform);
        xNodes.Add("L3-N3", GameObject.Find("L3-N3").transform);
        xNodes.Add("W-R", GameObject.Find("W-R").transform);
        xNodes.Add("R-W", GameObject.Find("R-W").transform);
        xNodes.Add("V-M3", GameObject.Find("V-M3").transform);
        xNodes.Add("W-V", GameObject.Find("W-V").transform);
        xNodes.Add("S-R", GameObject.Find("S-R").transform);
        xNodes.Add("R-T", GameObject.Find("R-T").transform);
        xNodes.Add("W-E3", GameObject.Find("W-E3").transform);
        xNodes.Add("E3-W", GameObject.Find("E3-W").transform);
        xNodes.Add("R-U", GameObject.Find("R-U").transform);
        xNodes.Add("U-R", GameObject.Find("U-R").transform);
        xNodes.Add("O2-P2", GameObject.Find("O2-P2").transform);
        xNodes.Add("T-S", GameObject.Find("T-S").transform);
        xNodes.Add("M3-W", GameObject.Find("M3-W").transform);
        xNodes.Add("L2-P", GameObject.Find("L2-P").transform);
        xNodes.Add("P-L2", GameObject.Find("P-L2").transform);
        xNodes.Add("P-Q", GameObject.Find("P-Q").transform);
        xNodes.Add("Q-P", GameObject.Find("Q-P").transform);
        xNodes.Add("P-M2", GameObject.Find("P-M2").transform);
        xNodes.Add("M2-P", GameObject.Find("M2-P").transform);
        xNodes.Add("R-N", GameObject.Find("R-N").transform);
        xNodes.Add("Q-R", GameObject.Find("Q-R").transform);
        xNodes.Add("T-P", GameObject.Find("T-P").transform);
        xNodes.Add("P-T", GameObject.Find("P-T").transform);
        xNodes.Add("N-N2", GameObject.Find("N-N2").transform);
        xNodes.Add("N2-N", GameObject.Find("N2-N").transform);
        xNodes.Add("M2-N", GameObject.Find("M2-N").transform);
        xNodes.Add("N-M", GameObject.Find("N-M").transform);
        xNodes.Add("N-O", GameObject.Find("N-O").transform);
        xNodes.Add("O-N", GameObject.Find("O-N").transform);
        xNodes.Add("N-Q", GameObject.Find("N-Q").transform);
        xNodes.Add("M-M2", GameObject.Find("M-M2").transform);

    }
    
}
