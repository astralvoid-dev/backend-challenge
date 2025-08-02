using Moq;
using Xunit;

namespace backend_challenge;

public class FileUploadTests
{
    [Fact]
    public async Task AddFile_PDFAsInput_ReturnsFileNameKeyAndCompletedStatus()
    {
        // Arrange
        Mock<IFormFile> fileMock = new ();
        fileMock.Setup(f => f.FileName).Returns("test.pdf");
        fileMock.Setup(f => f.Length).Returns(1024);
        fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

        Session session = new (1, 1, 1);
        
        // Act
        await session.AddFile(fileMock.Object);
        
        // Assert
        Assert.Contains("test.pdf", session.files.Keys);
        Assert.Equal(UploadStatus.Completed, session.status);
    }
}