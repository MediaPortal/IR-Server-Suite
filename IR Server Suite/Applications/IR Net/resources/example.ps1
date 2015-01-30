
## Set Application path
$IRSSdir = 'E:\Starwer\Projects\Software\GitHub\IR-Server-Suite\bin\Debug'


## Load IR Net assembly 
Add-Type -Path "$IRSSdir\IRNet.dll"
Import-Module  "$IRSSdir\IRNet.dll"


## Instanciate the Client 
$irnet = new-object IRNet.IRClient


## Debug mode auto-selection and setup
$debug = ($Host.UI -ne "")
if ($debug) {
    $DebugPreference = "Continue"
    $irnet.LogSetup("IRnet.log", "Debug")
} else {
    $irnet.LogSetup("IRnet.log", "Info")
}


## Tie the RemoteEvent to its actions
Register-ObjectEvent -InputObject $irnet -EventName RemoteEvent -SourceIdentifier IRnetRemoteEvent -Action {
    ## retrieve the Input object because this is not a global variable
    $irnet  = $EventSubscriber.SourceObject

    ## retrieve the event arguments
    $sender = $Args.Sender
    $key    = $Args.Key
    $data   = $Args.Data

    ## Debug Trace
    Write-Debug "  +> RemoteEvent:  Sender: $sender - Key: $key  - Data: $data"
    
    switch ($key) {
        "ServerShutdown" {
            Write-Warning "disconnect from $($irnet.ServerHost)..."
        }
        default {
            Write-Debug "nothing to do"
        
        }
    }


} | Out-Null


$irnet.Connect() | Out-Null

if ($debug) {
    Get-EventSubscriber
}

# $irnet.Learn("test1"); Wait-Event -SourceIdentifier IrssMessageEvent -Timeout 15
# $irnet.Blast("test1")


## Close the script
Unregister-Event IRnetRemoteEvent
$irnet.Disconnect()
