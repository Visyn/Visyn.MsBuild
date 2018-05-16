

Function Root-ScriptPath {
    $root = Get-Variable psscriptroot -Scope ((Get-PSCallStack).count -2) 
    return $root.Value.ToString();
}

Function Get-FunctionName { 
    (Get-Variable MyInvocation -Scope 1).Value.MyCommand.Name;
}

Function Trace { Param( [string]$str, [switch]$Trace=$false )
    if($Trace -eq $true)
    {
        $out = if($str -eq $null) { "[NULL]" } 
                else { $str.ToString().Replace([System.Environment]::NewLine,'') }
        Write-Host $out -ForegroundColor Black -BackgroundColor Gray
    }
}

Function Get-Verbose {Param()
    # Write the string "TEST" to the verbose output stream,
    # but redirect that to standard output using 4>&1.
    # Discard the output using Out-Null, but capture it in a variable
    # using -OutVariable (an ArrayList!)
    (Write-Verbose "TEST" -OutVariable output 4>&1) | Out-Null
 
    # If Verbose is $true, the output ArrayList will contain data;
    # if not, it will be empty
    return (!!$output)
}

# Extract named argument from argument list (in pipeline input)
# Usage: $args | Get-Flag "-Verbose"
Function Get-Flag { Param( [Parameter(ValueFromPipeline)] $args, [string] $name )
    begin {
        $result=$false;
        $found=$false;
        $assignNext = $false;
        $index=0;
    }
    process { #Process all items in the pipeline
        $args | ForEach-Object { 
            $index++;
            if($_ -eq $null) { continue; }
            $str = $_.ToString();

            if($assignNext -eq $true)
            {
                $result = $_ -eq "True";
                $assignNext = $false;
                Write-Verbose "$(Get-FunctionName) ($str) Assigned Next {$result}"
            }
            elseif(($found -eq $false) -and 
                $str.StartsWith($name,[System.StringComparison]::OrdinalIgnoreCase))
            {
                $found = $true;
                if($str.Contains(":")) {
                    $split = $str.Split(":");
                    if($split[1].Length -gt 3)
                    {
                        $result = $split[1] -ne "False";
                        Write-Verbose "$(Get-FunctionName) ($str) Assigned {$result}"
                    }
                    else
                    {
                        $assignNext=$true;
                    }
                }
                else {
                    Write-Verbose "$(Get-FunctionName) Flag $true"
                    $result=$true;
                }
            }
            else {
                Write-Verbose  "$(Get-FunctionName) Skipped $str"
            }
        }   
    }
    end { #after all processes are done
        if($found -eq $false) {
            Write-Verbose "$(Get-FunctionName)[$index] Flag not found" 
        }
        return $result;
    }
}

# Returns the last item in a [System.String[]]
Function Last { Param([String[]] $dirs, [int] $offset=0)
    $index = $dirs.Count-1 + $offset;
    return $dirs[$index];
} 


exit

#Test Get-Flag Routines
Write-Host "Test Results should all be True" -ForegroundColor Green
@("-Flag") | Get-Flag -Name "-Flag" -Verbose
@("-Flag:$true") | Get-Flag -Name "-flag" -Verbose
@("-Flag:","$true") | Get-Flag -Name "-flag" -Verbose
@("-Other", "-Flag") | Get-Flag -Name "-Flag" -Verbose
@("-Other", "-Next", "-Flag:$true", "Other") | Get-Flag -Name "-flag" -Verbose
@("-Other","$false", "-Next","$false", "-Flag:","$true", "Other","$false") | Get-Flag -Name "-flag" -Verbose

Write-Host "Test Results should all be False" -ForegroundColor Green
@("-Flag:$false") | Get-Flag -Name "-Flag" -Verbose
@("-Flag:","$false") | Get-Flag -Name "-Flag" -Verbose
@("-NotFlag") | Get-Flag -Name "-flag" -Verbose
@("-Other", "-Flag:$false") | Get-Flag -Name "-Flag" -Verbose
@("-Other", "-Next", "-Flag:$false", "-Other") | Get-Flag -Name "-flag" -Verbose
@("-Other","$true", "-Next","$true", "-Flag:","$false", "Other","$true") | Get-Flag -Name "-flag" -Verbose

#@("Flag") | Get-Flag -Verbose


