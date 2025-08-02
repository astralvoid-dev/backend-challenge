namespace backend_challenge;

public class User
{
    public int userID { get; set; }
    public Client[] clients { get; set; }

    public User(int userID, Client[] clients)
    {
        this.userID = userID;
        this.clients = clients;
    }
}