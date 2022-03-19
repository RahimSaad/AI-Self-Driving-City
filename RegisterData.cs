using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class RegisterData
{
    public string name, pw, carModel;
    public RegisterData(string name, string pw, string carModel)
    {
        this.name = name;
        this.pw = pw;
        this.carModel = carModel;
    }
}