using System;

public static class ItemFactory
{
    public static HealthPotion CreateSmallHealthPotion()
        => new HealthPotion("Poción Pequeña de Vida", 20, "Restaura 20 puntos de vida.");

    public static HealthPotion CreateHealthPotion()
        => new HealthPotion("Poción de Vida", 50, "Restaura 50 puntos de vida.");

    public static HealthPotion CreateLargeHealthPotion()
        => new HealthPotion("Poción Grande de Vida", 100, "Restaura 100 puntos de vida.");

    public static DamagePotion CreateDamagePotion()
        => new DamagePotion("Poción de Fuerza", 5, "Incrementa tu daño en 5 puntos permanentemente.");

    public static DamagePotion CreateGreaterDamagePotion()
        => new DamagePotion("Poción de Fuerza Mayor", 10, "Incrementa tu daño en 10 puntos permanentemente.");

    public static CombatItem CreateFireBomb()
        => new CombatItem("Bomba de Fuego", 30, "Lanza una bomba que inflige 30 de daño al enemigo.");

    public static CombatItem CreatePoisonVial()
        => new CombatItem("Vial de Veneno", 20, "Veneno concentrado que inflige 20 de daño al enemigo.");

    public static CombatItem CreateThunderOrb()
        => new CombatItem("Orbe de Trueno", 50, "Un orbe mágico que descarga 50 de daño sobre el enemigo.");
}
