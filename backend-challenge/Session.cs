namespace backend_challenge;

public class Session
{
    public int sessionID { get; set; }
    public int userID { get; set; }
    public int clientID { get; set; }
    public Dictionary<string, byte[]> files { get; set; }
    public UploadStatus status { get; set; }

    public Session(int sessionID, int userID, int clientID)
    {
        this.sessionID = sessionID;
        this.userID = userID;
        this.clientID = clientID;
        files = new Dictionary<string, byte[]>();
        status = UploadStatus.Pending;
    }

    public async Task AddFile(IFormFile file)
    {
        MemoryStream memoryStream = new ();
        await file.CopyToAsync(memoryStream);
        byte[] fileData = memoryStream.ToArray();

        files[file.FileName] = fileData;
        status = UploadStatus.Completed;
    }
}