using DotNetOpen.PrerenderModule;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("DotNetOpen.PrerenderModule")]
[assembly: AssemblyDescription("ASP.NET Middleware for Prerender.io")]
[assembly: AssemblyProduct("DotNetOpen.PrerenderModule")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("d6c66972-bae0-4e00-92f3-6299472c8200")]

[assembly: PreApplicationStartMethodAttribute(typeof(PrerenderPreApplicationStart), PrerenderPreApplicationStart.StartMethodName)]
