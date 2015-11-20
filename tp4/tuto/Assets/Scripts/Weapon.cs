using System.Collections;

public abstract class Weapon {
    public bool[][] weaponRange;
    public float damageFactor;
    public WeaponRarity weaponRarity;

    public float damage;
    public int weaponLevel;

    public enum WeaponRarity
    {
        Normal, Rare, Unique
    }

    public override string ToString()
    {
        return "Multiplier: " + damageFactor + "\nLevel: " + weaponLevel + "\nDamage: " + damage;
    }
}
