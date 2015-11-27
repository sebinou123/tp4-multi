using UnityEngine;
using System.Collections;

public class KnightSword : Weapon {
    private static int[] weaponRange = { 
                                 0, 0, 1, 0, 0,
                                 0, 1, 1, 1, 0,
                                 0, 0, 1, 0, 0,
                                 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0};
    private const float damageFactor = 1.5f;
    private const Weapon.WeaponRarity weaponRarity = Weapon.WeaponRarity.Rare;
    private const string weaponName = "Knight Sword";

    public KnightSword(int weaponLevel)
    {
        setWeaponDamage(weaponLevel * damageFactor);
        setWeaponLevel(weaponLevel);
        setWeaponName(weaponName);
        setWeaponDamageFactor(damageFactor);
        setWeaponRange(weaponRange);
    }
}
