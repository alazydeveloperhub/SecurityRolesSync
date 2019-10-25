using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace SecurityRolesSync
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Security Roles Sync"),
        ExportMetadata("Description", "This plugin will copy security roles from one user and apply them to another."),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAMAAABEpIrGAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAFiUAABYlAUlSJPAAAAHXUExURQAAAAEBAQICAQMDAgQEAwQEBAUFBAUFBQcHBggIBwoKCAoKCQsLCgwMCwwMDA0NDA4ODQ4ODg8PDhISEhMTEhgYFxgYGCIiISQkJCUlJSYmJikpKCkpKSoqKSsrKisrKywsKywsLC0tLC4uLi8vLzAwLzAwMDQ0MzQ0NDU1NDU1NTY2NjY2Nz4+PT8/PUBAP0FBQEREREZGRkhIR0pKSE1NTU9PUFJSUlNTUlNTU1VVVVZWVVtbW11dXGRkY2ZmZWZmZmdnZmdnZ2pqamxsa2xsbG1tbW5ubnJycnNzcnNzc3V1dHV1dXd3dnx8e319fX9/f4SEhIWFhIWFhYWFhoaGhoyMjI2NjY6Ojo+Pj4+PkJGRkJGRkZSUlJSUlZWVlZaWlZaWlpiYmJmZmZqamp2dnZ+fn6CgoKKioqampaenp6mpqKmpqaurqq2tra+vr7CwsLKysra2tri4t7m5ub29vb6+vr+/v8PDwsTExMXFxcbGxsnJyMnJyc7Ozs/Pz9DQz9DQ0NLS0tPT09TU09TU1NXV1dbW1tfX19jY19ra2tzc297e3uLi4+Tk5Onp6erq6erq6vDw8Pb29vf39/n5+Pn5+fv7+/v7/Pz8/P39/f7+/v///6xzdQ4AAAFFSURBVDjLY5hDADAMQgVmpngVzGRITWGYjltBCUOHqmQlYzEuBbYqbQwcXAw1ajbYFQhHBDPwCwkJMLjFCGNRUMXQbsggICQEUiFbx1CBrsBbop2FVQgKOFhKFLxQFYi5ZTLwCsABP0OkqyiSgg6GPE0GNCCewdAKUxDFUDsLxO4B4j4QY+IMsGbGMIgCFft6jb7gMqfwZP34WMeUaJ9Wvur8JIOcgGxX5dlABdMUnWcF2ckF+2VYBLt6Bqi7+jskBGs3JIZYtnjITwVbEcrQjR5FMWDrA2GObGTIUUJ3pHERQz2SN0V800GhCAO8DEX+gqgB5S7TxMwGk2finiTlgh7U5QxdurCgtu5nKMUSWYJxASBreBiyYnixR7eVVjMDJwt7r445rgRTyNAprTeBoQB3kpvCkJbLMBlvojUxGgI5CwMAACjk8bGhnMDYAAAAAElFTkSuQmCC"),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "Lavender"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class MyPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new MyPluginControl();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public MyPlugin()
        {
            // If you have external assemblies that you need to load, uncomment the following to 
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}