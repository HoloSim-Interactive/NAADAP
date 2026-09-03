using Naadap.Core;

namespace Naadap.Ingestion.Tests;

public class DocumentTypeClassifierTests
{
    [Theory]
    [InlineData("sow-01-beq-m400-paintflooring.pdf", DocType.Sow)]
    [InlineData("sow-02-advanced-power.docx", DocType.Sow)]
    [InlineData("pws-01-nswccd-ta-instruments.pdf", DocType.Pws)]
    [InlineData("cdrl-01-nswccd-waters-inspection.pdf", DocType.Cdrl)]
    [InlineData("sources-sought-01-rot-frcs.pdf", DocType.SourcesSought)]
    [InlineData("open-source-text-01-v22-osprey-testimony.pdf", DocType.OpenSource)]
    public void Classify_RecognizesDocTypeFromFileName(string fileName, DocType expected)
    {
        Assert.Equal(expected, DocumentTypeClassifier.Classify(fileName, extractedText: string.Empty));
    }

    [Fact]
    public void Classify_FallsBackToTextWhenFileNameInconclusive()
    {
        var docType = DocumentTypeClassifier.Classify(
            "attachment-14.pdf",
            "STATEMENT OF WORK (SOW) for grounds maintenance services.");

        Assert.Equal(DocType.Sow, docType);
    }

    [Fact]
    public void Classify_ReturnsUnknownWhenNothingMatches()
    {
        var docType = DocumentTypeClassifier.Classify("attachment-14.pdf", "General correspondence with no keywords.");

        Assert.Equal(DocType.Unknown, docType);
    }
}
