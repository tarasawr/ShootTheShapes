using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Physics.defaultContactOffset = 0.01f;
        Physics.defaultSolverIterations = 255;
        
        FirstStart();
    }

    private void FirstStart()
    {
        ProjectContext.Instance.WeaponService.Init();
        ProjectContext.Instance.EnemyService.Init();
    }
}
