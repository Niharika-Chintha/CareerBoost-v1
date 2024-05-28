public static class ValidationHelper
{
    public static bool IsValidMobileNumber(string mobileNumber)
    {
        return mobileNumber?.Length == 10 && mobileNumber.All(char.IsDigit);
    }

    public static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidPassword(string password)
    {
        // Implement your password policy validation logic like password length, complexity, etc.
        return !string.IsNullOrWhiteSpace(password) && password.Length >= 8;
    }
}
