//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace E_Commerce_WebApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Product()
        {
            this.tbl_Images = new HashSet<tbl_Images>();
            this.tbl_Order = new HashSet<tbl_Order>();
        }
    
        public int Pro_Id { get; set; }
        public string Pro_Name { get; set; }
        public string Pro_Details { get; set; }
        public Nullable<int> Pro_Price { get; set; }
        public string Pro_status { get; set; }
        public Nullable<int> Cat_Id { get; set; }
        public string Pro_Img { get; set; }
    
        public virtual tbl_Categories tbl_Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Images> tbl_Images { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Order> tbl_Order { get; set; }
    }
}
