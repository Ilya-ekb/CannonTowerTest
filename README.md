# CannonTower Test Project

## Оглавление
1. [Общее описание](#общее-описание)  
2. [Технологии](#технологии)  
3. [Архитектура](#архитектура)  
   - [DI / VContainer](#di--vcontainer)  
   - [Update Loop](#update-loop)  
   - [Пулы объектов](#пулы-объектов)  
   - [Сервисы](#сервисы)  
   - [Башни](#башни)  
   - [Снаряды](#снаряды)  
   - [Монстры и спавн](#монстры-и-спавн)  
   - [Коллизии и урон](#коллизии-и-урон)  
4. [Типы прицеливания](#типы-прицеливания)  
5. [Жизненный цикл снаряда](#жизненный-цикл-снаряда)  
6. [Паттерны и принципы](#паттерны-и-принципы)  
7. [Как запустить](#как-запустить)  
8. [Как расширять](#как-расширять)  

---

## Общее описание
Прототип башенной обороны с архитектурой, ориентированной на **SOLID** и **Dependency Injection (VContainer)**.  

- Башни (`CannonTower`, `GuidedTower`) атакуют монстров.  
- Снаряды управляются **пулом объектов** и сервисами.  
- Поддерживаются разные типы прицеливания: мгновенное, предиктивное, параболическое, самонаводящееся.  
- Код разделён на слои и сервисы для простоты расширения.  

---

## Технологии
- **Unity 6000.0.50f1**  
- **VContainer** — внедрение зависимостей  
- **TriInspector** — расширения для инспектора
---

## Архитектура

### DI / VContainer
Все зависимости регистрируются в `GameInstaller`:

```csharp
builder.Register<ClosestTargetService>(Lifetime.Scoped).As<ITargetService>();
builder.Register<ProjectileLifecycleService>(Lifetime.Scoped).As<IProjectileLifecycleService>();
builder.Register<PoolService>(Lifetime.Scoped).As<IPoolService>().WithParameter(transform).AsSelf();
```

Prefab'ы (`Monster`, `CannonProjectile`, `GuidedProjectile`) регистрируются через `PoolService`.

---

### Update Loop
- Вместо `MonoBehaviour.Update` используется `UpdateService`.
- Все наследники `UpdateableBehaviour` регистрируются в нём и получают вызовы `OnUpdate(float deltaTime)`.
- Это делает апдейты управляемыми и тестируемыми.

---

### Пулы объектов
- Интерфейс: `IPoolService`, `IPoolable`.
- Реализация: `PoolService` хранит очереди объектов.
- Используется для всех снарядов и монстров (оптимизация под GC).

---

### Сервисы
- **TargetService** — выбор ближайшей цели.
- **AimingService** — стратегия прицеливания.
- **ProjectileLifecycleService** — управление жизненным циклом снарядов.
- **CollisionRegistry / HitService** — обработка попаданий.
- **CooldownService** — простой таймер интервалов.

---

### Башни
- Базовый класс: `BaseTower<TProjectile>`
- Настройки задаются в `TowerConfig`:
    - интервал стрельбы
    - скорость поворота
    - урон
    - тип прицеливания
    - гравитация на снаряд (опционально)

**Реализации:**
- `CannonTower` — классическая пушка с баллистикой.
- `GuidedTower` — стреляет самонаводящимися снарядами.

---

### Снаряды
- Базовый класс: `BaseProjectile`
    - управляется через `Mover` (`TransformMover`),
    - учитывает гравитацию,
    - имеет `lifeTime`,
    - по попаданию генерирует события.

**Типы:**
- `CannonProjectile` — баллистическая траектория.
- `GuidedProjectile` — самонаведение на цель.

---

### Монстры и спавн
- `MonsterSpawner` — создает врагов с интервалом.
- Монстры реализуют `IShootTarget` и регистрируются в `TargetRegistry`.
- управляются через `Mover` (`RigidbodyMover`),
- Возвращаются в пул после уничтожения.

---

### Коллизии и урон
- `ICollisionRegistry` — хранит связь коллайдера и логики урона.
- `IHitService` — сообщает о попадании.
- `IHit` — интерфейс для объектов, которые являются инициаторами столкновений.
- `IDamage` — интерфейс для объектов, которые являются источниками урона.

---

## Типы прицеливания
- **InstantAimingService** — мгновенно целится в позицию цели.
- **SmoothAimingService** — плавно доворачивает ствол.
- **PredictiveAimingService** — стреляет с упреждением (учёт скорости цели).
- **ParabolicAimingService** — решает баллистическую задачу (низкая/высокая траектория, поддержка упреждения).

---

## Жизненный цикл снаряда
1. Башня вызывает `ProjectileLifecycleService.LaunchProjectile`.
2. Снаряд берется из пула.
3. Двигается через `Mover`.
4. При столкновении вызывает `IHitService.ReportHit`.
5. Возвращается в пул по истечении времени жизни или после попадания.

---

## Паттерны и принципы
- **SOLID**
    - SRP: каждая сущность отвечает только за одну задачу.
    - OCP: новые башни и снаряды можно добавлять через расширение.
    - LSP: IProjectile, IShootTarget, IAimingService подменяются без ошибок.
    - ISP: интерфейсы маленькие и специализированные.
    - DIP: зависимости внедряются через интерфейсы и DI.
- **Dependency Injection** — через VContainer.
- **Object Pool** — для оптимизации памяти.

---

## Как запустить
1. Установите Unity **6000.0.50f1**.
2. Откройте проект.
3. Загрузите сцену `Start`.
4. Нажмите **Play** — башни начнут стрелять по монстрам.

---

## Как расширять

### Новый тип башни
1. Унаследуйте `BaseTower<TProjectile>`.
2. Создайте новый класс снаряда (`BaseProjectile`).
3. Добавьте prefab в `GameInstaller`.

### Новый тип прицеливания
1. Реализуйте `IAimingService`.
2. Зарегистрируйте в `AimingFactory`.
3. Выберите в `TowerConfig`.

### Новый враг
1. Реализуйте `IShootTarget`.
2. Добавьте prefab в пул.

---

## Пример TowerConfig
```csharp
[CreateAssetMenu(menuName = "Configs/TowerConfig")]
public class TowerConfig : ScriptableObject
{
        public float shootInterval = 0.5f;
        public float projectileSpeed = 10f;
        public float findTargetDistance = 10f;
        public float rotateSpeed = 10f;
        public int projectileDamage = 10;
        public AimingType aimingType = AimingType.Instant;
        public Vector3 gravity = Physics.gravity;
}
```

---

## Итог
Архитектура проекта:
- Чётко разделены роли (сервисы, башни, снаряды).
- Расширяема для новых механик.
- Оптимизирована под производительность (пулы, отсутствие лишних аллокаций).
