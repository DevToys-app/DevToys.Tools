using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using DevToys.Tools.Models;
using Microsoft.Extensions.Logging;

namespace DevToys.Tools.Helpers;

internal static class XmlHelper
{
    /// <summary>
    /// Format a string to the specified Xml format.
    /// </summary>
    internal static ResultInfo<string> Format(string? input, Indentation indentationMode, bool newLineOnAttributes, ILogger logger)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new(string.Empty, false);
        }

        try
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(input);

            if (xmlDocument.FirstChild == null)
            {
                return new(string.Empty, false);
            }

            var xmlWriterSettings = new XmlWriterSettings()
            {
                Async = true,
                OmitXmlDeclaration = xmlDocument.FirstChild.NodeType != XmlNodeType.XmlDeclaration,
                NewLineOnAttributes = newLineOnAttributes,
            };

            switch (indentationMode)
            {
                case Indentation.TwoSpaces:
                    xmlWriterSettings.Indent = true;
                    xmlWriterSettings.IndentChars = "  ";
                    break;
                case Indentation.FourSpaces:
                    xmlWriterSettings.Indent = true;
                    xmlWriterSettings.IndentChars = "    ";
                    break;
                case Indentation.OneTab:
                    xmlWriterSettings.Indent = true;
                    xmlWriterSettings.IndentChars = "\t";
                    break;
                case Indentation.Minified:
                    xmlWriterSettings.Indent = false;
                    break;
                default:
                    throw new NotSupportedException();
            }

            var stringBuilder = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings))
            {
                xmlDocument.Save(xmlWriter);
            }

            if (xmlDocument.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
            {
                Match match = Regex.Match(xmlDocument.FirstChild.InnerText, @"(?<=encoding\s*=\s*"")[^""]*", RegexOptions.None);
                if (match.Success)
                {
                    stringBuilder = stringBuilder.Replace("utf-16", match.Value);
                }
                else
                {
                    stringBuilder = stringBuilder.Replace("encoding=\"utf-16\"", "");
                }
            }
            return new(stringBuilder.ToString());
        }
        catch (XmlException ex)
        {
            return new(ex.Message, false);
        }
        catch (Exception ex) // some other exception
        {
            logger.LogError(ex, "Xml formatter. Indentation: {indentationMode}", indentationMode);
            return new(ex.Message, false);
        }
    }
}
