using UnityEngine;
using System.Collections;

public class WhirlwindAxe : Weapon {
    private static int[,] weaponRange = new int [5, 5]{ 
                                 {0, 0, 1, 0, 0},
                                 {0, 1, 1, 1, 0},
                                 {0, 1, 1, 1, 0},
                                 {0, 1, 1, 1, 0},
                                 {0, 0, 0, 0, 0}};
    private const float damageFactor = 1.25f;
    private const string weaponName = "Whirlwind Axe";
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
