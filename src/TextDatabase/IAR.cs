using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextDatabase
{
    public class IAR
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// This property will be initialized as true.
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// This property will be initialized with the current date
        /// </summary>
        public DateTime RowUpdateDate { get; set; }

        public IAR()
        {
            Active = true;
            RowUpdateDate = DateTime.Now;
        }

    }
}
