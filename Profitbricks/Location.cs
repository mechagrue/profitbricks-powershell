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
    /// <summary>
    /// <para type="synopsis">This commandlet will get one or list of Locations.</para>
    /// <para type="synopsis">If LocationID parameter is provided then it will return only the specified snapshot.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-PBImage </para>
    /// <para type="description">Get-PBImage -ImageId[UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "PBLocation")]
    [OutputType(typeof(Location))]
    public class GetLocation : Cmdlet
    {
        #region Parameters

        /// <summary>
        /// <para type="description">Location Id.</para>
        /// </summary>
        [Parameter(HelpMessage ="Location Id",Position =0,ValueFromPipeline = true)]
        public string LocationId { get; set; }

        #endregion
        protected override void BeginProcessing()
        {
            try
            {
                var locationApi = new LocationApi(Utilities.Configuration);

                if (!string.IsNullOrEmpty(this.LocationId))
                {
                    if (LocationId.Contains("/"))
                    {
                        var location = locationApi.FindById(LocationId.Split('/')[0], LocationId.Split('/')[1], depth: 5);
                        WriteObject(location);
                    }else
                    {
                        WriteWarning("Location Id must consist of region name and location name separated by '/' for example   us/las ");
                    }
                }else
                {
                    var locations = locationApi.FindAll(depth: 5);

                    WriteObject(locations.Items);
                }
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }
}
