namespace Items.Weapons
{
    public abstract class ChargedWeaponAction : WeaponAction
    {
        protected float ChargeTime;
        protected bool IsCharging;
        protected bool IsCharged;
    }
}