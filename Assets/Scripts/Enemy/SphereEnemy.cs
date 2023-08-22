using UnityEngine;

namespace Taras.Enemy
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class SphereEnemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshCollider meshCollider;
        [SerializeField] private MeshRenderer meshRenderer;

        public void Create(Color color)
        {
            Mesh mesh = new Mesh();

            int numSegments = 12;
            float radius = 0.5f;

            Vector3[] vertices = new Vector3[(numSegments + 1) * (numSegments + 1)];
            int[] triangles = new int[numSegments * numSegments * 6];

            for (int lat = 0; lat <= numSegments; lat++)
            {
                float angle1 = Mathf.PI * (float)lat / numSegments;
                float y = Mathf.Cos(angle1) * radius;
                float xzRadius = Mathf.Sin(angle1) * radius;

                for (int lon = 0; lon <= numSegments; lon++)
                {
                    float angle2 = Mathf.PI * 2.0f * (float)lon / numSegments;
                    float x = Mathf.Cos(angle2) * xzRadius;
                    float z = Mathf.Sin(angle2) * xzRadius;

                    vertices[lat * (numSegments + 1) + lon] = new Vector3(x, y, z);
                }
            }

            for (int lat = 0; lat < numSegments; lat++)
            {
                for (int lon = 0; lon < numSegments; lon++)
                {
                    int currentVert = lat * (numSegments + 1) + lon;
                    int nextVert = currentVert + 1;
                    int nextRowVert = (lat + 1) * (numSegments + 1) + lon;
                    int nextRowNextVert = nextRowVert + 1;

                    triangles[(lat * numSegments + lon) * 6] = currentVert;
                    triangles[(lat * numSegments + lon) * 6 + 1] = nextRowVert;
                    triangles[(lat * numSegments + lon) * 6 + 2] = nextVert;

                    triangles[(lat * numSegments + lon) * 6 + 3] = nextVert;
                    triangles[(lat * numSegments + lon) * 6 + 4] = nextRowVert;
                    triangles[(lat * numSegments + lon) * 6 + 5] = nextRowNextVert;
                }
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            meshFilter.mesh = mesh;

            //rigidbody.isKinematic = true;

            Material sphereMaterial = new Material(Shader.Find("Standard"));
            sphereMaterial.color = color;
            meshRenderer.material = sphereMaterial;

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