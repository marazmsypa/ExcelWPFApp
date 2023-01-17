using ExcelWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelWPF.Models
{
    public partial class Payment
    {
        Core db = new Core();
        public string categoryname { get {
                return db.context.Category.FirstOrDefault(x => x.id_category == category_id).name_category;
            }
        }
    }
}
