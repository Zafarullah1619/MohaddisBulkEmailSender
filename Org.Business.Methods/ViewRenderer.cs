using ServiceStack.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Org.Business.Methods
{
    public static class ViewRenderer
    {
        public static string RenderViewToString(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindView(controller.ControllerContext, viewName, null);
                System.Web.Mvc.ViewContext viewContext = new System.Web.Mvc.ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.ToString();
            }
        }

        public static string ReplacePlaceHolders(string body, List<Dictionary<string, object>> content)
        {
            // Replace placeholders in the provided HTML body
            string renderedHtml = body;

            foreach (var field in content)
            {
                foreach (var entry in field)
                {
                    var placeholder = $"[[{entry.Key}]]";
                    var value = entry.Value.ToString();
                    renderedHtml = renderedHtml.Replace(placeholder, value);
                }
            }

            return renderedHtml;
        }

        public static List<Dictionary<string, object>> GetSelectedPlaceholders(string selectedPlaceholdersCsv, List<Dictionary<string, object>> content)
        {
            var selectedContent = new List<Dictionary<string, object>>();

            // Split the comma-separated string into a list of placeholders
            var selectedPlaceholders = selectedPlaceholdersCsv.Split(',').Select(p => p.Trim()).ToList();

            foreach (var field in content)
            {
                var selectedField = new Dictionary<string, object>();

                foreach (var entry in field)
                {
                    if (selectedPlaceholders.Contains(entry.Key))
                    {
                        selectedField[entry.Key] = entry.Value;
                    }
                }

                selectedContent.Add(selectedField);
            }

            return selectedContent;
        }


        public static string ReplacePlaceHolders(string body, List<Dictionary<string, object>> content, string cssFilePath)
        {
            // Replace placeholders in the provided HTML body
            string renderedHtml = body;

            // Load CSS from the provided file path
            string css = File.ReadAllText(cssFilePath);

            // Replace the <style></style> tag with the loaded CSS
            renderedHtml = renderedHtml.Replace("<style></style>", $"<style>{css}</style>");

            foreach (var field in content)
            {
                foreach (var entry in field)
                {
                    var placeholder = $"[\"{entry.Key}\"]";
                    var value = entry.Value.ToString();
                    renderedHtml = renderedHtml.Replace(placeholder, value);
                }
            }

            return renderedHtml;
        }

        public static string BuildHtmlFromContent(List<Dictionary<string, object>> content, string heading = null)
        {
            if (content == null || content.Count == 0)
            {
                return string.Empty;
            }

            var firstRow = content.First();
            var columns = new List<string>(firstRow.Keys);

            foreach (var row in content)
            {
                foreach (var key in row.Keys)
                {
                    if (!columns.Contains(key))
                    {
                        columns.Add(key);
                    }
                }
            }

            var sb = new StringBuilder();
            sb.Append("<!DOCTYPE html><html><body>");
            if (!string.IsNullOrWhiteSpace(heading))
            {
                sb.Append("<h3>").Append(heading).Append("</h3>");
            }
            sb.Append("<table style=\"border-collapse:collapse;width:100%\" border=\"1\">");
            // Table header
            sb.Append("<thead><tr>");
            foreach (var col in columns)
            {
                sb.Append("<th style=\"text-align:left;padding:6px\">").Append(col).Append("</th>");
            }
            sb.Append("</tr></thead>");
            // Table body
            sb.Append("<tbody>");
            foreach (var row in content)
            {
                sb.Append("<tr>");
                foreach (var col in columns)
                {
                    row.TryGetValue(col, out object valueObj);
                    var valueStr = valueObj == null ? string.Empty : valueObj.ToString();
                    sb.Append("<td style=\"padding:6px\">").Append(valueStr).Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</tbody></table></body></html>");

            return sb.ToString();
        }

    }
}
