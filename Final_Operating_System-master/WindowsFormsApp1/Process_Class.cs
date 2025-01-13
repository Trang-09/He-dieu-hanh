public class Process_Class
{
    public int ProcessId { get; set; } 
    public int ArrivalTime { get; set; }
    public int BurstTime { get; set; }
    public int RemainingTime { get; set; }
    public int CompletionTime { get; set; }
    public int WaitingTime { get; set; }
    public int ResponseTime { get; set; } = -1;
}
