using UnityEngine;
using System.Collections;

public class BasicSword : Weapon
{
    private static int[,] weaponRange = new int[5,5]{ 
                                 {0, 0, 0, 0, 0},
                                 {0, 0, 1, 0, 0},
                                 {0, 0, 1, 0, 0},
                                 {0, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0}};
    private const float damageFactor = 1f;
    private const string weaponName = "Basic Sword";

    public BasicSword(int weaponLevel)
    {
        setWeaponDamage(weaponLevel * damageFactor);
        setWeaponLevel(weaponLevel);
        setWeaponName(weaponName);
        setWeaponDamageFactor(damageFactor);
        setWeaponRange(weaponRange);
    }
}
