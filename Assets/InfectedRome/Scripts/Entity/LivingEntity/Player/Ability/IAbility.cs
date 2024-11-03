using UnityEngine;

public interface IAbility {

    float maxCooldown { get; set; }
    float currentCooldown { get; set; }

    Sprite icon { get; set; }
	
    void UseAbility(Player player);

}
