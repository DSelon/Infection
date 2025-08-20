using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour {

    [NonSerialized] public int auraReleaseNumber = 0;
    [NonSerialized] public int pheonixSlashNumber = 1;
    [NonSerialized] public int electricWhirlwindNumber = 2;
    [NonSerialized] public int rollingNumber = 3;
    [NonSerialized] public int barrierNumber = 4;
    [NonSerialized] public int healingAreaNumber = 5;
    
    public Sprite auraReleaseIcon;
    public Sprite pheonixSlashIcon;
    public Sprite electricWhirlwindIcon;
    public Sprite rollingIcon;
    public Sprite barrierIcon;
    public Sprite healingAreaIcon;
    public Sprite emptyIcon;

    [NonSerialized] public string auraReleaseName = "오러방출";
    [NonSerialized] public string pheonixSlashName = "불사조";
    [NonSerialized] public string electricWhirlwindName = "전기폭풍";
    [NonSerialized] public string rollingName = "구르기";
    [NonSerialized] public string barrierName = "방벽";
    [NonSerialized] public string healingAreaName = "치유영역";
    [NonSerialized] public string emptyName = "";

    [NonSerialized] public string[] auraReleaseDescriptions = {
        "검을 360도로 휘둘러, 범위 내의 몬스터에게 피해를 가합니다.",
        "재사용 대기 시간 40% 가속",
        "범위 30% 증가",
        "사용 중 받는 피해 80% 감소"
    };
    [NonSerialized] public string[] pheonixSlashDescriptions = {
        "바라보는 방향으로 검을 휘둘러, 불사조를 날립니다.",
        "재사용 대기 시간 40% 가속",
        "범위 15% 증가",
        "사용 중 받는 피해 80% 감소"
    };
    [NonSerialized] public string[] electricWhirlwindDescriptions = {
        "검을 360도로 계속해서 휘둘러, 범위 내의 몬스터에게 지속적으로 피해를 가합니다.",
        "재사용 대기 시간 40% 가속",
        "범위 30% 증가",
        "사용 중 받는 피해 60% 감소",
        "흡혈 1% 효과 추가"
    };
    [NonSerialized] public string[] rollingDescriptions = {
        "바라보는 방향으로 빠르게 굴러 이동합니다.",
        "재사용 대기 시간 40% 가속",
        "사용 시 체력 5% 회복",
        "사용 시 일시적으로 이동 속도 30% 증가"
    };
    [NonSerialized] public string[] barrierDescriptions = {
        "모든 공격을 막는 방벽으로 플레이어를 감쌉니다.",
        "재사용 대기 시간 25% 가속",
        "지속 시간 10% 증가",
        "사용 시 체력 5% 회복"
    };
    [NonSerialized] public string[] healingAreaDescriptions = {
        "플레이어를 치유하는 영역을 생성합니다.",
        "재사용 대기 시간 20% 가속",
        "지속 시간 15% 증가",
        "범위 10% 증가",
        "치유량 10% 증가"
    };
    [NonSerialized] public string emptyDescription = "";

    public (Sprite, string, string)[] cards = new (Sprite, string, string)[27];
    [NonSerialized] public int[] selectedCards = new int[3];
    [NonSerialized] public List<int> usingCards = new List<int>();

    [NonSerialized] public int[] abilityCardNumbers = new int[6];



    private void Start() {
        int number = 0;
        cards[number] = (emptyIcon, emptyName, emptyDescription);
        number++;
        abilityCardNumbers[0] = number;
        foreach (string auraReleaseDescription in auraReleaseDescriptions) {
            cards[number] = (auraReleaseIcon, auraReleaseName, auraReleaseDescription);
            number++;
        }
        abilityCardNumbers[1] = number;
        foreach (string pheonixSlashDescription in pheonixSlashDescriptions) {
            cards[number] = (pheonixSlashIcon, pheonixSlashName, pheonixSlashDescription);
            number++;
        }
        abilityCardNumbers[2] = number;
        foreach (string electricWhirlwindDescription in electricWhirlwindDescriptions) {
            cards[number] = (electricWhirlwindIcon, electricWhirlwindName, electricWhirlwindDescription);
            number++;
        }
        abilityCardNumbers[3] = number;
        foreach (string rollingDescription in rollingDescriptions) {
            cards[number] = (rollingIcon, rollingName, rollingDescription);
            number++;
        }
        abilityCardNumbers[4] = number;
        foreach (string barrierDescription in barrierDescriptions) {
            cards[number] = (barrierIcon, barrierName, barrierDescription);
            number++;
        }
        abilityCardNumbers[5] = number;
        foreach (string healingAreaDescription in healingAreaDescriptions) {
            cards[number] = (healingAreaIcon, healingAreaName, healingAreaDescription);
            number++;
        }
    }
    
    
    
    public void PrintCard(Player player) {
        for (int i = 0; i < selectedCards.Length; i++) {
            int number = 0;
            int loopCount = 0;
            while (loopCount < 1000) {
                loopCount++;
                number = UnityEngine.Random.Range(1, cards.Length);

                // 이미 사용된 적 있는 카드일 경우
                if (usingCards.Contains(number)) continue;

                // 앞선 카드들과 같은 카드일 경우
                bool selectedCard = false;
                for (int s = 0; s < i; s++) {
                    if (selectedCards[s] == number) {
                        selectedCard = true;
                        break;
                    }
                }
                if (selectedCard) continue;

                int abilityCardNumber = 0;
                bool isAbilityCardNumber = false;
                for (int a = 0; a < abilityCardNumbers.Length; a++) {
                    if (abilityCardNumbers[a] <= number) {
                        if (abilityCardNumbers[a] == number) {
                            isAbilityCardNumber = true;
                            break;
                        }
                        abilityCardNumber = abilityCardNumbers[a];
                    }
                }
                if (isAbilityCardNumber) {

                    // 능력 슬롯이 가득 찬 경우
                    bool isFullAbility = true;
                    for (int f = 0; f < player.equippedAbilities.Length; f++) {
                        if (player.equippedAbilities[f] == null) {
                            isFullAbility = false;
                            break;
                        }
                    }
                    if (isFullAbility) continue;

                    // 능력 슬롯이 모두 비워져 있을 경우
                    bool isEmptyAbility = true;
                    for (int e = 0; e < player.equippedAbilities.Length; e++) {
                        if (player.equippedAbilities[e] != null) {
                            isEmptyAbility = false;
                            break;
                        }
                    }
                    if (isEmptyAbility) {
                        if (number != abilityCardNumbers[0]
                        && number != abilityCardNumbers[1]
                        && number != abilityCardNumbers[2]) continue;
                    }

                }
                else {

                    // 장착된 능력의 트리 카드가 아닐 경우
                    if (!usingCards.Contains(abilityCardNumber)) continue;

                }

                break;
            }

            selectedCards[i] = number;
        }
    }
    
    public void PickCard(int number, Player player) {
        usingCards.Add(selectedCards[number]);

        IAbility[] equippedAbilities = player.equippedAbilities;
        if (selectedCards[number] == abilityCardNumbers[0]) {
            int nullNumber = 0;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] == null) {
                    nullNumber = i;
                    break;
                }
            }
            player.equippedAbilities[nullNumber] = player.abilities[auraReleaseNumber];
            player.abilityIcons[nullNumber].sprite = player.abilities[0].icon;
        }
        else if (selectedCards[number] == abilityCardNumbers[0] + 1) {
            AuraReleaseAbility auraReleaseAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is AuraReleaseAbility) {
                    auraReleaseAbility = (AuraReleaseAbility) player.equippedAbilities[i];
                    break;
                }
            }
            auraReleaseAbility.cooldownTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[0] + 2) {
            AuraReleaseAbility auraReleaseAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is AuraReleaseAbility) {
                    auraReleaseAbility = (AuraReleaseAbility) player.equippedAbilities[i];
                    break;
                }
            }
            auraReleaseAbility.radiusTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[0] + 3) {
            AuraReleaseAbility auraReleaseAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is AuraReleaseAbility) {
                    auraReleaseAbility = (AuraReleaseAbility) player.equippedAbilities[i];
                    break;
                }
            }
            auraReleaseAbility.damageReductionTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[1]) {
            int nullNumber = 0;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] == null) {
                    nullNumber = i;
                    break;
                }
            }
            player.equippedAbilities[nullNumber] = player.abilities[pheonixSlashNumber];
            player.abilityIcons[nullNumber].sprite = player.abilities[1].icon;
        }
        else if (selectedCards[number] == abilityCardNumbers[1] + 1) {
            PheonixSlashAbility pheonixSlashAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is PheonixSlashAbility) {
                    pheonixSlashAbility = (PheonixSlashAbility) player.equippedAbilities[i];
                    break;
                }
            }
            pheonixSlashAbility.cooldownTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[1] + 2) {
            PheonixSlashAbility pheonixSlashAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is PheonixSlashAbility) {
                    pheonixSlashAbility = (PheonixSlashAbility) player.equippedAbilities[i];
                    break;
                }
            }
            pheonixSlashAbility.radiusTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[1] + 3) {
            PheonixSlashAbility pheonixSlashAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is PheonixSlashAbility) {
                    pheonixSlashAbility = (PheonixSlashAbility) player.equippedAbilities[i];
                    break;
                }
            }
            pheonixSlashAbility.damageReductionTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[2]) {
            int nullNumber = 0;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] == null) {
                    nullNumber = i;
                    break;
                }
            }
            player.equippedAbilities[nullNumber] = player.abilities[electricWhirlwindNumber];
            player.abilityIcons[nullNumber].sprite = player.abilities[2].icon;
        }
        else if (selectedCards[number] == abilityCardNumbers[2] + 1) {
            ElectricWhirlwindAbility electricWhirlwindAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is ElectricWhirlwindAbility) {
                    electricWhirlwindAbility = (ElectricWhirlwindAbility) player.equippedAbilities[i];
                    break;
                }
            }
            electricWhirlwindAbility.cooldownTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[2] + 2) {
            ElectricWhirlwindAbility electricWhirlwindAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is ElectricWhirlwindAbility) {
                    electricWhirlwindAbility = (ElectricWhirlwindAbility) player.equippedAbilities[i];
                    break;
                }
            }
            electricWhirlwindAbility.radiusTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[2] + 3) {
            ElectricWhirlwindAbility electricWhirlwindAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is ElectricWhirlwindAbility) {
                    electricWhirlwindAbility = (ElectricWhirlwindAbility) player.equippedAbilities[i];
                    break;
                }
            }
            electricWhirlwindAbility.damageReductionTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[2] + 4) {
            ElectricWhirlwindAbility electricWhirlwindAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is ElectricWhirlwindAbility) {
                    electricWhirlwindAbility = (ElectricWhirlwindAbility) player.equippedAbilities[i];
                    break;
                }
            }
            electricWhirlwindAbility.bloodSuckingTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[3]) {
            int nullNumber = 0;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] == null) {
                    nullNumber = i;
                    break;
                }
            }
            player.equippedAbilities[nullNumber] = player.abilities[rollingNumber];
            player.abilityIcons[nullNumber].sprite = player.abilities[3].icon;
        }
        else if (selectedCards[number] == abilityCardNumbers[3] + 1) {
            RollingAbility rollingAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is RollingAbility) {
                    rollingAbility = (RollingAbility) player.equippedAbilities[i];
                    break;
                }
            }
            rollingAbility.cooldownTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[3] + 2) {
            RollingAbility rollingAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is RollingAbility) {
                    rollingAbility = (RollingAbility) player.equippedAbilities[i];
                    break;
                }
            }
            rollingAbility.recoveryHealthTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[3] + 3) {
            RollingAbility rollingAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is RollingAbility) {
                    rollingAbility = (RollingAbility) player.equippedAbilities[i];
                    break;
                }
            }
            rollingAbility.incresedMoveSpeedTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[4]) {
            int nullNumber = 0;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] == null) {
                    nullNumber = i;
                    break;
                }
            }
            player.equippedAbilities[nullNumber] = player.abilities[barrierNumber];
            player.abilityIcons[nullNumber].sprite = player.abilities[4].icon;
        }
        else if (selectedCards[number] == abilityCardNumbers[4] + 1) {
            BarrierAbility barrierAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is BarrierAbility) {
                    barrierAbility = (BarrierAbility) player.equippedAbilities[i];
                    break;
                }
            }
            barrierAbility.cooldownTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[4] + 2) {
            BarrierAbility barrierAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is BarrierAbility) {
                    barrierAbility = (BarrierAbility) player.equippedAbilities[i];
                    break;
                }
            }
            barrierAbility.durationTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[4] + 3) {
            BarrierAbility barrierAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is BarrierAbility) {
                    barrierAbility = (BarrierAbility) player.equippedAbilities[i];
                    break;
                }
            }
            barrierAbility.recoveryHealthTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[5]) {
            int nullNumber = 0;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] == null) {
                    nullNumber = i;
                    break;
                }
            }
            player.equippedAbilities[nullNumber] = player.abilities[healingAreaNumber];
            player.abilityIcons[nullNumber].sprite = player.abilities[5].icon;
        }
        else if (selectedCards[number] == abilityCardNumbers[5] + 1) {
            HealingAreaAbility healingAreaAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is HealingAreaAbility) {
                    healingAreaAbility = (HealingAreaAbility) player.equippedAbilities[i];
                    break;
                }
            }
            healingAreaAbility.cooldownTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[5] + 2) {
            HealingAreaAbility healingAreaAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is HealingAreaAbility) {
                    healingAreaAbility = (HealingAreaAbility) player.equippedAbilities[i];
                    break;
                }
            }
            healingAreaAbility.durationTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[5] + 3) {
            HealingAreaAbility healingAreaAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is HealingAreaAbility) {
                    healingAreaAbility = (HealingAreaAbility) player.equippedAbilities[i];
                    break;
                }
            }
            healingAreaAbility.radiusTree = true;
        }
        else if (selectedCards[number] == abilityCardNumbers[5] + 4) {
            HealingAreaAbility healingAreaAbility = null;
            for (int i = 0; i < equippedAbilities.Length; i++) {
                if (equippedAbilities[i] is HealingAreaAbility) {
                    healingAreaAbility = (HealingAreaAbility) player.equippedAbilities[i];
                    break;
                }
            }
            healingAreaAbility.increasedHealTree = true;
        }

    }

}
