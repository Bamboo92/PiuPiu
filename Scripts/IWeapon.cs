public interface IWeapon
{
    void Shoot();
    void SetWeapon(string weaponName, float damage, float range, float fireRate, float BulletSpeed, float spreadAngle, float spreadMagnitude, int numOfBullets);
    float GetRange();
    string GetWeaponName();
}