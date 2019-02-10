using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalSpawner : MonoBehaviour
{
    [SerializeField] private  Mesh[] meshes;
    [SerializeField] private  Material[] materials;
    [SerializeField] private  int max_floor = 10;
    [SerializeField] private  float child_scale = 0.5f;
    [SerializeField] private  float angle_min = -25.0f;
    [SerializeField] private  float angle_max = 25.0f;

    private List<GameObject> fractal_floors = new List<GameObject>();
 
    public void SpawnFractal(Vector3 movement, int depth) {
                
        Debug.Log("SpawnFractal: "+ gameObject.name);
        Mesh mesh = chooseMesh();
        Material material = chooseMaterial();
 
       	GameObject go = new GameObject("Fractal");
        go.AddComponent<Fractal>().SetProperties(transform, depth, child_scale, mesh, material);


        if(fractal_floors.Count > 0) {
            Transform previousFractal = fractal_floors[fractal_floors.Count-1].transform;
            float angle = Random.Range(angle_min,angle_max);

		    Quaternion rotation = Quaternion.Euler(0,0,angle);
		    Vector3 direction = rotation * Vector3.up*fractal_floors.Count*go.transform.localScale.z;
            go.transform.Rotate(Vector3.up*angle, Space.World);
            go.transform.localPosition += direction; 


            // go.GetComponent<Fractal>().Rotate(Random.Range(angle_min,angle_max));
        } 

        fractal_floors.Add(go); 
       

        if(fractal_floors.Count >= max_floor) {
            //Démarrer cooroutine de destruction
            fractal_floors.Clear();
        }
    }


    private Material chooseMaterial() {
        return materials[Random.Range(0,materials.Length)];
    }

    private Mesh chooseMesh() {
        return meshes[Random.Range(0,meshes.Length)];
    }
}
