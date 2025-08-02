namespace backend_challenge;

public class Session
{
    public int sessionID { get; set; }
    public int userID { get; set; }
    public int clientID { get; set; }
    public byte[] data { get; set; }

    public Session(int sessionID, int userID, int clientID)
    {
        this.sessionID = sessionID;
        this.userID = userID;
        this.clientID = clientID;
    }
}