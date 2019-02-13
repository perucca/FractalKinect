using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalSpawner : MonoBehaviour
{
    [SerializeField] private Mesh[] meshes;
    [SerializeField] private Material[] materials;
    [SerializeField] private int max_floor = 10;
    [SerializeField] private float child_scale = 0.5f;
    [SerializeField] private float angle_min = -25.0f;
    [SerializeField] private float angle_max = 25.0f;
    [SerializeField] private float size_ratio = 1.0f;


    private List<GameObject> fractal_floors = new List<GameObject>();

    public void SpawnFractal(Vector3 movement, float depth)
    {

        Debug.Log("SpawnFractal: " + gameObject.name);
        Mesh mesh = chooseMesh();
        Material material = chooseMaterial();

        if (fractal_floors.Count >= max_floor)
        {
            Destroy(fractal_floors[0]);
            fractal_floors.Clear();
        }

        GameObject go = new GameObject("Fractal");


        if (fractal_floors.Count > 0)
        {
            Transform previousFractal = fractal_floors[fractal_floors.Count - 1].transform;
            go.AddComponent<Fractal>().SetProperties(previousFractal, depth, child_scale, mesh, material);
            float angle = Random.Range(angle_min, angle_max);

            //Quaternion rotation = Quaternion.Euler(0,0,angle);
            go.transform.Rotate(Vector3.right * angle);
            go.transform.localPosition = (previousFractal.localScale.z) * size_ratio * previousFractal.transform.up;
            //Vector3 direction = rotation * previousFractal.transform.forward;
            // go.transform.localPosition = direction; 


            // go.GetComponent<Fractal>().Rotate(Random.Range(angle_min,angle_max));
        }
        else
        {
            go.AddComponent<Fractal>().SetProperties(transform, depth, child_scale, mesh, material);
        }

        fractal_floors.Add(go);



    }

    private Material chooseMaterial()
    {
        return materials[Random.Range(0, materials.Length)];
    }

    private Mesh chooseMesh()
    {
        return meshes[Random.Range(0, meshes.Length)];
    }
}
