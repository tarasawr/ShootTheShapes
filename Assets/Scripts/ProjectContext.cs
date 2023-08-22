using Taras.Enemy;
using Taras.Player;
using Taras.Weapon;
using UnityEngine;

public class ProjectContext : MonoBehaviour
{
    public PlayerController PlayerController;
    public WeaponService WeaponService;
    public EnemyService EnemyService;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private static ProjectContext instance;

    public static ProjectContext Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("ProjectContext: Instance is null");
            }

            return instance;
        }
    }
}