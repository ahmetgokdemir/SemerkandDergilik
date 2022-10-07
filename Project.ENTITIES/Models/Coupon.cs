using Project.ENTITIES.CoreInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class Coupon : EntityBase, IEntity
    {
        public string CouponName { get; set; }
        public string Code { get; set; }
        public DateTime CouponExpireDay { get; set; }

    }
}
