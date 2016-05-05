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
    [Cmdlet(VerbsCommon.Get, "PBIPBlock")]
    [OutputType(typeof(IpBlock))]
    public class GetIPBlock : Cmdlet
    {

        #region Parameters 

        [Parameter(Position = 0, HelpMessage = "IP Block Id", ValueFromPipeline = true)]
        public string IpBlockId { get; set; }
        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                var ipblockApi = new IPBlocksApi(Utilities.Configuration);

                if (!string.IsNullOrEmpty(IpBlockId))
                {
                    var ipblock = ipblockApi.FindById(IpBlockId, depth: 5);

                    WriteObject(ipblock);
                }
                else
                {
                    var ipblocks = ipblockApi.FindAll(depth: 5);
                    WriteObject(ipblocks.Items);
                }
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    [Cmdlet(VerbsCommon.New, "PBIPBlock")]
    [OutputType(typeof(IpBlock))]
    public class NewIPBlock : Cmdlet
    {

        #region Parameters 

        [Parameter(Position = 0, HelpMessage = "Location (see: Get-Location)", Mandatory = true, ValueFromPipeline = true)]
        public string Location { get; set; }

        [Parameter(Position = 1, HelpMessage = "The size of the IP block", Mandatory = true, ValueFromPipeline = true)]
        public int Size { get; set; }
        
        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                var ipblockApi = new IPBlocksApi(Utilities.Configuration);

                var newProps = new IpBlockProperties { Size = this.Size, Location = this.Location };

                var ipblock = ipblockApi.Create(new IpBlock { Properties = newProps }, depth: 5);
                WriteObject(ipblock);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    [Cmdlet(VerbsCommon.Remove, "PBIPBlock", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(IpBlock))]
    public class RemoveIpBlock : Cmdlet
    {
        #region Parameters

        [Parameter(Position = 0, HelpMessage = "IPBlock Id", Mandatory = true, ValueFromPipeline = true)]
        public string IpBlockId { get; set; }
        
        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                var ipblockApi = new IPBlocksApi(Utilities.Configuration);

                var resp = ipblockApi.Delete(IpBlockId);

                WriteObject("IPBlock successfully removed ");
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }
}
