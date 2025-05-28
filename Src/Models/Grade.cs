using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using System;
using System.Collections.Generic;


namespace GradingApp.Models
{

    public class Grade
    {
        public int GradeId { get; set; }
        public required string Subject { get; set; }
        public int Score { get; set; }
        public DateOnly DateTaken { get; set; }
        public int StudentId { get; set; }
    }    
}
