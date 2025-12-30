// Acelerador/ScaffoldResult.cs
using System.Collections.Generic;

namespace Acelerador;

public record ScaffoldResult(bool Success, IReadOnlyList<CommandResult> CommandResults, string? ErrorMessage = null);