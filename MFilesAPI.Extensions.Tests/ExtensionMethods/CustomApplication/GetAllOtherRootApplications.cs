using System;
using System.Collections.Generic;
using System.Linq;

namespace MFilesAPI.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="CustomApplication"/> object according the MasterApplication field
    /// </summary>
    public static partial class CustomApplicationExtensionMethods
    {
        /// <summary>
        /// Returns the <see cref="ObjVerEx"/> of the owner of the specified <see cref="ObjVerEx"/>
        /// according to the <see cref="MFIdentifier"/> specified as object type.
        /// The identifier should be placed into the <see cref="Configuration"/>.
        /// </summary>
        /// <param name="potentialChild"><see cref="CustomApplication"/> object which is potential child</param>
        /// <param name="potentialParent"><see cref="CustomApplication"/> object which is potential parent</param>
        /// <returns>true if all mandatory fields are filled - otherwise false</returns>
        public static List<CustomApplication> GetAllOtherRootApplications(this CustomApplication currentApplication, Vault vault)
        {
            // Sanity
            if (null == currentApplication)
                throw new ArgumentNullException(nameof(currentApplication));
            if (null == vault)
                throw new ArgumentNullException(nameof(vault));
            if (null == vault.CustomApplicationManagementOperations)
                throw new InvalidOperationException(
                    $"The specified {nameof(vault)} contains no {nameof(vault.CustomApplicationManagementOperations)} object.");

                // switch parameters to call the IsMasterApplicationOf method
                return vault.CustomApplicationManagementOperations
                        .GetCustomApplications()
                        .Cast<CustomApplication>()
                        .Where(_ => _.ID != currentApplication.ID && _.IsRootApplication())
                        .ToList();
        }
    }
}
