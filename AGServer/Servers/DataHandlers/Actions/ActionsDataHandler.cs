using System;
using System.Linq;
using System.IO;
using System.Collections.Specialized;
using System.Reflection;
using AGData;

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
                        result.Data = LoadDash(postData["name"]);
                        if (result.Data != null)
                        {
                            result.Result = true;
                        }
                        break;

                }
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
