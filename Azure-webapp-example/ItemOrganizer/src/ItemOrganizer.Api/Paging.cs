using Microsoft.AspNetCore.WebUtilities;

namespace ItemOrganizer.Api;

public sealed record PageRequest(int PageSize, int Offset)
{
    public const int DefaultPageSize = 50;
    public const int MaximumPageSize = 100;

    public static bool TryCreate(
        int? pageSize,
        string? continuationToken,
        out PageRequest? request,
        out string? error)
    {
        var size = pageSize ?? DefaultPageSize;
        if (size is < 1 or > MaximumPageSize)
        {
            request = null;
            error = $"pageSize must be between 1 and {MaximumPageSize}.";
            return false;
        }

        var offset = 0;
        if (!string.IsNullOrWhiteSpace(continuationToken))
        {
            try
            {
                var bytes = WebEncoders.Base64UrlDecode(continuationToken);
                if (!int.TryParse(
                        System.Text.Encoding.UTF8.GetString(bytes),
                        out offset)
                    || offset < 0)
                {
                    throw new FormatException();
                }
            }
            catch (FormatException)
            {
                request = null;
                error = "continuationToken is invalid.";
                return false;
            }
        }

        request = new(size, offset);
        error = null;
        return true;
    }

    public string NextToken()
    {
        return WebEncoders.Base64UrlEncode(
            System.Text.Encoding.UTF8.GetBytes((Offset + PageSize).ToString()));
    }
}
