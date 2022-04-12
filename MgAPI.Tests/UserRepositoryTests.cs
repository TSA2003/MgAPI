using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MgAPI.Data;
using MgAPI.Data.Entities;
using MgAPI.Data.Repositories;
using NUnit.Framework;

namespace MgAPI.Tests
{
    public class UserRepositoryTests
    {
        UserRepository repository = new UserRepository(new Context());

        [TearDown]
        public void ClearUsersAfterEachTest()
        {
            List<User> users = repository.ReadAll().ToList();

            for (int i = 0; i < users.Count; i++)
            {
                repository.Delete(users[i].ID);
            }

        }

        [Test]
        public void AfterCreateUsersCountShouldReturnOne()
        {
            repository.Create(new User() { ID = Guid.NewGuid().ToString() });
            int count = repository.ReadAll().Count();

            Assert.AreEqual(1, count);
        }

        [Test]
        public void AfterReadUserIdIsCorrect()
        {
            repository.Create(new User() { ID = "1" });
            User user = repository.Read("1");

            Assert.AreEqual(user.ID, "1");
        }

        [Test]
        public void AfterUpdateUserHasNewName()
        {
            repository.Create(new User() { ID = "1", Username = "A" });
            User user = repository.Read("1");
            repository.Update(new User() { ID = "1", Username = "B" });

            Assert.AreEqual(user.Username, "B");
        }

        [Test]
        public void AfterDeleteUsersAreZero()
        {
            repository.Create(new User() { ID = "1", Username = "A" });
            repository.Delete("1");
            int count = repository.ReadAll().Count();

            Assert.AreEqual(0, count);
        }
    }
}
