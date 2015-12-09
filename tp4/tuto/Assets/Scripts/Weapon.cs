using System.Collections;
using UnityEngine;

/**
 * Class extends by each weapon, who have his own weapon range, damage factor, name, damage, level and image
 * */
public abstract class Weapon {
    private int[,] weaponRange;
    private float damageFactor;
    private string weaponName;

    private float damage;
    private int weaponLevel;
    private int weaponImage;

	//get image of the weapon
    public int getWeaponImage()
    {
        return weaponImage;
    }
	//set image of the weapon
    public void setWeaponImage(int weaponImage)
    {
        this.weaponImage = weaponImage;
    }
	//get level of the weapon
    public int getWeaponLevel()
    {
        return weaponLevel;
    }
	//set level of the weapon
    public void setWeaponLevel(int weaponLevel)
    {
        this.weaponLevel = weaponLevel;
    }
	//get weapon damage
    public float getWeaponDamage()
    {
        return damage;
    }
	//set weapon damage
    public void setWeaponDamage(float weaponDamage)
    {
        this.damage = weaponDamage;
    }
	//get weapon damage factor
    public float getWeaponDamageFactor()
    {
        return damageFactor;
    }
	//set weapon damage factor
    public void setWeaponDamageFactor(float weaponDamageFactor)
    {
        this.damageFactor = weaponDamageFactor;
    }
	//get an array of int about the range of the weapon
    public int[,] getWeaponRange()
    {
        return weaponRange;
    }
	//get a specific range with the specified facing direction, 1 = damage range, 0 = not in damage range
	//return an array of int about the range of the weapon
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

	//method for the drop at the end of the level
	//return the droping weapon or null if the player don't got a weapon
    public static Weapon getWeaponDrop(int level)
    {
        Weapon returnValue = null;
        int randomType = Random.Range(0, 100);
        int randomWeapon = Random.Range(0, 100);
     
        if (randomType < 5)
        {
            returnValue = new SwordOfTruth(level);
        }
        else if (randomType < 25)
        {
            if (randomWeapon < 50)
                returnValue = new KnightSword(level);
            else
                returnValue = new WhirlwindAxe(level);
        }
        else
        {
            returnValue = new BasicSword(level);
        }

        return returnValue;
    }

	//set an array of int about the range of the weapon
    public void setWeaponRange(int[,] weaponRange)
    {
        this.weaponRange = weaponRange;
    }

	//get the weapon name
    public string getWeaponName()
    {
        return weaponName;
    }
	//set the weapon name
    public void setWeaponName(string weaponName)
    {
        this.weaponName = weaponName;
    }
	//string of the attributes of the weapon
    public override string ToString()
    {
        return "Multiplier: " + damageFactor + "\nLevel: " + weaponLevel + "\nDamage: " + damage;
    }
}
