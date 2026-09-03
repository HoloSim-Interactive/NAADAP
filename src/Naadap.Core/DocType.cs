namespace Naadap.Core;

/// <summary>
/// Document-category tag attached to every <see cref="DocumentRecord"/>
/// (DATA-IN-100). This is a content classification (what kind of
/// acquisition document it is), independent of file format (PDF/DOCX/
/// plain text) — a SOW can arrive as a PDF or a DOCX and is still
/// <see cref="Sow"/>.
/// </summary>
public enum DocType
{
    /// <summary>Statement of Work.</summary>
    Sow,

    /// <summary>Performance Work Statement.</summary>
    Pws,

    /// <summary>Contract Data Requirements List.</summary>
    Cdrl,

    /// <summary>Sources-sought notice / request for information.</summary>
    SourcesSought,

    /// <summary>Open-source text not itself a procurement document (e.g. Congressional testimony).</summary>
    OpenSource,

    /// <summary>Could not be confidently classified into any of the above categories.</summary>
    Unknown,
}
