﻿using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour
{
    public float SpringConstant = 10; //k.s
    public float DampingFactor = 10;  //k.d
    public float RestLength;
    public float breakLength = 100000;
    [SerializeField]
    GameObject _goA;
    [SerializeField]
    GameObject _goB;

    public GameObject goA
    {
        get { return _goA; }
        set
        {
            _goA = value;
            A = value.GetComponent<Node>();
        }
    }
    public GameObject goB
    {
        get { return _goB; }
        set
        {
            _goB = value;
            B = value.GetComponent<Node>();
        }
    }
    Node A, B;

    void OnEnable()
    {
        if (goA != null && goB != null && A == null && B == null)
        {
            A = goA.GetComponent<Node>();
            B = goB.GetComponent<Node>();
            RestLength = Vector3.Magnitude(goA.transform.position - goB.transform.position);
        }
  
        if (!GetComponent<LineRenderer>())
            gameObject.AddComponent<LineRenderer>();

        Material a = FindObjectOfType<LineRenderer>().material;

        GetComponent<LineRenderer>().material = a;
        GetComponent<LineRenderer>().SetColors(Color.red, Color.red);
        GetComponent<LineRenderer>().SetWidth(.1f, .1f);
    }

    void Update()
    {
        //if (goA != null && goB != null && A == null && B == null)
        //{
        //    RestLength = Vector3.Magnitude(goA.transform.position - goB.transform.position);
        //    A = goA.GetComponent<Node>();
        //    B = goB.GetComponent<Node>();
        //}
        if (goA != null && goB != null)
        {

           // print("Hello");

            // Debug.DrawLine(goA.transform.position, goB.transform.position);

            Vector3 middle;

            middle.x = (goA.transform.position.x + goB.transform.position.x) / 2;
            middle.y = (goA.transform.position.y + goB.transform.position.y) / 2;
            middle.z = (goA.transform.position.z + goB.transform.position.z) / 2;

            gameObject.transform.position = middle;

            GetComponent<LineRenderer>().SetPosition(0, goA.transform.position);
            GetComponent<LineRenderer>().SetPosition(1, goB.transform.position);

            CalForce();



            MoveNode(A);
            MoveNode(B);
        }
        else Destroy(gameObject);
    }

    Vector3 Ftotal = Vector3.zero;
    
    public void CalForce()
    {
        //print("something");
        if (goA != null && goB != null)
        {
            //computes the unity length vector e from goA to goB
            //then computes the distance
            Vector3 vectorDistance = goB.transform.position - goA.transform.position;
          
            
            float abMag = Vector3.Magnitude(vectorDistance);
            if (abMag < breakLength)
            {
                Vector3 e = vectorDistance / abMag;

                float Fs = 0;
                float Fd = 0;
                Vector3 Fg = new Vector3(0, -1.8f, 0);


                //gets the 1d velocity
                float aVel = Vector3.Dot(e, A.vel);
                float bVel = Vector3.Dot(e, B.vel);


                ///Computes the forces
                Fs = -SpringConstant * (RestLength - abMag /*- RestLength*/);
                Fd = -DampingFactor * (aVel - bVel);
                //    print(Vector3.Magnitude(Ftotal));

                Ftotal = (Fs * e) + (Fd * e);

                ///Total's up the forces

                // Ftotal = 
                //print("something20");

                ///Gives the node's their new force
                A.frc = Ftotal + Fg + A.airfrc;
                B.frc = -Ftotal + Fg + A.airfrc;

            }
            else Destroy(gameObject);
            
        }
        
        

    }


    static public void MoveNode(Node a)
    {
        if (!a.GetComponent<Node>().isAnchor)
        {
            Vector3 newPos;
            a.acl = a.frc / a.mass;
            a.vel += a.frc * Time.fixedDeltaTime;
           newPos = a.vel * Time.fixedDeltaTime;
            if(newPos.x + a.transform.position.x > 1000 ||newPos.y + a.transform.position.y > 1000 || newPos.z + a.transform.position.z > 1000)
            { Destroy(a.gameObject); }
            else 
            a.transform.position += newPos;
           // a.frc = Vector3.zero;
         }
    }



    static public GameObject MakeSpring(GameObject A, GameObject B, GameObject springPrefab)
    {
        GameObject spring = new GameObject("Spring: " + A.name + "->" + B.name);
        spring.AddComponent<Spring>();
        spring.GetComponent<Spring>().goA = A;
        spring.GetComponent<Spring>().goB = B;
        return spring;
    }
    static public GameObject MakeSpring(GameObject A, GameObject B, GameObject springPrefab, float springConst, float dampFactor)
    {
        GameObject spring = new GameObject("Spring: " + A.name + "->" + B.name);
        spring.AddComponent<Spring>();
        spring.GetComponent<Spring>().goA = A;
        spring.GetComponent<Spring>().goB = B;
        spring.GetComponent<Spring>().SpringConstant = springConst;
        spring.GetComponent<Spring>().DampingFactor = dampFactor;
        return spring;
    }

}
