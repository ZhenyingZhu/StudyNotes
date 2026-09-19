using ItemOrganizer.Domain;

namespace ItemOrganizer.Domain.Tests;

public sealed class AssignmentAndDeletionTests
{
    private static readonly DateTimeOffset Now =
        new(2026, 1, 1, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Suggestion_CanOnlyAcceptMatchingContainer()
    {
        var suggestedContainerId = Guid.NewGuid();
        var assignment = ItemAssignment.Suggested(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            suggestedContainerId,
            Now);

        Assert.Throws<DomainException>(
            () => assignment.AcceptSuggestion(Guid.NewGuid(), Now.AddMinutes(1)));

        assignment.AcceptSuggestion(suggestedContainerId, Now.AddMinutes(1));
        Assert.Equal(AssignmentStatus.Confirmed, assignment.Status);
        Assert.Equal(suggestedContainerId, assignment.ContainerId);
        Assert.Null(assignment.SuggestedContainerId);
    }

    [Fact]
    public void RemovingAssignment_ClearsAllContainerReferences()
    {
        var assignment = ItemAssignment.Confirmed(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            AssignmentSource.User,
            Now);

        assignment.Remove(Now.AddMinutes(1));

        Assert.Equal(AssignmentStatus.Unassigned, assignment.Status);
        Assert.Equal(AssignmentSource.None, assignment.Source);
        Assert.Null(assignment.ContainerId);
        Assert.Null(assignment.SuggestedContainerId);
    }

    [Fact]
    public void PhotoDeletion_IsBlockedWhileAnalysisRuns()
    {
        var photo = CreatePhoto();

        Assert.Throws<DomainException>(
            () => photo.RequestDeletion(true, Now.AddMinutes(1)));
    }

    [Fact]
    public void ContainerDeletion_IsBlockedWhenItContainsItems()
    {
        var container = new StorageContainer(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Box",
            null,
            null,
            null,
            Now);

        Assert.Throws<DomainException>(
            () => container.MarkDeleted(true, Now.AddMinutes(1)));
    }

    private static Photo CreatePhoto()
    {
        return new Photo(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            "photo.webp",
            "image/webp",
            1_024,
            512,
            512,
            new string('a', 64),
            Now.AddDays(7),
            Now);
    }
}
