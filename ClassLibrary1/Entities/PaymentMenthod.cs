using System;
using System.Collections.Generic;

namespace ClassLibrary1.Entities;

public partial class PaymentMenthod
{
    public int PaymentMenthodId { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
