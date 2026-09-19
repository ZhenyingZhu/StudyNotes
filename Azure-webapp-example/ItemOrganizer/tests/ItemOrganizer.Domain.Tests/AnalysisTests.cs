using ItemOrganizer.Domain;

namespace ItemOrganizer.Domain.Tests;

public sealed class AnalysisTests
{
    private static readonly DateTimeOffset Now =
        new(2026, 1, 1, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void QueuedAnalysis_CanBeCancelledImmediately()
    {
        var analysis = CreateAnalysis();

        analysis.RequestCancellation(Now.AddMinutes(1));

        Assert.Equal(AnalysisStatus.Cancelled, analysis.Status);
        Assert.Equal(Now.AddMinutes(1), analysis.CancelledAt);
    }

    [Fact]
    public void RunningAnalysis_RecordsCancellationBeforeWorkerCancels()
    {
        var analysis = CreateAnalysis();
        analysis.Start(Now.AddMinutes(1));

        analysis.RequestCancellation(Now.AddMinutes(2));

        Assert.Equal(AnalysisStatus.Running, analysis.Status);
        Assert.Equal(Now.AddMinutes(2), analysis.CancellationRequestedAt);

        analysis.CancelFromWorker(Now.AddMinutes(3));
        Assert.Equal(AnalysisStatus.Cancelled, analysis.Status);
    }

    [Fact]
    public void CancellationRequest_PreventsCompletion()
    {
        var analysis = CreateAnalysis();
        analysis.Start(Now.AddMinutes(1));
        analysis.RequestCancellation(Now.AddMinutes(2));

        var exception = Assert.Throws<DomainException>(
            () => analysis.Complete([], Now.AddMinutes(3)));

        Assert.Contains("cancellation", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void TransientFailures_RetryUntilSixthAttemptThenFail()
    {
        var analysis = CreateAnalysis();

        for (var attempt = 1; attempt <= Analysis.MaximumDeliveryAttempts; attempt++)
        {
            analysis.Start(Now.AddMinutes(attempt * 2));
            analysis.RecordTransientFailure(
                "AI_PROVIDER_TEMPORARY_FAILURE",
                "Provider unavailable.",
                $"correlation-{attempt}",
                Now.AddMinutes((attempt * 2) + 1));

            var expected = attempt < Analysis.MaximumDeliveryAttempts
                ? AnalysisStatus.Queued
                : AnalysisStatus.Failed;
            Assert.Equal(expected, analysis.Status);
        }
    }

    [Fact]
    public void TerminalAnalysis_RejectsFurtherCancellation()
    {
        var analysis = CreateAnalysis();
        analysis.Start(Now.AddMinutes(1));
        analysis.Complete([], Now.AddMinutes(2));

        Assert.Throws<DomainException>(
            () => analysis.RequestCancellation(Now.AddMinutes(3)));
    }

    private static Analysis CreateAnalysis()
    {
        return new Analysis(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            "prompt-v1",
            "schema-v1",
            "model-v1",
            "test",
            null,
            Now);
    }
}
