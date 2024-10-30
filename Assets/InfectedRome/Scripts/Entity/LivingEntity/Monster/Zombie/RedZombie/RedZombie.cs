public class RedZombie : Zombie {

    protected override void StatusInit() {
        MaxHealth = 70.0f;
        CurrentHealth = MaxHealth;
        MoveSpeed = 3.0f;
        AttackPower = 10.0f;
        IsDead = MaxHealth > 0 ? false : true;
        IsAttacking = false;
    }

}
