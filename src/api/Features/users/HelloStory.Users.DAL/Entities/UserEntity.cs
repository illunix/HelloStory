using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloStory.Users.DAL.Entities;

public readonly record struct UserEntity(
    string Email,
    string Username,
    string Password,
    string Salt
)
{
    public DateTime CreatedAt { get; }
}