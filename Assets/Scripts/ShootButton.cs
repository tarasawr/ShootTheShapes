using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShootButton : MonoBehaviour
{
    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        ProjectContext.Instance.WeaponService.Shoot();
    }
}