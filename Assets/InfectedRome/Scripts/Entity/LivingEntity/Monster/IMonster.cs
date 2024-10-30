using UnityEngine;

public interface IMonster {

    float DetectDistance { get; set; }
    float AttackDistance { get; set; }

    GameObject Target { get; set; }

    void Detect(GameObject target);
    void Chase(Transform targetTransform);
    void Attack(GameObject target);
    void Idle();

}
