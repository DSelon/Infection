public class RedZombie : Zombie {

    protected override void StatusInit() {
        MaxHealth = 70.0f;
        CurrentHealth = MaxHealth;
        MoveSpeed = 3.0f;
        AttackPower = 15.0f;
        IsDead = MaxHealth > 0 ? false : true;
        IsAttacking = false;
    }

    protected override void OptionInit()
    {
        AttackSpeed = 1.0f;
        AttackTime = 0.3f;
        DetectDistance = 100.0f;
        AttackDistance = 1.5f;
    }

}
