namespace NomadCode.BotFramework
{
	public interface IConversationUpdateActivity : IActivity
	{
		string Action { get; set; }
	}
}
