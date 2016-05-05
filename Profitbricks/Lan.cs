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
    [Cmdlet(VerbsCommon.Get, "PBLan")]
    [OutputType(typeof(Lan))]
    public class GetLan : Cmdlet
    {
        #region Parameters

        [Parameter(Position = 0, HelpMessage = "Virtual Datacenter Id", Mandatory = true, ValueFromPipeline = true)]
        public string DataCenterId { get; set; }

        [Parameter(Position = 1, HelpMessage = "Lan Id", ValueFromPipeline = true)]
        public string LanId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                var lanApi = new LanApi(Utilities.Configuration);

                if (!string.IsNullOrEmpty(LanId))
                {
                    var lan = lanApi.FindById(DataCenterId, LanId, depth: 5);

                    WriteObject(lan);
                }
                else
                {
                    var lans = lanApi.FindAll(DataCenterId, depth: 5);

                    WriteObject(lans.Items);
                }
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    [Cmdlet(VerbsCommon.New, "PBLan")]
    [OutputType(typeof(Lan))]
    public class NewLan : Cmdlet
    {
        #region Parameters

        [Parameter(Position = 0, HelpMessage = "Virtual Datacenter Id", Mandatory = true, ValueFromPipeline = true)]
        public string DataCenterId { get; set; }

        [Parameter(Position = 1, HelpMessage = "The name of your LAN.", ValueFromPipeline = true)]
        public string Name { get; set; }

        [Parameter(Position = 1, HelpMessage = "Boolean indicating if the LAN faces the public Internet or not.", ValueFromPipeline = true)]
        public bool? Public { get; set; }

        #endregion
        protected override void BeginProcessing()
        {
            try
            {
                var lanApi = new LanApi(Utilities.Configuration);

                var newProps = new LanProperties { Public = this.Public };

                if (!string.IsNullOrEmpty(Name))
                {
                    newProps.Name = Name;
                }

                var newLan = lanApi.Create(DataCenterId, new Lan { Properties = newProps });

                WriteObject(newLan);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }
    [Cmdlet(VerbsCommon.Remove, "PBLan")]
    [OutputType(typeof(Lan))]
    public class RemoveLan : Cmdlet
    {
        #region Parameters

        [Parameter(Position = 0, HelpMessage = "Virtual Datacenter Id", Mandatory = true, ValueFromPipeline = true)]
        public string DataCenterId { get; set; }

        [Parameter(Position = 1, HelpMessage = "LAN Id", Mandatory = true, ValueFromPipeline = true)]
        public string LanId { get; set; }
        
        #endregion
        protected override void BeginProcessing()
        {
            try
            {
                var lanApi = new LanApi(Utilities.Configuration);

                var lan = lanApi.Delete(DataCenterId, LanId);
                WriteObject(lan);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    [Cmdlet(VerbsCommon.Set, "PBLan")]
    [OutputType(typeof(Lan))]
    public class SetLan : Cmdlet
    {
        #region Parameters

        [Parameter(Position = 0, HelpMessage = "Virtual Datacenter Id", Mandatory = true, ValueFromPipeline = true)]
        public string DataCenterId { get; set; }

        [Parameter(Position = 1, HelpMessage = "LAN Id", Mandatory = true, ValueFromPipeline = true)]
        public string LanId { get; set; }

        [Parameter(Position = 2, HelpMessage = "The name of your LAN.", ValueFromPipeline = true)]
        public string Name { get; set; }

        [Parameter(Position = 3, HelpMessage = "Boolean indicating if the LAN faces the public Internet or not.", ValueFromPipeline = true)]
        public bool? Public { get; set; }

        #endregion
        protected override void BeginProcessing()
        {
            try
            {
                var lanApi = new LanApi(Utilities.Configuration);
                var newProps = new LanProperties { Public = this.Public};

                if (!string.IsNullOrEmpty(Name))
                {
                    newProps.Name = Name;
                }

                var resp = lanApi.PartialUpdate(DataCenterId, LanId,newProps);

                WriteObject("Lan successfully removed.");
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }
}
