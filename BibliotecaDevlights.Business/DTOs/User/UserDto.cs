using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDevlights.Business.DTOs.User
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Img { get; set; }
        public bool IsAdmin { get; set; }


    }
}
