using System.Collections.Generic;
using Jsons;
using Newtonsoft.Json;
using Taras.Enemy;
using UnityEngine;

namespace Taras.Weapon
{
    public class WeaponService : MonoBehaviour
    {
        [SerializeField] private Transform shootPosition;
        [SerializeField] private List<GameObject> weaponsPrefab;
        [SerializeField] private TextAsset jsonFile;
        
        private IWeapon _currentWeapon;
        private List<Color> _colorsFromJson;
        
        public void Init()
        {
            SettingsWeaponData swd = JsonConvert.DeserializeObject<SettingsWeaponData>(jsonFile.text);
            _colorsFromJson = new List<Color>(swd.ColorWeapon.Length);
            foreach (string colorHex in swd.ColorWeapon)
            {
                _colorsFromJson.Add(ColorExtensions.HexToColor(colorHex));
            }
            
            Create();
        }

        private void Create()
        {
            if (weaponsPrefab.Count == 0)
            {
                Debug.LogError("No weapons available.");
                return;
            }

            GameObject weaponPrefab = weaponsPrefab[Random.Range(0, weaponsPrefab.Count)];
            _currentWeapon = Instantiate(weaponPrefab, shootPosition).GetComponent<IWeapon>();
            _currentWeapon.Create(_colorsFromJson[Random.Range(0, _colorsFromJson.Count)]);
        }

        public void Shoot()
        {
            if (_currentWeapon != null)
            {
                _currentWeapon.Shoot();
                _currentWeapon = null;
                Create();
            }
        }
    }
}