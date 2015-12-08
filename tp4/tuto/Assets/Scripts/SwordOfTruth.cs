using System.Collections;
using UnityEngine;


public class SwordOfTruth : Weapon {
    private static int[,] weaponRange = new int [5, 5]{ 
                                 {1, 1, 1, 1, 1},
                                 {0, 1, 1, 1, 0},
                                 {0, 0, 1, 0, 0},
                                 {0, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0}};
    private const float damageFactor = 2.5f;
    private const string weaponName = "Sword of Truth";
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
