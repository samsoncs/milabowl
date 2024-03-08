namespace Milabowl.Processing.DataImport.Models;

public record User(
    int Id,
    string UserName,
    string TeamName,
    int Rank,
    int LastRank,
    int EventTotal
);
