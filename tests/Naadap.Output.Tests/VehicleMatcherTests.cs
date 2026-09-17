using Naadap.Core;

namespace Naadap.Output.Tests;

/// <summary>
/// Vehicle matching behavior (derived requirements CORE-270 to CORE-273,
/// CORE-285; client direction 2026-09-17 on the evidence floor). Uses the
/// shipped knowledge base and small hand-written clusters so each assertion
/// is about one rule.
/// </summary>
public class VehicleMatcherTests
{
    private static readonly VehicleKnowledgeBase kb = VehicleKnowledgeBase.Load();

    [Fact]
    public void Match_EveryCandidate_IsAKnowledgeBaseRowWithEvidence()
    {
        var (documents, clusters) = TrainingSystemsCluster();

        var (recs, ranking) = VehicleMatcher.Match(documents, clusters, kb);

        var rec = Assert.Single(recs);
        Assert.NotEmpty(rec.Candidates);
        var ids = kb.Vehicles.Select(v => v.VehicleId).ToHashSet(StringComparer.Ordinal);
        foreach (var c in rec.Candidates.Concat(rec.BelowEvidenceFloor))
        {
            Assert.Contains(c.VehicleId, ids);                       // CORE-270: never a term-derived id
            Assert.InRange(c.Score, 0.0, 1.0);
            Assert.InRange(c.Lexical.Cosine, 0.0, 1.0);
            Assert.InRange(c.AcquisitionPathTier, 1, 5);
            Assert.NotEmpty(c.DecidingKey);
        }

        Assert.True(rec.Candidates.All(c => c.Score >= VehicleMatcher.EvidenceFloor));
        Assert.True(rec.BelowEvidenceFloor.All(c => c.Score < VehicleMatcher.EvidenceFloor));
        Assert.Contains(ranking, r => r.Status == "candidate");
    }

    [Fact]
    public void Match_TrainingSystemsText_NamingNawctsd_RanksAnN61340VehicleFirstWithAffinity()
    {
        var (documents, clusters) = TrainingSystemsCluster();

        var (recs, _) = VehicleMatcher.Match(documents, clusters, kb);

        var rec = recs[0];
        Assert.Contains("N61340", rec.RequestingOffices);
        var top = rec.Candidates[0];
        Assert.Equal("N61340", top.OfficeAffinity.Office);
        Assert.True(top.OfficeAffinity.Share > 0);
        var vehicle = kb.Vehicles.Single(v => v.VehicleId == top.VehicleId);
        Assert.Equal("N61340", vehicle.OwnerOffice);
    }

    [Fact]
    public void Match_RequestingOfficeOutsideARowsOrderingActivities_EliminatesTheRowAndNamesTheConstraint()
    {
        // NAVFAC Pacific (N62742) never ordered from a NAVAIR single-award
        // IDIQ in FY2025, so every parent-IDV row is ruled out (CORE-272,
        // CORE-285) while the family rows, whose eligibility is prose, remain.
        var documents = new List<DocumentRecord>
        {
            new("navfac.txt", DocType.Sow, "N62742 NAVFAC Pacific statement of work for environmental remediation services at Pearl Harbor", null),
            new("navfac2.txt", DocType.Sow, "N62742 NAVFAC Pacific remediation services statement of work, sampling and reporting", null),
        };
        var clusters = new List<DocumentCluster> { new("cluster-0001", ["navfac.txt", "navfac2.txt"], ["remediation"]) };

        var (recs, ranking) = VehicleMatcher.Match(documents, clusters, kb);

        var rec = recs[0];
        Assert.NotEmpty(rec.Eliminations);
        Assert.All(rec.Eliminations, e => Assert.Equal(VehicleMatcher.OfficeNotEligible, e.Constraint));
        var eliminated = rec.Eliminations.Select(e => e.VehicleId).ToHashSet(StringComparer.Ordinal);
        Assert.DoesNotContain(rec.Candidates.Concat(rec.BelowEvidenceFloor), c => eliminated.Contains(c.VehicleId));
        Assert.Contains(ranking, r => r.Status.StartsWith("eliminated:", StringComparison.Ordinal));
        // Family rows carry prose eligibility and are scored, not eliminated;
        // the full ranking file lists every scored vehicle whatever its score.
        Assert.Contains(ranking, r => r.VehicleId == "SEAPORT-NXG" && !r.Status.StartsWith("eliminated:", StringComparison.Ordinal));
    }

    [Fact]
    public void Match_IsDeterministic_AcrossRepeatedCalls()
    {
        var (documents, clusters) = TrainingSystemsCluster();

        var (first, _) = VehicleMatcher.Match(documents, clusters, kb);
        var (second, _) = VehicleMatcher.Match(documents, clusters, kb);

        Assert.Equal(
            first[0].Candidates.Select(c => (c.VehicleId, c.Score, c.DecidingKey)),
            second[0].Candidates.Select(c => (c.VehicleId, c.Score, c.DecidingKey)));
    }

    [Fact]
    public void Match_ContentFreeCluster_ReportsNoCandidatesAboveFloorAndNewVehicleIndicated()
    {
        var documents = new List<DocumentRecord>
        {
            new("a.txt", DocType.Unknown, "zzxq qqzx", null),
            new("b.txt", DocType.Unknown, "qqzx zzxq", null),
        };
        var clusters = new List<DocumentCluster> { new("cluster-0001", ["a.txt", "b.txt"], ["zzxq"]) };

        var (recs, _) = VehicleMatcher.Match(documents, clusters, kb);

        Assert.Empty(recs[0].Candidates);
        Assert.True(recs[0].NewVehicleIndicated);
        Assert.Equal(kb.Vehicles.Count, recs[0].BelowEvidenceFloorTotal);   // nothing suppressed, nothing eliminated
    }

    [Fact]
    public void Match_EmptyClusterList_ReturnsEmpty()
    {
        var (recs, ranking) = VehicleMatcher.Match([], [], kb);

        Assert.Empty(recs);
        Assert.Empty(ranking);
    }

    private static (List<DocumentRecord>, List<DocumentCluster>) TrainingSystemsCluster()
    {
        var documents = new List<DocumentRecord>
        {
            new("tsc-a.txt", DocType.Sow, "NAWCTSD N61340 statement of work: training system design, development, production, test and evaluation, delivery, modification and support of a flight simulator training device, including instructional systems development and courseware.", null),
            new("tsc-b.txt", DocType.Pws, "NAWCTSD N61340 performance work statement: contractor operation and maintenance of fielded training systems and simulator devices, instructional services, training system support.", null),
        };
        var clusters = new List<DocumentCluster> { new("cluster-0001", ["tsc-a.txt", "tsc-b.txt"], ["training", "system"]) };
        return (documents, clusters);
    }
}
