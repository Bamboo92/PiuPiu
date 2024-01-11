using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public IWeapon currentWeapon;
    private Sensor sensor;
    private WeaponLVL weaponLVL;

    public List<IWeapon> weaponsInventory = new List<IWeapon>();  // Stores the player's weapons
    public int currentWeaponIndex = 0;  // The index of the currently equipped weapon in the weaponsInventory list

    void Awake()
    {
        sensor = GameObject.FindObjectOfType<Sensor>();
        weaponLVL = GetComponent<WeaponLVL>();

        var initialWeapon = GetComponent<Pistol>();
        initialWeapon.SetWeapon("Pistol", 15f, 20f, 4f, 100f, 1f, .1f, 0);
        AddWeapon(initialWeapon);
        weaponLVL.setWeaponLVL("Pistol");
    }

    public void AddWeapon(IWeapon weapon)
    {
        if (!weaponsInventory.Contains(weapon)) {
            this.weaponsInventory.Add(weapon);
            EquipWeapon(weapon);
        }
    }

    public void EquipWeapon(IWeapon weapon)
    {
        this.currentWeapon = weapon;
        this.currentWeaponIndex = weaponsInventory.IndexOf(weapon);
        weaponLVL.setWeaponLVL(currentWeapon.GetWeaponName());
    }

    public void NextWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % weaponsInventory.Count; // Loops back to the first weapon after the last weapon
        EquipWeapon(weaponsInventory[currentWeaponIndex]);
    }

    public void PreviousWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex - 1 + weaponsInventory.Count) % weaponsInventory.Count;
        EquipWeapon(weaponsInventory[currentWeaponIndex]);
    }

    public IWeapon GetCurrentWeapon()
    {
        return this.currentWeapon;
    }

    public void Shoot()
    {
        if (!sensor.inCollider){
            if(this.weaponsInventory.Count > 0) {
                this.weaponsInventory[currentWeaponIndex].Shoot();
            }
        }
    }
}