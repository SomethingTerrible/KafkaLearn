namespace KafkaLearnConsoleConsumer;

public class UserDto
{
	public string UserName { get; set; }
		
	public string Password { get; set; }

	public string Email { get; set; }

	public UserDto(string userName, string password, string email)
	{
		UserName = userName;
		Password = password;
		Email = email;
	}
}