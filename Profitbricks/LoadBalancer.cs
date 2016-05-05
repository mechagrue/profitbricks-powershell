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
    [Cmdlet(VerbsCommon.Get, "PBLoadbalancer")]
    [OutputType(typeof(Loadbalancer))]
    public class GetLoadbalancer : Cmdlet
    {
        [Parameter(Position = 0, HelpMessage = "Virtual Datacenter Id", Mandatory = true)]
        public string DataCenterId { get; set; }

        [Parameter(Position = 1, HelpMessage = "Loadbalancer Id", Mandatory = true)]
        public string LoadbalancerId { get; set; }

        protected override void BeginProcessing()
        {
            try
            {

                var loadbalancerApi = new LoadBalancerApi(Utilities.Configuration);

                if (!string.IsNullOrEmpty(LoadbalancerId))
                {
                    var loadbalancer = loadbalancerApi.FindById(DataCenterId, LoadbalancerId, depth: 5);
                    WriteObject(loadbalancer);
                }
                else
                {
                    var loadbalancers = loadbalancerApi.FindAll(DataCenterId, depth: 5);

                    WriteObject(loadbalancers.Items);
                }
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    [Cmdlet(VerbsCommon.New, "PBLoadbalancer")]
    [OutputType(typeof(Loadbalancer))]
    public class NewLoadbalancer : Cmdlet
    {
        #region Parameters

        [Parameter(Position = 0,HelpMessage = "Virtual Datacenter ID", Mandatory = true)]
        public string DataCenterId { get; set; }

        [Parameter(Position = 1,HelpMessage = "Loadbalancer Name", Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Position = 2, HelpMessage = "IPv4 address of the loadbalancer.")]
        public string Ip { get; set; }


        [Parameter(Position = 3, HelpMessage = "Indicates if the loadbalancer will reserve an IP using DHCP.")]
        public bool? Dhcp { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                var loadbalancerApi = new LoadBalancerApi(Utilities.Configuration);
                var newProps = new LoadbalancerProperties
                {
                    Name = this.Name,
                    Dhcp = this.Dhcp
                };

                if (!string.IsNullOrEmpty(this.Ip))
                {
                    newProps.Ip = this.Ip;
                }

                var loadbalancer = loadbalancerApi.Create(DataCenterId, new Loadbalancer { Properties = newProps });

                WriteObject(loadbalancer);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    [Cmdlet(VerbsCommon.Remove, "PBLoadbalancer")]
    [OutputType(typeof(Loadbalancer))]
    public class RemoveLoadbalancer : Cmdlet
    {
        #region Parameters

        [Parameter(Position = 0, HelpMessage = "Virtual Datacenter Id", Mandatory = true, ValueFromPipeline = true)]
        public string DataCenterId { get; set; }

        [Parameter(Position = 1, HelpMessage = "Loadbalancer Id", Mandatory = true, ValueFromPipeline = true)]
        public string LoadbalancerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                var loadBalancerApi = new LoadBalancerApi(Utilities.Configuration);

                var resp = loadBalancerApi.Delete(this.DataCenterId, this.LoadbalancerId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }


    [Cmdlet(VerbsCommon.Set, "PBLoadbalancer")]
    [OutputType(typeof(Loadbalancer))]
    public class SetLoadbalancer : Cmdlet
    {
        #region Parameters

        [Parameter(Position = 0, HelpMessage = "Virtual Datacenter Id", Mandatory = true, ValueFromPipeline = true)]
        public string DataCenterId { get; set; }

        [Parameter(Position = 1, HelpMessage = "Loadbalancer Id", Mandatory = true, ValueFromPipeline = true)]
        public string LoadBalancerId { get; set; }
        [Parameter(Position = 2, HelpMessage = "Loadbalancer Name", ValueFromPipeline = true)]
        public string Name { get; set; }

        [Parameter(Position = 3, HelpMessage = "Virtual Datacenter Description", ValueFromPipeline = true)]
        public string Ip { get; set; }

        [Parameter(Position =  4,HelpMessage = "Indicates if the loadbalancer will reserve an IP using DHCP.")]
        public bool? Dhcp { get; set; }
        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                var loadbalancerApi = new LoadBalancerApi(Utilities.Configuration);

                var newProps = new LoadbalancerProperties { Dhcp = Dhcp };
                if (!string.IsNullOrEmpty(Ip))
                {
                    newProps.Ip = Ip;
                }
                if (!string.IsNullOrEmpty(Name))
                {
                    newProps.Name = Name;
                }

                var resp = loadbalancerApi.PartialUpdate(DataCenterId, LoadBalancerId, newProps);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    [Cmdlet(VerbsCommon.Get, "PBBalancedNics")]
    [OutputType(typeof(BalancedNics))]
    public class GetBalancedNics : Cmdlet
    {
        #region Parameters

        [Parameter(Position = 0, HelpMessage = "Virtual Datacenter Id", Mandatory = true, ValueFromPipeline = true)]
        public string DataCenterId { get; set; }

        [Parameter(Position = 1, HelpMessage = "Loadbalancer Id", Mandatory = true, ValueFromPipeline = true)]
        public string LoadBalancerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                var loadbalancerApi = new LoadBalancerApi(Utilities.Configuration);

                var resp = loadbalancerApi.FindAll(DataCenterId, LoadBalancerId);
                WriteObject(resp.Items);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    [Cmdlet(VerbsCommon.Set, "NicToLoadbalancer")]
    [OutputType(typeof(BalancedNics))]
    public class SetNicToLoadbalancer : Cmdlet
    {
        #region Parameters

        [Parameter(Position = 0, HelpMessage = "Virtual Datacenter Id", Mandatory = true, ValueFromPipeline = true)]
        public string DataCenterId { get; set; }

        [Parameter(Position = 1, HelpMessage = "Loadbalancer Id", Mandatory = true, ValueFromPipeline = true)]
        public string LoadbalancerId { get; set; }

        [Parameter(Position = 2, HelpMessage = "Loadbalancer Id", Mandatory = true, ValueFromPipeline = true)]
        public string NicId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                var networkInterfacesApi = new NetworkInterfacesApi(Utilities.Configuration);

                var resp = networkInterfacesApi.AttachNic(DataCenterId, LoadbalancerId, new Nic { Id = NicId });
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    [Cmdlet(VerbsCommon.Remove, "PBNicFromLoadbalancer")]
    [OutputType(typeof(BalancedNics))]
    public class RemoveNicFromLoadbalancer : Cmdlet
    {
        #region Parameters

        [Parameter(Position = 0, HelpMessage = "Virtual Datacenter Id", Mandatory = true, ValueFromPipeline = true)]
        public string DataCenterId { get; set; }

        [Parameter(Position = 1, HelpMessage = "Loadbalancer Id", Mandatory = true, ValueFromPipeline = true)]
        public string LoadbalancerId { get; set; }

        [Parameter(Position = 2, HelpMessage = "Nic Id", Mandatory = true, ValueFromPipeline = true)]
        public string NicId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                var networkInterfacesApi = new NetworkInterfacesApi(Utilities.Configuration);

                var resp = networkInterfacesApi.DetachNic(DataCenterId, LoadbalancerId, NicId);

                WriteObject(resp);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }
}