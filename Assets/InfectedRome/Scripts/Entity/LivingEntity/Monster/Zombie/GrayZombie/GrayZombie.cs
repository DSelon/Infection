public class GrayZombie : Zombie {

    protected override void StatusInit() {
        MaxHealth = 50.0f;
        CurrentHealth = MaxHealth;
        MoveSpeed = 2.0f;
        AttackPower = 10.0f;
        IsDead = MaxHealth > 0 ? false : true;
        IsAttacking = false;
    }

}
