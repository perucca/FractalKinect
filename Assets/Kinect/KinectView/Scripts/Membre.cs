using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;

public class Membre
{
    private string name;
    public string getName()
    {
        return name;
    }
    private List<Kinect.Joint> currentJoints;
    public List<Kinect.Joint> getCurrent()
    {
        return currentJoints;
    }
    private List<Kinect.Joint> oldJoints;
    private Dictionary<Kinect.JointType, Vector3> newVectors;
    private Dictionary<Kinect.JointType, Vector3> oldVectors;
    private Vector3 res = new Vector3();

    public void Update(List<Kinect.Joint> joints)
    {
       
        oldJoints.Clear();
        oldJoints = cpdict(currentJoints);
        
        // Debug.Log(stringJold(currentJoints));
        currentJoints.Clear();
       // Debug.Log(joints.Count);
        currentJoints = cpdict(joints);
       // Debug.Log(stringJ(joints));
        movedLevel();
    }
    public string stringJold(List<Kinect.Joint> joints)
    {
        string res = "{ current ";
        foreach (var item in joints)
        {
            res += item.Position.X.ToString() + ";";
        }
        res += " }";
        return res;
    }
    public string stringJ(List<Kinect.Joint> joints)
    {
        string res = "{ joints ";
        foreach (var item in joints)
        {
            res += item.Position.X.ToString() + ";";
        }
        res += " }";
        return res;
    }

    public List<Kinect.Joint> cpdict(List<Kinect.Joint> list)
    {
        List<Kinect.Joint> res = new List<Kinect.Joint>();
        foreach (var item in list)
        {
            res.Add(item);
        }
        return res;
    }
    public Vector3 getRes()
    {
        return res;
    }
    public void setRes(Vector3 res)
    {
        this.res = res;
    }
    public Membre(string name)
    {
        this.name = name;
        currentJoints = new List<Kinect.Joint>();
        oldJoints = new List<Kinect.Joint>();
        oldVectors = new Dictionary<Kinect.JointType, Vector3>();
        newVectors = new Dictionary<Kinect.JointType, Vector3>();
    }
    public Membre(string name, List<Kinect.Joint> joints)
    {
        this.name = name;
        currentJoints = new List<Kinect.Joint>();
        oldJoints = new List<Kinect.Joint>();
        oldVectors = new Dictionary<Kinect.JointType, Vector3>();
        newVectors = new Dictionary<Kinect.JointType, Vector3>();
    }
    public void movedLevel()
    {
        Vector3 somme = new Vector3();
        foreach (Kinect.Joint item in currentJoints)
        {
            Vector3 vector3 = new Vector3(item.Position.X * 10, item.Position.Y * 10, item.Position.Z * 10);
            if (!newVectors.ContainsKey(item.JointType))
            {
                newVectors.Add(item.JointType, vector3);
            }
            else
            {
                newVectors[item.JointType] = vector3;


            }

        }
        foreach (Kinect.Joint item in oldJoints)
        {
            Vector3 vector3 = new Vector3(item.Position.X * 10, item.Position.Y * 10, item.Position.Z * 10);

            if (!oldVectors.ContainsKey(item.JointType))
            {
                oldVectors.Add(item.JointType, vector3);
            }
            else
            {
                oldVectors[item.JointType] = vector3;

            }
        }
        foreach (KeyValuePair<Kinect.JointType, Vector3> newItem in newVectors)
        {
            Vector3 origin = new Vector3();
            if (oldVectors.TryGetValue(newItem.Key, out origin))
            {
                Vector3 newpos = newItem.Value;
                Vector3 velocity = newpos - origin;
                somme += velocity;
            }
        }

        res = somme;
    }

}
