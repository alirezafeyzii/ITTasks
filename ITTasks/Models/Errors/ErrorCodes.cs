namespace ITTasks.Models.Errors
{
	public enum ErrorCodes
	{
		NoError,
		NotAllowedDateTimeFormat, 
		DatabaseError,            
		NullObjectError, 
		ServerError,
		UserFullNameError,
		UserIdError,
		UserNotFound,
		ConflictTask,
		ConflictSprint,
		ConflictUser,
		ConflictTaskType,
		TaskTypeNotFound,
		SprintNotFound,
		UserNameError,
		PasswordError,
		SignInFaild

	}
}
