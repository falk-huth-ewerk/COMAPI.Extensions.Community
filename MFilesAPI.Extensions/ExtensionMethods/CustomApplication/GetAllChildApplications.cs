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
        /// Filters the list of all <see cref="CustomApplication"/> objects of the vault
        /// to those where the <paramref name="currentApplication"/> is set as MasterApplication of them.
        /// </summary>
        /// 
        /// <param name="currentApplication"><see cref="CustomApplication"/> object used as base</param>
        /// <param name="vault"><see cref="Vault"/> object</param>
        /// 
        /// <returns><see cref="List{CustomApplication}"/> with all child appliations of the current</returns>
        public static List<CustomApplication> GetAllChildApplications(this CustomApplication currentApplication, Vault vault)
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
                        .Where(_ => _.IsChildApplicationOf(currentApplication))
                        .ToList();
        }
    }
}
