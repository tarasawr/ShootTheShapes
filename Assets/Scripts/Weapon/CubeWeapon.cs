using UnityEngine;

namespace Taras.Weapon
{
    public class CubeWeapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private MeshRenderer meshRenderer;

        public float speed = 10.0f;
        public float maxDistance = 10.0f;

        private bool _isShot;
        private Vector3 _initialPosition;

        private void Start()
        {
            _initialPosition = transform.position;
        }

        public void Shoot()
        {
            transform.parent = new GameObject("Shoot").transform;
            _isShot = true;
        }

        private void Update()
        {
            if (_isShot)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                float distance = Vector3.Distance(_initialPosition, transform.position);
                if (distance >= maxDistance)
                {
                    Destroy(gameObject);
                }
            }
        }

        public void Create(Color color)
        {
            meshRenderer.material.color = color;
        }
    }
}