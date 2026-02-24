using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; //EntityFramework to ORM
using Microsoft.EntityFrameworkCore;
using QuickFun.Domain.Entities;

namespace QuickFun.Infrastructure.Data;

//ponizsza klasa odpowiada calej bazie danych, dziedziczy po wersji Identity, ktora od razu daje nam kilka tabel
//ApplicationDbContext to taki plan budowy naszej bazy danych
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    //konstruktor dostaje obiekt options typu DbContextOptions, ktory jest tworzony w Program.cs
    //przez typ generyczny paczka ustawien zarezerwowana jest tylko i wyłącznie dla klasy ApplicationDbContext
    //przekazujemy ustawienia do rodzica - IdentityDbContext za pomocą base
    public ApplicationDbContext(DbContextOptions <ApplicationDbContext> options) : base(options){}

    //w bazie danych powstanie tabela o nazwie UserGameStats, dzieki temu mozna pisac db.UserGameStats.Add() itp
    public DbSet<UserGameStat> UserGameStats { get; set; }

    //Fluent API sluzy do definiowania metadanych i uzywa method chaining

    //z dokumentacji:
    //"Override this method to further configure the model that was discovered by convention from the entity types exposed in
    // DbSet<TEntity> properties on your derived context."
    //czyli ustalamy specjalne, dodatkowe zasady
    protected override void OnModelCreating(ModelBuilder builder)
    {
        //ta klasa dziedziczy po IdentityDbContext, ktora ma wlasna metode OnModelCreating dlatego przekazujemy najpierw buildera do rodzica
        base.OnModelCreating(builder);

        //kombinacja UserId i GameType musi byc unique
        builder.Entity<UserGameStat>()
            .HasIndex(u => new { u.UserId, u.GameType })
            .IsUnique();

        //w bazie w kolumnie gametype bedzie string
        builder.Entity<UserGameStat>()
            .Property(u => u.GameType)
            .HasConversion<string>();
    }

}
