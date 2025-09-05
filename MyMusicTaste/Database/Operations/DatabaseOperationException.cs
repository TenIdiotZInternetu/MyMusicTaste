using Microsoft.AspNetCore.Identity;

namespace MyMusicTaste.Database.Operations;

/// <summary>
/// Represents errors that occur during general database operations.
/// </summary>
public class DatabaseOperationException : Exception
{
    public DatabaseOperationException() {}
    public DatabaseOperationException(string message) : base(message) {}
    public DatabaseOperationException(string message, Exception innerException) : base(message, innerException) {}
}

/// <summary>
/// Thrown when an attempt is made to create an entry that already exists in the database.
/// </summary>
public class EntryAlreadyExistsException(string message) : DatabaseOperationException(message);

/// <summary>
/// Thrown when a requested entry cannot be found in the database.
/// </summary>
public class EntryNotFoundException(string message) : DatabaseOperationException(message);

/// <summary>
/// Thrown when a user signup attempt fails due to validation or other identity errors.
/// </summary>
public class UserSignupFailedException(IEnumerable<IdentityError> errors) : DatabaseOperationException
{
    public readonly IEnumerable<IdentityError> Errors = errors;
}