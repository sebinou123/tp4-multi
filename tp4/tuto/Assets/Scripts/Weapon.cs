using System.Collections;
using UnityEngine;

public abstract class Weapon {
    private int[] weaponRange;
    private float damageFactor;
    private WeaponRarity weaponRarity;
    private string weaponImage;
    private string weaponName;

    private float damage;
    private int weaponLevel;

    public enum WeaponRarity
    {
        Unique, Rare, Common
        //Normal(new Color(255,255,255,255)), Rare(), Unique();
        //
        // Color color;
        //WeaponRarity(Color color){
        //    this.color = color;
        //}
    }

    public int getWeaponLevel()
    {
        return weaponLevel;
    }

    public void setWeaponLevel(int weaponLevel)
    {
        this.weaponLevel = weaponLevel;
    }

    public float getWeaponDamage()
    {
        return damage;
    }

    public void setWeaponDamage(float weaponDamage)
    {
        this.damage = weaponDamage;
    }

    public float getWeaponDamageFactor()
    {
        return damageFactor;
    }

    public void setWeaponDamageFactor(float weaponDamageFactor)
    {
        this.damageFactor = weaponDamageFactor;
    }

    public int[] getWeaponRange()
    {
        return weaponRange;
    }
    public void setWeaponRange(int[] weaponRange)
    {
        this.weaponRange = weaponRange;
    }

    public string getWeaponImage()
    {
        return weaponImage;
    }

    public void setWeaponImage(string weaponImage)
    {
        this.weaponImage = weaponImage;
    }

    public WeaponRarity getWeaponRarity()
    {
        return weaponRarity;
    }

    public void setWeaponRarity(WeaponRarity weaponRarity)
    {
        this.weaponRarity = weaponRarity;
    }

    public string getWeaponName()
    {
        return weaponName;
    }

    public void setWeaponName(string weaponName)
    {
        this.weaponName = weaponName;
    }
    public override string ToString()
    {
        return "Multiplier: " + damageFactor + "\nLevel: " + weaponLevel + "\nDamage: " + damage;
    }
}
