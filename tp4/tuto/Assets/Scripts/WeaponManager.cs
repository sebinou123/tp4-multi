using UnityEngine;
using System.Collections;

public class WeaponManager
{
    public static int WEAPONS_AVAILABLE = 1;
    public static Weapon[] weaponsAvailable;
    private Weapon currentWeapon;
    private int weaponIndex;

    public WeaponManager()
    {
        weaponsAvailable = new Weapon[WeaponManager.WEAPONS_AVAILABLE];

        weaponsAvailable[0] = new SwordOfTruth(15);
        weaponIndex = 0;
        currentWeapon = weaponsAvailable[weaponIndex];
    }

    // Constructor to load from save file with levels on weapons
    public WeaponManager(int[] weaponsLevels, int equippedWeapon)
    {
        weaponsAvailable = new Weapon[WeaponManager.WEAPONS_AVAILABLE];

        weaponsAvailable[0] = new SwordOfTruth(weaponsLevels[0]);
        weaponIndex = equippedWeapon;
        currentWeapon = weaponsAvailable[weaponIndex];
    }

    public Weapon getCurrentWeapon()
    {
        return this.currentWeapon;
    }

    public Weapon nextAvailable(bool switchWeapon)
    {
        Weapon returnWeapon;
        // Get next weapon available given the amount of weapons that are available
        int nextAvail = weaponIndex;

        do
        {
            nextAvail = nextAvail + 1 == WeaponManager.WEAPONS_AVAILABLE ? 0 : nextAvail++;
            returnWeapon = weaponsAvailable[nextAvail];
        } while (weaponsAvailable[nextAvail].getWeaponLevel() != 0);

        if (switchWeapon)
        {
            weaponIndex = nextAvail;
            currentWeapon = weaponsAvailable[weaponIndex];
        }
        return returnWeapon;
    }

    public Weapon previousAvailable(bool switchWeapon)
    {
        Weapon returnWeapon;
        // Get next weapon available given the amount of weapons that are available
        int prevAvail = weaponIndex;

        do
        {
            prevAvail = prevAvail - 1 < 0 ? WeaponManager.WEAPONS_AVAILABLE - 1 : prevAvail--;
            returnWeapon = weaponsAvailable[prevAvail];
        } while (weaponsAvailable[prevAvail].getWeaponLevel() != 0);

        if (switchWeapon)
        {
            weaponIndex = prevAvail;
            currentWeapon = weaponsAvailable[weaponIndex];
        }
        return returnWeapon;
    }
	
}
