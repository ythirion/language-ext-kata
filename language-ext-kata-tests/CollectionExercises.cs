using System;
using FluentAssertions;
using language_ext.kata.Persons;
using LanguageExt;
using Xunit;
using static language_ext.kata.Persons.PetType;
using static LanguageExt.Prelude;

namespace language_ext.kata.tests
{
    public class CollectionExercises : PetDomainKata
    {
        [Fact]
        public void GetFirstNamesOfAllPeople()
        {
            // Replace it, with a transformation method on people.
            var firstNames = people.Map(p => p.FirstName);
            var expectedFirstNames = Seq("Mary", "Bob", "Ted", "Jake", "Barry", "Terry", "Harry", "John");

            firstNames.Should().BeEquivalentTo(expectedFirstNames);
        }

        [Fact]
        public void GetNamesOfMarySmithsPets()
        {
            var person = GetPersonNamed("Mary Smith");

            // Replace it, with a transformation method on people.
            var names = person.Pets.Map(p => p.Name);

            names.Single()
                .Should()
                .Be("Tabby");
        }

        [Fact]
        public void GetPeopleWithCats()
        {
            // Replace it, with a positive filtering method on people.
            var peopleWithCats = people.Filter(p => p.HasPetType(Cat));

            peopleWithCats.Should().HaveCount(2);
        }

        [Fact]
        public void GetPeopleWithoutCats()
        {
            // Replace it, with a negative filtering method on Seq.
            var peopleWithoutCats = people.Filter(p => !p.HasPetType(Cat));

            peopleWithoutCats.Should().HaveCount(6);
        }

        [Fact]
        public void DoAnyPeopleHaveCats()
        {
            //replace null with a Predicate lambda which checks for PetType.CAT
            var doAnyPeopleHaveCats = people.Find(p => p.HasPetType(Cat)).IsSome;
            doAnyPeopleHaveCats.Should().BeTrue();
        }

        [Fact]
        public void DoAllPeopleHavePets()
        {
            Func<Person, bool> predicate = p => p.IsPetPerson();
            // OR use local functions -> static bool predicate(Person p) => p.IsPetPerson();
            // replace with a method call send to this.people that checks if all people have pets
            var result = people.ForAll(predicate);

            result.Should().BeFalse();
        }

        [Fact]
        public void HowManyPeopleHaveCats()
        {
            // replace 0 with the correct answer
            var count = people.Count(p => p.HasPetType(Cat));
            count.Should().Be(2);
        }

        [Fact]
        public void FindMarySmith()
        {
            var result = GetPersonNamed("Mary Smith");

            result.FirstName.Should().Be("Mary");
            result.LastName.Should().Be("Smith");
        }

        [Fact]
        public void GetPeopleWithPets()
        {
            // replace with only the pets owners
            var petPeople = people.Filter(p => p.IsPetPerson());

            petPeople.Should().HaveCount(7);
        }

        [Fact]
        public void GetAllPetTypesOfAllPeople()
        {
            var petTypes =
                people.Bind(p => p.GetPetTypes().Keys)
                    .ToSeq()
                    .Distinct();

            petTypes.Should()
                .BeEquivalentTo(Seq(Cat, Dog, Snake, Bird, Turtle, Hamster));
        }

        [Fact]
        public void TotalPetAge()
        {
            var totalAge =
                people.Bind(p => p.Pets)
                    .Map(pet => pet.Age)
                    .Sum();

            totalAge.Should().Be(17);
        }

        [Fact]
        public void PetsNameSorted()
        {
            var sortedPetNames =
                people.Bind(p => p.Pets)
                    .Map(pet => pet.Name)
                    .OrderBy(s => s)
                    .ToSeq()
                    .ToFullString();

            sortedPetNames.Should()
                .Be("Dolly, Fuzzy, Serpy, Speedy, Spike, Spot, Tabby, Tweety, Wuzzy");
        }

        [Fact]
        public void SortByAge()
        {
            // Create a Seq<int> with ascending ordered age values.
            var sortedAgeList = people.Bind(p => p.Pets)
                .Map(pet => pet.Age)
                .Distinct()
                .OrderBy(a => a)
                .ToSeq();

            sortedAgeList.Should()
                .HaveCount(4)
                .And
                .BeEquivalentTo(Seq(1, 2, 3, 4));
        }

        [Fact]
        public void SortByDescAge()
        {
            // Create a Seq<int> with descending ordered age values.
            var sortedAgeList =
                people.Bind(p => p.Pets)
                    .Map(pet => pet.Age)
                    .Distinct()
                    .OrderBy(a => a)
                    .ToSeq();

            sortedAgeList.Should()
                .HaveCount(4)
                .And
                .BeEquivalentTo(Seq(4, 3, 2, 1));
        }

        [Fact]
        public void Top3OlderPets()
        {
            // Create a Seq<string> with the 3 older pets.
            var top3OlderPets =
                people.Bind(p => p.Pets)
                    .OrderByDescending(pet => pet.Age)
                    .Map(pet => pet.Name).ToSeq()
                    .Take(3);

            top3OlderPets.Should()
                .HaveCount(3)
                .And
                .BeEquivalentTo(Seq("Spike", "Dolly", "Tabby"));
        }

        [Fact]
        public void GetFirstPersonWithAtLeast2Pets()
        {
            // Find the first person who owns at least 2 pets
            var firstPersonWithAtLeast2Pets = people.Find(person => person.Pets.Count >= 2);

            firstPersonWithAtLeast2Pets.GetUnsafe()
                .FirstName
                .Should()
                .Be("Bob");
        }

        [Fact]
        public void IsThereAnyPetOlderThan4()
        {
            // Check whether any exercises older than 4 exists or not
            var isThereAnyPetOlderThan4 =
                people.Bind(p => p.Pets)
                    .Find(pet => pet.Age > 4)
                    .IsSome;

            isThereAnyPetOlderThan4.Should().BeFalse();
        }

        [Fact]
        public void IsEveryPetsOlderThan1()
        {
            // Check whether all pets are older than 1 or not
            var allOlderThan1 =
                people.Bind(p => p.Pets)
                    .Filter(pet => pet.Age < 1)
                    .IsEmpty;

            allOlderThan1.Should().BeTrue();
        }

        [Fact]
        public void GetListOfPossibleParksForAWalkPerPerson()
        {
            // For each person described as "firstName lastName" returns the list of names possible parks to go for a walk
            var possibleParksForAWalkPerPerson =
                people.ToDictionary(
                    p => $"{p.FirstName} {p.LastName}",
                    p => FilterParksFor(p.Pets.Map(pet => pet.Type)));

            possibleParksForAWalkPerPerson["John Doe"]
                .Should()
                .BeEquivalentTo(Seq("Jurassic", "Central", "Hippy"));

            possibleParksForAWalkPerPerson["Jake Snake"]
                .Should()
                .BeEquivalentTo(Seq("Jurassic", "Hippy"));
        }

        private Seq<string> FilterParksFor(Seq<PetType> petTypes)
            => parks.Filter(park => petTypes.Except(park.AuthorizedPetTypes).ToSeq().Count == 0)
                .Map(park => park.Name);
    }
}
