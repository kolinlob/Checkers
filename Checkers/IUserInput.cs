﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public interface IUserInput
    {

        bool PlaysWhites { get; set; }
        string InputCoordinates();

    }
}