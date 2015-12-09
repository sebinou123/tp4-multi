using System.Collections;
using UnityEngine;

/**
 * Class for the sword of truth stats (range, name, damage, etc)
 * */
public class SwordOfTruth : Weapon {
	//the range representation of the weapon to show in the canva infoplayer
    private static int[,] weaponRange = new int [5, 5]{ 
                                 {1, 1, 1, 1, 1},
                                 {0, 1, 1, 1, 0},
                                 {0, 0, 1, 0, 0},
                                 {0, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0}};
	//damage against enemy
    private const float damageFactor = 2.5f;
	//name
    private const string weaponName = "Sword of Truth";
	//level of our weapon
    private const int weaponLevel = 1;

    public SwordOfTruth(int weaponLevel)
    {
        setWeaponDamage(weaponLevel * damageFactor);
        setWeaponLevel(weaponLevel);
        setWeaponName(weaponName);
        setWeaponDamageFactor(damageFactor);
        setWeaponRange(weaponRange);
    }
}
