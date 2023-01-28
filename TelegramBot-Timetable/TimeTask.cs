namespace TelegramBot_Timetable;

public record TimeTask
{
    public TimeTask()
    {
        taskId = -1;
        userId = -1;
        status = -1;
        heading = "";
        description = null;
    }

    public TimeTask(long taskId1, long userId1, long status1, string head, string? text1) : base()
    {
        taskId = (int)taskId1;
        userId = (int)userId1;
        status = (int)status1;
        heading = head;
        description = text1;
    }
    
    public int taskId { get; set; }

    public int userId { get; set; }

    public int status { get; set; }

    public string heading { get; set; }
    
    public string? description { get; set; }
}