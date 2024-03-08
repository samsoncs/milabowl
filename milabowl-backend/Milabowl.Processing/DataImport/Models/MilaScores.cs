namespace Milabowl.Processing.DataImport.Models;

public record MilaScores(
    decimal TotalMilaScore,
    decimal TotalCumulativeAvgMilaScore,
    decimal CumulativeTotalMilaScore,
    decimal AvgCumulativeTotalMilaScore
);
