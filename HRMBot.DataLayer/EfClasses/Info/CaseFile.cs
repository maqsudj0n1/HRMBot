using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMBot.DataLayer.EfClasses;

[Table("info_case_file")]
public partial class CaseFile : FileEntity<long>
{

}
