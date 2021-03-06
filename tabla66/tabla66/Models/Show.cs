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
    using System.ComponentModel;
    
    public partial class Show
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Show()
        {
            this.News = new HashSet<News>();
        }
    
        public int Id { get; set; }
        [DisplayName("Titel")]
        public string Title { get; set; }
        public Nullable<int> Channel_id { get; set; }
        public Nullable<int> Genre_id { get; set; }
        [DisplayName("Visas")]
        public System.DateTime Start_time { get; set; }
        [DisplayName("L�ngd")]
        public System.TimeSpan Duration { get; set; }
        public string Info { get; set; }
    
        public virtual Channel Channel { get; set; }
        public virtual Genre Genre { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<News> News { get; set; }
    }
}
