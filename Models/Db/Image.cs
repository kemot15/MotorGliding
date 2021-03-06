﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Models.Db
{
    public class Image
    {
        public int Id { get; set; }
        [MaxLength]
        public string Name { get; set; }
        public string Category { get; set; }
        public int SourceId { get; set; }
        [DefaultValue(false)]
        public bool Default { get; set; }      
        
        public bool Active { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public IList<IFormFile> Gallery { get; set; }

    }
}
