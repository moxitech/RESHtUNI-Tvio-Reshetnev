namespace RESHtUNI.Services
{

    public enum STATUS
    {
        SUCCESS = 0,
        ERRORPARSE = 1,
        UPDATEPROGRAM = 2,
        ERRORNETWORK = 3,
        ETC = 4,
    }

    public class Result
    {
        public STATUS STATUS { get; set; } = STATUS.SUCCESS;
    }
}