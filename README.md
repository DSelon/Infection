# Infection

### 프로젝트 개요

졸업 작품으로 제출한 유니티 게임 개발 프로젝트

___

### 개발 기간

2024.9. ~ 2024.11. (3개월)

___

### 역할 분담

|이름|역할|
|:--:|:--:|
|고성현|기획+개발|
|김예현|캐릭터 3D 모델링|
|정현수|배경 3D 모델링|

___

### 적용된 기술

**Unity** | **C#**

___

### 작동 영상

https://blog.naver.com/cgko321/223679401063

___

### 프로젝트 세팅 방법

#### 과정
1. **Unity 2022.3.44f1** 버전의 유니티를 설치한다.
2. 해당 프로젝트를 컴퓨터로 내려받는다.
3. 프로젝트를 열고, 패키지 매니저에서 아래의 에셋들을 임포트한다.

#### 필요 에셋 목록
- Ancient City Ruins
- AllSky - 220+ Sky / Skybox Set
- Creep Horror Creature
- Epic Toon FX
- 5800 Fantasy RPG Icons Pack
- Joystick Pack
- Medieval Kingdom UI
- Morbid Creatures: Mutant 1
- Nodachi AnimSet
- Magic Slashes FX
- Ultimate Game Music Collection
- Ultimate Sound FX Bundle
- Unity-Chan! Model
- Zombie
- Hungry Zombie

___

### 프로젝트 구조

#### 폴더 구조
|경로|용도|
|:--|:--|
|Assets \ Infection|작업이 이루어지는 폴더|
|Assets \ ...|에셋 리소스/프로젝트 설정 등의 폴더|

|경로|용도|
|:--|:--|
|Assets \ Infection \ Animations|애니메이터/애니메이션 폴더|
|Assets \ Infection \ Audios|사운드 폴더|
|Assets \ Infection \ Fonts|폰트 폴더|
|Assets \ Infection \ Images|이미지 폴더|
|Assets \ Infection \ Materials|재료 폴더|
|Assets \ Infection \ Models|3D 모델 폴더|
|Assets \ Infection \ Particles|파티클 폴더|
|Assets \ Infection \ Prefabs|프리팹 폴더|
|Assets \ Infection \ Scenes|씬 폴더|
|Assets \ Infection \ Scripts|스크립트 폴더|

#### 씬 구조
|이름|용도|
|:--|:--|
|Title|게임을 처음 실행했을 때, 보여지는 화면|
|Main|게임의 메인이 되는 화면 (게임 시작/설정 버튼 등이 있음)|
|Room|실제로 플레이가 진행되는 화면|

#### 객체 구조
##### Block
맵에 배치되어 특정 기능을 실행하는 클래스 모음
|클래스|용도|
|:--|:--|
|Spawner \ ...|소환 클래스 모음|
|Spawner \ MonsterSpawner|몬스터 소환 클래스|
|System \ ...|시스템 클래스 모음|
|System \ WaveSystem|웨이브 시스템 클래스|

##### Camera
카메라 클래스 모음
|클래스|용도|
|:--|:--|
|CameraRatio|화면 비율 자동 조정 클래스|
|MainSceneCameraMovement|메인 씬 카메라 움직임 클래스|

##### Entity
플레이어/몬스터 등 개체 클래스 모음
|클래스|용도|
|:--|:--|
|Effect \ ...|효과 클래스 모음|
|Effect \ AuraReleaseEffect|오러방출 효과 클래스|
|Effect \ BarrierEffect|방벽 효과 클래스|
|Effect \ ElectricSlashEffect|전기 슬래시 효과 클래스|
|Effect \ FlameThrowerEffect|화염 방사 효과 클래스|
|Effect \ FlareSlash|불꽃 효과 클래스|
|Effect \ GasEffect|가스 효과 클래스|
|Effect \ GatheringAuraEffect|오러방출 시전 효과 클래스|
|Effect \ HealingAreaEffect|치유 영역 효과 클래스|
|Effect \ HealingEffect|치유 효과 클래스|
|Effect \ HighSpeedEffect|빠른 속도 효과 클래스|
|Effect \ JumpDownEffect|점프 다운 효과 클래스|
|Effect \ JumpUpEffect|점프 업 효과 클래스|
|Effect \ LightningStrikeEffect|번개 효과 클래스|
|Effect \ MagicCircleEffect|마법 소환진 효과 클래스|
|Effect \ PheonixEffect|불사조 효과 클래스|
|Effect \ VenomExplosionEffect|독 폭발 효과 클래스|
|Exp \ ...|경험치 클래스 모음|
|Exp \ Exp|경험치 클래스|
|LivingEntity \ ...|생명체 클래스 모음|
|LivingEntity \ Monster \ ...|몬스터 클래스 모음|
|LivingEntity \ Monster \ Creep \ ...|크립 몬스터 클래스 모음|
|LivingEntity \ Monster \ Creep \ Creep|크립 몬스터 클래스|
|LivingEntity \ Monster \ Venom \ ...|베놈 몬스터 클래스 모음|
|LivingEntity \ Monster \ Venom \ Venom|베놈 몬스터 클래스|
|LivingEntity \ Monster \ Zombie \ ...|좀비 몬스터 클래스 모음|
|LivingEntity \ Monster \ Zombie \ GrayZombie \ ...|그레이 좀비 몬스터 클래스 모음|
|LivingEntity \ Monster \ Zombie \ GrayZombie \ GrayZombie|그레이 좀비 몬스터 클래스|
|LivingEntity \ Monster \ Zombie \ RedZombie \ ...|레드 좀비 몬스터 클래스 모음|
|LivingEntity \ Monster \ Zombie \ RedZombie \ RedZombie|레드 좀비 몬스터 클래스|
|LivingEntity \ Monster \ Zombie \ Zombie|좀비 몬스터 클래스|
|LivingEntity \ Player \ ...|플레이어 클래스 모음|
|LivingEntity \ Player \ Ability \ ...|능력 클래스 모음|
|LivingEntity \ Player \ Ability \ AuraReleaseAbility|오러방출 능력 클래스|
|LivingEntity \ Player \ Ability \ BarrierAbility|방벽 능력 클래스|
|LivingEntity \ Player \ Ability \ ElectricWhirlwindAbility|전기폭풍 능력 클래스|
|LivingEntity \ Player \ Ability \ HealingArea|치유 영역 능력 클래스|
|LivingEntity \ Player \ Ability \ IAbility|능력 인터페이스|
|LivingEntity \ Player \ Ability \ PheonixSlashAbility|불사조 능력 클래스|
|LivingEntity \ Player \ Ability \ RollingAbility|구르기 능력 클래스|
|LivingEntity \ Player \ Player|플레이어 클래스|
|LivingEntity \ Player \ PlayerAbility|플레이어 능력 클래스|
|LivingEntity \ Player \ PlayerInput|플레이어 입력 클래스|
|LivingEntity \ Player \ PlayerMovement|플레이어 이동 클래스|

##### Manager
씬 별로 하나만 존재하는 관리 클래스 모음
|클래스|용도|
|:--|:--|
|MainSceneManager|메인 씬 관리 클래스|
|RoomSceneManager|룸 씬 관리 클래스|
|TitleSceneManager|타이틀 씬 관리 클래스|

##### Other
디자인 패턴 코드같은 기타 클래스 모음
|클래스|용도|
|:--|:--|
|Singleton|싱글톤 패턴 구조 생성 클래스|

##### Utility
자주 사용되는 기능들을 함수로 묶은 클래스 모음
|클래스|용도|
|:--|:--|
|AnimationUtility|애니메이션 기능 클래스|
|BGMUtility|BGM 기능 클래스|
|CoroutineUtility|코루틴 기능 클래스|
|DisplayUtility|화면 기능 클래스|
|LoadUtility|로딩 기능 클래스|

___

### 핵심 기능

#### Singleton 패턴 활용

##### Singleton.cs

```C#

using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    
    private static T instance;
    public static T Instance {
        get {
            if (instance == null) {

                instance = (T) FindObjectOfType(typeof(T));
                if (instance == null) {
                    GameObject gameObject = new GameObject(typeof(T).Name, typeof(T));
                    instance = gameObject.GetComponent<T>();
                }

            }

            return instance;
        }
    }

}

```

싱글톤 패턴을 상속을 통해 사용할 수 있게 만든 코드이다.\
제너릭과 제약조건 where을 활용해서 만든 코드로,\
MonoBehaviour 클래스를 상속받고 있는 클래스라면 해당 코드를 상속받아 사용할 수 있다.\
입력된 타입을 가진 객체를 찾고, 없으면 객체를 새로 생성하여, instance 변수에 초기화하는 방식이다.

#### 능력 구현

플레이어의 능력은 '능력 시전 → 능력 효과 생성 → 효과 기능 수행' 단계를 거쳐서 수행된다.\
이에 맞춰 클래스 역시 3구역(PlayerAbility, Ability, Effect)으로 나누었다.

|구역|용도|
|:--|:--|
|Entity \ LivingEntity \ Player \ PlayerAbility.cs|능력의 설명/아이콘이 담겨 있고, 능력 선택 기능을 수행|
|Entity \ LivingEntity \ Player \ Ability \ ...Ability.cs|능력의 재사용 대기 시간/시전 속도가 담겨 있고, 능력 사용 시 애니메이션 재생/시전 효과음 재생/능력 효과 생성을 수행|
|Entity \ Effect \ ...Effect.cs|능력 효과의 크기/피해량 등이 담겨 있고, 능력 효과의 충돌 감지 후 피해를 가하는 기능을 수행|

3구역으로 나누어서 구현하였기에,\
능력별로 애니메이션/효과음/효과/효과 기능이 다르더라도 대응하여 개발할 수 있다.

#### 카메라 진동 효과

카메라가 플레이어를 추적할 수 있도록 아래와 같이 코드를 작성하였다.

##### PlayerMovement.cs

```C#

using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    // 카메라 추적
    public void FollowCamera(Player player, Vector3 cameraPosition) {
        Vector3 position = player.transform.position;

        player.Camera.transform.position = Vector3.Lerp(cameraPosition, new Vector3(
            position.x + player.cameraOffsetPosition.x,
            position.y + player.cameraOffsetPosition.y,
            position.z + player.cameraOffsetPosition.z
            ), player.CameraFollowSpeed * Time.deltaTime);
    }

    ...

}

```

Vector3.Lerp()를 사용하여 카메라가 부드럽게 플레이어를 따라가도록 구현하였다.\
카메라의 위치가 갑작스럽게 바뀌더라도, 자연스럽게 플레이어를 다시 추적한다.\
그렇기에 카메라 진동 효과를 구현할 때, 카메라의 위치만 바꿔주면 되기에 굉장히 간편하다.\
아래는 카메라 위치를 변경하여 카메라 진동 효과를 준 코드이다.

##### Creep.cs

```C#

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Creep : MonoBehaviour, ILivingEntity, IMonster {

    ...

    // JumpDown
    private IEnumerator JumpDown() {

        ...

        // 카메라 흔들림 효과 부여
        Transform cameraTransform = camera.transform;
        Vector3 cameraPosition = cameraTransform.position;
        cameraTransform.position = new Vector3(cameraPosition.x, cameraPosition.y + 1.5f, cameraPosition.z);
        CoroutineUtility.CallWaitForSeconds(0.25f, () => {
            cameraPosition = cameraTransform.position;
            cameraTransform.position = new Vector3(cameraPosition.x, cameraPosition.y + 0.75f, cameraPosition.z);
        });

        ...

    }

    ...

}

```

#### 게임 종료 효과

게임이 종료될 때, 시간이 느려지는 효과를 주고 싶었다.\
시간이 느려지는 효과를 주기 위하여 아래의 코드를 작성하였다.

##### RoomSceneManager.cs

```C#

using System;
using System.Collections;
// using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class RoomSceneManager : Singleton<RoomSceneManager> {

    ...

    private IEnumerator COnPlayerDeath() {

        StartCoroutine(CTimeExpansionEffect());

        ...

    }

    ...

    private IEnumerator COnCreepDeath() {

        StartCoroutine(CTimeExpansionEffect());

        ...

    }

    private IEnumerator CTimeExpansionEffect() {
        float timeScaleChangeSpeed = 0.2f;
        float differenceTimeScale = Time.timeScale;
        float currentTimeScale = Time.timeScale;
        while (currentTimeScale > 0.2) {
            currentTimeScale -= differenceTimeScale * timeScaleChangeSpeed * Time.unscaledDeltaTime;
            Time.timeScale = currentTimeScale;

            yield return null;
        }
        Time.timeScale = 0.2f;
    }

}

```

Coroutine을 사용하여 Time.timeScale을 서서히 변경시켜주는 방식으로 시간이 느려지는 효과를 구현하였다.