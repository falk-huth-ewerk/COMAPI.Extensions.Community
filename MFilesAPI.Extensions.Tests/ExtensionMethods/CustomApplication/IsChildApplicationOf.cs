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
        /// Checks if <paramref name="potentialChild"/> is the child custom application
        /// of the custom application <paramref name="potentialParent"/>
        /// </summary>
        /// 
        /// <param name="potentialChild"><see cref="CustomApplication"/> object which is potential child</param>
        /// <param name="potentialParent"><see cref="CustomApplication"/> object which is potential parent</param>
        /// 
        /// <returns>
        /// <list type="table">
        /// <item>
        /// <term><see cref="true"/></term>
        /// <description>if the <see cref="Guid"/> value of parent's ID equals the <see cref="Guid"/> value</description>
        /// <description>of the child's MasterApplication property</description>
        /// </item>
        /// <item>
        /// <term><see cref="false"/></term>
        /// <description>if the <see cref="Guid"/> values above are not equal - or  -</description>
        /// <description>if the MasterApplication <see cref="string"/> is no valid <see cref="Guid"/></description>
        /// </item>
        /// </list>
        /// </returns>
        public static bool IsChildApplicationOf(this CustomApplication potentialChild, CustomApplication potentialParent)
        {
            // Sanity
            if (null == potentialChild)
                throw new ArgumentNullException(nameof(potentialChild));
            if (null == potentialParent)
                throw new ArgumentNullException(nameof(potentialParent));

            // ID of the parent application must be a valid GUID - otherwise throw Exception
            if (!Guid.TryParse(potentialParent.ID, out Guid guidParentID))
                throw new InvalidOperationException(
                    $"The string value of {nameof(potentialParent.ID)} does not represent a valid GUID.");

            // Cannot be a child application if the GUID value of the MasterApplication string value is invalid
            if (!Guid.TryParse(potentialChild.MasterApplication, out Guid guidChildMasterApplication))
                return false;

            // Return the result of the GUID comparism
            return guidParentID.Equals(guidChildMasterApplication);
        }
    }
}
