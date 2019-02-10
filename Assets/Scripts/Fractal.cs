using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{
	[SerializeField] private int max_depth;
    [SerializeField] private float child_scale;
    
	private Mesh mesh;
	private Material material;
    private int depth;
	private static Vector3[] childDirections = {
		Vector3.back,
		Vector3.right,
		Vector3.left,
		Vector3.forward,
		Vector3.up,
		Vector3.down
	};

	private static Vector3[] rootDirections = {
		Vector3.back,
		Vector3.right,
		Vector3.left,
		Vector3.forward
	};


 	private void Start () {
		gameObject.AddComponent<MeshFilter>().mesh = mesh;
		gameObject.AddComponent<MeshRenderer>().material = material;
        if (depth < max_depth) {
			if(depth == 0) {
				StartCoroutine(CreateChildren(rootDirections));
			} else {
				StartCoroutine(CreateChildren(childDirections));
			}
		}
	}
    
	private IEnumerator CreateChildren (Vector3[] directions) {
		for (int i = 0; i < directions.Length; i++) {
			yield return new WaitForSeconds(0.01f);
			new GameObject("Fractal Child").AddComponent<Fractal>().
				Initialize(this, directions[i]);
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
