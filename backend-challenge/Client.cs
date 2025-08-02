namespace backend_challenge;

public class Client
{
    public int clientID { get; set; }
    public Session[] sessions { get; set; }

    public Client(int clientID)
    {
        this.clientID = clientID;
        sessions = [];
    }
    
    public void AddSession(Session session)
    {
        sessions = sessions.Append(session).ToArray();
    }
}