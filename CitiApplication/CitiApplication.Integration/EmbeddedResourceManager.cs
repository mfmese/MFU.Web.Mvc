using System.IO;
using System.Text;

namespace Application.Integration
{
    internal class EmbeddedResourceManager
    {
        public static string GetText(string fullResourceName)
        {
            var text = "";

            var assembly = typeof(EmbeddedResourceManager).Assembly;

            var list = assembly.GetManifestResourceNames();

            using (var stream = assembly.GetManifestResourceStream(fullResourceName))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        text = reader.ReadToEnd();
                    }
                }
            }
            return text;
        }

        
    }
}
