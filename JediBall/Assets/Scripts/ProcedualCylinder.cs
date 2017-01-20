// Toshiaki Koike-Akino, 2017 Jan.
using UnityEngine;
using System.Collections;	

//http://wiki.unity3d.com/index.php/ProceduralPrimitives
//http://catlikecoding.com/unity/tutorials/rounded-cube/
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[ExecuteInEditMode]
public class ProcedualCylinder : MonoBehaviour {
	public float height = 1f;
	public float bottomRadius = .5f;
	public float topRadius = 1f;
	public int nbSides = 36;

	private int nbVerticesCap; 

	private Mesh mesh;
	private Vector3[] normales;
	private Vector3[] vertices;
	private int[] triangles;
	private Vector2[] uvs;

	private void GenerateVertices() {
		#region Vertices

		// bottom + top + sides
		vertices = new Vector3[nbVerticesCap + nbVerticesCap + nbSides * 2 + 2];
		int vert = 0;

		// Bottom cap
		vertices [vert++] = new Vector3 (0f, 0f, 0f);
		while (vert <= nbSides) {
			float rad = (float)vert / nbSides * Mathf.PI * 2f;
			vertices [vert] = new Vector3 (Mathf.Cos (rad) * bottomRadius, 0f, Mathf.Sin (rad) * bottomRadius);
			vert++;
		}

		// Top cap
		vertices [vert++] = new Vector3 (0f, height, 0f);
		while (vert <= nbSides * 2 + 1) {
			float rad = (float)(vert - nbSides - 1) / nbSides * Mathf.PI * 2f;
			vertices [vert] = new Vector3 (Mathf.Cos (rad) * topRadius, height, Mathf.Sin (rad) * topRadius);
			vert++;
		}

		// Sides
		int v = 0;
		while (vert <= vertices.Length - 4) {
			float rad = (float)v / nbSides * Mathf.PI * 2f;
			vertices [vert] = new Vector3 (Mathf.Cos (rad) * topRadius, height, Mathf.Sin (rad) * topRadius);
			vertices [vert + 1] = new Vector3 (Mathf.Cos (rad) * bottomRadius, 0, Mathf.Sin (rad) * bottomRadius);
			vert += 2;
			v++;
		}
		vertices [vert] = vertices [nbSides * 2 + 2];
		vertices [vert + 1] = vertices [nbSides * 2 + 3];
		#endregion

		mesh.vertices = vertices;
	}

	private void GenerateNormals() {
		#region Normales

		// bottom + top + sides
		normales = new Vector3[vertices.Length];
		int vert = 0;

		// Bottom cap
		while (vert <= nbSides) {
			normales [vert++] = Vector3.down;
		}

		// Top cap
		while (vert <= nbSides * 2 + 1) {
			normales [vert++] = Vector3.up;
		}

		// Sides
		int v = 0;
		while (vert <= vertices.Length - 4) {			
			float rad = (float)v / nbSides * Mathf.PI * 2f;
			float cos = Mathf.Cos (rad);
			float sin = Mathf.Sin (rad);

			normales [vert] = new Vector3 (cos, 0f, sin);
			normales [vert + 1] = normales [vert];

			vert += 2;
			v++;
		}
		normales [vert] = normales [nbSides * 2 + 2];
		normales [vert + 1] = normales [nbSides * 2 + 3];
		#endregion
		mesh.normals = normales;
	}

	private void GenerateUV() {
		#region UVs
		uvs = new Vector2[vertices.Length];

		// Bottom cap
		int u = 0;
		uvs [u++] = new Vector2 (0.5f, 0.5f);
		while (u <= nbSides) {
			float rad = (float)u / nbSides * Mathf.PI * 2f;
			uvs [u] = new Vector2 (Mathf.Cos (rad) * .5f + .5f, Mathf.Sin (rad) * .5f + .5f);
			u++;
		}

		// Top cap
		uvs [u++] = new Vector2 (0.5f, 0.5f);
		while (u <= nbSides * 2 + 1) {
			float rad = (float)u / nbSides * Mathf.PI * 2f;
			uvs [u] = new Vector2 (Mathf.Cos (rad) * .5f + .5f, Mathf.Sin (rad) * .5f + .5f);
			u++;
		}

		// Sides
		int u_sides = 0;
		while (u <= uvs.Length - 4) {
			float t = (float)u_sides / nbSides;
			uvs [u] = new Vector3 (t, 1f);
			uvs [u + 1] = new Vector3 (t, 0f);
			u += 2;
			u_sides++;
		}
		uvs [u] = new Vector2 (1f, 1f);
		uvs [u + 1] = new Vector2 (1f, 0f);
		#endregion 
		mesh.uv = uvs;
	}

	private void GenerateTriangles() {
		#region Triangles
		int nbTriangles = nbSides + nbSides + nbSides * 2;
		triangles = new int[nbTriangles * 3 + 3];

		// Bottom cap
		int tri = 0;
		int i = 0;
		while (tri < nbSides - 1) {
			triangles [i] = 0;
			triangles [i + 1] = tri + 1;
			triangles [i + 2] = tri + 2;
			tri++;
			i += 3;
		}
		triangles [i] = 0;
		triangles [i + 1] = tri + 1;
		triangles [i + 2] = 1;
		tri++;
		i += 3;

		// Top cap
		//tri++;
		while (tri < nbSides * 2) {
			triangles [i] = tri + 2;
			triangles [i + 1] = tri + 1;
			triangles [i + 2] = nbVerticesCap;
			tri++;
			i += 3;
		}

		triangles [i] = nbVerticesCap + 1;
		triangles [i + 1] = tri + 1;
		triangles [i + 2] = nbVerticesCap;		
		tri++;
		i += 3;
		tri++;

		// Sides
		while (tri <= nbTriangles) {
			triangles [i] = tri + 2;
			triangles [i + 1] = tri + 1;
			triangles [i + 2] = tri + 0;
			tri++;
			i += 3;

			triangles [i] = tri + 1;
			triangles [i + 1] = tri + 2;
			triangles [i + 2] = tri + 0;
			tri++;
			i += 3;
		}
		#endregion
		mesh.triangles = triangles;
	}

	private void CreateColliders() {
		MeshCollider c = GetComponent<MeshCollider>();
		c.sharedMesh = mesh;
		c.convex = true;
		c.inflateMesh = true;
		c.skinWidth = 0.01f;
	}

	private void Generate() {
		nbVerticesCap = nbSides + 1;
		GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
		mesh.name = "Procedual Cylinder";

		GenerateVertices ();
		GenerateNormals ();
		GenerateUV ();
		GenerateTriangles ();
		CreateColliders ();
	}

	public void Awake() {
		Generate ();
	}

	public void Update() {
		Generate ();
	}

	public void Start() {

		mesh.RecalculateBounds ();
//		mesh.Optimize ();
	}
}