﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour
{

   //public Vector3 pos; //Position
   public Vector3 vel; //Velocity
   public Vector3 acl; //Acceleration
   public Vector3 mom; //Momentum
   public Vector3 frc; // Force

   public Vector3 airfrc = Vector3.zero;

    public Vector3 mouseFrc = Vector3.zero;

   public  float mass =1;

    public bool isAnchor = false;
    private List<GameObject> _list = new List<GameObject>();
    public List<GameObject> list
    {
        get {return  _list; }

        set {
            if (this != null)
            {
                _list = value;
                if (_list.Count < 1) Destroy(gameObject);
            }
        }

    }

  

    void Awake()
    {

        vel = acl = frc = mom = Vector3.zero;
        mass += .0000001f;
    }



   
    //static public GameObject MakeNode(bool anchor, PrimitiveType type)
    //{
    //    GameObject newNode = GameObject.CreatePrimitive(type);
    //    new 
    //    return 
    //}



}
