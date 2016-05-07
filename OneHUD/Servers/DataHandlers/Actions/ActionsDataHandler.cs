using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Text.RegularExpressions;
using OneHUDData;

namespace AGServer.Servers.DataHandlers.Actions
{
    static class ActionsDataHandler
    {

        public static ActionsDataHandlerResult ProcessFileRequest(TelemetryData telemetry, NameValueCollection postData)
        {
            ActionsDataHandlerResult result = new ActionsDataHandlerResult();

            if (postData.AllKeys.Contains("action"))
            {
                switch (postData["action"])
                {
                    case "LoadDash":
                        string name = FindDash(postData);
                        result.Data = LoadDash(name);
                        if (result.Data != null)
                        {
                            result.Result = true;
                        }
                        break;

                }
            }

            return result;
        }

        private static string FindDash(NameValueCollection postData)
        {
            int screenx;
            int screeny;

            string result = postData["name"];
            // NOTE all dimensions are in LANDSCAPE mode

            if (result == "default")
            {
                if (Int32.TryParse(postData["screenx"], out screenx))
                {
                    if (Int32.TryParse(postData["screeny"], out screeny))
                    {
                        if (screenx < screeny)
                        {
                            int temp = screeny;
                            screeny = screenx;
                            screenx = temp;
                        }

                        List<Tuple<int, int, int>> screens = new List<Tuple<int, int, int>>();
                        string basePath = Directory.GetCurrentDirectory();
                        basePath += @"\Panels\Defaults";
                        string[] defaultDashboards = Directory.GetFiles(basePath, "*.json");
                        for (int i = 0; i < defaultDashboards.Length; i++)
                        {
                            string pageText = File.ReadAllText(defaultDashboards[i]);

                            int dashScreenx = ParseScreenVariable(pageText, "devicex");
                            int dashScreeny = ParseScreenVariable(pageText, "devicey");

                            screens.Add(new Tuple<int,int, int>(dashScreenx, dashScreeny, i));
                        }

                        screens = screens.OrderBy(i => i.Item1).ToList();
                        for (int i = screens.Count-1; i >= 0; i--)
                        {
                            Tuple<int, int, int> size = screens[i];
                            int dashScreenx = size.Item1;
                            int dashScreeny = size.Item2;
                            int pos = size.Item3;
                            if (dashScreenx <= screenx && dashScreeny <= screeny)
                            {
                                string filePath = defaultDashboards[pos];
                                string fileName = Path.GetFileNameWithoutExtension(filePath);

                                result = @"Defaults\" + fileName;
                                break;
                            }
                        }
                    }
                }
            }
            return result;
        }

        private static int ParseScreenVariable(string text, string varName)
        {
            int result = 0;

            Regex regex = new Regex(".+\"" + varName + "\":\\s+(\\d+)");
            Match varFound = regex.Match(text);
            if (varFound.Success)
            {
                Int32.TryParse(varFound.Groups[1].Value, out result);
            }

            return result;
        }

        private static string LoadDash(string Name) {
            string dash = null;

            string basePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Panels";
            string fileName = basePath + "\\" + Name + ".json";

            try
            {
                dash = File.ReadAllText(fileName);
            }
            catch (Exception ex)
            {
                dash = null;
            }

            return dash;
        }
    }
}
