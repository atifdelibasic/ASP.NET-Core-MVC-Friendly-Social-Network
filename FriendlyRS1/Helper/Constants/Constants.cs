using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendlyRS1.Helper.Messages
{
    public static class Constants
    {
        public static class Messages
        {
            // Data annotations
            public const string Required = "Please fill out this field.";
            public const string PasswordMatch = "Password and confirm password does not match.";
            public const string BrithDateReq = "Please pick your birth date.";
            public const string LettersOnly = "Please use letters only.";
            public const string BirthDateErr = "Please pick your actual birth date.";
            public const string EmailErr = "Please enter a valid email address.";

            public const string SuccessEdit = "Successfully edited.";
            public const string InvalidFormat = "Invalid image format.";

            public const string PostAdd = "Post is successfully published.";
            public const string PostDelete = "Post is successfully deleted.";

            public const string Error = "We are sorry about that.";
            public const string Welcome = "Welcome to friendly!";

            public const string EmailConfirmation = "Hello there, finish your registration by clicking this";
            public const string PasswordReset = "Reset your password by clicking on the ";

        }
    }
}
