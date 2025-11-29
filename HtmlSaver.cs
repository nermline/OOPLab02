using System.Xml.Xsl;

namespace Lab02
{
    internal static class HtmlSaver
    {
        public static void Transform(string xmlPath, string xslPath, string outputPath)
        {
            if (!File.Exists(xmlPath))
                throw new FileNotFoundException("XML file not found", xmlPath);

            if (!File.Exists(xslPath))
                throw new FileNotFoundException("XSL file not found", xslPath);

            var xslt = new XslCompiledTransform();
            xslt.Load(xslPath);
            xslt.Transform(xmlPath, outputPath);
        }
    }
}
