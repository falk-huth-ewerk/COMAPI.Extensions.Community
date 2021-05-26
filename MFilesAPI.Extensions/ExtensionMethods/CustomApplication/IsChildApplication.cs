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
        /// <list type="table">
        /// <description>Check the</description>
        /// <term>MasterApplication</term>
        /// <description>property of the</description>
        /// <term><paramref name="potentialChild"/></term>
        /// <description>object</description>
        /// </list>
        /// </summary>
        /// 
        /// <param name="potentialChild">The <see cref="CustomApplication"/> object to check.</param>
        /// 
        /// <returns>
        /// <list type="table">
        /// <item>
        /// <term><see cref="true"/></term>
        /// <description>if the string represents a valid <see cref="Guid"/> value</description>
        /// <description>which is not <see cref="Guid.Empty"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="false"/></term>
        /// <description>if the string represents an invalid <see cref="Guid"/></description>
        /// <description>or if it represents <see cref="Guid.Empty"/></description>
        /// </item>
        /// </list>
        /// </returns>
        public static bool IsChildApplication(this CustomApplication potentialChild)
        {
            // Sanity
            if (null == potentialChild)
                throw new ArgumentNullException(nameof(potentialChild));

            // No error while parsing
            if (Guid.TryParse(potentialChild.MasterApplication, out Guid masterApplicationGuid))
            {
                // true if the valid GUID represents NOT empty, otherwise false
                return !Guid.Empty.Equals(masterApplicationGuid);
            }

            // invalid value for MasterApplication = no MasterApplication set -> IsChildApplication_EW = false
            return false;
        }
    }
}
