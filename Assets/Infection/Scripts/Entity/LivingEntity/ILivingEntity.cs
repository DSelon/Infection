public interface ILivingEntity {
	
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
    float MoveSpeed { get; set; }

    bool IsDead { get; }



    void Heal(float amount);
    void Damage(float amount, bool isGenerateBloodEffect = true);
    void Die();

}
