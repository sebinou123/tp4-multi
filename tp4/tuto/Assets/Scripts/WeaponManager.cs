using UnityEngine;
using System.Collections;

/**
 * Class who show the array of weapon for the player, hava his current weapon and deal when he get new weapon or change his current weapon
 * */
public class WeaponManager
{
    public static int WEAPONS_AVAILABLE = 4;	//max of 4 weapon in the weapon manager
    public static Weapon[] weaponsAvailable;    //array of weapon 
    private Weapon currentWeapon;				//the current weapon use by the player
    private int weaponIndex;					//the current index of the weapon array

    public WeaponManager()
    {
        weaponsAvailable = new Weapon[WeaponManager.WEAPONS_AVAILABLE];

        weaponsAvailable[0] = new BasicSword(1);
        weaponsAvailable[1] = new SwordOfTruth(0);
        weaponsAvailable[2] = new KnightSword(0);
        weaponsAvailable[3] = new WhirlwindAxe(0);
        weaponIndex = 0;
        currentWeapon = weaponsAvailable[weaponIndex];
    }

    // Constructor to load from save file with levels on weapons
    public WeaponManager(int[] weaponsLevels, int equippedWeapon)
    {
        weaponsAvailable = new Weapon[WeaponManager.WEAPONS_AVAILABLE];

        weaponsAvailable[0] = new BasicSword(weaponsLevels[0]);
        weaponsAvailable[1] = new SwordOfTruth(weaponsLevels[1]);
        weaponsAvailable[2] = new KnightSword(weaponsLevels[2]);
        weaponsAvailable[2] = new WhirlwindAxe(weaponsLevels[3]);
        weaponIndex = equippedWeapon;
        currentWeapon = weaponsAvailable[weaponIndex];
    }

	//return the weapon use by the player
    public Weapon getCurrentWeapon()
    {
        return this.currentWeapon;
    }

	//method who return the next weapon in the array of weapon when the player decide to change
    public Weapon nextAvailable(bool switchWeapon)
    {
        Weapon returnWeapon;
        // Get next weapon available given the amount of weapons that are available
        int nextAvail = weaponIndex;

        do
        {
            nextAvail = nextAvail + 1 == WeaponManager.WEAPONS_AVAILABLE ? 0 : ++nextAvail;
            returnWeapon = weaponsAvailable[nextAvail];
        } while (weaponsAvailable[nextAvail].getWeaponLevel() == 0);

        if (switchWeapon)
        {
            weaponIndex = nextAvail;
            currentWeapon = weaponsAvailable[weaponIndex];
        }
        return returnWeapon;
    }

	//method who return the previous weapon in the array of weapon when the player decide to change
    public Weapon previousAvailable(bool switchWeapon)
    {
        Weapon returnWeapon;
        // Get next weapon available given the amount of weapons that are available
        int prevAvail = weaponIndex;

        do
        {
            prevAvail = prevAvail - 1 < 0 ? WeaponManager.WEAPONS_AVAILABLE - 1 : --prevAvail;
            returnWeapon = weaponsAvailable[prevAvail];
        } while (weaponsAvailable[prevAvail].getWeaponLevel() == 0);

        if (switchWeapon)
        {
            weaponIndex = prevAvail;
            currentWeapon = weaponsAvailable[weaponIndex];
        }
        return returnWeapon;
    }

	//return if the player got a new weapon (50%)
    public Weapon lootWeapon(int level)
    {
        Weapon newWeapon = null;

        if (Random.Range(0, 2) == 1)
        {
            newWeapon = Weapon.getWeaponDrop(level);
            weaponsAvailable[newWeapon.getWeaponImage()] = newWeapon;
        }

        return newWeapon;
    }

	//get index of the current weapon use by the player
    public int getCurrentWeaponIndex()
    {
        return this.weaponIndex;
    }
	
}
