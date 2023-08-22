using System;
using System.Collections.Generic;
using Jsons;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Taras.Enemy
{
    public class EnemyService : MonoBehaviour
    {
        [SerializeField] private Transform centerSpawn;
        [SerializeField] private TextAsset jsonFile;
        [SerializeField] private List<GameObject> enemysPrefab;

        public float minDistance = 5.0f;
        public float maxDistance = 10.0f;
        
        private IEnemy _currentEnemy;
        private List<Color> _colorsFromJson;

        public void Init()
        {
            SettingsEnemyData settingsEnemyData = JsonConvert.DeserializeObject<SettingsEnemyData>(jsonFile.text);
            _colorsFromJson = new List<Color>(settingsEnemyData.ColorEnemy.Length);
            foreach (string colorHex in settingsEnemyData.ColorEnemy)
            {
                _colorsFromJson.Add(ColorExtensions.HexToColor(colorHex));
            }

            Create();
        }

        public void Create()
        {
            if (enemysPrefab.Count == 0)
            {
                Debug.LogError("No enemies available.");
                return;
            }

            float randomAngle = Random.Range(0.0f, Mathf.PI * 2);

            float randomDistance = Random.Range(minDistance, maxDistance);
            Vector3 randomPosition = new Vector3(
                randomDistance * Mathf.Cos(randomAngle),
                0.0f,
                randomDistance * Mathf.Sin(randomAngle)
            );

            randomPosition += centerSpawn.position;

            GameObject enemyPrefab = enemysPrefab[Random.Range(0, enemysPrefab.Count)];
            _currentEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity).GetComponent<IEnemy>();

            _currentEnemy.Create(_colorsFromJson[Random.Range(0, _colorsFromJson.Count)]);

            ProjectContext.Instance.PlayerController.SetTargetObject(randomPosition);
        }
    }

    public static class ColorExtensions
    {
        public static Color HexToColor(string hex)
        {
            hex = hex.TrimStart('#');

            byte r = Convert.ToByte(hex.Substring(0, 2), 16);
            byte g = Convert.ToByte(hex.Substring(2, 2), 16);
            byte b = Convert.ToByte(hex.Substring(4, 2), 16);

            return new Color32(r, g, b, 255);
        }
    }
}