namespace Dictionary

open Microsoft.EntityFrameworkCore

type DictionaryContext(options: DbContextOptions<DictionaryContext>) =
    inherit DbContext(options)

    member val Entries : DbSet<WordEntity> = base.Set<WordEntity>() with get, set

    override this.OnModelCreating(modelBuilder: ModelBuilder) =
        modelBuilder.Entity<WordEntity>().ToTable("Words") |> ignore
        base.OnModelCreating(modelBuilder)
