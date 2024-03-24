using DevToys.Tools.Tools.EncodersDecoders.Certificate;
using DevToys.Tools.UnitTests.Tools.Helpers;

namespace DevToys.Tools.UnitTests.Tools.EncodersDecoders.Certificate;

[Collection(nameof(TestParallelizationDisabled))]
public sealed class CertificateDecoderCommandLineToolTests : TestBase
{
    private readonly CertificateDecoderCommandLineTool _tool;
    private readonly string _baseTestDataDirectory = Path.Combine("Tools", "TestData", nameof(CertificateDecoder));

    public CertificateDecoderCommandLineToolTests()
    {
        _tool = new CertificateDecoderCommandLineTool();
        _tool._fileStorage = new MockIFileStorage();
    }

    [Fact]
    public async Task ReadCertificateFromFile()
    {
        string filePath = Path.Combine(_baseTestDataDirectory, "PfxWithPassword.pfx");
        FileInfo inputFile = TestDataProvider.GetFile(filePath);

        _tool.Input = inputFile;

        using (var consoleOutput = new ConsoleOutputMonitor())
        {
            await _tool.InvokeAsync(new MockILogger(), CancellationToken.None);

            string result = consoleOutput.GetError().Trim();
            result.Should().Be(CertificateDecoder.InvalidPasswordError);
        }

        _tool.Password = "test1234";

        using (var consoleOutput = new ConsoleOutputMonitor())
        {
            await _tool.InvokeAsync(new MockILogger(), CancellationToken.None);

            string expectedResult = CertificateHelperTests.CleanDateTimes(File.ReadAllText(TestDataProvider.GetFile(Path.Combine(_baseTestDataDirectory, "CertDecoded.txt")).FullName)).Trim();
            string result = CertificateHelperTests.CleanDateTimes(consoleOutput.GetOutput()).Trim();
            result.Should().Be(expectedResult);
        }
    }

    [Fact]
    public async Task ReadCertificateFromText()
    {
        string input = File.ReadAllText(TestDataProvider.GetFile(Path.Combine(_baseTestDataDirectory, "PemCertPublic.txt")).FullName);

        _tool.Input = input;

        using (var consoleOutput = new ConsoleOutputMonitor())
        {
            await _tool.InvokeAsync(new MockILogger(), CancellationToken.None);

            string expectedResult = CertificateHelperTests.CleanDateTimes(File.ReadAllText(TestDataProvider.GetFile(Path.Combine(_baseTestDataDirectory, "CertDecoded.txt")).FullName)).Trim();
            string result = CertificateHelperTests.CleanDateTimes(consoleOutput.GetOutput()).Trim();
            result.Should().Be(expectedResult);
        }
    }

    [Fact]
    public async Task InvalidCertificate()
    {
        _tool.Input = "hello";

        using (var consoleOutput = new ConsoleOutputMonitor())
        {
            await _tool.InvokeAsync(new MockILogger(), CancellationToken.None);

            string result = consoleOutput.GetError().Trim();
            result.Should().Be(CertificateDecoder.UnsupportedFormatError);
        }
    }
}
