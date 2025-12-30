namespace Acelerador;

public record UserInput(
    string ApiName,
    string EntityName,
    string AggregateName,
    string RelationType,
    string ApiVersion
);