using UnityEngine;
using TMPro;

public class WeaponLVL : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro weaponLVLText;

    public void setWeaponLVL(string weaponLVL)
    {
        weaponLVLText.text = weaponLVL;
    }
}
