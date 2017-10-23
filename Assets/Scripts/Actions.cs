using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour {

    [SerializeField]
    protected string actionName;

    public string ActionName { get { return actionName; } }

    public virtual void PerformAction() {
        
    }

}
