using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIsManager : Singleton<APIsManager>
{

    public delegate void onGettingResponse(string json);
    
    public readonly string startTripURL = "http://sweeney-001-site1.htempurl.com/api/agent/getRoute";
    public readonly string sendCarInfo_URL = "http://kairyessam.pythonanywhere.com/moveTOnode";
    public readonly string LogIn_URL = "http://kairyessam.pythonanywhere.com/login";
    public readonly string Register_URL = "http://kairyessam.pythonanywhere.com/registration";
    public readonly string postMap_URL = "http://kairyessam.pythonanywhere.com/put_new_graph";
    public readonly string sendEdgeTimeURL = "http://kairyessam.pythonanywhere.com/trip_initiation";


    public readonly string initiateTripURL = "http://kairyessam.pythonanywhere.com/trip_initiation";

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public IEnumerator getRequest(string paramList, string baseURL, onGettingResponse xCallBack)
    {
        
        UnityWebRequest uwr = UnityWebRequest.Get(baseURL + paramList);
        yield return uwr.SendWebRequest();
        Debug.Log(uwr.downloadHandler.text);
        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.LogError(uwr.error);
        }
        else
        {
            xCallBack(uwr.downloadHandler.text);
        }
    }

    //public IEnumerator postRequest(List<IMultipartFormSection> paramList, string Post_URL, onGettingResponse xCallBack)
    //{
    //    Debug.Log("entered web");
    //    UnityWebRequest uwr = UnityWebRequest.Post(Post_URL, paramList);
    //    yield return uwr.SendWebRequest();

    //    if (uwr.isNetworkError || uwr.isHttpError)
    //    {
    //        Debug.LogError(uwr.error);
    //    }
    //    else
    //    {
    //        xCallBack(uwr.downloadHandler.text);
    //    }
    //}


}
