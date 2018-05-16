Import-Module $PSScriptRoot\Utility.ps1 -Force
Import-Module $PSScriptRoot\BuildLog.ps1 -Force


$global:suppressed =  @(
    "CS0618", # Obsolete Method or Property
    "CS1591", # Missing XML comment for publicly visible type or member 
    "CS0672" # overrides obsolete member
);

Function Script-Banner { Param([string]$project)
    $Global:ErrorCount=0;
    $Global:WarningCount=0;
    $Global:SuppressedWarningCount=0;

    $start = Get-Date;
    $script=Last($MyInvocation.ScriptName.Split("\"));

    $banner = [System.String]::Join(" ", @($start, $project, $script ) );
    Write-Host "******************************************************************************"
    Write-Host $banner
    BuildLog-Create;
    return $start;
}

Function Script-Footer([DateTime] $start=[System.DateTime]::MaxValue, [string]$project) {

    $script=Last($MyInvocation.ScriptName.Split("\"));

    $end = Get-Date;
    Error-String "$Global:ErrorCount Error(s)  $Global:WarningCount Warning(s) $Global:SuppressedWarningCount Suppressed warning(s)"
    $elapsed_seconds = $end.Subtract($start).TotalSeconds;

    $banner = [System.String]::Join(" ", @($end, $project, $script, $elapsed_seconds ) );
    Write-Host $banner

    $operation = $script.Split(".")[1];
    $logDirectory = [System.IO.Path]::Combine($PSScriptRoot,"..","Build","Logs");
    $log = [System.IO.Path]::Combine($logDirectory,"$project.$operation.log");
    BuildLog-Save $log ;
    
    # Build.Summary.csv
    $log =  [System.IO.Path]::Combine($logDirectory,"Build.Summary.csv");
    $logHeader = @("Date/Time", "Project", "Build Operation", "Errors", "Warnings", "Suppressed Warnings", "Elapsed Time [s]" ) ;
    $logItems = @($end, $project, $script, $Global:ErrorCount, $Global:WarningCount, $Global:SuppressedWarningCount, $elapsed_seconds );
    BuildLog-Summary $log $logItems $logHeader;
    Write-Host "******************************************************************************"
}


Function Error-Coloring {
    [CmdletBinding()]
    Param( [Parameter(ValueFromPipeline)] $pipe, [switch]$Trace=$false )
    
    begin { #Things to do only once, before processing
            $Verbose = Get-Verbose
            Trace "Error-Coloring -Verbose:$Verbose '$pipe'" -Trace:$Trace #-ForegroundColor Magenta 
    } #End Begin

    Process { #What  you want to do with each item in $Folders
        foreach($item in $pipe)
        {
            if($item.Contains([Environment]::NewLine))
            {
                $lines = $item.ToString().Split([System.Environment]::NewLine);
                $lines | ForEach-Object { Error-String $_ -Verbose:$Verbose -Trace:$Trace };
            }
            else
            {
                Error-String $item -Verbose:$Verbose -Trace:$Trace
            }
        }
    } #End Process 

    end { #Things to do at the end of the function, after all processes are done
    }#End end
}

Function Error-String { Param( [string]$str, [switch]$Trace=$false, [switch]$Verbose=$false )
    begin { #Things to do only once, before processing
        Trace "`tError-String -Verbose:$Verbose '$str'" -Trace:$Trace
    } #End Begin

    Process { #What  you want to do with each item in $Folders
        if([System.String]::IsNullOrEmpty($str) -eq $false)
        {
            $lower = $str.ToString().ToLower();
            if($lower.Contains("error") -and !$lower.Contains("0 error(s)")) {
                return Build-Error $str;
            }
            if($lower.Contains("warning")) {
                if($lower.Contains("0 warning(s)")) {   # MSBuild compile summary line
                    return Build-Text $str -ForegroundColor Green
                }
                foreach($sup in $global:suppressed)
                {
                    if($str.Contains($sup)) {  # Warning is on supression list
                        return Build-Warning $str -Supress -Verbose:$Verbose;
                    }
                }
                return Build-Warning $str;
            }
            if($Verbose -eq $false) { return; }
            if($lower.Contains("###")) {
                return Build-Text $str -ForegroundColor Cyan
            }
            return Build-Text $str -ForegroundColor White
        }
    } #End Process 
    end { #Things to do at the end of the function, after all processes are done
    }#End end
}

Function Build-Error { Param( [string]$line, [switch]$Verbose=$false )
    $Global:ErrorCount++;
    return Build-Text $line -ForegroundColor Red;
}

Function Build-Warning { Param( [string]$line, [switch]$Supress, [switch]$Verbose=$false )
    if($Supress -eq $false) {
        $Global:WarningCount++;
        return Build-Text $line -ForegroundColor Yellow;
     }
     $Global:SuppressedWarningCount++;
    if($Verbose -eq $true) {
        return Build-Text $line -ForegroundColor DarkYellow
    }
}

Function Build-Text { Param( [string]$line, [System.ConsoleColor]$ForegroundColor , [switch]$Verbose=$false )

    Write-Host $line -ForegroundColor:$ForegroundColor;
    BuildLog-Add $line ;
}

exit

# Test routines!  
cls
$start=Script-Banner;

$Verbose = $false;

$message =  [System.Environment]::NewLine + "Error-String string"
Write-Host $message -ForegroundColor Green
Error-String "text" -Verbose 
Error-String "###" -Verbose:$verbose 
Error-String "Error" -Verbose:$verbose 
Error-String "Warning" -Verbose:$verbose 

$message =  [System.Environment]::NewLine + "Error-Coloring string"
Write-Host $message -ForegroundColor Green

"text" | Error-Coloring -Verb 
"###" | Error-Coloring -Verb 
"Error" | Error-Coloring  -Verb
"Warning" | Error-Coloring -Verb

$message =  [System.Environment]::NewLine + "Error-Coloring with NewLine -Verbose"
Write-Host $message -ForegroundColor Green

$parse = "pre" + [System.Environment]::NewLine + "text" 
$parse | Error-Coloring  -Verbose
$parse = "pre" + [System.Environment]::NewLine + "###"
$parse | Error-Coloring -Verbose
$parse = "pre" + [System.Environment]::NewLine + "Error"
$parse | Error-Coloring  -Verbose
$parse = "pre" + [System.Environment]::NewLine + "Warning"
$parse | Error-Coloring -Verbose

$message =  [System.Environment]::NewLine + "Error-Coloring with NewLine"
Write-Host $message -ForegroundColor Green

$parse = "pre" + [System.Environment]::NewLine + "text" 
$parse | Error-Coloring
$parse = "pre" + [System.Environment]::NewLine + "###"
$parse | Error-Coloring
$parse = "pre" + [System.Environment]::NewLine + "Error"
$parse | Error-Coloring
$parse = "pre" + [System.Environment]::NewLine + "Warning"
$parse | Error-Coloring

"  Code Analysis Complete -- 0 error(s), 0 warning(s)" | Error-Coloring
"  Code Analysis Complete -- 0 error(s), 1 warning(s)" | Error-Coloring
"  Code Analysis Complete -- 4 error(s), 0 warning(s)" | Error-Coloring

Write-Host "-------------------------------"

$Verbose=$true;
"WARNING: Source.cs(144,16): warning CS1591: Missing XML comment for publicly visible type or member 'Class.Method()' [C:\Code\Path\Source.cs]" | Error-Coloring

Write-Host "-------------------------------"
$Verbose=$true;
"WARNING: Source.cs(144,16): warning CS1591: Missing XML comment for publicly visible type or member 'Class.Method()' [C:\Code\Path\Source.cs]" | Error-Coloring -Verbose
Write-Host "-------------------------------"
Script-Footer($start);

$Verbose=$false;
