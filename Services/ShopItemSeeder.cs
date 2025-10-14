using DungeonCrawlerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonCrawlerAPI.Data
{
    /// <summary>
    /// Seeder para poblar la tienda del sistema con items predefinidos.
    /// Los items con IsSystemItem = true son plantillas que se pueden comprar infinitamente.
    /// </summary>
    public class ShopItemsSeeder
    {
        private readonly AppDBContext _context;

        public ShopItemsSeeder(AppDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Ejecuta el seed de items de la tienda del sistema
        /// </summary>
        public async Task SeedShopItemsAsync()
        {
            // Verificar si ya existen items de sistema
            var existingSystemItems = await _context.Items
                .Where(i => i.IsSystemItem)
                .AnyAsync();

            if (existingSystemItems)
            {
                Console.WriteLine("Los items de la tienda ya existen. Saltando seed...");
                return;
            }

            var shopItems = GetShopItemsTemplate();

            foreach (var item in shopItems)
            {
                await _context.Items.AddAsync(item);
            }

            await _context.SaveChangesAsync();
            Console.WriteLine($"Se agregaron {shopItems.Count} items a la tienda del sistema.");
        }

        /// <summary>
        /// Define los items que estarán disponibles en la tienda
        /// </summary>
        private List<MItems> GetShopItemsTemplate()
        {
            return new List<MItems>
            {
                // ARMAS
                new MItems
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Espada de Hierro",
                    Description = "Una espada básica forjada en hierro. Perfecta para aventureros principiantes.",
                    ItemType = ItemType.Weapon,
                    Value = 100,
                    IsSystemItem = true,
                    InventaryId = null, // Items de tienda no tienen inventario
                    Stats = new ItemStats
                    {
                        Strength = 5,
                        AttackSpeed = 2
                    },
                    CreatedBy = "System"
                },
                new MItems
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Espada de Acero",
                    Description = "Una espada de acero templado. Más resistente y afilada que el hierro.",
                    ItemType = ItemType.Weapon,
                    Value = 250,
                    IsSystemItem = true,
                    InventaryId = null,
                    Stats = new ItemStats
                    {
                        Strength = 12,
                        AttackSpeed = 3,
                        CriticalChance = 5
                    },
                    CreatedBy = "System"
                },
                new MItems
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Báculo de Mago",
                    Description = "Un báculo imbuido con energía mágica arcana.",
                    ItemType = ItemType.Weapon,
                    Value = 200,
                    IsSystemItem = true,
                    InventaryId = null,
                    Stats = new ItemStats
                    {
                        Intelligence = 10,
                        Mana = 30
                    },
                    CreatedBy = "System"
                },
                new MItems
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Arco Largo",
                    Description = "Un arco de madera reforzada con alcance superior.",
                    ItemType = ItemType.Weapon,
                    Value = 180,
                    IsSystemItem = true,
                    InventaryId = null,
                    Stats = new ItemStats
                    {
                        Dexterity = 8,
                        CriticalChance = 10,
                        AttackSpeed = 4
                    },
                    CreatedBy = "System"
                },

                // ARMADURAS
                new MItems
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Armadura de Cuero",
                    Description = "Protección básica de cuero endurecido.",
                    ItemType = ItemType.Armor,
                    Value = 80,
                    IsSystemItem = true,
                    InventaryId = null,
                    Stats = new ItemStats
                    {
                        Armor = 5,
                        Health = 20
                    },
                    CreatedBy = "System"
                },
                new MItems
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Armadura de Placas",
                    Description = "Armadura pesada de placas de acero. Ofrece gran protección.",
                    ItemType = ItemType.Armor,
                    Value = 300,
                    IsSystemItem = true,
                    InventaryId = null,
                    Stats = new ItemStats
                    {
                        Armor = 15,
                        Health = 50,
                        Vitality = 5
                    },
                    CreatedBy = "System"
                },
                new MItems
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Túnica Mágica",
                    Description = "Túnica encantada que protege contra magia enemiga.",
                    ItemType = ItemType.Armor,
                    Value = 220,
                    IsSystemItem = true,
                    InventaryId = null,
                    Stats = new ItemStats
                    {
                        MagicResist = 12,
                        Mana = 40,
                        Intelligence = 5
                    },
                    CreatedBy = "System"
                },
                new MItems
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Capa del Explorador",
                    Description = "Capa ligera que mejora la agilidad.",
                    ItemType = ItemType.Armor,
                    Value = 150,
                    IsSystemItem = true,
                    InventaryId = null,
                    Stats = new ItemStats
                    {
                        Armor = 3,
                        Dexterity = 8,
                        AttackSpeed = 2
                    },
                    CreatedBy = "System"
                },

                // CONSUMIBLES
                new MItems
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Poción de Vida Menor",
                    Description = "Restaura una pequeña cantidad de vida.",
                    ItemType = ItemType.Consumible,
                    Value = 25,
                    IsSystemItem = true,
                    InventaryId = null,
                    Stats = new ItemStats
                    {
                        Health = 50
                    },
                    CreatedBy = "System"
                },
                new MItems
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Poción de Vida Mayor",
                    Description = "Restaura una gran cantidad de vida.",
                    ItemType = ItemType.Consumible,
                    Value = 75,
                    IsSystemItem = true,
                    InventaryId = null,
                    Stats = new ItemStats
                    {
                        Health = 150
                    },
                    CreatedBy = "System"
                },
                new MItems
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Poción de Maná",
                    Description = "Restaura energía mágica.",
                    ItemType = ItemType.Consumible,
                    Value = 30,
                    IsSystemItem = true,
                    InventaryId = null,
                    Stats = new ItemStats
                    {
                        Mana = 80
                    },
                    CreatedBy = "System"
                },
                new MItems
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Elixir de Fuerza",
                    Description = "Aumenta temporalmente la fuerza del usuario.",
                    ItemType = ItemType.Consumible,
                    Value = 50,
                    IsSystemItem = true,
                    InventaryId = null,
                    Stats = new ItemStats
                    {
                        Strength = 10
                    },
                    CreatedBy = "System"
                }
            };
        }

        /// <summary>
        /// Método para limpiar items de la tienda (útil para desarrollo/testing)
        /// </summary>
        public async Task ClearShopItemsAsync()
        {
            var systemItems = await _context.Items
                .Where(i => i.IsSystemItem)
                .ToListAsync();

            _context.Items.RemoveRange(systemItems);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Se eliminaron {systemItems.Count} items de la tienda.");
        }

        /// <summary>
        /// Método para agregar un item individual a la tienda
        /// </summary>
        public async Task AddShopItemAsync(string name, string description, ItemType itemType,
            int value, ItemStats stats)
        {
            var item = new MItems
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Description = description,
                ItemType = itemType,
                Value = value,
                IsSystemItem = true,
                InventaryId = null,
                Stats = stats,
                CreatedBy = "System"
            };

            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Item '{name}' agregado a la tienda.");
        }
    }
}