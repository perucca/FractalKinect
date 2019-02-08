using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{
	public Mesh mesh;
	public Material material;
	public int max_depth;
    public int depth;
    public float child_scale;
    
	private static Vector3[] childDirections = {
		Vector3.up,
		Vector3.right,
		Vector3.left
	};

	private static Quaternion[] childOrientations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 0f, -90f),
		Quaternion.Euler(0f, 0f, 90f)
	};

 	private void Start () {
		gameObject.AddComponent<MeshFilter>().mesh = mesh;
		gameObject.AddComponent<MeshRenderer>().material = material;
        if (depth < max_depth) {
			new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.forward);
			new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.right);
			new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.left);
			new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.back);
		}
	}

    

	public void Initialize (Fractal parent, Vector3 direction) {
		setProperties(parent.transform, parent.max_depth, parent.child_scale, parent.mesh, parent.material);
		
		depth = parent.depth + 1;
        transform.localScale = Vector3.one * child_scale;
		transform.localPosition = direction * (0.5f + 0.5f * child_scale);
	}

    // Settings
    public void setProperties (Transform origin, int max_depth, float child_scale, Mesh mesh, Material material) {

		this.mesh = mesh;
		this.material = material;
		this.max_depth = max_depth;
		this.depth = 0;
        transform.parent = origin;
        this.child_scale = child_scale;
	}
}
