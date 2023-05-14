using Domain.Entities;

namespace Persistence.Seed.Data
{
    public static class TableSeed
    {
        public static readonly Table[] Data =
        {
            new Table
            {
                Id = Guid.NewGuid(),
                Size = 6,
                FreeSpace = 6
            },
            new Table
            {
                Id = Guid.NewGuid(),
                Size = 5,
                FreeSpace = 5
            },
            new Table
            {
                Id = Guid.NewGuid(),
                Size = 4,
                FreeSpace = 4
            },
            new Table
            {
                Id = Guid.NewGuid(),
                Size = 4,
                FreeSpace = 4
            },
            new Table
            {
                Id = Guid.NewGuid(),
                Size = 3,
                FreeSpace = 3
            }
        };
    }
}
