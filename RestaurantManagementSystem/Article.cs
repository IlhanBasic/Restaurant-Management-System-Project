//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RestaurantManagementSystem
{
    using System;
    using System.Collections.Generic;
    
    public partial class Article
    {
        public int idArticle { get; set; }
        public string nameArticle { get; set; }
        public double priceArticle { get; set; }
        public int categoryID { get; set; }
        public double quantityArticle { get; set; }
        public string unitArticle { get; set; }
        public bool activityArticle { get; set; }
        public byte[] imageArticle { get; set; }
        public Nullable<int> idRestaurant { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}