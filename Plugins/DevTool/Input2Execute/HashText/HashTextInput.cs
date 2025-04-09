using System.ComponentModel.DataAnnotations;
using DevTool.Roles;
namespace DevTool.Input2Execute.HashText
{
    public class HashTextInput
    {
        [Required]
        public string textInput { get; set; }
        [Required]
        public Dictionary<string, bool> digitalEncoding {get; set;} 
    }

    public enum HashTextDigitalEncoding
    {
        Binary,
        Hexadecimal,
        Base64,
        Base64url
    }
}