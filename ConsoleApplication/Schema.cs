//-------------------------------------------------------------------------------------------------
// <copyright file="Schema.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <summary>
//     Delta Query sample application.
// 
//     This source is subject to the Sample Client End User License Agreement
//     included in this project.
// </summary>
//
// <remarks />
//
// <disclaimer>
//     THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//     EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
//     WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </disclaimer>
//-------------------------------------------------------------------------------------------------

namespace DeltaQueryApplication
{
    /// <summary>
    /// Sample implementation of User class that show the schema mapping.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique name used for login.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active or disabled.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the title of the user.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the address of the user.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the city of the user.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state of the user.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the manager of the user.
        /// </summary>
        public string Manager { get; set; }
    }

    /// <summary>
    /// Sample implementation of Group class that show the schema mapping.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Gets or sets the display name of the group.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the members of the group.
        /// </summary>
        public string[] Members { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the group is a security group.
        /// </summary>
        public bool SecurityGroup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the group is a mail-distribution group.
        /// </summary>
        public bool MailEnabledGroup { get; set; }
    }
}
