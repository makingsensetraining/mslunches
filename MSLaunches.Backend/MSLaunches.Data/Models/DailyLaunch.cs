namespace MSLaunches.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DailyLaunch : BaseEntity
    {
        public DailyLaunch()
        {
        }

        [Required]
        public Launch CaloricoId { get; set; }

        [Required]
        public Launch LightId { get; set; }

        [Required]
        public Launch VegetarianoId { get; set; }

        [Required]
        public Launch SandwichId { get; set; }

        [Required]
        public int DessertId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public virtual Launch Calorico { get; set; }
        public virtual Launch Light { get; set; }
        public virtual Launch Vegetariano { get; set; }
        public virtual Launch Sandwich { get; set; }
        public virtual Launch Dessert { get; set; }
    }
}
