﻿using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Testing.Mongo;
using NUnit.Framework;

namespace MongoDBImplementationSample.Tests
{
    /// <summary>
    /// These are the tests which assume that there is a MongoDB server running on localhost:27017.
    /// </summary>
    public class MyCounterServiceWithSetupTests : TestBase
    {
        [Test]
        public async Task HasEnoughRating_Should_Throw_InvalidOperationException_When_The_User_Is_Not_Found()
        {
            using (MongoTestServer server = SetupServer())
            {
                // ARRANGE
                var collection = server.Database.GetCollection<UserEntity>("users");
                var service = new MyCounterService(collection);
                await collection.InsertOneAsync(new UserEntity
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = "foo",
                    Rating = 23
                });

                // ACT, ASSERT
                Assert.Throws<InvalidOperationException>(
                    () => service.HasEnoughRating(ObjectId.GenerateNewId().ToString()));
            }
        }

        [Test]
        public async Task HasEnoughRating_Should_Return_True_When_The_User_Has_More_Then_100_Rating()
        {
            using (MongoTestServer server = SetupServer())
            {
                // ARRANGE
                var collection = server.Database.GetCollection<UserEntity>("users");
                var service = new MyCounterService(collection);
                var userId = ObjectId.GenerateNewId().ToString();
                await collection.InsertOneAsync(new UserEntity
                {
                    Id = userId,
                    Name = "foo",
                    Rating = 101
                });

                // ACT
                bool isEnough = service.HasEnoughRating(userId);

                // ASSERT
                Assert.True(isEnough);
            }
        }

        [Test]
        public async Task HasEnoughRating_Should_Return_False_When_The_User_Has_Less_Then_100_Rating()
        {
            using (MongoTestServer server = SetupServer())
            {
                // ARRANGE
                var collection = server.Database.GetCollection<UserEntity>("users");
                var service = new MyCounterService(collection);
                var userId = ObjectId.GenerateNewId().ToString();
                await collection.InsertOneAsync(new UserEntity
                {
                    Id = userId,
                    Name = "foo",
                    Rating = 90
                });

                // ACT
                bool isEnough = service.HasEnoughRating(userId);

                // ASSERT
                Assert.False(isEnough);
            }
        }

        [Test]
        public async Task HasEnoughRating_Should_Return_False_When_The_User_Has_100_Rating()
        {
            using (MongoTestServer server = SetupServer())
            {
                // ARRANGE
                var collection = server.Database.GetCollection<UserEntity>("users");
                var service = new MyCounterService(collection);
                var userId = ObjectId.GenerateNewId().ToString();
                await collection.InsertOneAsync(new UserEntity
                {
                    Id = userId,
                    Name = "foo",
                    Rating = 100
                });

                // ACT
                bool isEnough = service.HasEnoughRating(userId);

                // ASSERT
                Assert.False(isEnough);
            }
        }
    }
}
