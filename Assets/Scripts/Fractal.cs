using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{
	[SerializeField] private float max_depth;
    [SerializeField] private float child_scale;
    
	private Mesh mesh;
	private Material material;
    private float depth;

	private static Vector3[] rootDirections = {
		Vector3.back,
		Vector3.right,
		Vector3.left,
		Vector3.forward
	};


	private static Quaternion[] rootOrientations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 0f, -90f),
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.identity,
	};

	private static Vector3[] childOddDirections = {
		Vector3.right,
		Vector3.left,
		Vector3.forward,
		// Vector3.back,
		// Vector3.up,
		// Vector3.down
	};
	private static Quaternion[] childOddOrientations = {
		Quaternion.Euler(0f, 0f, -90f),
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(90f, 0f, 0f),
		// Quaternion.Euler(-90f, 0f, 0f)
		// Quaternion.identity,
		// Quaternion.identity,

	};

     private static Vector3[] childEvenDirections = {
//		Vector3.right,
//		Vector3.left,
//		Vector3.forward,
		Vector3.back,
		Vector3.up,
		// Vector3.down
	};
	private static Quaternion[] childEvenOrientations = {
		// Quaternion.Euler(0f, 0f, -90f),
		// Quaternion.Euler(0f, 0f, 90f),
		// Quaternion.Euler(90f, 0f, 0f),
		Quaternion.Euler(-90f, 0f, 0f),
		Quaternion.identity,
		// Quaternion.identity,
	};

 	private void Start () {
		gameObject.AddComponent<MeshFilter>().mesh = mesh;
		gameObject.AddComponent<MeshRenderer>().material = material;
        if (depth < max_depth) {
			if(depth == 0) {
				StartCoroutine(CreateChildren(rootDirections, rootOrientations));
			} else {
				if(depth%2 == 0) {
					StartCoroutine(CreateChildren(childOddDirections, childOddOrientations));
				} else {
					StartCoroutine(CreateChildren(childEvenDirections, childEvenOrientations));
				}
			}
		}
	}
    
	private IEnumerator CreateChildren (Vector3[] directions, Quaternion[] orientations) {
		for (int i = 0; i < directions.Length; i++) {
			yield return new WaitForSeconds(0.05f);
			new GameObject("Fractal Child").AddComponent<Fractal>().
				Initialize(this, directions[i], orientations[i]);
		}
	}
	public void Initialize (Fractal parent, Vector3 direction, Quaternion orientation) {
		SetProperties(parent.transform, parent.max_depth, parent.child_scale, parent.mesh, parent.material);
		
		depth = parent.depth + 1;
        transform.localScale = Vector3.one * child_scale;
		transform.localPosition = direction * (0.5f + 0.5f * child_scale);
		transform.localRotation = orientation;
	}

    // Settings
    public void SetProperties (Transform origin, float max_depth, float child_scale, Mesh mesh, Material material) {

		/*
        transform.parent = origin;
		transform.localScale = origin.localScale;*/
		transform.SetParent(origin, false);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;

		this.mesh = mesh;
		this.material = material;
		this.max_depth = max_depth;
		this.depth = 0;
        this.child_scale = child_scale;
	}

   public void Rotate(float angle)
    {

    }

	public void Update() {
		 if(depth == 0) {
		 	transform.Rotate(Vector3.up* 5 * Time.deltaTime);
		 }
	}
}
