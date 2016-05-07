using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using OneHUDData;
using OneHUDInterface;

namespace AGServer.Servers.DataHandlers.Startup
{
    class StartupDataHandler
    {
        public static StartupDataHandlerResult ProcessStartupRequest(TelemetryData telemetry, Dictionary<string, IGame> plugins, NameValueCollection postData)
        {
            StartupDataHandlerResult result = new StartupDataHandlerResult() { Result = false };
            result.Plugins = new List<SartupDataHandlerPlugin>();
            result.Pages = new List<StartupDataPageInfo>();
            result.Widgets = new List<StartupDataWidgetInfo>();

            result.DefaultPage = "Home";

            result.Version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

            foreach (var key in plugins)
            {
                IGame game = key.Value;
                SartupDataHandlerPlugin plugin = new SartupDataHandlerPlugin();
                plugin.PluginName = "";
                plugin.GameShortName = game.Name;
                plugin.GameLongName = game.DisplayName;
                plugin.PluginVersion = game.Version;
                result.Plugins.Add(plugin);
            }

            string basePath = Directory.GetCurrentDirectory();
            basePath += @"\Web\js\pages";
            string[] pageJsonFiles = Directory.GetFiles(basePath, "*.js");

            for (int i = 0; i < pageJsonFiles.Length; i++)
            {
                string pageText = File.ReadAllText(pageJsonFiles[i]);

                string pageName = ParseVariable(pageText, "_name");
                string pageIcon = ParseVariable(pageText, "_icon");
                string menuIcon = ParseVariable(pageText, "_menuIcon");
                
                string pageDescription = ParseVariable(pageText, "_description");
                string order = ParseVariable(pageText, "_order", false);
                int pageOrder = 1;

                if (!int.TryParse(order, out pageOrder))
                {
                    pageOrder = 1;
                }

                if (pageName != null && pageIcon != null && pageDescription != null)
                {
                    StartupDataPageInfo pageInfo = new StartupDataPageInfo();
                    pageInfo.Name = pageName;
                    pageInfo.Icon = pageIcon;
                    pageInfo.Menuicon = menuIcon;
                    pageInfo.Description = pageDescription;
                    pageInfo.Order = pageOrder;
                    pageInfo.FileName = "js/pages/" + Path.GetFileName(pageJsonFiles[i]);
                    result.Pages.Add(pageInfo);
                }
            }

            result.Pages = result.Pages.OrderBy(o => o.Order).ToList();

            basePath = Directory.GetCurrentDirectory();
            basePath += @"\Web\js\widgets";
            DirSearch(basePath, result);

            return result;
        }

        static void DirSearch(string dir, StartupDataHandlerResult result)
        {
            string basePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Web";
            try
            {
                foreach (string f in Directory.GetFiles(dir))
                {
                    string widgetJS = File.ReadAllText(f);
                    string widgetName = ParseVariable(widgetJS, "_name");
                    string widgetIcon = ParseVariable(widgetJS, "_icon");
                    string widgetDescription = ParseVariable(widgetJS, "_description");
                    string widgetPath = f.Replace(basePath, "");
                    widgetPath = widgetPath.Replace("\\", "/");

                    StartupDataWidgetInfo widget = new StartupDataWidgetInfo() { Name = widgetName, Icon = widgetIcon, Description = widgetDescription, Path = widgetPath };
                    result.Widgets.Add(widget);
                }

                foreach (string d in Directory.GetDirectories(dir))
                {
                    DirSearch(d, result);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static string ParseVariable(string text, string varName, bool quoted = true)
        {
            string result = null;
            Regex regex;

            if (quoted)
            {
                regex = new Regex(@"var " + varName + " = '(.)+';");
            }
            else
            {
                regex = new Regex(@"var " + varName + " = (.)+;");
            }

            Match lineFound = regex.Match(text);
            if (lineFound.Success)
            {
                if (quoted)
                {
                    regex = new Regex(@"'(.)+';");
                }
                else
                {
                    regex = new Regex(@"= (.)+;");
                }
                Match stringFound = regex.Match(lineFound.Value);
                if (stringFound.Success)
                {
                    if (quoted)
                    {
                        result = stringFound.Value.Substring(1, stringFound.Value.Length - 3);
                    }
                    else
                    {
                        result = stringFound.Value.Substring(2, stringFound.Value.Length - 3);
                    }
                }
            }

            return result;
        }
    }
}