public class GrayZombie : Zombie {

    protected override void StatusInit() {
        MaxHealth = 50;
        CurrentHealth = MaxHealth;
        MoveSpeed = 2;
        AttackPower = 10;
        IsDead = MaxHealth > 0 ? false : true;
        IsAttacking = false;
    }

}
