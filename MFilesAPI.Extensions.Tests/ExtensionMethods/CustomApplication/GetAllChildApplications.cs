using System;
using System.Collections.Generic;
using System.Linq;

using MFilesAPI.Extensions.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MFilesAPI.Extensions.Tests.ExtensionMethods.CustomApplication
{
    /// <summary>
    /// Extension methods for the <see cref="CustomApplication"/> object according the MasterApplication field
    /// </summary>
    [TestClass]
    public class GetAllChildApplications : CustomApplicationTestBase
    {
        /// <summary>
        /// Ensures that a null <see cref="MFilesAPI.CustomApplication"/> reference throws an <see cref="ArgumentNullException"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetAllChildApplications_ThrowsIfNullCustomApplication()
        {
            ((MFilesAPI.CustomApplication) null).GetAllChildApplications(GetVaultMock().Object);
        }

        /// <summary>
        /// Ensures that an object type ID under zero throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetAllChildApplications_ThrowsIfNullVault()
        {
            GetCustomApplicationMock().Object.GetAllChildApplications((Vault) null);
        }

        /// <summary>
        /// Ensures that an object type ID under zero throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetAllChildApplications_ThrowsIfNullVaultCustomApplicationOperations()
        {
            GetCustomApplicationMock().Object.GetAllChildApplications((Vault) GetVaultMock().Object);
        }

        /// <summary>
        /// Ensures that an object type ID under zero throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetAllChildApplications_ThrowsIfNullVaultCustomApplicationOperations()
        {
            // Mock the class.
            //var mock = this.GetVaultClassOperationsMock();
            //mock
            //    .Setup(m => m.GetObjectClass(classId))
            //    .Returns((int id) => throw new InvalidOperationException("Sample error"));
            //var mock = GetVaultMock();
            //mock
            //    .Setup(m => m.CustomApplicationManagementOperations = (CustomApplicationManagementOperations)null)
            //    .Returns(() => 
            //{
            //    this.obj
            //};
            GetCustomApplicationMock().Object.GetAllChildApplications((Vault) GetVaultMock().Object);
        }

        /// <summary>
        /// Ensures that the call to <see cref="MFilesAPI.Extensions.PropertyDefOrObjectTypesExtensionMethods.AddObjectTypeIndirectionLevel"/> adds an item to the <see cref="MFilesAPI.PropertyDefOrObjectTypes"/> collection.
        /// </summary>
        [TestMethod]
        public void AddObjectTypeIndirectionLevel_AddsToCollection()
        {
            // Create the collection and ensure it's blank.
            var collection = new MFilesAPI.PropertyDefOrObjectTypes();
            Assert.AreEqual(0, collection.Count);

            // Add the item.
            collection.AddObjectTypeIndirectionLevel(123);

            // Ensure the collection increased in size.
            Assert.AreEqual(1, collection.Count);
        }

        /// <summary>
        /// Ensures that the call to <see cref="MFilesAPI.Extensions.PropertyDefOrObjectTypesExtensionMethods.AddObjectTypeIndirectionLevel"/> adds a correctly-configured item to the collection.
        /// </summary>
        [TestMethod]
        public void AddObjectTypeIndirectionLevel_ValueCorrect()
        {
            // Create the collection and ensure it's blank.
            var collection = new MFilesAPI.PropertyDefOrObjectTypes();
            Assert.AreEqual(0, collection.Count);

            // Add the item.
            collection.AddObjectTypeIndirectionLevel(1234);

            // Check the added item.
            var item = collection[1];
            Assert.IsNotNull(item);
            Assert.AreEqual(1234, item.ID);
            Assert.IsFalse(item.PropertyDef);
        }
    }
}
