namespace ItemOrganizer.Domain;

public enum AnalysisStatus
{
    Queued,
    Running,
    Completed,
    Failed,
    Cancelled
}

public enum AssignmentStatus
{
    Unassigned,
    Suggested,
    Confirmed
}

public enum AssignmentSource
{
    None,
    AiSuggestion,
    User,
    ConvenienceWorkflow
}

public enum PhotoRetentionState
{
    Active,
    PendingDeletion,
    Deleted
}

public enum IdempotencyStatus
{
    InProgress,
    Completed,
    Failed
}

public enum OutboxStatus
{
    Pending,
    Dispatched,
    Failed
}
