namespace DevToys.Tools.UnitTests.Mocks;

internal class MockSandboxedFileReader : SandboxedFileReader
{
    private readonly FileInfo _file;
    private readonly IFileStorage _fileStorage;

    public MockSandboxedFileReader(FileInfo file, IFileStorage fileStorage)
        : base(file.Name)
    {
        _file = file;
        _fileStorage = fileStorage;
    }

    protected override ValueTask<Stream> OpenReadFileAsync(CancellationToken cancellationToken)
    {
        return new ValueTask<Stream>(_fileStorage.OpenReadFile(_file.FullName));
    }
}
