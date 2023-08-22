using UnityEngine;

namespace Taras.Enemy
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class CapsuleEnemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshCollider meshCollider;
        [SerializeField] private MeshRenderer meshRenderer;

        public float height = 1f;
        public float radius = 1f;
        public int segments = 12;

        public void Create(Color color)
        {
            if (segments % 2 != 0)
                segments++;

            int points = segments + 1;

            float[] pX = new float[points];
            float[] pZ = new float[points];
            float[] pY = new float[points];
            float[] pR = new float[points];

            float calcH = 0f;
            float calcV = 0f;

            for (int i = 0; i < points; i++)
            {
                pX[i] = Mathf.Sin(calcH * Mathf.Deg2Rad);
                pZ[i] = Mathf.Cos(calcH * Mathf.Deg2Rad);
                pY[i] = Mathf.Cos(calcV * Mathf.Deg2Rad);
                pR[i] = Mathf.Sin(calcV * Mathf.Deg2Rad);

                calcH += 360f / (float)segments;
                calcV += 180f / (float)segments;
            }

            Vector3[] vertices = new Vector3[points * (points + 1)];
            Vector2[] uvs = new Vector2[vertices.Length];
            int ind = 0;

            float yOff = (height - (radius * 2f)) * 0.5f;
            if (yOff < 0)
                yOff = 0;

            float stepX = 1f / ((float)(points - 1));
            float uvX, uvY;

            int top = Mathf.CeilToInt((float)points * 0.5f);

            for (int y = 0; y < top; y++)
            {
                for (int x = 0; x < points; x++)
                {
                    vertices[ind] = new Vector3(pX[x] * pR[y], pY[y], pZ[x] * pR[y]) * radius;
                    vertices[ind].y = yOff + vertices[ind].y;

                    uvX = 1f - (stepX * (float)x);
                    uvY = (vertices[ind].y + (height * 0.5f)) / height;
                    uvs[ind] = new Vector2(uvX, uvY);

                    ind++;
                }
            }

            int btm = Mathf.FloorToInt((float)points * 0.5f);

            for (int y = btm; y < points; y++)
            {
                for (int x = 0; x < points; x++)
                {
                    vertices[ind] = new Vector3(pX[x] * pR[y], pY[y], pZ[x] * pR[y]) * radius;
                    vertices[ind].y = -yOff + vertices[ind].y;

                    uvX = 1f - (stepX * (float)x);
                    uvY = (vertices[ind].y + (height * 0.5f)) / height;
                    uvs[ind] = new Vector2(uvX, uvY);

                    ind++;
                }
            }


            int[] triangles = new int[(segments * (segments + 1) * 2 * 3)];

            for (int y = 0, t = 0; y < segments + 1; y++)
            {
                for (int x = 0; x < segments; x++, t += 6)
                {
                    triangles[t + 0] = ((y + 0) * (segments + 1)) + x + 0;
                    triangles[t + 1] = ((y + 1) * (segments + 1)) + x + 0;
                    triangles[t + 2] = ((y + 1) * (segments + 1)) + x + 1;

                    triangles[t + 3] = ((y + 0) * (segments + 1)) + x + 1;
                    triangles[t + 4] = ((y + 0) * (segments + 1)) + x + 0;
                    triangles[t + 5] = ((y + 1) * (segments + 1)) + x + 1;
                }
            }

            Mesh mesh = meshFilter.sharedMesh;
            if (!mesh)
            {
                mesh = new Mesh();
                meshFilter.sharedMesh = mesh;
            }

            mesh.Clear();

            mesh.name = "ProceduralCapsule";

            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.Optimize();


            Material capsuleMaterial = new Material(Shader.Find("Standard"));
            capsuleMaterial.color = color;
            meshRenderer.material = capsuleMaterial;

            meshCollider.sharedMesh = meshFilter.mesh;
            meshCollider.convex = true;
            meshCollider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            ProjectContext.Instance.EnemyService.Create();
            Destroy(other.gameObject.transform.parent.gameObject);
            Destroy(gameObject);
        }
    }
}