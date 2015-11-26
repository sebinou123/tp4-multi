using System.Collections;

public class SwordOfTruth : Weapon {
    private static int[] weaponRange = { 
                                 1, 1, 1, 1, 1,
                                 0, 1, 1, 1, 0,
                                 0, 0, 1, 0, 0,
                                 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0};
    private const float damageFactor = 2.5f;
    private const Weapon.WeaponRarity weaponRarity = Weapon.WeaponRarity.Unique;
    private const string imageName = "imgSwordOfTruth";
    private const string weaponName = "Sword of Truth";

    public SwordOfTruth(int weaponLevel)
    {
        setWeaponDamage(weaponLevel * damageFactor);
        setWeaponLevel(weaponLevel);
        setWeaponName(weaponName);
        setWeaponDamageFactor(damageFactor);
        setWeaponRange(weaponRange);
    }
}
