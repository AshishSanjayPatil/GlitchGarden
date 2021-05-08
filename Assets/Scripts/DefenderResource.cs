using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderResource : MonoBehaviour
{
    [SerializeField]
    float health = 100;

    [SerializeField]
    int cost = 100;

    [SerializeField]
    GameObject deathVFX;

    StartsDisplay starsDispaly;

    LevelLoader levelLoad;

    DefenderSpawner defenderSpawner;

    void Start()
    {
        health *= PlayerPrefsController.GetDifficulty();
        levelLoad = FindObjectOfType<LevelLoader>();
        defenderSpawner = FindObjectOfType<DefenderSpawner>();
        starsDispaly = FindObjectOfType<StartsDisplay>();
        defenderSpawner.AddGridOccupancy(transform.position);
    }

    private void AddStars(int amount)
    {
        starsDispaly.AddStars(amount);
    }

    public int GetDefenderCost()
    {
        return cost;
    }

    public void DamageHealth(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            GameObject newVFX = Instantiate(deathVFX, transform.position, Quaternion.identity);
            newVFX.transform.parent = levelLoad.VFXContainer();
            Destroy(newVFX, 1f);
            defenderSpawner.RemoveGridOccupancy(transform.position);
            Destroy(this.gameObject);
        }
    }
}
