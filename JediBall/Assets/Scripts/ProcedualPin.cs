// Toshiaki Koike-Akino, Jan. 2017
using System.Collections;
using UnityEngine;

//http://catlikecoding.com/unity/tutorials/rounded-cube/
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[ExecuteInEditMode]
public class ProcedualPin : MonoBehaviour {
	private Mesh mesh = null;
	private Vector3[] vertices;
	private Vector2[] uvs;
	private int[] triangles;

	public int nSides = 8;
	public int nFloors = 10;
	public float height = 2f;
	public float width = 0.35f;

	// Pin radius approx: F(x)=a+b*x+c*x^2+d*x^3+e*x^4+f*x^5+g*x^6
	// a=2.03581, b=1.21807, c=-0.301886, d=0.0948147, e=-0.0190656, f=0.00158241, g=-4.46443e-5
	private float GetRadius(float x) {
		x *= 15f / height;
		float a=2.03581f, b=1.21807f, c=-0.301886f, d=0.0948147f, e=-0.0190656f, f=0.00158241f, g=-0.0000446443f;
		return ((((((g*x+f)*x+e)*x+d)*x+c)*x+b)*x+a) * width / 4.8f;
	}

	private void CreateVertices() {
		// side x floor + top + bottom
		vertices = new Vector3 [(nSides + 1) * nFloors + 2]; // floor height
		uvs = new Vector2 [vertices.Length];

		float hFloor = height / nFloors; // floor height
		int v = 0;
		// bottom
		uvs [v] = Vector2.zero;
		vertices [v++] = Vector3.zero;
		// sides
		for (int k = 0; k < nFloors; k++) {
			float height = (float)k * hFloor;
			float radius = GetRadius (height);
			for (int i = 0; i <= nSides; i++) {
				float theta = (float)i / nSides * Mathf.PI * 2f;
				uvs [v] = new Vector2 ((float) i / nSides, (float) k / nFloors);
				vertices [v++] = new Vector3 (Mathf.Cos (theta) * radius, height, Mathf.Sin (theta) * radius);
			}
		}
		// top
		uvs[v] = Vector2.up;
		vertices[v] = Vector3.up * height;

		// centering
		for (v = 0; v < vertices.Length; v++) {
			vertices [v] -= Vector3.up * height * 0.5f;
		}

		// 
		mesh.vertices = vertices;
		mesh.uv = uvs;
	}

	private void CreateTriangles() {
		// Sides, bottom, top
		int nTriangles = nSides * (nFloors-1) * 2 + nSides * 2;
		triangles = new int [nTriangles * 3];
		int i = 0;
		// bottom
		for (int v = 0; v < nSides; v++) {
			triangles [i++] = 0;
			triangles [i++] = 1 + v;
			triangles [i++] = 2 + v;
		}

		// sides
		for (int h = 0; h < nFloors - 1; h++) {
			for (int v = 0; v < nSides; v++) {
				triangles [i++] = 1 + v + h * (nSides + 1);
				triangles [i++] = 1 + v + (h + 1) * (nSides + 1);
				triangles [i++] = 2 + v + h * (nSides + 1);

				triangles [i++] = 2 + v + h * (nSides + 1);
				triangles [i++] = 1 + v + (h + 1) * (nSides + 1);
				triangles [i++] = 2 + v + (h + 1) * (nSides + 1);
			}
		}
		// top
		for (int v = 0; v < nSides; v++) {
			triangles [i++] = 1 + v + (nFloors-1) * (nSides + 1);
			triangles [i++] = vertices.Length - 1;
			triangles [i++] = 2 + v + (nFloors-1) * (nSides + 1);
		}
		//
		mesh.triangles = triangles;
	}

	private void CreateNormals() {
		mesh.RecalculateNormals ();
	}

	private void CreateColliders() {
		MeshCollider c = GetComponent<MeshCollider> ();
		c.sharedMesh = mesh;
		c.convex = true;
		c.inflateMesh = true;
		c.skinWidth = 0.01f;
	}

	private void Generate() {
		GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
		mesh.name = "Procedual Pin";
		CreateVertices (); 
		CreateTriangles ();
		CreateNormals ();
		CreateColliders ();
	}

	// Use this for initialization
	void Start () {
		Generate ();
	}



	// Update is called once per frame
	void Update () {
		Generate ();		
	}

	/*
	private void OnDrawGizmos() {
		if (vertices == null) {
			return;
		}
		Gizmos.color = Color.blue;
		foreach (Vector3 v in vertices) {
			Gizmos.DrawSphere (v, 0.01f);
		}
	}
	*/
}
