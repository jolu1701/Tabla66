//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace tabla66.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.ComponentModel;
    public partial class News
    {
        public int Id { get; set; }
        [DisplayName("Titel")]
        public string Title { get; set; }
        public string Text { get; set; }
        [DisplayName("Bild")]
        public string Image { get; set; }
        [DisplayName("Show")]
        public int Show_id { get; set; }
    
        public virtual Show Show { get; set; }

    }
}
