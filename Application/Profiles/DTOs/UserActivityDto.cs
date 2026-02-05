using System;

namespace Application.Profiles.DTOs;

public class UserActivityDto
{
 public string Id { get; set; } = "";
 public  string Title { get; set; }="";
 public  string Category { get; set; }="";
 public DateTime Date { get; set; }
}
