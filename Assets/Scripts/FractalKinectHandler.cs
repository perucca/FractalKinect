using System.Collections.Generic;
using UnityEngine;

public class FractalKinectHandler : MonoBehaviour
{
    [SerializeField] private FractalSpawner[] leftFoot;
    [SerializeField] private FractalSpawner[] rightFoot;
    [SerializeField] private FractalSpawner[] leftHand;
    [SerializeField] private FractalSpawner[] rightHand;


    [SerializeField] private int seuil_min;
    [SerializeField] private int seuil_medium;
    [SerializeField] private int seuil_strong;
    [SerializeField] private int seuil_max;


    public void SpawnManual()
    {
        foreach (var membre in leftHand)
        {
            membre.SpawnFractal(Vector3.zero, 5);
        }

        foreach (var membre in rightHand)
        {
            membre.SpawnFractal(Vector3.zero, 5);
        }
        foreach (var membre in leftFoot)
        {
            membre.SpawnFractal(Vector3.zero, 5);
        }
        foreach (var membre in rightFoot)
        {
            membre.SpawnFractal(Vector3.zero, 5);
        }

    }

    public void HandleAction(Dictionary<Membre, Vector3> kinectActions)
    {
        foreach (KeyValuePair<Membre, Vector3> item in kinectActions)
        {

            int depth = TransformeMagnitude(item.Value.magnitude);

            if (depth == 0)
            {
                Debug.Log("Ignored.");
                return;
            }

            switch (item.Key.getName())
            {
                case "BrasL":
                    foreach (var membre in leftHand)
                    {
                        membre.SpawnFractal(item.Value, depth);
                    }
                    break;

                case "BrasR":
                    foreach (var membre in rightHand)
                    {
                        membre.SpawnFractal(item.Value, depth);
                    }
                    break;

                case "JambeL":
                    foreach (var membre in leftFoot)
                    {
                        membre.SpawnFractal(item.Value, depth);
                    }
                    break;
                case "JambeR":
                    foreach (var membre in rightFoot)
                    {
                        membre.SpawnFractal(item.Value, depth);
                    }
                    break;
            }

        }
    }

    private int TransformeMagnitude(float magnitude)
    {
        if (magnitude < seuil_min) return 0;
        if (magnitude < seuil_medium) return 1;
        if (magnitude < seuil_strong) return 2;
        if (magnitude < seuil_max) return 3;

        return 4;
    }
}
