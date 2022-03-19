using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class CarMovement : MonoBehaviour
{
    // car nav mesh agent component
    private NavMeshAgent nma;
    // the camera GameObject that follows the car
    [SerializeField]
    private GameObject cameraGameObject;
    // distance between the car and the camera that follows it
    private Vector3 cameraOffset;
    // a reference to carInfo instance that holds the car Information for each frame, to be sent to server side
    private CarInfo carInfo;
    // hold data about the current trip , if a trip has been started
    private Trip currentTrip;
    // the two point between them the car is located
    private string startPoint, endPoint;
    // equals true when the car is given an order to move to a certain point
    private bool hasStartedTowardTarget = false;
    // reference to the current trip coroutine 
    IEnumerator Trip_Coroutine;
    // the greatest distance between car and target node before switching to the next node
    // when the distance between the car and the target node is less than or equal this value
    // the target mode switch to the next node
    private int targetSwitchingThreshold = 4;
    // flag for edgeTime, if it's true the timer counter is on  
    private bool isTimerOn = false;
    //the timer counter to hold elapsed time for the passed edge
    private float edgeTime;
    private string carIDDD;
    void Start()
    {
        this.nma = GetComponentInParent<NavMeshAgent>();
        this.carInfo = btnHandler.instance.userData.carInfo;
        this.cameraOffset = transform.position - cameraGameObject.transform.position;
        this.startPoint = btnHandler.instance.userData.carInfo.startPoint;
        this.endPoint = btnHandler.instance.userData.carInfo.endPoint;
        GameManager.instance.onStartTrip += this.StartTrip;
        GameManager.instance.onStopTrip += this.StopTrip;
        displayTripInfo();
        carIDDD = btnHandler.instance.userData.carInfo.carID;
        GameManager.instance.sendMapToServerOnchanges();
    }

    string globalTripID;
    void initiateTrip(string Destination)
    {
        // send to essam dest and carID  and return tripID
        
        StartCoroutine(APIsManager.instance.getRequest(
              new paramListBuilder("carID", carIDDD).appendParam("destination", Destination).appendParam("mapID", "2").ToString()
            , APIsManager.instance.initiateTripURL
            , (response) => { this.globalTripID = response; }
            ));
    }

    void displayTripInfo()
    {
        GameManager.instance.startPoint.text = "StartPoint : " + this.carInfo.startPoint;
        GameManager.instance.endPoint.text = "EndPoint : " + this.carInfo.endPoint;
        GameManager.instance.xPos.text = "Pos : " + this.carInfo.pos;
        if (currentTrip != null) { GameManager.instance.xRoute.text = "Route : " + currentTrip.route.ToString(); }
    }
   
    void Update()
    {
        displayTripInfo();
        //toggle Trip panel on pressing [T]
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameManager.instance.TripPanel.SetActive(!GameManager.instance.TripPanel.activeSelf);
        }
        //moveToPositionUsingCameraRay(this.cameraGameObject.GetComponent<Camera>());
        if (isTimerOn) { this.edgeTime += Time.deltaTime; }
    }
    
    public void StartTrip()
    {
        initiateTrip(GameManager.instance.destPoint.text);
        StartCoroutine(getTripAndStartIt());
    }

    public void StopTrip()
    {
        if (Trip_Coroutine != null) 
        {
            StopCoroutine(Trip_Coroutine);
        }
    }
    
    private IEnumerator getTripAndStartIt()
    {
        IEnumerator TripRequest_coroutine = APIsManager.instance.getRequest(
              new paramListBuilder("carID", carIDDD).appendParam("tripID", this.globalTripID).ToString()
             ,APIsManager.instance.startTripURL
             , (TripJson) => 
             {
                 if (!string.IsNullOrEmpty(TripJson))
                 {
                     if (Trip_Coroutine == null) // no Current Trip (in the beginning)
                     {
                         Trip receivedTrip = JsonUtility.FromJson<Trip>(TripJson);
                         currentTrip = receivedTrip;
                         startTimer();
                         Trip_Coroutine = StartTrip(currentTrip);
                         StartCoroutine(Trip_Coroutine);
                     }
                     else
                     {
                         if (TripJson.Equals("0"))
                         {
                             Trip receivedTrip = JsonUtility.FromJson<Trip>(TripJson);
                             StopCoroutine(Trip_Coroutine);
                             currentTrip = receivedTrip;
                             Trip_Coroutine = StartTrip(currentTrip);
                             StartCoroutine(Trip_Coroutine);
                             OnTripChanged();
                         }
                     }   
                 }
                 else
                 {
                     Debug.Log("data has not been received from the API");
                 }
             }
        );
        yield return StartCoroutine(TripRequest_coroutine);
    }
    
    private void OnTripChanged()
    {

    }

    bool isLastNode = false;
    public IEnumerator StartTrip(Trip trip)
    {
        for (int i = 0; i < trip.route.Count; i++)
        {
            if (i == trip.route.Count-1) { isLastNode = true; }

            if (i > 0)
            {
                startPoint = trip.route[i - 1];
                endPoint = trip.route[i];
            }  
            hasStartedTowardTarget = false;
            
            yield return StartCoroutine(moveToNode(trip.route[i]));
            stopTimerAndSendIt();
            startTimer();
            hasStartedTowardTarget = false;
            // ask for new trip route and check ,if it is the same given route it continue
            // , else it stop the coroutine and start a new one with the new route
            StartCoroutine(getTripAndStartIt());
            if (i < trip.route.Count - 1)
            {
                yield return StartCoroutine(moveToNode(trip.route[i] + "-" + trip.route[i + 1]));
            }
        }
        isTimerOn = false;
        this.edgeTime = 0;
    }

    IEnumerator moveToNode(string target)
    {
        while ( Vector3.Distance(GameManager.instance.xNodes[target].position , transform.position) > targetSwitchingThreshold)
        {
            if (!hasStartedTowardTarget)
            {
                nma.SetDestination(GameManager.instance.xNodes[target].position);
                hasStartedTowardTarget = true;
            }
            this.carInfo = new CarInfo(startPoint, endPoint, transform.position);
            string carInfoJson = JsonUtility.ToJson(this.carInfo);
            IEnumerator sendCarInfo_coroutine = APIsManager.instance.getRequest(
                 new paramListBuilder("carID", carIDDD).appendParam("jsonParam", carInfoJson)
                     .appendParam("curSpeed", nma.velocity.magnitude.ToString()).ToString()
                ,APIsManager.instance.sendCarInfo_URL
                , (moveSettingJson) => {
                    moveSettings ms = JsonUtility.FromJson<moveSettings>(moveSettingJson);
                    nma.speed = ms.velocity;
                    nma.acceleration = ms.acceleration;
            });
            StartCoroutine(sendCarInfo_coroutine);
            yield return null;
        }
    }

    private void startTimer() { isTimerOn = true; }
    private IEnumerator stopTimerAndSendIt()
    {
        isTimerOn = false;
        yield return StartCoroutine(APIsManager.instance.getRequest(
            new paramListBuilder("start", this.startPoint).appendParam("end", this.endPoint).appendParam("time", edgeTime.ToString())
            .appendParam("tripID",globalTripID).appendParam("tripEnded", isLastNode.ToString()).ToString()
            , APIsManager.instance.sendEdgeTimeURL
            , (response) => { }
        ));
        this.edgeTime = 0;
    }

    private void moveToPositionUsingCameraRay(Camera camera)
    {
        if (!GameManager.instance.ctr.activeSelf && !GameManager.instance.settings.activeSelf) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit rh;
                if (Physics.Raycast(ray, out rh, Mathf.Infinity))
                {
                    CamSingleton.instance.RestoreDefaultRotation();
                    nma.SetDestination(rh.point);
                    
                }
            }
            if (Input.GetMouseButtonUp(0))
            {

            }
        }
    }

}
