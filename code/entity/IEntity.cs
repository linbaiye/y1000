using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.networking.message;

namespace y1000.code.entity
{
    public interface IEntity
    {
        string EntityName { get; }

        long Id { get; }

        void Handle(IEntityMessage message)
        {
        }
    }
}