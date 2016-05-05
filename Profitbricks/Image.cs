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
    [Cmdlet(VerbsCommon.Get, "PBImage")]
    [OutputType(typeof(Image))]
    public class GetImage :Cmdlet
    {
        #region Parameters

        [Parameter(HelpMessage ="Image Id", Position = 0, ValueFromPipeline = true)]
        public string ImageId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                var imageApi = new ImageApi(Utilities.Configuration);

                if (!string.IsNullOrEmpty(this.ImageId))
                {
                    var image = imageApi.FindById(this.ImageId, depth: 5);

                    WriteObject(image);
                }
                else
                {
                    var images = imageApi.FindAll(depth: 5);

                    WriteObject(images.Items);
                }
            }catch(Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }
}
