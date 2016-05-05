using Api;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Profitbricks
{
    [Cmdlet(VerbsCommon.Get, "PBFirewallRule")]
    [OutputType(typeof(FirewallRule))]
    public class GetFirewallRule : Cmdlet
    {
        #region Parameters

        [Parameter(Position = 0, HelpMessage = "Virtual Datacenter Id", Mandatory = true, ValueFromPipeline = true)]
        public string DataCenterId { get; set; }

        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        [Parameter(Position = 0, HelpMessage = "Nic Id", Mandatory = true, ValueFromPipeline = true)]
        public string NicId { get; set; }

        [Parameter(Position = 0, HelpMessage = "Firewall Rule Id")]
        public string FirewallRuleId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                var fwApi = new FirewallRuleApi(Utilities.Configuration);
                if (!string.IsNullOrEmpty(FirewallRuleId))
                {
                    var fw = fwApi.FindById(DataCenterId, ServerId, NicId, FirewallRuleId, depth: 5);

                    WriteObject(fw);
                }
                else
                {
                    var fws = fwApi.FindAll(DataCenterId, ServerId, NicId, depth: 5);

                    WriteObject(fws.Items);
                }
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }


    [Cmdlet(VerbsCommon.New, "PBFirewallRule")]
    [OutputType(typeof(FirewallRule))]
    public class NewFirewallRule : Cmdlet
    {
        #region Parameters

        [Parameter(Position = 0, HelpMessage = "Virtual Datacenter Id", Mandatory = true, ValueFromPipeline = true)]
        public string DataCenterId { get; set; }

        [Parameter(Position = 1, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        [Parameter(Position = 2, HelpMessage = "Nic Id", Mandatory = true, ValueFromPipeline = true)]
        public string NicId { get; set; }

        [Parameter(Position = 3, HelpMessage = "The protocol for the rule: TCP, UDP, ICMP, ANY.", Mandatory = true, ValueFromPipeline = true)]
        public string Protocol { get; set; }

        [Parameter(Position = 4, HelpMessage = "The name of the Firewall Rule.")]
        public string Name { get; set; }

        [Parameter(Position = 5, HelpMessage = "Only traffic originating from the respective MAC address is allowed. Valid format: aa:bb:cc:dd:ee:ff. Value null allows all source MAC address.")]
        public string SourceMac { get; set; }

        [Parameter(Position = 6, HelpMessage = "Only traffic originating from the respective IPv4 address is allowed. Value null allows all source IPs.")]
        public string SourceIp { get; set; }

        [Parameter(Position = 7, HelpMessage = "In case the target NIC has multiple IP addresses, only traffic directed to the respective IP address of the NIC is allowed. Value null allows all target IPs.")]
        public string TargetIp { get; set; }

        [Parameter(Position = 8, HelpMessage = "Defines the start range of the allowed port (from 1 to 65534) if protocol TCP or UDP is chosen. Leave portRangeStart and portRangeEnd value null to allow all ports.")]
        public int PortRangeStart { get; set; }

        [Parameter(Position = 9, HelpMessage = "Defines the end range of the allowed port (from 1 to 65534) if the protocol TCP or UDP is chosen. Leave portRangeStart and portRangeEnd null to allow all ports.")]
        public int PortRangeEnd { get; set; }

        [Parameter(Position = 10, HelpMessage = "Defines the allowed type (from 0 to 254) if the protocol ICMP is chosen. Value null allows all types.")]
        public int IcmpType { get; set; }

        [Parameter(Position = 11, HelpMessage = "Defines the allowed code (from 0 to 254) if protocol ICMP is chosen. Value null allows all codes.")]
        public int IcmpCode { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                var fwApi = new FirewallRuleApi(Utilities.Configuration);

                var newProps = new FirewallruleProperties
                {
                    Protocol = Protocol,
                    PortRangeStart = PortRangeStart,
                    PortRangeEnd = PortRangeEnd,
                    IcmpType = IcmpType,
                    IcmpCode = IcmpCode
                };

                if (!string.IsNullOrEmpty(Name))
                    newProps.Name = Name;
                if (!string.IsNullOrEmpty(SourceMac))
                    newProps.SourceMac = SourceMac;
                if (!string.IsNullOrEmpty(SourceIp))
                    newProps.SourceIp = SourceIp;
                if (!string.IsNullOrEmpty(TargetIp))
                    newProps.TargetIp = TargetIp;

                var fw = fwApi.Create(DataCenterId, ServerId, NicId, new FirewallRule { Properties = newProps });

                WriteObject(fw);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    [Cmdlet(VerbsCommon.Set, "PBFirewallRule")]
    [OutputType(typeof(FirewallRule))]
    public class SetFirewallRule : Cmdlet
    {
        #region Parameters

        [Parameter(Position = 0, HelpMessage = "Virtual Datacenter Id", Mandatory = true, ValueFromPipeline = true)]
        public string DataCenterId { get; set; }

        [Parameter(Position = 1, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        [Parameter(Position = 2, HelpMessage = "Nic Id", Mandatory = true, ValueFromPipeline = true)]
        public string NicId { get; set; }

        [Parameter(Position = 3, HelpMessage = "Firewall Rule Id")]
        public string FirewallRuleId { get; set; }

        [Parameter(Position = 4, HelpMessage = "The protocol for the rule: TCP, UDP, ICMP, ANY.", Mandatory = true, ValueFromPipeline = true)]
        public string Protocol { get; set; }

        [Parameter(Position = 5, HelpMessage = "The name of the Firewall Rule.")]
        public string Name { get; set; }

        [Parameter(Position = 6, HelpMessage = "Only traffic originating from the respective MAC address is allowed. Valid format: aa:bb:cc:dd:ee:ff. Value null allows all source MAC address.")]
        public string SourceMac { get; set; }

        [Parameter(Position = 7, HelpMessage = "Only traffic originating from the respective IPv4 address is allowed. Value null allows all source IPs.")]
        public string SourceIp { get; set; }

        [Parameter(Position = 8, HelpMessage = "In case the target NIC has multiple IP addresses, only traffic directed to the respective IP address of the NIC is allowed. Value null allows all target IPs.")]
        public string TargetIp { get; set; }

        [Parameter(Position = 9, HelpMessage = "Defines the start range of the allowed port (from 1 to 65534) if protocol TCP or UDP is chosen. Leave portRangeStart and portRangeEnd value null to allow all ports.")]
        public int? PortRangeStart { get; set; }

        [Parameter(Position = 10, HelpMessage = "Defines the end range of the allowed port (from 1 to 65534) if the protocol TCP or UDP is chosen. Leave portRangeStart and portRangeEnd null to allow all ports.")]
        public int? PortRangeEnd { get; set; }

        [Parameter(Position = 11, HelpMessage = "Defines the allowed type (from 0 to 254) if the protocol ICMP is chosen. Value null allows all types.")]
        public int? IcmpType { get; set; }

        [Parameter(Position = 12, HelpMessage = "Defines the allowed code (from 0 to 254) if protocol ICMP is chosen. Value null allows all codes.")]
        public int? IcmpCode { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                var fwApi = new FirewallRuleApi(Utilities.Configuration);

                var newProps = new FirewallruleProperties
                {
                    PortRangeStart = PortRangeStart,
                    PortRangeEnd = PortRangeEnd,
                    IcmpType = IcmpType,
                    IcmpCode = IcmpCode
                };

                if (!string.IsNullOrEmpty(Name))
                    newProps.Name = Name;
                if (!string.IsNullOrEmpty(SourceMac))
                    newProps.SourceMac = SourceMac;
                if (!string.IsNullOrEmpty(SourceIp))
                    newProps.SourceIp = SourceIp;
                if (!string.IsNullOrEmpty(TargetIp))
                    newProps.TargetIp = TargetIp;
                if (!string.IsNullOrEmpty(Protocol))
                    newProps.Protocol = Protocol;

                var fw = fwApi.PartialUpdate(DataCenterId, ServerId, NicId, FirewallRuleId, newProps);

                WriteObject(fw);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    [Cmdlet(VerbsCommon.Remove, "PBFirewallRule")]
    [OutputType(typeof(FirewallRule))]
    public class RemoveFirewallRule : Cmdlet
    {
        #region Parameters

        [Parameter(Position = 0, HelpMessage = "Virtual Datacenter Id", Mandatory = true, ValueFromPipeline = true)]
        public string DataCenterId { get; set; }

        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        [Parameter(Position = 0, HelpMessage = "Nic Id", Mandatory = true, ValueFromPipeline = true)]
        public string NicId { get; set; }

        [Parameter(Position = 0, HelpMessage = "Firewall Rule Id", Mandatory = true, ValueFromPipeline = true)]
        public string FirewallRuleId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                var fwApi = new FirewallRuleApi(Utilities.Configuration);

                var fws = fwApi.Delete(DataCenterId, ServerId, NicId, FirewallRuleId, depth: 5);

                WriteObject("Firewall Rule successfuly deleted.");
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }
}
