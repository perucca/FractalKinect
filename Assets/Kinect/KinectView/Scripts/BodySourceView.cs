using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class BodySourceView : MonoBehaviour
{
    #region attribute
    public Material BoneMaterial;
    public GameObject BodySourceManager;
    public GameObject prefab;

    List<Kinect.Joint> armGjoint = new List<Kinect.Joint>();
    List<Kinect.Joint> armDjoint = new List<Kinect.Joint>();
    List<Kinect.Joint> legLjoint = new List<Kinect.Joint>();
    List<Kinect.Joint> legRjoint = new List<Kinect.Joint>();
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private Dictionary<Membre, Vector3> MembreVelo = new Dictionary<Membre, Vector3>();
    public GameObject KinectHandler;
    private BodySourceManager _BodyManager;
    private float timepassed = 0f;
    [SerializeField]
    private float timepassedToprint = 0f;
    [SerializeField]
    private float timetogo = 2f;

    private float timetoprint = 0f;
    private Vector3 origin;
    [SerializeField]
    private float step = 0f;
    [SerializeField]
    private float force = 100f;
   /* public Kinect.JointType handL = Kinect.JointType.HandRight;
    public Kinect.JointType handR = Kinect.JointType.HandLeft;
    public Kinect.JointType wristR = Kinect.JointType.WristRight;
    public Kinect.JointType wristL = Kinect.JointType.WristLeft;
    public Kinect.JointType elbowR = Kinect.JointType.ElbowRight;
    public Kinect.JointType elbowL = Kinect.JointType.ElbowLeft;*/
    public Membre armR;
    public Membre armL;
    public Membre legR;
    public Membre legL;
    private ulong idTrackedBody;

    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },

        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };
    #endregion

    #region start/update
    void Update()
    {
        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        Kinect.Body[] data = _BodyManager.GetData();
        if (BodySourceManager == null)
        {
            return;
        }
        if (_BodyManager == null)
        {
            return;
        }
        if (data == null)
        {
            return;
        }
        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }
            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
            }
        }
        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);
        // First delete untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }
        foreach (var body in data)
        {
          
            if (body == null)
            {
                continue;
            }
            idTrackedBody = idOfClosestBody(data);
            if (body.IsTracked && body.TrackingId == idTrackedBody)
            {
               // debugPositionZ(body);
                if (!_Bodies.ContainsKey(body.TrackingId))
                {

                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }
                RefreshBodyObject(body, _Bodies[body.TrackingId]);
                timetoprint += Time.deltaTime;
                if (timepassed == 0)
                {
                    RefreshJoint(body);
                }
                timepassed += Time.deltaTime;
                if (timepassed >= timetogo)
                {

                    //CalculVelocity(body);
                    updateMembre(body);
                    updateDictionary();
                   
                    timepassed = 0f;
                    if (!MembreVelo.ContainsKey(armL))
                    {
                        MembreVelo.Add(armL, armL.getRes());
                        MembreVelo.Add(armR, armR.getRes());
                        MembreVelo.Add(legL, legL.getRes());
                        MembreVelo.Add(legR, legR.getRes());
                    }
                    else
                    {
                        MembreVelo[armL] = armL.getRes();
                        MembreVelo[armR] = armR.getRes();
                        MembreVelo[legL] = legL.getRes();
                        MembreVelo[legR] = legR.getRes();
                    }
                }
                if (timetoprint >= timepassedToprint)
                { //callici le game object
                  /* Debug.Log("arm gauche : " + armL.getRes().magnitude);
                   Debug.Log("arm droite : " + armR.getRes().magnitude);
                   Debug.Log("leg gauche : " + legL.getRes().magnitude);
                   Debug.Log("leg droite : " + legR.getRes().magnitude);*/
                    KinectHandler.GetComponent<FractalKinectHandler>().HandleAction(MembreVelo);
                    timetoprint = 0f;
                }
            }
        }
    }
    private void Start()
    {
        armR = new Membre("BrasR");
        armL = new Membre("BrasL");
        legR = new Membre("JambeR");
        legL = new Membre("JambeL");
    }
    #endregion
    #region TrackedTheOne
    private ulong idOfClosestBody(Kinect.Body[] data)
    {
        ulong id = 0;
        float z = 100;
        foreach (var item in data)
        {
            if(item.IsTracked)
            {
               // Debug.Log(Mathf.Abs(item.Joints[Kinect.JointType.SpineBase].Position.Z));
                if (Mathf.Abs(item.Joints[Kinect.JointType.SpineBase].Position.Z) < z)
                {
                    z = item.Joints[Kinect.JointType.SpineBase].Position.Z;
                    id = item.TrackingId;
                }
            }
        }
        return id;
    }
    #endregion
    #region velocityMethod
    private void RefreshJoint(Kinect.Body body)
    {
        armDjoint.Clear();
        armGjoint.Clear();
        legLjoint.Clear();
        legRjoint.Clear();
        armGjoint.Add(body.Joints[Kinect.JointType.WristLeft]);
        armGjoint.Add(body.Joints[Kinect.JointType.ElbowLeft]);
        armGjoint.Add(body.Joints[Kinect.JointType.ShoulderLeft]);
        armDjoint.Add(body.Joints[Kinect.JointType.HandRight]);
        armDjoint.Add(body.Joints[Kinect.JointType.WristRight]);
        armDjoint.Add(body.Joints[Kinect.JointType.ElbowRight]);
        armDjoint.Add(body.Joints[Kinect.JointType.ShoulderRight]);
        legLjoint.Add(body.Joints[Kinect.JointType.HipLeft]);
        legLjoint.Add(body.Joints[Kinect.JointType.KneeLeft]);
        legLjoint.Add(body.Joints[Kinect.JointType.AnkleLeft]);
        legLjoint.Add(body.Joints[Kinect.JointType.FootLeft]);
        legRjoint.Add(body.Joints[Kinect.JointType.HipRight]);
        legRjoint.Add(body.Joints[Kinect.JointType.KneeRight]);
        legRjoint.Add(body.Joints[Kinect.JointType.AnkleRight]);
        legRjoint.Add(body.Joints[Kinect.JointType.FootRight]);

    }
    private bool IsTracked(Kinect.JointType jt, Kinect.Body body)
    {
        return (body.Joints[jt].TrackingState == Kinect.TrackingState.Tracked);
    }
    private void updateMembre(Kinect.Body body)
    {
        if (isMembreTracked(armL, body))
        {
            armL.Update(armGjoint);
        }
        else
        {
            //Debug.Log("ArmL not tracked");
            armL.setRes(new Vector3(0, 0, 0));
        }
        if (isMembreTracked(armR, body))
        {
            armR.Update(armDjoint);
        }
        else
        {
            //  Debug.Log("ArmR not tracked");
            armR.setRes(new Vector3(0, 0, 0));
        }
        if (isMembreTracked(legL, body))
        {
            legL.Update(legLjoint);
        }
        else
        {
            // Debug.Log("LegL not tracked");
            legL.setRes(new Vector3(0, 0, 0));
        }
        if (isMembreTracked(legR, body))
        {

            legR.Update(legRjoint);
        }
        else
        {
            //Debug.Log("LegR not tracked");
            legR.setRes(new Vector3(0, 0, 0));
        }
    }
    private void updateDictionary()
    {
        MembreVelo[armR] = armR.getRes();
        MembreVelo[armL] = armL.getRes();
        MembreVelo[legR] = legR.getRes();
        MembreVelo[legL] = legL.getRes();
    }
    private bool isMembreTracked(Membre m, Kinect.Body body)
    {

        bool res = true;
        foreach (var item in m.getCurrent())
        {
            if (!(body.Joints[item.JointType].TrackingState == Kinect.TrackingState.Tracked))
            {
                res = false;
            }
        }
        return res;
    }
    private bool isHandClosed(Kinect.Body body)
    {
        return (body.HandRightState == Kinect.HandState.Closed);
    }
   /* private void initialOrigin(Kinect.Body body)
    {
        origin.x = body.Joints[handR].Position.X;
        origin.y = body.Joints[handR].Position.Y;
        origin.z = body.Joints[handR].Position.Z;
    }
    public void CalculVelocity(Kinect.Body body)
    {
        Vector3 newpos = new Vector3(body.Joints[handR].Position.X, body.Joints[handR].Position.Y, body.Joints[handR].Position.Z);
        Vector3 velocity = newpos - origin;
        Vector3 oneframedVelo = velocity;
        Debug.Log("Origin : " + origin);
        Debug.Log(velocity + " / " + timepassed + " => " + oneframedVelo.magnitude);
        if (oneframedVelo.magnitude >= step)
        {
            Debug.Log("drawing line from " + origin + " to " + newpos);
            Debug.DrawLine(origin, newpos, Color.yellow, 2f);
            Vector3 velocityNormal = oneframedVelo.normalized;
            Rigidbody rigidBody = Instantiate(prefab, newpos, Quaternion.Euler(velocityNormal)).GetComponent<Rigidbody>();

            rigidBody.AddForce(oneframedVelo * force);
        }

    }*/
    public Vector3 vectorCalculator()
    {
        return origin;
    }
    #endregion
    #region initialScript

    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);

        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.material = BoneMaterial;
            lr.SetWidth(0.05f, 0.05f);

            jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;
        }
        body.SetActive(false);
        return body;
    }

    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;
            if (_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
            }

            Transform jointObj = bodyObject.transform.Find(jt.ToString());
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);

            LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            if (targetJoint.HasValue)
            {
                lr.SetPosition(0, jointObj.localPosition);
                lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
                lr.SetColors(GetColorForState(sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));
            }
            else
            {
                lr.enabled = false;
            }
        }
    }

    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
            case Kinect.TrackingState.Tracked:
                return Color.green;

            case Kinect.TrackingState.Inferred:
                return Color.red;

            default:
                return Color.black;
        }
    }

    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }

    #endregion
    #region sysoutdebug
    private int nbreBodyTracked(Kinect.Body[] data)
    {
        int i = 0;
        foreach (var item in data)
        {
            if (item.IsTracked)
            {
                i++;
            }
        }
        return i;
    }
    private void debugPositionZ(Kinect.Body body)
    {
        Debug.Log(body.TrackingId + " : " + body.Joints[Kinect.JointType.SpineBase].Position.Z);
    }
    #endregion
}
