using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
	public class ApplicationDbContexts : DbContext
	{
		public ApplicationDbContexts(DbContextOptions options) : base(options) { }	
		public DbSet<Person> Persons { get; set;}
		public DbSet<Country> Countries { get; set;}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Country>().ToTable("Countries");
			modelBuilder.Entity<Person>().ToTable("Persons");

			//seed data from json file 
			string countriesJson = System.IO.File.ReadAllText("countries.json");

			List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);
			foreach(Country country in countries)
			{
				modelBuilder.Entity<Country>().HasData(country);	
			}


			string personsJson = System.IO.File.ReadAllText("persons.json");

			List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);
			foreach (Person person in persons)
			{
				modelBuilder.Entity<Person>().HasData(person);
			}

			///Fluent API
			modelBuilder.Entity<Person>().Property(temp => temp.TIN)
				.HasColumnName("TaxIdentifierNumbers")
				.HasColumnType("nvarchar(8)")
				.HasDefaultValue("123ABC");
			//Kiem tra duy nhat 
			//modelBuilder.Entity<Person>().HasIndex(temp => temp.TIN).IsUnique();
			//Kiem tra phu thuoc
			modelBuilder.Entity<Person>().HasCheckConstraint("CHK_TIN", "len([TaxIdentifierNumbers]) = 8");

			//Table Relations
			//modelBuilder.Entity<Person>(entity =>
			//{
			//	entity.HasOne<Country>(p => p.Country).WithMany(c => c.Person).HasForeignKey(p => p.CountryID);
			//});
		}

		public List<Person> sp_GetAllPersons()
		{
			return this.Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
		}

		public int sp_InsertPerson(Person person)
		{
			SqlParameter[] parameters = new SqlParameter[] {
				new SqlParameter("@PersonID",person.PersonID),
				new SqlParameter("@PersonName",person.PersonName),
				new SqlParameter("@Email",person.Email),
				new SqlParameter("@DateOfBirth",person.DateOfBirth),
				new SqlParameter("@Gender",person.Gender),
				new SqlParameter("@CountryID",person.CountryID),
				new SqlParameter("@Address",person.Address),
				new SqlParameter("@ReceiveNewsLetters",person.ReceiveNewsLetters)
			};
			return this.Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson]" +
				"@PersonID,@PersonName, @Email, @DateOfBirth, @Gender, @CountryID," +
				" @Address, @ReceiveNewsLetters", parameters);
		}

		public int sp_UpdatePerson(Person person)
		{
			SqlParameter[] parameters = new SqlParameter[] {
				new SqlParameter("@PersonID",person.PersonID),
				new SqlParameter("@PersonName",person.PersonName),
				new SqlParameter("@Email",person.Email),
				new SqlParameter("@DateOfBirth",person.DateOfBirth),
				new SqlParameter("@Gender",person.Gender),
				new SqlParameter("@CountryID",person.CountryID),
				new SqlParameter("@Address",person.Address),
				new SqlParameter("@ReceiveNewsLetters",person.ReceiveNewsLetters)
			};
			return this.Database.ExecuteSqlRaw("EXECUTE [dbo].[UpdatePerson] " +
				"@PersonID,@PersonName, @Email, @DateOfBirth, " +
				"@Gender, @CountryID, @Address, @ReceiveNewsLetters", parameters);
		}
		public int sp_DeletePerson(Person person)
		{
			SqlParameter[] parameters = new SqlParameter[] {
				new SqlParameter("@PersonID",person.PersonID)	
			};
			return this.Database.ExecuteSqlRaw("EXECUTE [dbo].[DeletePerson] " +
				"@PersonID", parameters);
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
		}
	}
}
