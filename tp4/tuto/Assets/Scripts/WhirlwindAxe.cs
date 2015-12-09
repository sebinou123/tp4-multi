using UnityEngine;
using System.Collections;
/**
 * Class for the WhirlwindAxe stats (range, name, damage, etc)
 * */
public class WhirlwindAxe : Weapon {
	//the range representation of the weapon to show in the canva infoplayer
    private static int[,] weaponRange = new int [5, 5]{ 
                                 {0, 0, 1, 0, 0},
                                 {0, 1, 1, 1, 0},
                                 {0, 1, 1, 1, 0},
                                 {0, 1, 1, 1, 0},
                                 {0, 0, 0, 0, 0}};
	//damage against enemy
    private const float damageFactor = 1.25f;
	//name
    private const string weaponName = "Whirlwind Axe";
	//level of our weapon
    private const int weaponImage = 3;

    public WhirlwindAxe(int weaponLevel)
    {
        setWeaponDamage(weaponLevel * damageFactor);
        setWeaponLevel(weaponLevel);
        setWeaponName(weaponName);
        setWeaponDamageFactor(damageFactor);
        setWeaponRange(weaponRange);
    }
}
