using UnityEngine;
using System.Collections;

public class KnightSword : Weapon {
    private static int[,] weaponRange = new int[5,5]{ 
                                 {0, 0, 1, 0, 0},
                                 {0, 1, 1, 1, 0},
                                 {0, 0, 1, 0, 0},
                                 {0, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0}};
    private const float damageFactor = 1.5f;
    private const string weaponName = "Knight Sword";
    private const int weaponLevel = 2;

    public KnightSword(int weaponLevel)
    {
        setWeaponDamage(weaponLevel * damageFactor);
        setWeaponLevel(weaponLevel);
        setWeaponName(weaponName);
        setWeaponDamageFactor(damageFactor);
        setWeaponRange(weaponRange);
    }
}
