using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class logInData
{
    public string name, pw;
    public logInData(string name, string pw)
    {
        this.name = name;
        this.pw = pw;
    }
}