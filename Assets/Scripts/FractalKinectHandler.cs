using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalKinectHandler : MonoBehaviour
{
    [SerializeField] private FractalSpawner[] leftFoot;
    [SerializeField] private FractalSpawner[] rightFoot;
    [SerializeField] private FractalSpawner[] leftHand;
    [SerializeField] private FractalSpawner[] rightHand;


    [SerializeField] private  int seuil_min;
    [SerializeField] private  int seuil_medium;
    [SerializeField] private  int seuil_strong;
    [SerializeField] private  int seuil_max;


     public enum STRENGTH_MOVEMENT {
        MIN=1, MEDIUM=2, STRONG=3, MAX=4
    }
 	private void Start () {
        Vector3 vector = new Vector3(255,255,255) - new Vector3(1,1,1);
       for(int i=0; i<5; i++) {
        rightHand[i].SpawnFractal(vector, Random.Range(0,5));
        rightHand[i].SpawnFractal(vector, Random.Range(0,5));
        rightHand[i].SpawnFractal(vector, Random.Range(0,5));
        rightHand[i].SpawnFractal(vector, Random.Range(0,5));
       }
	}

    public void handleAction(Dictionary<string, Vector3> kinectActions) {
        foreach(KeyValuePair<string, Vector3> item in kinectActions) {
           
            break;
        }
    }

}
