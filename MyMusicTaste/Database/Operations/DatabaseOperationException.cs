namespace MyMusicTaste.Database.Operations;

public class DatabaseOperationException(string message) : Exception(message);

public class EntryAlreadyExistsException(string message) : DatabaseOperationException(message);

public class EntryNotFoundException(string message) : DatabaseOperationException(message);