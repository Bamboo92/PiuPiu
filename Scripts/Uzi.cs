using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uzi : MonoBehaviour, IWeapon
{
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private List<ParticleSystem> muzzleFlashes;
    [SerializeField]
    private TrailRenderer BulletTrail;
    [SerializeField]
    private GameObject impactEffect;
    [SerializeField]
    private LayerMask raycastLayers;
    private AudioManager audioManager;

    private string weaponName;
    private float nextTimeToFire = 0f;
    private float spreadAngle = 10f; // in degrees
    private float damage = 10f;
    private float range = 50f;
    private float fireRate = 20f;
    private float BulletSpeed = 375f;
    private float spreadMagnitude = 1f;
    private int numOfBullets;
    
    void Awake()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    public void Shoot()
    {
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            ParticleSystem chosenMuzzleFlash = muzzleFlashes[Random.Range(0, muzzleFlashes.Count)];
            chosenMuzzleFlash.Play();

            audioManager.Play("Shoot");

            RaycastHit hit;
            TrailRenderer trail = Instantiate(BulletTrail, firePoint.position, Quaternion.identity);

            // Calculate direction with spread
            Vector3 spread = firePoint.forward;
            spread += firePoint.right * Random.Range(-spreadMagnitude, spreadMagnitude) * Mathf.Sin(Random.Range(0, spreadAngle) * Mathf.Deg2Rad);
            spread += firePoint.up * Random.Range(-spreadMagnitude, spreadMagnitude) * Mathf.Sin(Random.Range(0, spreadAngle) * Mathf.Deg2Rad);

            if (Physics.Raycast(firePoint.position, spread.normalized, out hit, range, raycastLayers))
            {
                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));

                IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damage);
                }

                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                StartCoroutine(SpawnTrail(trail, firePoint.position + spread.normalized * range, Vector3.zero, false));
            }
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
    {
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= BulletSpeed * Time.deltaTime;

            yield return null;
        }
        Trail.transform.position = HitPoint;

        Destroy(Trail.gameObject, Trail.time);
    }

    public void SetWeapon(string weaponName, float damage, float range, float fireRate, float BulletSpeed, float spreadAngle, float spreadMagnitude, int numOfBullets)
    {
        this.weaponName = weaponName;
        this.damage = damage;
        this.range = range;
        this.fireRate = fireRate;
        this.BulletSpeed = BulletSpeed;
        this.spreadAngle = spreadAngle;
        this.spreadMagnitude = spreadMagnitude;
        this.numOfBullets = numOfBullets;
    }

    public string GetWeaponName()
    {
        return this.weaponName;
    }

    public float GetRange()
    {
        return this.range;
    }
}