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


 	private void Start () {
       // Vector3 vector = new Vector3(255,255,255) - new Vector3(1,1,1);
     /*  for(int i=0; i<rightHand.Length; i++) {
           for(int j=0; j<6; j++) {
                rightHand[i].SpawnFractal(vector, Random.Range(1,5));
           }
       }*/
	}

    public void HandleAction(Dictionary<Membre, Vector3> kinectActions) {
        foreach(KeyValuePair<Membre, Vector3> item in kinectActions) {

            int depth = TransformeMagnitude(item.Value.magnitude);

            if (depth == 0)
            {
                Debug.Log("Ignored.");
                return;
            }

            switch (item.Key.getName())
            {
                case "BrasL":
                    leftHand[Random.Range(0, leftHand.Length)].SpawnFractal(item.Value, depth);
                    break;

                case "BrasR":
                    rightHand[Random.Range(0, rightHand.Length)].SpawnFractal(item.Value, depth);
                    break;

                case "JambeL":
                    leftFoot[Random.Range(0, leftFoot.Length)].SpawnFractal(item.Value, depth);
                    break;
                case "JambeR":
                    rightFoot[Random.Range(0,rightFoot.Length)].SpawnFractal(item.Value, depth);
                    break;
            }

        }
    }

    private int TransformeMagnitude(float magnitude)
    {
        if (magnitude < seuil_min) return 0;
        if (magnitude < seuil_medium) return 1;
        if (magnitude < seuil_max) return 2;
        if (magnitude < seuil_strong) return 3;

        return 4;
    }
}
