namespace FastFood.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Enums;

    public class Order
    {
        //za da ne bude dostapvan ot vsichki
        // this.Id = Guid.NewGuid().ToString();

        public Order()
            {
                

                this.OrderItems = new HashSet<OrderItem>();
            }

            [Key]
            public int Id { get; set; }

            public string Customer { get; set; } = null!;

            public DateTime DateTime { get; set; }

            public OrderType Type { get; set; }

            [NotMapped]
            public decimal TotalPrice { get; set; }

            [ForeignKey(nameof(Employee))]
            public int EmployeeId { get; set; }  

            public virtual Employee Employee { get; set; } = null!;

            public virtual ICollection<OrderItem>? OrderItems { get; set; }
        }
    }
