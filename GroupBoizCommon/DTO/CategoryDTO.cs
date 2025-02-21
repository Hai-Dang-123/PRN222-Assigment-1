using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBoizCommon.DTO
{
    
        public class CategoryDTO
        {
           
            public short CategoryId { get; set; }

       
            public string CategoryName { get; set; } = null!;

           
            public string CategoryDesciption { get; set; } = null!;

           
            public short? ParentCategoryId { get; set; }

            public bool? IsActive { get; set; }

            
          
            
        }
    }



