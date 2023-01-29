namespace TelegramBot_Timetable;

public record User
{
    public User()
    {
        
    }
    
    public User(long id1, long userId1, long chatId1)
    {
        id = (int)id1;
        userId = userId1;
        chatId = chatId1;
    }
    public int id { get; set; }
    
    public long userId { get; set; }
    
    public long chatId { get; set; }
}