using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class paramListBuilder
    {
        private string URL;
        public paramListBuilder(string key, string value)
        {
            URL = "?" + key + "=" + value;
        }

        public paramListBuilder appendParam(string key, string value)
        {
            URL += ("&" + key + "=" + value);
            return this;
        }

        public override string ToString()
        {
            return this.URL;
        }
    }

