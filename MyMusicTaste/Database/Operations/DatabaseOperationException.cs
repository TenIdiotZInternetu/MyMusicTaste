using Microsoft.AspNetCore.Identity;

namespace MyMusicTaste.Database.Operations;

public class DatabaseOperationException : Exception
{
    public DatabaseOperationException() {}
    public DatabaseOperationException(string message) : base(message) {}
    public DatabaseOperationException(string message, Exception innerException) : base(message, innerException) {}
}

public class EntryAlreadyExistsException(string message) : DatabaseOperationException(message);

public class EntryNotFoundException(string message) : DatabaseOperationException(message);

public class UserSignupFailedException(IEnumerable<IdentityError> errors) : DatabaseOperationException
{
    public readonly IEnumerable<IdentityError> Errors = errors;
}