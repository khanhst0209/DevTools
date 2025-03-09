using System.Text;
using DevTools.Services.Interfaces;

namespace DevTools.Services
{
    public class StringEncoderService : IStringEncoderService
    {
        public string EncodeStringToBase64(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            string base64Encoded = Convert.ToBase64String(bytes);
            return base64Encoded;
        }

    }
}