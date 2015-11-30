using System.Collections;
using UnityEngine;

public abstract class Weapon {
    private int[,] weaponRange;
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

    public int[,] getWeaponRange()
    {
        return weaponRange;
    }

    public int[,] getWeaponRange(Player.FacingDirection dir)
    {
        int[,] returnValue = new int[5,5];
        int maxInt = 4;
        switch (dir)
        {
            case Player.FacingDirection.Up:
                for (int i = 0; i <= maxInt; i++)
                {
                    for (int j = 0; j <= maxInt; j++)
                    {
                        returnValue[i, j] = this.getWeaponRange()[i, j] == 1 ? 1 : 0;
                    }
                }
                break;
            case Player.FacingDirection.Right:
                for (int i = 0; i <= maxInt; i++)
                {
                    for (int j = 0; j <= maxInt; j++)
                    {
                        returnValue[j, maxInt-i] = this.getWeaponRange()[i, j] == 1 ? 1 : 0;
                    }
                }
                break;
            case Player.FacingDirection.Down:
                for (int i = 0; i <= maxInt; i++)
                {
                    for (int j = 0; j <= maxInt; j++)
                    {
                        returnValue[maxInt-i, maxInt-j] = this.getWeaponRange()[i, j] == 1 ? 1 : 0;
                    }
                }
                break;
            case Player.FacingDirection.Left:
                for (int i = 0; i <= maxInt; i++)
                {
                    for (int j = 0; j <= maxInt; j++)
                    {
                        returnValue[maxInt-j, i] = this.getWeaponRange()[i, j] == 1 ? 1 : 0;
                    }
                }
                break;
        }
        return returnValue;
    }
    public void setWeaponRange(int[,] weaponRange)
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
