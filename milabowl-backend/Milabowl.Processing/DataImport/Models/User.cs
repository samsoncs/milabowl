namespace Milabowl.Processing.DataImport.Models;

public record User(
    int Id,
    int EntryId,
    string UserName,
    string TeamName,
    int Rank,
    int LastRank,
    int EventTotal
);
