using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;
using DataServiceLayer.Models;
using DataServiceLayer.Models.Name;
using DataServiceLayer.Models.Title;
using DataServiceLayer.Models.Person;

namespace DataServiceLayer;

public class ImdbContext : DbContext
{
    //Title
    public DbSet<TitleBasics> TitleBasics { get; set; }
    public DbSet<TitleEpisode> TitleEpisodes { get; set; }
    public DbSet<TitleGenre> TitleGenres { get; set; }
    public DbSet<TitlePrincipal> TitlePrincipals { get; set; }
    public DbSet<TitleAka> TitleAkas { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Award> Awards { get; set; }
    public DbSet<OverallRating> OverallRatings { get; set; }

    //Name
    public DbSet<NameBasics> NameBasics { get; set; }
    public DbSet<NameProfession> NameProfessions { get; set; }
    public DbSet<Profession> Professions { get; set; }
    public DbSet<KnownFor> KnownFors { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<NameTitleRole> NameTitleRoles { get; set; }


    // Person
    public DbSet<Person> Persons { get; set; }
    public DbSet<SearchHistory> SearchHistories { get; set; }
    public DbSet<Bookmark> Bookmarks { get; set; }
    public DbSet<IndividualRating> IndividualRatings { get; set; }


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

        modelBuilder.Entity<Genre>().ToTable("genre");
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId);
            entity.Property(e => e.GenreId).HasColumnName("id");
            entity.Property(e => e.GenreName).HasColumnName("genre");
        });

        modelBuilder.Entity<Award>().ToTable("award");
        modelBuilder.Entity<Award>(entity =>
        {
            entity.HasKey(e => e.Tconst);
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.AwardInfo).HasColumnName("award_info");
        });

        modelBuilder.Entity<OverallRating>().ToTable("overall_rating");
        modelBuilder.Entity<OverallRating>(entity =>
        {
            entity.HasKey(e => e.Tconst);
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Votes).HasColumnName("votes");
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

        // Configure the many-to-many relationship between NameBasics and TitleBasics
        modelBuilder.Entity<NameBasics>()
            .HasMany(n => n.Titles)
            .WithMany(t => t.Names)
            .UsingEntity<KnownFor>(
                j => j.HasOne<DataServiceLayer.Models.Title.TitleBasics>()
                      .WithMany()
                      .HasForeignKey(k => k.Tconst),
                j => j.HasOne<NameBasics>()
                      .WithMany()
                      .HasForeignKey(k => k.Nconst),
                j =>
                {
                    j.ToTable("known_for");
                    j.HasKey(k => new { k.Nconst, k.Tconst });
                    j.Property(k => k.Nconst).HasColumnName("nconst");
                    j.Property(k => k.Tconst).HasColumnName("tconst");
                }
            );

        modelBuilder.Entity<NameTitleRole>().ToTable("name_title_role");
        modelBuilder.Entity<NameTitleRole>(entity =>
        {
            entity.HasKey(e => new { e.Nconst, e.Tconst, e.RoleId });
            entity.Property(e => e.Nconst).HasColumnName("nconst");
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
        });

        // Person
        modelBuilder.Entity<Person>().ToTable("person");
        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Location).HasColumnName("location");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.LastLogin).HasColumnName("last_login");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        // search history
        modelBuilder.Entity<SearchHistory>().ToTable("search_history");
        modelBuilder.Entity<SearchHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.Search_string).HasColumnName("search_string");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        // bookmarks
        modelBuilder.Entity<Bookmark>().ToTable("bookmark");
        modelBuilder.Entity<Bookmark>(entity =>
        {
            entity.HasKey(e => new { e.PersonId, e.Tconst });
            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        // individual rating
        modelBuilder.Entity<IndividualRating>().ToTable("individual_rating");
        modelBuilder.Entity<IndividualRating>(entity =>
        {
            entity.HasKey(e => new { e.PersonId, e.Tconst });
            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.RatingValue).HasColumnName("rating");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        // awards
        modelBuilder.Entity<Award>().ToTable("award");
        modelBuilder.Entity<Award>(entity =>
        {
            entity.HasKey(e => e.Tconst);
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.AwardInfo).HasColumnName("award_info");
        });

        // overall rating
        modelBuilder.Entity<OverallRating>().ToTable("overall_rating");
        modelBuilder.Entity<OverallRating>(entity =>
        {
            entity.HasKey(e => e.Tconst);
            entity.Property(e => e.Tconst).HasColumnName("tconst");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Votes).HasColumnName("votes");
        });
    }
}
