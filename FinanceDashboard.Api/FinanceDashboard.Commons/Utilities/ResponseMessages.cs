namespace FinanceDashboard.Commons.Utilities
{
    public static class ResponseMessages
    {
        //General
        public const string Success = "Operation completed successfully";
        public const string Failed = "Operation failed";

        //Authentication
        public const string InvalidCredentials = "Invalid username or password";
        public const string UserNotFound = "User not found";
        public const string Unauthorized = "You are not authorized to perform this action";
        public const string AccountInactive = "Account is inactive";
        public const string LogoutSuccess = "Logged out successfully";

        //Registration
        public const string UserCreated = "User registered successfully";
        public const string UserAlreadyExists = "User already exists";
        public const string EmailAlreadyExists = "Email already exists";

        //Validation
        public const string ValidationFailed = "One or more validation errors occurred";

        //Financial Records
        public const string RecordCreated = "Financial record created successfully";
        public const string RecordUpdated = "Financial record updated successfully";
        public const string RecordDeleted = "Financial record deleted successfully";
        public const string RecordNotFound = "Financial record not found";

        //Dashboard
        public const string DashboardFetched = "Dashboard data retrieved successfully";
        public const string TrendsFetched = "Trends retrieved successfully";

        //Generic Errors
        public const string ServerError = "An unexpected error occurred";
        public const string BadRequest = "Invalid request";

        //Rate Limiting
        public const string TooManyRequests = "Too many requests. Please try again later.";
    }
}
