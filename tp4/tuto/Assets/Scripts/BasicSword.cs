using UnityEngine;
using System.Collections;

/**
 * Class for the basic sword stats (range, name, damage, etc).
 * */
public class BasicSword : Weapon
{
	//the range representation of the weapon to show in the canva infoplayer
    private static int[,] weaponRange = new int[5,5]{ 
                                 {0, 0, 0, 0, 0},
                                 {0, 0, 1, 0, 0},
                                 {0, 0, 1, 0, 0},
                                 {0, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0}};
	//damage against enemy
    private const float damageFactor = 1f;
	//name
    private const string weaponName = "Basic Sword";
	//level of our weapon
    private const int weaponLevel = 0;

    public BasicSword(int weaponLevel)
    {
        setWeaponDamage(weaponLevel * damageFactor);
        setWeaponLevel(weaponLevel);
        setWeaponName(weaponName);
        setWeaponDamageFactor(damageFactor);
        setWeaponRange(weaponRange);
    }
}
