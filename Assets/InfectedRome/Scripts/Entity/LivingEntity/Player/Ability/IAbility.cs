public interface IAbility {

    float cooldown { get; set; }
    float damage { get; set; }

    float currentCooldown { get; set; }

    float operatingSpeed { get; set; }
    float operatingTime { get; set; }
    

	
    void UseAbility(Player player);

}
