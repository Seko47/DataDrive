﻿using DataDrive.DAO.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDrive.DAO.Models
{
    public class File : FileAbstract
    {
        public string Name { get; set; }

        public string Path { get; set; }
    }
}