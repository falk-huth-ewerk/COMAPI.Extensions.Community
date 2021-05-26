using System;

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
        /// <returns><see cref="CustomApplication"/> object which is set as MasterApplication
        /// of the <paramref name="currentApplication"/> object</returns>
        public static CustomApplication GetMasterCustomApplication(this CustomApplication currentApplication, Vault vault)
        {
            // Sanity
            if (null == currentApplication)
                throw new ArgumentNullException(nameof(currentApplication));
            if (null == vault)
                throw new ArgumentNullException(nameof(vault));
            if (null == vault.CustomApplicationManagementOperations)
                throw new InvalidOperationException(
                    $"The specified {nameof(vault)} contains no {nameof(vault.CustomApplicationManagementOperations)} object.");

            // Remark: The string representation of MasterApplication and ID guid values are return in different formats:
            // Example:
            // ID:                 "{97ADBCF5-3823-4171-9E99-876E2AC87F46}"
            // MasterApplication:   "97ADBCF5-3823-4171-9E99-876E2AC87F46"
            //
            // Convert MasterApplication to ID requires <Guid>.Format("B").ToUpper() transformation

            // Get the value as Guid
            Guid.TryParse(currentApplication.MasterApplication, out Guid currentMasterApplicationGuid);
            string currentMasterApplicationGuidAsID = currentMasterApplicationGuid.ToString("B").ToUpper();

            // switch parameters to call the IsMasterApplicationOf method
            return vault.CustomApplicationManagementOperations.GetCustomApplication(currentMasterApplicationGuidAsID);
        }
    }
}
