Import-Module $PSScriptRoot\Utility.ps1 -Force

Function BuildLog-Create { 
    $Global:BuildLog = New-Object 'System.Collections.Generic.List[system.String]';
    return $Global:BuildLog;
}

Function BuildLog-Add{ Param( [string]$item, [switch]$Verbose=$false)
        $Global:BuildLog.Add($item);
        Write-Verbose $"$(Get-FunctionName) $item" -Verbose:$Verbose;
}

Function BuildLog-Save{ Param([string]$path, [switch]$Verbose=$false)
    $count = $Global:BuildLog.Count;
    if($count -gt 0)
    {
        # Create directory if necessary
        $directory = [System.IO.Path]::GetDirectoryName($path);
        if([System.IO.Directory]::Exists($directory) -eq $false)
        {
            [System.IO.Directory]::CreateDirectory($directory);
            Write-Verbose "$(Get-FunctionName) Creating Directory: $directory" -Verbose:$Verbose;
        }

        Write-Verbose "$(Get-FunctionName) Writing $count lines to: $path" -Verbose:$Verbose;
        [System.IO.File]::WriteAllLines($path,$Global:BuildLog);
        $Global:BuildLog.Clear();
    }
}


Function BuildLog-Summary{ Param([string]$path, [string[]] $items, [string[]] $header, [switch]$Verbose=$false)

    if($items -ne $null)
    {
        # Create directory if necessary
        $directory = [System.IO.Path]::GetDirectoryName($path);
        if([System.IO.Directory]::Exists($directory) -eq $false)
        {
            [System.IO.Directory]::CreateDirectory($directory);
            Write-Verbose "$(Get-FunctionName) Creating Directory: $directory" -Verbose:$Verbose;
        }
        # Write Header if file does not exist
        if([System.IO.File]::Exists($path) -eq $false)
        {
            $line = [System.String]::Join(", ",$header);
            Write-Verbose "$(Get-FunctionName) Writing Header: $path $([Environment]::NewLine)$line" -Verbose:$Verbose;
            [System.IO.File]::AppendAllText($path, ($line + [Environment]::NewLine) );
        }
        $line = [System.String]::Join(", ",$items);

        Write-Verbose "$(Get-FunctionName) Writing Entry: $path $([Environment]::NewLine)$line" -Verbose:$Verbose;

        [System.IO.File]::AppendAllText($path, ($line + [Environment]::NewLine) );
    }
}

exit

# Test Methods
BuildLog-Create
BuildLog-Add "first" -Verbose
BuildLog-Add "second" -Verbose
BuildLog-Add "third" 
Write-Host "Count should be 3: " $Global:BuildLog.Count
$path = [System.IO.Path]::Combine($PSScriptRoot,"BuildLog.test");
BuildLog-Save $path -Verbose

Get-Content $path

[System.IO.File]::Delete($path);
