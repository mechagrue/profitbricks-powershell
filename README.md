
# Table of Contents

* [Concepts](#concepts)
* [Getting Started](#getting-started)
* [Installation](#installation)
* [Overview](#overview)
* [How To:](#how-tos)
    * [Create Data Center](#create-data-center)
    * [Create Server](#create-server)
    * [Update Server](#update-server)
    * [List Servers](#list-servers)
    * [Create Volume](#create-volume)
    * [Attach Volume](#attach-volume)
    * [List Volumes](#list-volumes)
    * [Create Snapshot](#create-snapshot)
    * [List Snapshots](#list-snapshots)
    * [Update Snapshot](#update-snapshot)
    * [Delete Snapshot](#delete-snapshot)
* [Reference](#reference)
    * [Data Center](#data-center)
    * [Server](#server)
    * [Volume](#volume)
    * [Snapshot](#snapshot)
    * [Load Balancer](#load-balancer)
    * [Image](#image)
    * [NIC](#nic)
    * [IP Block](#ip-block)
* [Support](#support)

## Concepts

Profitbricks Poweshell module wraps the [ProfitBricks REST API](https://devops.profitbricks.com/api/rest/) allowing you to interact with it from a command-line interface.

## Getting Started

Before you begin you will need to have [signed-up](https://www.profitbricks.com/signup/) for a ProfitBricks account. The credentials you establish during sign-up will be used to authenticate against the [ProfitBricks API](https://devops.profitbricks.com/api/rest/).

## Installation

Download the [ProfitBricks.Zip](---) and extract all. Use one of the followimg options to make the module availible for PowerShell:

1. Place the resulting folder `ProfitBricksSoapApi` (does contain 3 Files) in `%USERPROFILE%\Documents\WindowsPowerShell\Modules\` will auto load the module on PowerShell start for the User.
2. Place the resulting folder `ProfitBricksSoapApi` (does contain 3 Files) in `%SYSTEMROT%\System32\WindowsPowerShell\v1.0\Modules\` will make the module system wide availible (not recomendet)
3. Place the resulting folder in any folder ouf your choice and extend the eviromet variable `PSModulePath` by this folder will make the module system wide availible. 

## Configuration

Before using the ProfitBrick's Powershell module to perform any operations, we'll need to set our credentials:

```
$SecurePassword = "PB_PASSWORD" | ConvertTo-SecureString -AsPlainText -Force
$username = "PB_USERNAME","User"

$Credentials = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $username, $SecurePassword

$credentials = Get-Credential $Credentials

Set-Profitbricks -Credential $credentials
Authorization successful
```


You will be notified with the following message if you have provided incorrect credentials:

```
At line:9 char:1
+ Set-Profitbricks $credentials
+ ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    + CategoryInfo          : AuthenticationError: (:) [Set-Profitbricks], Exception
    + FullyQualifiedErrorId : 406,Profitbricks.SetProfitbricks
```

After successful authentication the credentials will be stored for the duration of the Powershell session.


# How To's:

These examples assume that you don't have any resources provisioned under your account. The first thing we will want to do is create a data center to hold all of our resources.

#Datacenter

We need to supply some parameters to get our first data center created. In this case, we will set the location to 'us/las' so that this data center is created under the [DevOps Data Center](https://devops.profitbricks.com/tutorials/devops-data-center-information/). Other valid locations can be determined by reviewing the [REST API Documentation](https://devops.profitbricks.com/api/rest/#locations). That documentation is an excellent resource since that is what the ProfitBricks CLI is calling to complete these operations.

```
>$datacenter =  New-PBDatacenter -Name "Example datacenter" -Description "Example description" -Location "us/las"
>$datacenter
```

```
Id         : dbe936f8-a536-49c5-b864-cec842e3ee65
Type       : datacenter
Href       : https://api.profitbricks.com/rest/v2/datacenters/dbe936f8-a536-49c5-b864-cec842e3ee65
Metadata   : class DatacenterElementMetadata {
               CreatedDate: 5/5/2016 10:38:28 AM
               CreatedBy: jasmin.gacic@gmail.com
               Etag: 75c469dc35faf7223943f4c6fe6d0689
               LastModifiedDate: 5/5/2016 10:38:28 AM
               LastModifiedBy: user@domain.com
               State: BUSY
             }
             
Properties : class DatacenterProperties {
               Name: Example datacenter
               Description: Example description
               Location: us/las
               Version: 
             }
             
Entities   : 
Request    : https://api.profitbricks.com/rest/v2/requests/d2cee0fc-a984-4d88-b56e-68203ad61660/status
```

Et voilà, we've successfully provisioned a data center. Notice the "Id" that was returned. That UUID was assigned to our new data center and will be needed for other operations. The "RequestID" that was returned can be used to check on the status of any `create` or `update` operations.

```
Get-Datacenter -DataCenterId [dbe936f8-a536-49c5-b864-cec842e3ee65]		
```

## Create Server

Next we'll create a server in the data center. This time we have to pass the 'Id' for the data center in, along with some other relevant properties (processor cores, memory, boot volume or boot CD-ROM) for the new server.

```
$server = New-PBServer -DataCenterId $datacenter.Id -Name "server_test" -Cores 1 -Ram 256 
$server
```

```
Id         : 3e7c254f-aeb6-481e-a594-1267cc19cac5
Type       : server
Href       : https://api.profitbricks.com/rest/v2/datacenters/dbe936f8-a536-49c5-b864-cec842e3ee65/servers/3e7c254f-aeb6-481e-a594-1267cc19cac5
Metadata   : class DatacenterElementMetadata {
               CreatedDate: 5/5/2016 10:45:29 AM
               CreatedBy: jasmin.gacic@gmail.com
               Etag: b4046bb75c48ce7bce9cb6ab3e82ae91
               LastModifiedDate: 5/5/2016 10:45:29 AM
               LastModifiedBy: user@domain.com
               State: BUSY
             }
             
Properties : class ServerProperties {
               Name: server_test
               Cores: 1
               Ram: 256
               AvailabilityZone: 
               VmState: 
               BootCdrom: 
               BootVolume: 
             }
             
Entities   : 
Request    : https://api.profitbricks.com/rest/v2/requests/3e72e83b-6e8b-43c4-bccb-97ce084292bf/status

```

## Update Server

Whoops, we didn't assign enough memory to our instance. Lets go ahead and update the server to increase the amount of memory it has assigned. We'll need the datacenterid, the id of the server we are updating, along with the parameters that we want to change.

```
$server = Set-PBServer -DataCenterId $datacenter.Id -ServerId $server.Id -Ram 1024
$server.Properties.Ram
1024
```

## List Servers

Lets take a look at the list of servers in our data center. There are a couple more listed in here for demonstration purposes.

```
Get-PBServer -DataCenterId dbe936f8-a536-49c5-b864-cec842e3ee65 | Format-Table

Id                                   Type   Href                                                                                                                          
--                                   ----   ----                                                                                                                          
cf2b3019-351c-438b-a5f8-b4f323fc9f23 server https://api.profitbricks.com/rest/v2/datacenters/dbe936f8-a536-49c5-b864-cec842e3ee65/servers/cf2b3019-351c-438b-a5f8-b4f32...
3e7c254f-aeb6-481e-a594-1267cc19cac5 server https://api.profitbricks.com/rest/v2/datacenters/dbe936f8-a536-49c5-b864-cec842e3ee65/servers/3e7c254f-aeb6-481e-a594-1267c...
```

## Create Volume

Now that we have a server provisioned, it needs some storage. We'll specify a size for this storage volume in GB as well as set the 'bus' and 'licencetype'. The 'bus' setting can have a serious performance impact and you'll want to use VIRTIO when possible. Using VIRTIO may require drivers to be installed depending on the OS you plan to install. The 'licencetype' impacts billing rate, as there is a surcharge for running certain OS types.

```
$volume = New-PBVolume -DataCenterId $datacenter.Id -Size 5 -Type HDD -ImageId 646023fb-f7bd-11e5-b7e8-52540005ab80 -Name "test_volume"
$volume | Format-Table

Id                                   Type   Href                                                                                                                          
--                                   ----   ----                                                                                                                          
12629ab1-20e4-4751-9d92-94986d424eec volume https://api.profitbricks.com/rest/v2/datacenters/dbe936f8-a536-49c5-b864-cec842e3ee65/volumes/12629ab1-20e4-4751-9d92-94986...

```

## Attach Volume

The volume we've created is not yet connected or attached to a server. To accomplish that we'll use the `dcid` and `serverid` values returned from the previous commands:

```
$attachedvolume = Attach-PBVolume -DataCenterId $datacenter.Id -ServerId $newServer.Id -VolumeId $volume.Id
$attachedvolume | Format-Table

Id                                   Type   Href                                                                                                                          
--                                   ----   ----                                                                                                                          
12629ab1-20e4-4751-9d92-94986d424eec volume https://api.profitbricks.com/rest/v2/datacenters/dbe936f8-a536-49c5-b864-cec842e3ee65/volumes/12629ab1-20e4-4751-9d92-94986...
```

## List Volumes

Let's take a look at all the volumes in the data center:

```
Get-PBVolume -DataCenterId $datacenter.Id | Format-Table

Id                                   Type   Href                                                                                                                          
--                                   ----   ----                                                                                                                          
12629ab1-20e4-4751-9d92-94986d424eec volume https://api.profitbricks.com/rest/v2/datacenters/dbe936f8-a536-49c5-b864-cec842e3ee65/volumes/12629ab1-20e4-4751-9d92-94986...
```

#Continue
## Create Snapshot

If we have a volume we'd like to keep a copy of, perhaps as a backup, we can take a snapshot:

```
$ profitbricks snapshot create --datacenterid 3fc832b1-558f-48a4-bca2-af5043975393 --volumeid d231cd2e-89c1-4ed4-b4ad-d0a2c8b2b4a7

Snapshot
-------------------------------------------------------------------------------------------------------------
Id                                    Name                                  Size  Created               State
------------------------------------  ------------------------------------  ----  --------------------  -----
cf90b2e3-179b-4bff-a84c-d53ca58487dd  Demo Srvr 1 Boot-Snapshot-07/27/2015  null  2015-07-27T17:41:41Z  BUSY
```

## List Snapshots

Here is a list of the snapshots in our account:

```
$ profitbricks snapshot list

Snapshots
-----------------------------------------------------------------------------------------------------------------
Id                                    Name                                  Size  Created               State
------------------------------------  ------------------------------------  ----  --------------------  ---------
cf90b2e3-179b-4bff-a84c-d53ca58487dd  Demo Srvr 1 Boot-Snapshot-07/27/2015  10    2015-07-27T17:41:42Z  AVAILABLE
```

## Update Snapshot

Now that we have a snapshot created, we can change the name to something more descriptive:

```
$ profitbricks snapshot update -i cf90b2e3-179b-4bff-a84c-d53ca58487dd --name "Demo Srvr 1 OS just installed"

Snapshot
------------------------------------------------------------------------------------------------------
Id                                    Name                           Size  Created               State
------------------------------------  -----------------------------  ----  --------------------  -----
cf90b2e3-179b-4bff-a84c-d53ca58487dd  Demo Srvr 1 OS just installed  10    2015-07-27T17:41:42Z  BUSY
```

## Delete Snapshot

We can delete our snapshot when we are done with it:

```
$ profitbricks snapshot delete -i cf90b2e3-179b-4bff-a84c-d53ca58487dd

You are about to delete a snapshot. Do you want to proceed? (y/n
prompt: yes:  y
```

## Summary

Now we've had a taste of working with the ProfitBricks CLI. The reference section below will provide some additional information regarding what parameters are available for various operations.

 Get-Server

New-Server

Set-Server

Reboot-Server

Remove-Server

Start-Server

Stop-Server

#Volume

New-Volume

 Set-Volume

 Attach-Volume

 Detach-Volume

 Get-AttachedVolume

 Get-Volume

 New-Volume

 Remove-Volume


#Image
 Get-Image

#Snapshot

Get-Snapshot

New-Snapshot
Remove-Snapshot

Restore-Snapshot

Set-Snapshot


#Loadbalancer

Get-Loadbalancer

New-Loadbalancer

Remove-Loadbalancer

Remove-NicFromLoadbalancer

Set-Loadbalancer

Set-NicToLoadbalancer

Get-BalancedNics

#Nic
Get-Nic

New-Nic

Remove-Nic

Set-Nic


#Firewall
Get-FirewallRule

New-FirewallRule

Remove-FirewallRule

Set-FirewallRule


#IPBlock
 Get-IPBlock
 New-IPBlock
 Remove-IPBlock

#lan
 Get-Lan
 New-Lan
 Remove-Lan

#location
 Get-PBLocation

#request status
 
 Get-RequestStatus
