[CmdletBinding()]
param (
    [Parameter(Mandatory = $true)]
    [System.String]
    $Path
)
$TestFiles = @(Get-ChildItem -Path $Path)
if ($TestFiles.Count -lt 1) {
    Write-Output "No test files found"
    return
}

$Markdown = "## Test results 🧪`n"

foreach ($File in $TestFiles) {
    [xml]$Xml = Get-Content -Path $File

    foreach ($TestSuite in $Xml.testsuites.testsuite) {
        $Markdown += "### $($TestSuite.name)  `n"
        $Markdown += "✅ $($TestSuite.tests - $TestSuite.failures) passed  `n"
        if ($TestSuite.failures -gt 0) {
            $Markdown += "❌ $($TestSuite.failures) failed  `n"
        }
        $Markdown += "⏰ Total time $([math]::Round($TestSuite.time, 1)) seconds  `n"
        $Markdown += "`n"
        foreach ($TestCase in $TestSuite.testcase) {
            if ($null -ne $TestCase.failure) {
                $Markdown += "<details><summary>❌ $($TestCase.name)</summary>"
                $Markdown += "$($TestCase.failure.message)`n"
                $Markdown += "$($TestCase.failure.innertext)`n"
                $Markdown += "</details>`n"
                $Markdown += "`n"
            }
        }
    }
}

Write-Output $Markdown