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

___

### 문제 & 해결 과정

___

### 부족한 점