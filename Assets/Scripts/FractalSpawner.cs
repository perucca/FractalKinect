using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalSpawner : MonoBehaviour
{
    [SerializeField] private  Mesh[] meshes;
    [SerializeField] private  Material[] materials;
    [SerializeField] private  int max_floor = 5;
    [SerializeField] private  float child_scale = 0.2f;

    private List<GameObject> fractal_floors = new List<GameObject>();


    public void SpawnFractal(Vector3 movement, int depth) {
                
        Debug.Log("SpawnFractal: "+ gameObject.name);
        Mesh mesh = chooseMesh();
        Material material = chooseMaterial();
 
       	GameObject go = new GameObject("Fractal");
        go.AddComponent<Fractal>().setProperties(transform, depth, child_scale, mesh, material);
		go.transform.localPosition = Vector3.up* fractal_floors.Count;
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
