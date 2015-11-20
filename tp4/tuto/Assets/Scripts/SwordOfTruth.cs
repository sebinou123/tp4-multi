using System.Collections;

public class SwordOfTruth : Weapon {
    public static int[] weaponRange = { 
                                 1, 1, 1, 1, 1,
                                 0, 1, 1, 1, 0,
                                 0, 0, 1, 0, 0,
                                 0, 0, 0, 0, 0,
                                 0, 0, 0, 0, 0};
    public static float damageFactor = 2.5f;
    public static Weapon.WeaponRarity weaponRarity = Weapon.WeaponRarity.Unique;

    public SwordOfTruth(int weaponLevel)
    {
        damage = weaponLevel * damageFactor;
        base.weaponLevel = weaponLevel;
    }
}
