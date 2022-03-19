using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class btnHandler : Singleton<btnHandler>
{
    public InputField log_id_Field;
    public InputField log_pw_Field;

    public InputField reg_id_Field;
    public InputField reg_pw_Field;

    public Text RegErrorLogger;
    public Text LogErrorLogger;

    public GameObject logForm;
    public GameObject RegForm;
    // user data after logging in
    public User userData;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        logForm.SetActive(true);
        RegForm.SetActive(false);
        RegErrorLogger.text = "";
        LogErrorLogger.text = "";
    }
    
    void Update()
    {

    }
    
    private void resetRegisterFields()
    {
        reg_id_Field.text = "";
        reg_pw_Field.text = "";
    }

    private void resetLogFields()
    {
        log_id_Field.text = "";
        log_pw_Field.text = "";
    }

    public void backArrow()
    {
        resetRegisterFields();
        logForm.SetActive(true);
        RegForm.SetActive(false);
    }
    
    public void LogInBTN()
    {
        if (!(string.IsNullOrEmpty(log_id_Field.text) || string.IsNullOrEmpty(log_pw_Field.text)))
        {
            IEnumerator Login_Corotine = APIsManager.instance.getRequest(
                    new paramListBuilder("loginData", JsonUtility.ToJson(new logInData(log_id_Field.text, log_pw_Field.text))).ToString()
                  , APIsManager.instance.LogIn_URL
                  , (json) =>
                  {
                      if (json.Equals("wrongLoginInfo"))
                      {
                          LogErrorLogger.text = "Wrong ID or PW";
                      }
                      else
                      {
                          this.userData = JsonUtility.FromJson<User>(json); 
                          SceneManager.LoadScene("SmartCity");
                      }
                  }
            );
            StartCoroutine(Login_Corotine);
        }
        else
        {
            LogErrorLogger.text = "Fields are required";
        }
    }

    public void ReigesterBTN()
    {
        resetLogFields();
        logForm.SetActive(false);
        RegForm.SetActive(true);
    }
    
    // send mapID with the registeration
    private void CompleteRegisteration(string carModel)
    {
        Debug.Log(JsonUtility.ToJson(new RegisterData(reg_id_Field.text, reg_pw_Field.text, carModel)));
        if (!(string.IsNullOrEmpty(reg_id_Field.text) || string.IsNullOrEmpty(reg_pw_Field.text)))
        {
            if (reg_id_Field.text.Length >= 5 && reg_pw_Field.text.Length >= 5)
            {
                IEnumerator Reg_Corotine =  APIsManager.instance.getRequest(
                    new paramListBuilder("RegData", JsonUtility.ToJson(new RegisterData(reg_id_Field.text, reg_pw_Field.text, carModel)))
                    .appendParam("mapID","2").ToString()           
                    , APIsManager.instance.Register_URL, (json) =>
                    {
                        if (json.Equals("existed"))
                        {
                            RegErrorLogger.text = "ID has Registered Before";
                        }
                        else
                        {
                            this.userData = JsonUtility.FromJson<User>(json);
                            SceneManager.LoadScene("SmartCity");
                        }
                    });
                StartCoroutine(Reg_Corotine);
            }
            else
            {
                RegErrorLogger.text = "ID and PW must be 5-Chars at least";
            }
        }
        else
        {
            LogErrorLogger.text = "Fields are required";
        }
    }

    public void xCar0()
    {
        CompleteRegisteration("Car_4");
    }
    public void xCar1()
    {
        CompleteRegisteration("Car_3");
    }
    public void xCar2()
    {
        CompleteRegisteration("Car_2");
    }
    public void xCar3()
    {
        CompleteRegisteration("Police_car");
    }
    public void xCar4()
    {
        CompleteRegisteration("Car_1");
    }

}
