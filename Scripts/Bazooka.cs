using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka : MonoBehaviour, IWeapon
{
    private string weaponName;
    private float nextTimeToFire = 0f;
    public float damage;
    private float range;
    private float fireRate; // The rate at which the bazooka can fire
    public float rocketSpeed; // The speed of the rocket
    public float homingSensitivity; // The sensitivity of the rocket's homing capability
    public float explosionRadius;
    private int numOfBullets;

    [SerializeField]
    private GameObject rocketPrefab; // The rocket prefab that will be launched
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private List<ParticleSystem> muzzleFlashes;
    [SerializeField]
    private TrailRenderer BulletTrail;
    [SerializeField]
    private LayerMask raycastLayers;
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    public void Shoot()
    {
        //Debug.Log(damage + " " + range + " " + fireRate + " " + rocketSpeed + " " + homingSensitivity + " " + explosionRadius + " " + numOfBullets);
        if (Time.time >= nextTimeToFire)
        {
            for (int i = 0; i < numOfBullets; i++)
            {
                nextTimeToFire = Time.time + 1f / fireRate;

                ParticleSystem chosenMuzzleFlash = muzzleFlashes[Random.Range(0, muzzleFlashes.Count)];
                chosenMuzzleFlash.Play();
                audioManager.Play("Bazooka");

                GameObject rocket = Instantiate(rocketPrefab, firePoint.position, firePoint.rotation);
                Rocket rocketScript = rocket.GetComponent<Rocket>();

                // Set the properties of the rocket
                rocketScript.SetRocket(damage, range, fireRate, rocketSpeed, homingSensitivity, explosionRadius, numOfBullets);
                /*
                RaycastHit hit;
                TrailRenderer trail = Instantiate(BulletTrail, firePoint.position, Quaternion.identity);

                if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range, raycastLayers))
                {
                    StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));
                }
                else
                {
                    StartCoroutine(SpawnTrail(trail, firePoint.position + firePoint.forward * range, Vector3.zero, false));
                }*/
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

            remainingDistance -= rocketSpeed * Time.deltaTime;

            yield return null;
        }
        Trail.transform.position = HitPoint;

        Destroy(Trail.gameObject, Trail.time);
    }

    public void SetWeapon(string weaponName, float damage, float range, float fireRate, float rocketSpeed, float homingSensitivity, float explosionRadius, int numOfBullets)
    {
        this.weaponName = weaponName;
        this.damage = damage;
        this.range = range;
        this.fireRate = fireRate;
        this.rocketSpeed = rocketSpeed;
        this.homingSensitivity = homingSensitivity;
        this.explosionRadius = explosionRadius;
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