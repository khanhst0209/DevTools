using System.ComponentModel.DataAnnotations;

namespace DevTool.Input2Execute.Bcrypt
{
    public class BcryptInput
    {
        public string text2hash { get; set; }
        public int saltCount { get; set; }

        public string text2compare { get; set; }
        public string hashtext { get; set; }



    }
}