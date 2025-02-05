using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FurnaceController : MonoBehaviour
{
    public float fuelAmount = 1000;
    const int maxFuelCapacity = 32;
    int fuelCapacity = 6;
    const int fuelAddAmount = 90;
    [SerializeField] TextMeshProUGUI fuel;

    // Update is called once per frame
    void Update()
    {
        fuelAmount -= Time.deltaTime;
        if (fuelAmount <= 2 && fuelCapacity >0)
        {
            fuelCapacity--;
            fuelAmount += 90;
        }
        if (fuelAmount < 0)
        {
            Debug.Log("Your fam is ded bruv");
        }
        fuel.text = fuelCapacity.ToString();
    }

    public SlotTrait AddFuel(SlotTrait fuel)
    {
        int fuelCanAdd = maxFuelCapacity - fuelCapacity;
        int maxFuel = Mathf.Clamp(fuel.GetQuantity(), 0, fuelCanAdd);
        int remain = fuel.GetQuantity() - maxFuel;

        fuelCapacity += maxFuel;
        fuel.ChangeQuantity(-remain);

        Debug.Log("Fuel added" + fuelCapacity);
        Debug.Log($"Cyka{fuelCanAdd} {maxFuel} {remain} ");
        if(remain > 0)
        {
            return new SlotTrait(fuel.GetItem(),remain);
        }


        return null;
    }

    
}
