﻿using System.ComponentModel.DataAnnotations;

namespace ScheduleBTEC.DTO
{
    public class CUser
    {
        public string Email { get; set; }
        
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
    }
}
