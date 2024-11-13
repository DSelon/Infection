public class RedZombie : Zombie {

    protected override void StatusInit() {
        MaxHealth = 70;
        CurrentHealth = MaxHealth;
        MoveSpeed = 3;
        AttackPower = 15;
        IsDead = MaxHealth > 0 ? false : true;
        IsAttacking = false;
    }

    protected override void OptionInit()
    {
        AttackSpeed = 1;
        AttackTime = 0.3f;
        DetectDistance = 500;
        AttackDistance = 1.5f;
    }

}
