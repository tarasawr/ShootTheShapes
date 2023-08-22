using UnityEngine;

namespace Taras.Enemy
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class CubeEnemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshCollider meshCollider;
        [SerializeField] private MeshRenderer meshRenderer;

        public void Create(Color color)
        {
            Mesh mesh = new Mesh();

            Vector3[] vertices =
            {
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f)
            };

            int[] triangles =
            {
                0, 2, 1, 0, 3, 2,
                1, 2, 5, 5, 2, 6,
                5, 6, 4, 4, 6, 7,
                4, 7, 0, 0, 7, 3,
                4, 0, 5, 5, 0, 1,
                3, 7, 2, 2, 7, 6
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            meshFilter.mesh = mesh;

            Material cubeMaterial = new Material(Shader.Find("Standard"));
            cubeMaterial.color = color;
            meshRenderer.material = cubeMaterial;

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