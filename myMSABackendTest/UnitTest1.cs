using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using myMSABackend.Data;
using myMSABackend.Model;
using myMSABackend.Controllers;
using System.Net.Http;
using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using Castle.Core.Resource;
using FluentValidation.Results;

namespace myMSABackendTest
{
    [TestFixture]
    public class Tests
    {
        PokemonController controller = null;
        IDBRepo repository = null;
        IHttpClientFactory client = null;
        IDBContext dbContextMock = null;

        PokemonValidator validator = new PokemonValidator();

        [SetUp]
        public void Setup()
        {
            var mockPokemons = new List<Pokemon>()
            {
                new Pokemon {Name = "Pikachu", Power = 112, Nickname = null},
                new Pokemon {Name = "Cubone", Power = 64, Nickname = null}
            }.AsQueryable();

            var mockSet = Substitute.For<DbSet<Pokemon>, IQueryable<Pokemon>>();

            // setup a mock dbset Pokemons
            ((IQueryable<Pokemon>)mockSet).Provider.Returns(mockPokemons.Provider);
            ((IQueryable<Pokemon>)mockSet).Expression.Returns(mockPokemons.Expression);
            ((IQueryable<Pokemon>)mockSet).ElementType.Returns(mockPokemons.ElementType);
            ((IQueryable<Pokemon>)mockSet).GetEnumerator().Returns(mockPokemons.GetEnumerator());
            dbContextMock = Substitute.For<IDBContext>();
            dbContextMock.Pokemons.Returns(mockSet);

            repository = Substitute.For<DBRepo>(dbContextMock);
            client = Substitute.For<IHttpClientFactory>();
            controller = new PokemonController(client, repository);
        }

        [Test]
        public async Task Test_Repository_and_DBContext()
        {
            var data = repository.GetPokemons();
            Assert.AreEqual("Pikachu", data.ToArray()[0].Name);
            ValidationResult results = validator.Validate(data.ToArray()[0]);
            Assert.AreEqual(true, results.IsValid);
        }

        [Test]
        public async Task TestGetTeam()
        {
            var data1 = controller.getPokemonTeam();
            Pokemon pikachu = new Pokemon { Name = "Pikachu", Power = 112, Nickname = null };
            Pokemon cubone = new Pokemon { Name = "Cubone", Power = 64, Nickname = null };

            var results = ((IEnumerable<Pokemon>)((ObjectResult)data1.Result).Value).ToList();
            ValidationResult validateResult = validator.Validate(results.ElementAt(0));
            Assert.AreEqual(true, validateResult.IsValid);
            Assert.AreEqual(results.ElementAt(0).Name, pikachu.Name);
            Assert.AreEqual(results.ElementAt(0).Power, pikachu.Power);
            Assert.AreEqual(results.ElementAt(0).Nickname, pikachu.Nickname);

            ValidationResult validateResult2 = validator.Validate(results.ElementAt(1));
            Assert.AreEqual(true, validateResult2.IsValid);
            Assert.AreEqual(results.ElementAt(1).Name, cubone.Name);
            Assert.AreEqual(results.ElementAt(1).Power, cubone.Power);
            Assert.AreEqual(results.ElementAt(1).Nickname, cubone.Nickname);
            Assert.AreEqual(results.Count(), 2);
        }

        [Test]
        public async Task TestAddPokemon()
        {
            var repository = Substitute.For<IDBRepo>();
            repository.GetPokemonByName("snorlax")
                .Returns(x => null);
            repository.AddPokemon(Arg.Any<Pokemon>())
                .Returns(new Pokemon { Name = "snorlax", Power = 189, Nickname = null });
            repository.AddPokemon(Arg.Any<Pokemon>())
                .Returns(new Pokemon { Name = "Wrydeer", Power = 0, Nickname = null });

            string aString = "true,\"slot\":3}],\"base_experience\":189,\"forms\":";
            var c = Substitute.ForPartsOf<PokemonController>(client, repository);
            // does not execute fetch from api
            c.WhenForAnyArgs(x => x.GetRawPokemon("snorlax")).DoNotCallBase();

            var rawString = c.addPokemon("snorlax");
            Assert.IsNotNull(rawString);
            // test GetRawPokemon method is reached by addPokemon method
            c.Received().GetRawPokemon("snorlax");

        }

        [Test]
        public async Task TestUpdatePokemon()
        {
            int nameChanged = 0;
            var repository = Substitute.For<IDBRepo>();
            repository.GetPokemonByName("Pikachu")
                .Returns(new Pokemon { Name = "Pikachu", Power = 112, Nickname = null });
            repository.When(x => x.SetPokemon(Arg.Any<String>(), Arg.Any<String>()))
                .Do(x => nameChanged++);
            var c = new PokemonController(client, repository);

            // pokemon is in team
            IActionResult setPokemon = c.setNickname("Pikachu", "yellow");
            var result = (setPokemon as OkObjectResult).Value;
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Pokemon>(result);
            ValidationResult validateResult1 = validator.Validate((Pokemon)result);
            Assert.AreEqual(true, validateResult1.IsValid);
            Assert.AreEqual(1, nameChanged);

            // pokemon is not in team
            IActionResult notSetPokemon = c.setNickname("Ditto", "slim");
            Assert.IsNotNull(notSetPokemon);
            Assert.IsNotInstanceOf<Pokemon>(notSetPokemon);
            Assert.AreEqual(1, nameChanged);
        }

        [Test]
        public async Task TestDeletePokemon()
        {
            var numInTeam = 2; // team initially has 2 pokemons
            var repository = Substitute.For<IDBRepo>();
            repository.When(x => x.RemovePokemon(Arg.Any<Pokemon>()))
                .Do(x => numInTeam--);
            repository.GetPokemonByName("Pikachu")
                .Returns(new Pokemon { Name = "Pikachu", Power = 112, Nickname = null });
            var c = new PokemonController(client, repository);

            // pokemon is in team
            IActionResult deleted = c.deletePokemon("Pikachu");
            Assert.IsNotNull(deleted);
            Assert.AreEqual(1, numInTeam);

            // pokemon is not in team
            IActionResult notDeleted = c.deletePokemon("Ditto");
            Assert.IsNotNull(notDeleted);
            Assert.AreEqual(1, numInTeam);
        }
    }
}