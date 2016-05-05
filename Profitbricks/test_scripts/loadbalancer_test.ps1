Import-Module ..\bin\Debug\Profitbricks.dll
$SecurePassword = [Environment]::GetEnvironmentVariable("PB_PASSWORD","User") | ConvertTo-SecureString -AsPlainText -Force
$username = [Environment]::GetEnvironmentVariable("PB_USERNAME","User")

$Credentials = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $username, $SecurePassword

$credentials = Get-Credential $Credentials

Set-Profitbricks $credentials

$newDc = New-PBDatacenter -Name "load balancer test" -Description "PS Unit Testing" -Location "us/las"

$dcstatus = get-PBRequestStatus -RequestUrl $newDc.Request

"DC status" 
$dcstatus.Metadata.Status
$dcstatus.Metadata.MEssage

$datacenter = Get-PBDatacenter $newDc.Id

$loadbalancer = New-PBLoadbalancer -DataCenterId $datacenter.Id -Name "test LB"


Do{
"Waiting on load balancer to provision"
 start-sleep -seconds 5

$status = Get-PBRequestStatus -RequestUrl $loadbalancer.Request

}While($status.Metadata.Status -ne "DONE")


$newname = $loadbalancer.Properties.Name + "updated"

$updated = Set-PBLoadbalancer -DataCenterId $datacenter.Id -LoadbalancerId $loadbalancer.Id -Name $newname

if($updated.Properties.Name -eq $newname){
"Update - check"
}
else{
"Update - fail"
}

Remove-PBDatacenter -DatacenterId $datacenter.Id

#Remove-Module Profitbricks