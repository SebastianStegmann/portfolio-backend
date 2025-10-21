using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;
using DataServiceLayer.Models;
using DataServiceLayer.Models.NameBasics;

namespace DataServiceLayer;

public class ImdbContext : DbContext
{
    //Title
    public DbSet<TitleBasics> TitleBasics { get; set; }
    public DbSet<TitleEpisode> TitleEpisodes { get; set; }
    public DbSet<TitleGenre> TitleGenres { get; set; }
    public DbSet<TitlePrincipal> TitlePrincipals { get; set; }
    public DbSet<TitleAka> TitleAka { get; set; }

    //Name
    public DbSet<NameBasics> NameBasics { get; set; }
    public DbSet<NameProfession> NameProfessions { get; set; }
    public DbSet<Profession> Professions { get; set; }
    public DbSet<KnownFor> KnownFors { get; set; }
    public DbSet<Role> Roles { get; set; }

    public ImdbContext(DbContextOptions<ImdbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Title
        modelBuilder.Entity<TitleBasics>().ToTable("title_basics");
        modelBuilder.Entity<TitleBasics>(entity =>
        {
            entity.HasKey(e => e.Tconst);
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.TitleType).HasColumnName("titletype");
            entity.Property(e => e.PrimaryTitle).HasColumnName("primarytitle");
            entity.Property(e => e.OriginalTitle).HasColumnName("originaltitle");
            entity.Property(e => e.IsAdult).HasColumnName("isadult");
            entity.Property(e => e.ReleaseDate).HasColumnName("releasedate");
            entity.Property(e => e.EndYear).HasColumnName("endyear");
            entity.Property(e => e.TotalSeasons).HasColumnName("totalseasons");
            entity.Property(e => e.Plot).HasColumnName("plot");
            entity.Property(e => e.Poster).HasColumnName("poster");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.RuntimeMinutes).HasColumnName("runtimeminutes");
        });

        modelBuilder.Entity<TitleEpisode>().ToTable("title_episode");
        modelBuilder.Entity<TitleEpisode>(entity =>
        {
            entity.HasKey(e => e.Tconst);
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.ParentTconst).HasColumnName("parenttconst");
            entity.Property(e => e.PrimaryTitle).HasColumnName("primarytitle");
            entity.Property(e => e.OriginalTitle).HasColumnName("originaltitle");
            entity.Property(e => e.IsAdult).HasColumnName("isadult");
            entity.Property(e => e.ReleaseDate).HasColumnName("releasedate");
            entity.Property(e => e.RuntimeMinutes).HasColumnName("runtimeminutes");
            entity.Property(e => e.Poster).HasColumnName("poster");
            entity.Property(e => e.Plot).HasColumnName("plot");
            entity.Property(e => e.SeasonNumber).HasColumnName("seasonnumber");
            entity.Property(e => e.EpisodeNumber).HasColumnName("episodenumber");
        });

        modelBuilder.Entity<TitleGenre>().ToTable("title_genre");
        modelBuilder.Entity<TitleGenre>(entity =>
        {
            entity.HasKey(e => new { e.Tconst, e.GenreId });
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");
        });

        modelBuilder.Entity<TitlePrincipal>().ToTable("title_principals");
        modelBuilder.Entity<TitlePrincipal>(entity =>
        {
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.Ordering).HasColumnName("ordering");
            entity.Property(e => e.Nconst).HasColumnName("nconst");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Job).HasColumnName("job");
            entity.Property(e => e.Characters).HasColumnName("characters");
        });

        modelBuilder.Entity<TitleAka>().ToTable("title_akas");
        modelBuilder.Entity<TitleAka>(entity =>
        {
            entity.HasKey(e => new { e.Tconst, e.Ordering });
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.Ordering).HasColumnName("ordering");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Region).HasColumnName("region");
            entity.Property(e => e.Language).HasColumnName("language");
            entity.Property(e => e.Types).HasColumnName("types");
            entity.Property(e => e.Attributes).HasColumnName("attributes");
            entity.Property(e => e.IsOriginalTitle).HasColumnName("isoriginaltitle");
        });

        //Name
        modelBuilder.Entity<NameBasics>().ToTable("name_basics");
        modelBuilder.Entity<NameBasics>(entity =>
        {
            entity.HasKey(e => e.Nconst);
            entity.Property(e => e.Nconst).HasColumnName("nconst");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.BirthYear).HasColumnName("birthyear");
            entity.Property(e => e.DeathYear).HasColumnName("deathyear");
            entity.Property(e => e.NameRating).HasColumnName("name_rating");
        });

        modelBuilder.Entity<NameProfession>().ToTable("name_profession");
        modelBuilder.Entity<NameProfession>(entity =>
        {
            entity.HasKey(e => new { e.Nconst, e.ProfessionId });
            entity.Property(e => e.Nconst).HasColumnName("nconst");
            entity.Property(e => e.ProfessionId).HasColumnName("profession_id");
        });

        modelBuilder.Entity<Profession>().ToTable("profession");
        modelBuilder.Entity<Profession>(entity =>
        {
            entity.HasKey(e => e.Id);  
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProfessionName).HasColumnName("profession");
        });

        modelBuilder.Entity<Role>().ToTable("role");
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId);  
            entity.Property(e => e.RoleId).HasColumnName("id");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
        });

        modelBuilder.Entity<KnownFor>().ToTable("known_for");
        modelBuilder.Entity<KnownFor>(entity =>
        {
            entity.HasKey(e => new { e.Nconst, e.Tconst });  
            entity.Property(e => e.Nconst).HasColumnName("nconst");
            entity.Property(e => e.Tconst).HasColumnName("tconst");
        });
    }
}
