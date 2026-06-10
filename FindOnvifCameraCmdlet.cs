using System.Management.Automation;
using System.Security;
using Onvif.Core.Client.Common;

namespace PowerCams;



[Cmdlet(VerbsCommon.Find, "OnvifCamera")]
[OutputType(typeof(OnvifCameraInfo))] // cmdlet output type

public class FindOnvifCameraCmdlet : PSCmdlet
{
    [Parameter(Mandatory = true)]
    [Credential] // credential parameter that auto prompts if Get-Credential is missing
    public PSCredential Credential { get; set; } = PSCredential.Empty;

    [Parameter] public int TimeoutSeconds { get; set; } = 5;

    protected override void ProcessRecord() // run
    {
        var username = Credential.UserName;
        var password = Credential.GetNetworkCredential().Password;

        WriteVerbose($"Starting ONVIF discovery, timeout {TimeoutSeconds}s");
        
        try
        {
            // collect on background thread. resume on main thread.
            var cams = CollectCamsAsync(username, password).GetAwaiter().GetResult();

            foreach (var cam in cams)
            {
                WriteVerbose($"Found: {cam.Manufacturer} {cam.Model} at IP: {cam.Address}.");
                WriteObject(cam);
            }
        }
        catch (Exception ex)
        {
            WriteError(new ErrorRecord(ex, "Discovery failed.", ErrorCategory.CloseError, null));
        }

    }

    private async Task<List<OnvifCameraInfo>> CollectCamsAsync(string username, string password)
    {
        var results = new List<OnvifCameraInfo>();
        var discovery = new OnvifDiscovery();

        await foreach (var cam in discovery.DiscoverAsync(username, password, TimeoutSeconds))
        {
            results.Add(cam);
        }
        return results;

    }
}
    

























































