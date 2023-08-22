using UnityEngine;

namespace Taras.Enemy
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class PyramidEnemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshCollider meshCollider;
        [SerializeField] private MeshRenderer meshRenderer;


        public void Create(Color color)
        {
            Mesh mesh = new Mesh();

            Vector3[] vertices =
            {
                new(0f, 0.5f, 0f),
                new(-0.5f, -0.5f, 0.5f),
                new(0.5f, -0.5f, 0.5f),
                new(0.5f, -0.5f, -0.5f),
                new(-0.5f, -0.5f, -0.5f)
            };

            int[] triangles =
            {
                0, 1, 2,
                0, 2, 3,
                0, 3, 4,
                0, 4, 1,
                1, 2, 3,
                4, 3, 2
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            meshFilter.mesh = mesh;

            //rigidbody.isKinematic = true;

            Material pyramidMaterial = new Material(Shader.Find("Standard"));
            pyramidMaterial.color = color;
            meshRenderer.material = pyramidMaterial;

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